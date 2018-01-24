using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using CryptoTool.CryptoLib.Utils;
using System.Security.Cryptography;
using System.IO.Compression;

namespace CryptoTool.CryptoLib
{
    public class SMFCipher
    {
        /// <summary>
        /// 单例模式
        /// </summary>
        private static SMFCipher uniqueInstance;
        private SMFCipher()
        {
            InitializeDelegates();
        }
        public static SMFCipher GetInstance()
        {
            if (uniqueInstance == null)
            {
                uniqueInstance = new SMFCipher();
            }
            return uniqueInstance;
        }
        bool isbusy = false;
        public bool isBusy()
        {
            return isbusy;
        }

        /// <summary>
        /// 加密器成员参数
        /// </summary>
        public string DestPath { get; set; }
        public string SourceFile { get; set; }
        public string DestFile { get; set; }
        private string Password { get; set; }

        /// <summary>
        /// 客户端异步调用服务的成员
        /// </summary>
        Task mTask;
        CancellationTokenSource tokenSource;
        CancellationToken token;

        /// <summary>
        /// 报告客户端异步操作状态的私有委托
        /// </summary>
        private SendOrPostCallback onTaskStateChangedReportDelgate;

        protected void InitializeDelegates()
        {
            onTaskStateChangedReportDelgate = new SendOrPostCallback(reportTaskStateChanged);
        }

        private void reportTaskStateChanged(object operationState)
        {
            TaskStateChangedEventArgs e = operationState as TaskStateChangedEventArgs;
            OnTaskStateChanged(this, e);
        }

        /// <summary>
        /// 委托
        /// </summary>
        public TaskStateChangedEventHandler OnTaskStateChanged { get; set; }

        /// <summary>
        /// 调用委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void onTaskStateChanged(object sender,TaskStateChangedEventArgs e)
        {
            if(OnTaskStateChanged != null)
            {
                OnTaskStateChanged(sender, e);
            }
        }

        /// <summary>
        /// 传入口令
        /// </summary>
        /// <param name="pwd"></param>
        public SMFCipher setPwd(string pwd)
        {
            if (!isBusy()){
                Password = pwd;
            }
            return this;
        }

        /// <summary>
        /// 传入口令和目标文件
        /// </summary>
        /// <param name="pwd"></param>
        /// <param name="srcfile"></param>
        public SMFCipher setPwdSrc(string pwd,string srcfile)
        {
            if (!isBusy()){
                Password = pwd;
                SourceFile = srcfile;
            }
            return this;
        }

        /// <summary>
        /// 传入口令、目标文件和加密目的地
        /// </summary>
        /// <param name="pwd"></param>
        /// <param name="srcfile"></param>
        /// <param name="destpath"></param>
        public SMFCipher setPwdSrcDest(string pwd,string srcfile,string destpath)
        {
            if (!isBusy()){
                Password = pwd;
                SourceFile = srcfile;
                DestPath = destpath;
            }
            return this;
        }
        
        private bool isParaOK(){
            if (String.IsNullOrEmpty(SourceFile) || !System.IO.File.Exists(SourceFile))
            {
                onTaskStateChanged(this, new TaskStateChangedEventArgs()
                {
                    CryptState = CryptState.Error,
                    Description = "请输入正确的目标文件"
                });
                return false;
            }
            else if (String.IsNullOrEmpty(Password))
            {
                onTaskStateChanged(this, new TaskStateChangedEventArgs()
                {
                    CryptState = CryptState.Error,
                    Description = "口令为空，请确认后重新调用"
                });
                return false;
            }
            else {
                //如果没有设置目标目录，或者设置了确不存在默认为源文件目录
                if (String.IsNullOrEmpty(DestPath) || !Directory.Exists(DestPath))
                {
                    try
                    {
                        DestPath = Path.GetDirectoryName(SourceFile);
                    }
                    catch (Exception e)
                    {
                        onTaskStateChanged(this, new TaskStateChangedEventArgs()
                        {
                            CryptState = CryptState.Error,
                            Description = "设定输出目录时出错"
                        });
                        return false;
                    }
                }
            }
            return true;
        }

