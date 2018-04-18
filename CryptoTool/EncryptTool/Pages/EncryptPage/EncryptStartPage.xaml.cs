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
       




        #region 界面逻辑
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
        #endregion

        #region 业务逻辑

        private System.Windows.Forms.OpenFileDialog sourcefiledialog;
        private System.Windows.Forms.FolderBrowserDialog destpathdialog;

        /// <summary>
        /// 在构造函数中初始化部分组件
        /// </summary>
        public EncryptStartPage()
        {
            sourcefiledialog = new System.Windows.Forms.OpenFileDialog();
            destpathdialog = new System.Windows.Forms.FolderBrowserDialog();
            InitializeComponent();
        }

        /// <summary>
        /// 起始页加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EncryptStartPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (ParentWindow != null)
            {
                if (ParentWindow.encryptSrcFile != null && ParentWindow.encryptDestPath != null)
                {
                    //设置待加密文件
                    txtSrcFile.Text = ParentWindow.encryptSrcFile;
                    txtDestPath.Text = ParentWindow.encryptDestPath;
                }
            }
        }
       
        /// <summary>
        /// 选择文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChooseSrcFile_Click(object sender, RoutedEventArgs e)
        {
            sourcefiledialog.Title = "请选择文件";
            sourcefiledialog.Filter = "所有文件(*.*)|*.*";
            if (sourcefiledialog.ShowDialog() == System.Windows.Forms.DialogResult.OK){
                txtSrcFile.Text = sourcefiledialog.FileName;
            }
        }

        /// <summary>
        /// 选择目标目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChooseDestPath_Click(object sender, RoutedEventArgs e)
        {
            //设置默认存储目录，初始值为加密文件所在目录
            if(txtSrcFile.Text != null && txtSrcFile.Text!= string.Empty){
                if (File.Exists(txtSrcFile.Text)){
                    destpathdialog.SelectedPath = System.IO.Path.GetDirectoryName(txtSrcFile.Text);
                }
            }

            destpathdialog.Description = "请选择存储目录";
            if (destpathdialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtDestPath.Text = destpathdialog.SelectedPath;
            }
        }

        /// <summary>
        /// 开始加密的页面跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            //加密文件检查
            if(!File.Exists(txtSrcFile.Text)){
                MessageBox.Show("加密文件不存在");
                return;
            }

            //输出目录检查
            if (!Directory.Exists(txtDestPath.Text)){
                MessageBox.Show("输出目录不存在");
                return;
            }

            //口令检查
            if(pwdBox.Password == string.Empty){
                MessageBox.Show("口令为空");
                return;
            }

            //跳转，设置传递到下一个页面的参数
            ParentWindow.encryptSrcFile = txtSrcFile.Text;
            ParentWindow.encryptDestPath = txtDestPath.Text;
            ParentWindow.encryptPassword = pwdBox.Password;
            Button obj = sender as Button;
            ParentWindow.NavigateEncrypt(obj.Tag.ToString());
        }
        #endregion
    }
}
