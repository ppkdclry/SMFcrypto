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
using System.IO;
using CustomControlLib;

namespace EncryptTool.Pages.EncryptPage
{
    /// <summary>
    /// EncryptStartPage.xaml 的交互逻辑
    /// </summary>
    public partial class EncryptStartPage : BasePage
    {
        public EncryptStartPage()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            Button obj = sender as Button;
            ParentWindow.NavigateEncrypt(obj.Tag.ToString());
        }

        private void EncryptStartPage_Loaded(object sender, RoutedEventArgs e)
        {
            if(ParentWindow != null){
                if (ParentWindow.encryptSrcFile != null && ParentWindow.encryptDestPath != null){
                    //设置待加密文件
                    txtSrcFile.Text = ParentWindow.encryptSrcFile;
                    txtDestPath.Text = ParentWindow.encryptDestPath;
                }
            }
        }

        /// <summary>
        /// 密码显示设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IsShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            pwdBox.IsTabStop = true;        //利用IsTabStop属性的值变动触发内部明文代码框的折叠和显示，仅适合该项目(不安全用法)
        }

        /// <summary>
        /// 密码隐藏设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IsShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            pwdBox.IsTabStop = false;
        }
    }
}