        public void DoWork(bool isEncrypt = true)
        {
            if (isBusy()) return;

            if (!isParaOK()){
                return;
            }

            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(1);
            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;
            mTask = new Task((para) =>
            {
                AsyncOperation asyOp = para as AsyncOperation;

                //临时文件
                string tmpFileName = null;
                try{
                    //有可能在开始加密前取消处理
                    if (token.IsCancellationRequested){
                        throw new OperationCanceledException(token);
                    }

                    //以下代码应替换为业务代码
                    if (isEncrypt)
                    {
                        //待加密文件增加后缀.enc作为加密后文件名
                        string sourceFileName = Path.GetFileName(SourceFile);
                        tmpFileName = Path.Combine(DestPath, sourceFileName + ".tmp");
                        string destfile = Path.Combine(DestPath, sourceFileName + ".enc");
                        compressFileAsync(SourceFile, tmpFileName, asyOp);
                        encryptFileAsync(tmpFileName, destfile, Password, asyOp);
                    }
                    else
                    {
                        //待解密文件需以.enc为后缀，去掉.enc后增加.rec作为解密文件名
                        string sourceFileExtension = Path.GetExtension(SourceFile);
                        if (!sourceFileExtension.Equals(".enc"))
                            throw new Exception("文件后缀名应该是.enc");
                        string srcFileNameWithoutExtension = Path.GetFileNameWithoutExtension(SourceFile);
                        string destfile = Path.Combine(DestPath, srcFileNameWithoutExtension + ".rec");
                        tmpFileName = Path.Combine(DestPath, srcFileNameWithoutExtension + ".tmp");
                        decryptFileAsync(SourceFile, tmpFileName, Password, asyOp);
                        decompressFileAsync(tmpFileName, destfile, asyOp);
                    }

                    //传出完成信息
                    asyOp.Post(onTaskStateChangedReportDelgate, new TaskStateChangedEventArgs()
                    {
                        CryptState = CryptState.Finish,
                        Description = "任务完成"
                    });
                }
                catch(Exception e){
                    if(!(e is OperationCanceledException)){
                        asyOp.Post(onTaskStateChangedReportDelgate, new TaskStateChangedEventArgs()
                        {
                            CryptState = CryptState.Error,
                            Description = e.Message
                        });
                    }else{
                        asyOp.Post(onTaskStateChangedReportDelgate, new TaskStateChangedEventArgs()
                        {
                            CryptState = CryptState.Cancel,
                            Description = "任务取消"
                        });
                    }
                    return;
                }
                finally{
                    //任务退出时清除临时文件
                    if (!string.IsNullOrEmpty(tmpFileName) && File.Exists(tmpFileName))
                    {
                        try
                        {
                            File.Delete(tmpFileName);
                        }
                        catch(Exception e)
                        {
                            //如果临时文件删除失败，什么都不做，就当什么也没发生
                        }
                    }
                    //任务退出时，加解密器忙位重置
                    isbusy = false;
                }
            }, asyncOp, token);

            isbusy = true;
            mTask.Start();
        }

        ///加密标识
        private const ulong SM4_TAG = 0x8765432112345678;
        private const int BUFSIZE = 128 * 1024;
        /// <summary>
        /// 可取消文件加密
        /// </summary>
        /// <param name="srcFile"></param>
        /// <param name="destFile"></param>
        /// <param name="password"></param>
        /// <param name="asyOp"></param>
        private void encryptFileAsync(string srcFile,string destFile,string password,AsyncOperation asyOp)
        {
            FileStream fsrc = null, fdest = null;
            CryptoStream cdest = null, chash = null;
            BinaryWriter bwdest = null;
            try
            {
                fsrc = File.OpenRead(srcFile);
                fdest = File.OpenWrite(destFile);

                long lSize = fsrc.Length;
                byte[] buffer = new byte[BUFSIZE];
                int tmpRead = -1;
                int totalRead = 0;

                byte[] IV = SMFCBC.GenerateRandomBytes(16);
                byte[] SALT = SMFCBC.GenerateRandomBytes(16);

                PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, SALT, "SHA256", 100);
                byte[] KEY = pdb.GetBytes(16);

                //往文件头添加IV和SALT值
                fdest.Write(IV, 0, IV.Length);
                fdest.Write(SALT, 0, SALT.Length);

                //创建SM4加密器和SHA256散列器
                SMFCBC smfencryptor = new SMFCBC();
                HashAlgorithm hasher = SHA256.Create();
                cdest = new CryptoStream(fdest, smfencryptor.CreateEncryptor(KEY, IV), CryptoStreamMode.Write);
                chash = new CryptoStream(Stream.Null, hasher, CryptoStreamMode.Write);
                //包装加密流
                bwdest = new BinaryWriter(cdest);

                //加密开头为长度和文件标识
                bwdest.Write(lSize);
                bwdest.Write(SM4_TAG);

                //增加报告进度
                int totalNumber = (int)(lSize / BUFSIZE);

                while ((tmpRead = fsrc.Read(buffer, 0, buffer.Length)) != 0)
                {
                    bwdest.Write(buffer, 0, tmpRead);
                    chash.Write(buffer, 0, tmpRead);
                    totalRead += tmpRead;

                    //报告进度
                    asyOp.Post(onTaskStateChangedReportDelgate, new TaskStateChangedEventArgs()
                    {
                        CryptState = CryptState.Encrypt,
                        Description = "加密中",
                        TaskID = 0,
                        CurrentNumber = totalRead / BUFSIZE,
                        TotalNumber = totalNumber
                    });

                    //如果取消，则抛出取消异常
                    if (token.IsCancellationRequested)
                    {
                        throw new OperationCanceledException(token);
                    }
                }
                //关闭SHA256散列器
                chash.Flush();
                chash.Close();

                byte[] HASH = hasher.Hash;

                bwdest.Write(HASH, 0, HASH.Length);
                //关闭二进制包装类
                bwdest.Flush();
                bwdest.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                //顺序不可颠倒
                if (bwdest != null) bwdest.Close();
                if (chash != null) chash.Close();
                if (cdest != null) cdest.Close();
                if (fdest != null) fdest.Close();
                if (fsrc != null) fsrc.Close();
            }
            return;
        }

