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

namespace EncryptTool.Pages.DecryptPage
{
    /// <summary>
    /// DecryptFinishPage.xaml 的交互逻辑
    /// </summary>
    public partial class DecryptFinishPage : BasePage
    {
        public DecryptFinishPage()
        {
            InitializeComponent();
        }

        private void btnFinish_Click(object sender, RoutedEventArgs e)
        {
            Button obj = sender as Button;
            ParentWindow.NavigateDecrypt(obj.Tag.ToString());
        }
    }
}
