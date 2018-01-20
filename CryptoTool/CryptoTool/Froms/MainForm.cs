using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CryptoTool.Froms
{
    public partial class MainForm : Form
    {
        private string[] args = null;

        public MainForm()
        {
            InitializeComponent();
        }

        public MainForm(string[] args)
        {
            this.args = args;
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //拖曳开启的文件不能超过1个
            if(null != args){
                if(args.Length > 0){
                    txtbox_sfile.Text = args[0];
                    if(args.Length > 1){
                        MessageBox.Show(string.Format("共有{0}个文件，将选择第一个", args.Length));
                    }
                }
            }
        }

        /// <summary>
        /// 拖放进入事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_DragEnter(object sender,DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)){
                e.Effect = DragDropEffects.Link;
            }else{
                e.Effect = DragDropEffects.None;
            }
        }

        /// <summary>
        /// 拖放放下时事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            Array arr = (System.Array)e.Data.GetData(DataFormats.FileDrop);
            if (arr != null && arr.Length > 0){
                //拖放多个文件时，选择第一个
                if (arr.Length > 1){
                    MessageBox.Show(string.Format("共有{0}个文件，选择第一个", arr.Length));
                }
                string str = arr.GetValue(0).ToString();
                if (System.IO.File.Exists(str)){
                    txtbox_sfile.Text = str;
                }else{
                    MessageBox.Show(string.Format("\"{0}\"非文件", str));
                }
            }
        }
        
        /// <summary>
        /// 显示口令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckbox_dp_CheckedChanged(object sender, EventArgs e)
        {
            if(ckbox_dp.CheckState == CheckState.Checked){
                txtbox_pwd.PasswordChar = '\0';
            }else if(ckbox_dp.CheckState == CheckState.Unchecked) {
                txtbox_pwd.PasswordChar = '*';
            }
        }

        
    }
}
