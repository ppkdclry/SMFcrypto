using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CryptoTool.CryptoLib;

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

        /// <summary>
        /// 界面初始化，将传入参数作为目标文件目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 选择文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_sfile_Click(object sender, EventArgs e)
        {
            sourcefiledialog.Title = "请选择文件";
            sourcefiledialog.Filter = "所有文件(*.*)|*.*";
            if(sourcefiledialog.ShowDialog() == System.Windows.Forms.DialogResult.OK){
                txtbox_sfile.Text = sourcefiledialog.FileName;
            }
        }

        /// <summary>
        /// 检查文件是否存在，密码是否为空
        /// </summary>
        /// <returns></returns>
        private bool checkForms(bool isEncrypt)
        {
            if (!System.IO.File.Exists(txtbox_sfile.Text)) {
                MessageBox.Show("请选择目标文件");
                txtbox_sfile.Focus();
                return false;
            }

            if (txtbox_pwd.Text == string.Empty){
                if (isEncrypt){
                    MessageBox.Show("请填入加密口令");
                }
                else{
                    MessageBox.Show("请填入解密口令");
                }
                txtbox_pwd.Focus();
                return false;
            }

            return true;
        }

        private SMFCipher smfCipher { get; set; }
        private ProBarForms taskProgressBarForm = new ProBarForms();
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_crypt_Click(object sender, EventArgs e)
        {
            if (checkForms(true)){
                smfCipher = SMFCipher.GetInstance("lry", txtbox_sfile.Text);
                smfCipher.OnTaskStateChanged += OnChanged;
                smfCipher.DoWork(true);
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_decrypt_Click(object sender, EventArgs e)
        {
            if (checkForms(false)){
                if (smfCipher != null)
                {
                    smfCipher.Cancel();
                }
            }
        }

        void OnChanged(object sender,TaskStateChangedEventArgs e)
        {
            string temp = string.Empty;
            if(e.CryptState != CryptState.ComProc){
                temp = string.Format("{0} - {1}", e.CryptState, e.Description);
                this.Text = temp;
                taskProgressBarForm.pB_task.Value = 0;
                taskProgressBarForm.Hide();
            }
            else{
                temp = string.Format("{0} - {1},任务:{2}", e.CryptState, e.Description, e.CurrentNumber);
                this.Text = temp;
                taskProgressBarForm.Show();
                taskProgressBarForm.pB_task.Maximum = e.TotalNumber;
                taskProgressBarForm.pB_task.Value = e.CurrentNumber;
            }
        }
    }
}
