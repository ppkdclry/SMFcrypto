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

namespace EncryptTool.Pages.EncryptPage
{
    /// <summary>
    /// EncryptFinishPage.xaml 的交互逻辑
    /// </summary>
    public partial class EncryptFinishPage : BasePage
    {
        public EncryptFinishPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初始化结果图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EncryptFinishPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (ParentWindow == null)
            {
                return;
            }

            //有重复检查嫌疑
            if (ParentWindow.encryptResult == null)
            {
                label.Content = "加密信息传入错误！";
                return;
            }

            BitmapImage image = null;
            if (ParentWindow.isEncryptOK)
            {
                image = new BitmapImage(new Uri("/image/Succeed.png", UriKind.Relative));
            }
            else
            {
                image = new BitmapImage(new Uri("/image/Error.png", UriKind.Relative));
            }
            imgResult.Source = image;
        }

        /// <summary>
        /// 跳回起始加密界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFinish_Click(object sender, RoutedEventArgs e)
        {
            Button obj = sender as Button;
            ParentWindow.NavigateEncrypt(obj.Tag.ToString());
        }

        
        /// <summary>
        /// 打开资源管理器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnScan_Click(object sender, RoutedEventArgs e)
        {
            if(ParentWindow.encryptDestPath == null || ParentWindow.encryptDestPath == string.Empty)
            {
                return;
            }

            //打开资源管理器
            try
            {
                System.Diagnostics.Process.Start(ParentWindow.encryptDestPath);
            }catch (Exception)
            {
                MessageBox.Show("目录可能不存在");
            }
            
        }
    }
}
