using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace EncryptTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(string[] args)
        {
            InitializeComponent();
        }

        #region 窗口功能键
        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("最小化");
        }
        #endregion

        #region 窗口行为
        /// <summary>
        /// 窗口拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Move(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //初始化时将加密、解密页面设置为StartPage
            NavigateEncrypt("EncryptStartPage");
            NavigateDecrypt("DecryptStartPage");
        }
        #endregion

        #region 加密页面跳转
        /// <summary>
        /// 通过传入字符串设置frmEncrypt框架的页面
        /// </summary>
        /// <param name="path"></param>
        public void NavigateEncrypt(string path)
        {
            string uri = "EncryptTool.Pages.EncryptPage." + path;
            Type type = Type.GetType(uri);
            if (type != null)
            {
                //反射实例化Page页
                object obj  = type.Assembly.CreateInstance(uri);
                Page control = obj as Page;
                this.frmEncrypt.Content = control;
                PropertyInfo[] infos = type.GetProperties();
                foreach(PropertyInfo info in infos)
                {
                    //将MainWindow设为Page页面的ParentWin
                    if(info.Name == "ParentWindow")
                    {
                        info.SetValue(control, this, null);
                        break;
                    }
                }
            }
        }
        #endregion

        #region 解密页面跳转
        /// <summary>
        /// 通过传入字符串设置frmEncrypt框架的页面
        /// </summary>
        /// <param name="path"></param>
        public void NavigateDecrypt(string path)
        {
            string uri = "EncryptTool.Pages.DecryptPage." + path;
            Type type = Type.GetType(uri);
            if (type != null)
            {
                //反射实例化Page页
                object obj = type.Assembly.CreateInstance(uri);
                Page control = obj as Page;
                this.frmDecrypt.Content = control;
                PropertyInfo[] infos = type.GetProperties();
                foreach (PropertyInfo info in infos)
                {
                    //将MainWindow设为Page页面的ParentWin
                    if (info.Name == "ParentWindow")
                    {
                        info.SetValue(control, this, null);
                        break;
                    }
                }
            }
        }
        #endregion


    }
}
