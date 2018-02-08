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

        private void btnFinish_Click(object sender, RoutedEventArgs e)
        {
            Button obj = sender as Button;
            ParentWindow.NavigateEncrypt(obj.Tag.ToString());
        }
    }
}
