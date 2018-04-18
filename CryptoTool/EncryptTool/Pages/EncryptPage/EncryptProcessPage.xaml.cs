using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EncryptTool.CryptoLib;

namespace EncryptTool.Pages.EncryptPage
{
    /// <summary>
    /// EncryptProcessPage.xaml 的交互逻辑
    /// </summary>
    public partial class EncryptProcessPage : BasePage
    {
        private SMFCipher smfCipher { get; set; }

        public EncryptProcessPage()
        {
            InitializeComponent();

            //创建加密器，并设置回调函数
            smfCipher = new SMFCipher();
            smfCipher.OnTaskStateChanged += OnChanged;
        }

        /// <summary>
        /// 进度报告页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EncryptProcessPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (ParentWindow == null)
            {
                return;
            }

            //有重复检查嫌疑
            if(ParentWindow.encryptSrcFile == null || ParentWindow.encryptDestPath == null || ParentWindow.encryptPassword == null)
            {
                label.Content = "加密信息传入错误！";
                return;
            }

            if (smfCipher.isBusy())
            {
                label.Content = "有文件正在加密...";
                return;
            }    
            //设置待加密文件，如果已有文件在加密则返回  
            smfCipher.setPwdSrcDest(ParentWindow.encryptPassword, ParentWindow.encryptSrcFile, ParentWindow.encryptDestPath);
            smfCipher.DoWork(true);
        }

        /// <summary>
        /// 取消加密操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            //加密取消，可重复调用
            if (smfCipher != null) smfCipher.Cancel();

            //取消后跳转至加密起始页面
            Button obj = sender as Button;
            ParentWindow.NavigateEncrypt(obj.Tag.ToString());
        }

        /// <summary>
        /// 加密过程回调，显示进度报告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnChanged(object sender, TaskStateChangedEventArgs e)
        {
            string temp = string.Empty;
            switch (e.CryptState)
            {
                case CryptState.Finish:
                    temp = "加密完成";
                    pbEncryptTask.Value = pbEncryptTask.Maximum;
                    //跳转至结果页面
                    ParentWindow.isEncryptOK = true;
                    ParentWindow.encryptResult = temp;
                    ParentWindow.NavigateEncrypt("EncryptFinishPage");
                    break;
                case CryptState.Error:
                    temp = "加密失败";
                    //跳转至结果页面
                    ParentWindow.isEncryptOK = false;
                    ParentWindow.encryptResult = temp;
                    ParentWindow.NavigateEncrypt("EncryptFinishPage");
                    break;
                case CryptState.Cancel:
                    temp = string.Format("{0}", e.Description);
                    label.Content = temp;
                    pbEncryptTask.Value = 0;
                    break;
                case CryptState.Encrypt:
                case CryptState.Decrypt:
                case CryptState.Compress:
                case CryptState.Decompress:
                    temp = string.Format("{0}:{1}/{2}", e.Description, e.CurrentNumber, e.TotalNumber);
                    label.Content = temp;
                    pbEncryptTask.Value = ((double)e.CurrentNumber) / ((double)e.TotalNumber);
                    break;
                default:
                    break;
            }
        }
    }
}
