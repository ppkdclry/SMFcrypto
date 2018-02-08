using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EncryptTool.Pages
{
    public class BasePage : Page
    {
        #region 父窗体
        private MainWindow _parentWin;
        public MainWindow ParentWindow
        {
            get { return _parentWin; }
            set { _parentWin = value; }
        }
        #endregion
    }
}