        /// <summary>
        /// 可取消文件解密
        /// </summary>
        /// <param name="srcFile"></param>
        /// <param name="destFile"></param>
        /// <param name="password"></param>
        /// <param name="asyOp"></param>
        private void decryptFileAsync(string srcFile, string destFile, string password, AsyncOperation asyOp)
        {
            FileStream fsrc = null, fdest = null;
            CryptoStream cdest = null, chash = null;
            BinaryReader brdest = null;
            try {
                fsrc = File.OpenRead(srcFile);
                fdest = File.OpenWrite(destFile);

                long lSize = (int)srcFile.Length;
                byte[] buffer = new byte[BUFSIZE];
                int hasRead = -1;
                int totalRead = 0;

                byte[] IV = new byte[16];
                byte[] SALT = new byte[16];
                fsrc.Read(IV, 0, 16);
                fsrc.Read(SALT, 0, 16);

                PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, SALT, "SHA256", 100);
                byte[] KEY = pdb.GetBytes(16);

                //创建SM4解密器和SHA256散列器
                SMFCBC smfdecryptor = new SMFCBC();
                HashAlgorithm hasher = SHA256.Create();
                cdest = new CryptoStream(fsrc, smfdecryptor.CreateDecryptor(KEY, IV), CryptoStreamMode.Read);
                chash = new CryptoStream(Stream.Null, hasher, CryptoStreamMode.Write);
                BinaryReader brDest = new BinaryReader(cdest);
                lSize = brDest.ReadInt64();
                ulong tag = brDest.ReadUInt64();

                if (tag != SM4_TAG)
                {
                    throw new Exception("文件解密出错(口令不对，或者不是加密文件)");
                }

                long numReads = lSize / BUFSIZE;
                long numRest = (long)lSize % BUFSIZE;

                //增加报告
                int totalNumber = (int)(lSize / BUFSIZE);

                for (int i = 0; i < numReads; i++)
                {
                    hasRead = brDest.Read(buffer, 0, buffer.Length);
                    fdest.Write(buffer, 0, hasRead);
                    chash.Write(buffer, 0, hasRead);
                    totalRead += hasRead;

                    //报告进度
                    asyOp.Post(onTaskStateChangedReportDelgate, new TaskStateChangedEventArgs()
                    {
                        CryptState = CryptState.Decrypt,
                        Description = "解密中",
                        TaskID = 0,
                        CurrentNumber = totalRead / BUFSIZE,
                        TotalNumber = totalNumber
                    });

                    //如果取消，则抛出取消异常
                    if (token.IsCancellationRequested)
                    {
                        throw new OperationCanceledException(token);
                    }
                }

                if (numRest > 0)
                {
                    hasRead = brDest.Read(buffer, 0, (int)numRest);
                    fdest.Write(buffer, 0, hasRead);
                    chash.Write(buffer, 0, hasRead);
                    totalRead += hasRead;
                }

                //关闭散列器
                chash.Flush();
                chash.Close();

                fdest.Flush();
                fdest.Close();

                byte[] curHash = hasher.Hash;

                //和文件中的散列值比较
                byte[] oldHash = new byte[hasher.HashSize / 8];
                hasRead = brDest.Read(oldHash, 0, oldHash.Length);
                if ((oldHash.Length != hasRead) || (!CheckByteArrays(oldHash, curHash)))
                    throw new Exception("文件经过修改");

                brDest.Close();

                if (totalRead != lSize)
                    throw new Exception("文件长度不对");
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                //顺序不可颠倒
                if (brdest != null) brdest.Close();
                if (chash != null) chash.Close();
                if (cdest != null) cdest.Close();
                if (fdest != null) fdest.Close();
                if (fsrc != null) fsrc.Close();
            }
            return;
        }
        /// <summary>
        /// 加密文件
        /// </summary>
        /// <param name="srcFile">源绝对路径</param>
        /// <param name="destFile">目的绝对路径</param>
        public void encryptFile(string srcFile, string destFile, string password)
        {
            using (FileStream fsrc = File.OpenRead(srcFile),
                fdest = File.OpenWrite(destFile))
            {
                long lSize = fsrc.Length;
                byte[] buffer = new byte[BUFSIZE];
                int tmpRead = -1;
                int totalRead = 0;

                byte[] IV = SMFCBC.GenerateRandomBytes(16);
                byte[] SALT = SMFCBC.GenerateRandomBytes(16);

                PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, SALT, "SHA256", 100);
                byte[] KEY = pdb.GetBytes(16);

                //往文件头添加IV和SALT值
                fdest.Write(IV, 0, IV.Length);
                fdest.Write(SALT, 0, SALT.Length);

                //创建SM4加密器和SHA256散列器
                SMFCBC smfencryptor = new SMFCBC();
                HashAlgorithm hasher = SHA256.Create();
                using (CryptoStream cdest = new CryptoStream(fdest,
                    smfencryptor.CreateEncryptor(KEY, IV),
                    CryptoStreamMode.Write),
                    chash = new CryptoStream(Stream.Null, hasher, CryptoStreamMode.Write))
                {
                    //包装加密流
                    BinaryWriter bw_dest = new BinaryWriter(cdest);

                    //加密开头为长度和文件标识
                    bw_dest.Write(lSize);
                    bw_dest.Write(SM4_TAG);

                    while((tmpRead = fsrc.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        bw_dest.Write(buffer, 0, tmpRead);
                        chash.Write(buffer, 0, tmpRead);
                        totalRead += tmpRead;
                    }
                    //关闭SHA256散列器
                    chash.Flush();
                    chash.Close();

                    byte[] HASH = hasher.Hash;

                    bw_dest.Write(HASH, 0, HASH.Length);
                    //关闭二进制包装类
                    bw_dest.Flush();
                    bw_dest.Close();
                } 
            }
        }

        /// <summary>
        /// 解密文件
        /// </summary>
        /// <param name="srcFile"></param>
        /// <param name="destFile"></param>
        /// <param name="password"></param>
        public void decryptFile(string srcFile,string destFile,string password)
        {
            using (FileStream fsrc = File.OpenRead(srcFile),
                fdest = File.OpenWrite(destFile))
            {
                long lSize = (int)srcFile.Length;
                byte[] buffer = new byte[BUFSIZE];
                int hasRead = -1;
                int totalRead = 0;

                byte[] IV = new byte[16];
                byte[] SALT = new byte[16];
                fsrc.Read(IV, 0, 16);
                fsrc.Read(SALT, 0, 16);

                PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, SALT, "SHA256", 100);
                byte[] KEY = pdb.GetBytes(16);

                //创建SM4解密器和SHA256散列器
                SMFCBC smfdecryptor = new SMFCBC();
                HashAlgorithm hasher = SHA256.Create();
                using (CryptoStream cdest = new CryptoStream(fsrc, smfdecryptor.CreateDecryptor(KEY, IV), CryptoStreamMode.Read),
                    chash = new CryptoStream(Stream.Null, hasher, CryptoStreamMode.Write))
                {
                    BinaryReader brDest = new BinaryReader(cdest);
                    lSize = brDest.ReadInt64();
                    ulong tag = brDest.ReadUInt64();

                    if(tag != SM4_TAG)
                    {
                        throw new Exception("文件非加密文件");
                    }

                    long numReads = lSize / BUFSIZE;
                    long numRest = (long)lSize % BUFSIZE;

                    for(int i = 0;i < numReads; i++)
                    {
                        hasRead = brDest.Read(buffer, 0, buffer.Length);
                        fdest.Write(buffer, 0, hasRead);
                        chash.Write(buffer, 0, hasRead);
                        totalRead += hasRead;
                    }

                    if(numRest > 0)
                    {
                        hasRead = brDest.Read(buffer, 0, (int)numRest);
                        fdest.Write(buffer, 0, hasRead);
                        chash.Write(buffer, 0, hasRead);
                        totalRead += hasRead;
                    }

                    //关闭散列器
                    chash.Flush();
                    chash.Close();

                    fdest.Flush();
                    fdest.Close();

                    byte[] curHash = hasher.Hash;

                    //和文件中的散列值比较
                    byte[] oldHash = new byte[hasher.HashSize / 8];
                    hasRead = brDest.Read(oldHash, 0, oldHash.Length);
                    if ((oldHash.Length != hasRead) || (!CheckByteArrays(oldHash, curHash)))
                        throw new Exception("文件经过修改");

                    brDest.Close();

                    if (totalRead != lSize)
                        throw new Exception("文件长度不对");
                }

            }
        }

        /// <summary>
        /// 可取消的压缩函数
        /// </summary>
        /// <param name="srcFile"></param>
        /// <param name="destFile"></param>
        /// <param name="asyOp"></param>
        private void compressFileAsync(string srcFile,string destFile,AsyncOperation asyOp)
        {
            FileStream fsIn = null, fsOut = null;
            GZipStream compressGZipStream = null;
            try
            {
                //待压缩
                fsIn = new FileStream(srcFile, FileMode.Open);
                fsOut = new FileStream(destFile, FileMode.Create);

                long lSize = fsIn.Length;
                int hasRead = -1;
                int totalRead = 0;
                byte[] buffer = new byte[BUFSIZE];

                compressGZipStream = new GZipStream(fsOut, CompressionMode.Compress);

                //待报告内容
                int totalNumber= (int)lSize / BUFSIZE;
                while((hasRead = fsIn.Read(buffer, 0, buffer.Length)) != 0)
                {
                    compressGZipStream.Write(buffer, 0, hasRead);
                    totalRead += hasRead;

                    //报告进度
                    asyOp.Post(onTaskStateChangedReportDelgate, new TaskStateChangedEventArgs()
                    {
                        CryptState = CryptState.Compress,
                        Description = "压缩中",
                        TaskID = 0,
                        CurrentNumber = totalRead / BUFSIZE,
                        TotalNumber = totalNumber
                    });

                    //如果取消，则抛出取消异常
                    if (token.IsCancellationRequested)
                    {
                        throw new OperationCanceledException(token);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (compressGZipStream != null)  compressGZipStream.Close();
                if (fsOut != null) fsOut.Close();
                if (fsIn != null) fsIn.Close();
            }
            return;
        }

        private void decompressFileAsync(string srcFile,string destFile,AsyncOperation asyOp)
        {
            FileStream fsIn = null, fsOut = null;
            GZipStream decompressGZipStream = null;
            try
            {
                //解压
                fsIn = new FileStream(srcFile, FileMode.Open);
                fsOut = new FileStream(destFile, FileMode.Create);

                long lSize = fsIn.Length;
                int hasRead = -1;
                int totalRead = 0;
                byte[] buffer = new byte[BUFSIZE];

                decompressGZipStream = new GZipStream(fsIn, CompressionMode.Decompress);

                //报告内容
                int totalNumber = (int)lSize / BUFSIZE;
                while((hasRead = decompressGZipStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    fsOut.Write(buffer, 0, hasRead);
                    totalRead += hasRead;

                    //报告进度，由于解压无法知道解压进度，所以当读取量为压缩文件大小时，置为100%
                    int curNumber = totalRead / BUFSIZE < totalNumber ? totalRead / BUFSIZE : totalNumber;
                    asyOp.Post(onTaskStateChangedReportDelgate, new TaskStateChangedEventArgs()
                    {
                        CryptState = CryptState.Decompress,
                        Description = "解压中",
                        TaskID = 0,
                        CurrentNumber = curNumber,
                        TotalNumber = totalNumber
                    });

                    //如果取消，则抛出取消异常
                    if (token.IsCancellationRequested)
                    {
                        throw new OperationCanceledException(token);
                    }
                }
            }
            catch(Exception e)
            {
                throw e;
            }
            finally
            {
                if (decompressGZipStream != null) decompressGZipStream.Close();
                if (fsIn != null) fsIn.Close();
                if (fsOut != null) fsOut.Close();
            }
            return;
        }

        static private bool CheckByteArrays(byte[] b1,byte[] b2)
        {
            if(b1.Length == b2.Length)
            {
                for(int i = 0;i < b1.Length; i++)
                {
                    if (b1[i] != b2[i])
                        return false;
                }
                return true;
            }
            return false;
        }

        public void Cancel(){
            if (isBusy()){
                tokenSource.Cancel();
            }
        }
    }
}
