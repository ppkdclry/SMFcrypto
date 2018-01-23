using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Threading;

namespace CryptoTool.CryptoLib
{
    class SMFCipher
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
            mTask = new Task(() =>
            {
                try{
                    //有可能在开始加密前取消处理
                    if (token.IsCancellationRequested){
                        throw new OperationCanceledException(token);
                    }

                    //以下代码应替换为业务代码
                    //主处理代码，10秒
                    int total = 10;
                    for(int i = 0;i < total; i++)
                    {
                        //进度报告
                        asyncOp.Post(onTaskStateChangedReportDelgate, new TaskStateChangedEventArgs()
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
                    asyncOp.Post(onTaskStateChangedReportDelgate, new TaskStateChangedEventArgs()
                    {
                        CryptState = CryptState.Finish,
                        Description = "加密任务结束"
                    });
                }
                catch(Exception e){
                    if(!(e is OperationCanceledException)){
                        asyncOp.Post(onTaskStateChangedReportDelgate, new TaskStateChangedEventArgs()
                        {
                            CryptState = CryptState.Error,
                            Description = e.Message
                        });
                    }else{
                        asyncOp.Post(onTaskStateChangedReportDelgate, new TaskStateChangedEventArgs()
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
            }, token);

            isbusy = true;
            mTask.Start();
        }

        public void Cancel(){
            if (isBusy()){
                tokenSource.Cancel();
            }
        }
    }
}
