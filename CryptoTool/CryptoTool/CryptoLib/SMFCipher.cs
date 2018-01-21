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
        private SMFCipher() { }
        public static SMFCipher GetInstance(string pwd)
        {
            if (uniqueInstance == null)
            {
                uniqueInstance = new SMFCipher(pwd);
            }
            return uniqueInstance;
        }
        public static SMFCipher GetInstance(string pwd, string srcfile)
        {
            if(uniqueInstance == null)
            {
                uniqueInstance = new SMFCipher(pwd, srcfile);
            }
            return uniqueInstance;
        }
        public static SMFCipher GetInstance(string pwd,string srcfile,string destpath)
        {
            if(uniqueInstance == null)
            {
                uniqueInstance = new SMFCipher(pwd, srcfile, destpath);
            }
            return uniqueInstance;
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
        bool isbusy = false;

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
        protected SMFCipher(string pwd){
            InitializeDelegates();
            Password = pwd;
        }

        /// <summary>
        /// 传入口令和目标文件
        /// </summary>
        /// <param name="pwd"></param>
        /// <param name="srcfile"></param>
        protected SMFCipher(string pwd,string srcfile)
        {
            InitializeDelegates();
            Password = pwd;
            SourceFile = srcfile;
        }

        /// <summary>
        /// 传入口令、目标文件和加密目的地
        /// </summary>
        /// <param name="pwd"></param>
        /// <param name="srcfile"></param>
        /// <param name="destpath"></param>
        protected SMFCipher(string pwd,string srcfile,string destpath)
        {
            InitializeDelegates();
            Password = pwd;
            SourceFile = srcfile;
            DestPath = destpath;
        }
        
        public void DoWork(bool isEncrypt)
        {
            if (isbusy)
                return;
            else
                isbusy = true;

            if (String.IsNullOrEmpty(SourceFile) || !System.IO.File.Exists(SourceFile)){
                onTaskStateChanged(this, new TaskStateChangedEventArgs(){
                    CryptState = CryptState.Error,
                    Description = "请输入正确的目标文件"
                });
                return;
            }else{
                if (String.IsNullOrEmpty(DestPath)){
                    try{
                        DestPath = System.IO.Path.GetDirectoryName(SourceFile);
                    }catch(Exception e){
                        onTaskStateChanged(this, new TaskStateChangedEventArgs() {
                            CryptState = CryptState.Error,
                            Description = "设定输出目录时出错"
                        });
                        return;
                    }
                }
            }

            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(1);
            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;
            mTask = new Task((asyOp) =>
            {
                AsyncOperation asyOperation = asyOp as AsyncOperation;
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
            }, asyncOp, token);

            mTask.Start();
        }

        public void Cancel(){
            tokenSource.Cancel();
        }
    }
}
