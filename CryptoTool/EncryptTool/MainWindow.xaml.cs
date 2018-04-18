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
using System.IO;
using EncryptTool.Pages.EncryptPage;
using EncryptTool.Pages.DecryptPage;
using System.Windows.Forms;

namespace EncryptTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 主界面初始化
        private string inputFile = null;
        private string defaultDestPath = null;
        private bool isEncryptFile = true;

        /// <summary>
        /// 用于在各个页面传递加解密文件数据（虽然不安全，但在单机加密工具中可行）
        /// </summary>
        public string encryptSrcFile = null;
        public string encryptDestPath = null;
        public string encryptPassword = null;
        public bool isEncryptOK = false;
        public string encryptResult = null;
        public string decryptSrcFile = null;
        public string decryptDestPath = null;
        public string decryptPassword = null;
        public bool isDecryptOK = false;
        public string decryptResult = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(string[] args)
        {
            InitializeComponent();
            //解析输入参数，并将输入参数中的"/d"和"/e"删去，若存在"/d"则将启示页面设置为解密
            ParseParameter(args);
        }

        private void ParseParameter(string[] args)
        {
            for(int i = 0;i < args.Length; i++)
            {
                if (args[i].Equals("/d")){
                    isEncryptFile = false;
                }else if (args[i].Equals("/e")){
                    isEncryptFile = true;
                }else if (File.Exists(args[i])){//当非"/e"或者"/d"时，判断该文件是否存在，如果存在设为源文件
                    inputFile = args[i];
                    //设置待输出目录
                    try{
                        defaultDestPath = System.IO.Path.GetDirectoryName(inputFile);
                    }
                    catch (Exception){
                        defaultDestPath = null;
                    }
                }
            }
        }

        /// <summary>
        /// 初始化设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //初始化时将加密、解密页面设置为StartPage
            NavigateEncrypt("EncryptStartPage");
            NavigateDecrypt("DecryptStartPage");
            if (isEncryptFile){
                TabPages.SelectedIndex = 0;
                encryptSrcFile = inputFile;
                encryptDestPath = defaultDestPath;
            }else{
                TabPages.SelectedIndex = 1;
                decryptSrcFile = inputFile;
                decryptDestPath = defaultDestPath;
            }
        }
        #endregion

        #region 窗口功能键

        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
