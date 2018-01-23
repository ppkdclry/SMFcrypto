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
                if (String.IsNullOrEmpty(DestPath))
                {
                    try
                    {
                        DestPath = System.IO.Path.GetDirectoryName(SourceFile);
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
                try{
                    //有可能在开始加密前取消处理
                    if (token.IsCancellationRequested){
                        throw new OperationCanceledException(token);
                    }

                    //以下代码应替换为业务代码
                    
                    int total = 10;
                    for(int i = 0;i < total; i++)
                    {
                        //进度报告
                        asyOp.Post(onTaskStateChangedReportDelgate, new TaskStateChangedEventArgs()
                        {
                            CryptState = CryptState.ComProc,
                            Description = "加密中",
                            TaskID = 0,
                            CurrentNumber = i,
                            TotalNumber = total
                        });
                        if (token.IsCancellationRequested)
                        {
                            throw new OperationCanceledException(token);
                        }
                        Thread.Sleep(500);
                    }

                    //传出完成信息
                    asyOp.Post(onTaskStateChangedReportDelgate, new TaskStateChangedEventArgs()
                    {
                        CryptState = CryptState.Finish,
                        Description = "加密任务结束"
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
                            Description = "任务已取消"
                        });
                    }
                    return;
                }
                finally{
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
