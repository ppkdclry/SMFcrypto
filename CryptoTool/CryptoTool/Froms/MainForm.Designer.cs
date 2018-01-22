using System;

namespace CryptoTool.Froms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.txtbox_sfile = new System.Windows.Forms.TextBox();
            this.btn_sfile = new System.Windows.Forms.Button();
            this.txtbox_pwd = new System.Windows.Forms.TextBox();
            this.lab_pwd = new System.Windows.Forms.Label();
            this.lab_sfile = new System.Windows.Forms.Label();
            this.btn_encrypt = new System.Windows.Forms.Button();
            this.btn_decrypt = new System.Windows.Forms.Button();
            this.ckbox_dp = new System.Windows.Forms.CheckBox();
            this.sourcefiledialog = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // txtbox_sfile
            // 
            this.txtbox_sfile.AllowDrop = true;
            this.txtbox_sfile.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtbox_sfile.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtbox_sfile.Location = new System.Drawing.Point(117, 52);
            this.txtbox_sfile.Name = "txtbox_sfile";
            this.txtbox_sfile.Size = new System.Drawing.Size(257, 23);
            this.txtbox_sfile.TabIndex = 0;
            this.txtbox_sfile.WordWrap = false;
            // 
            // btn_sfile
            // 
            this.btn_sfile.Font = new System.Drawing.Font("楷体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_sfile.Location = new System.Drawing.Point(400, 45);
            this.btn_sfile.Name = "btn_sfile";
            this.btn_sfile.Size = new System.Drawing.Size(90, 34);
            this.btn_sfile.TabIndex = 1;
            this.btn_sfile.Text = "浏览";
            this.btn_sfile.UseVisualStyleBackColor = true;
            this.btn_sfile.Click += new System.EventHandler(this.btn_sfile_Click);
            // 
            // txtbox_pwd
            // 
            this.txtbox_pwd.AllowDrop = true;
            this.txtbox_pwd.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtbox_pwd.Location = new System.Drawing.Point(117, 112);
            this.txtbox_pwd.Name = "txtbox_pwd";
            this.txtbox_pwd.PasswordChar = '*';
            this.txtbox_pwd.Size = new System.Drawing.Size(257, 23);
            this.txtbox_pwd.TabIndex = 2;
            // 
            // lab_pwd
            // 
            this.lab_pwd.AutoSize = true;
            this.lab_pwd.Font = new System.Drawing.Font("楷体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lab_pwd.Location = new System.Drawing.Point(26, 115);
            this.lab_pwd.Name = "lab_pwd";
            this.lab_pwd.Size = new System.Drawing.Size(85, 16);
            this.lab_pwd.TabIndex = 3;
            this.lab_pwd.Text = "加密口令:";
            // 
            // lab_sfile
            // 
            this.lab_sfile.AutoSize = true;
            this.lab_sfile.Font = new System.Drawing.Font("楷体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lab_sfile.Location = new System.Drawing.Point(26, 55);
            this.lab_sfile.Name = "lab_sfile";
            this.lab_sfile.Size = new System.Drawing.Size(85, 16);
            this.lab_sfile.TabIndex = 4;
            this.lab_sfile.Text = "目标文件:";
            // 
            // btn_encrypt
            // 
            this.btn_encrypt.Font = new System.Drawing.Font("楷体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_encrypt.Location = new System.Drawing.Point(117, 172);
            this.btn_encrypt.Name = "btn_encrypt";
            this.btn_encrypt.Size = new System.Drawing.Size(90, 34);
            this.btn_encrypt.TabIndex = 5;
            this.btn_encrypt.Text = "加密";
            this.btn_encrypt.UseVisualStyleBackColor = true;
            this.btn_encrypt.Click += new System.EventHandler(this.btn_crypt_Click);
            // 
            // btn_decrypt
            // 
            this.btn_decrypt.Font = new System.Drawing.Font("楷体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_decrypt.Location = new System.Drawing.Point(284, 172);
            this.btn_decrypt.Name = "btn_decrypt";
            this.btn_decrypt.Size = new System.Drawing.Size(90, 34);
            this.btn_decrypt.TabIndex = 6;
            this.btn_decrypt.Text = "解密";
            this.btn_decrypt.UseVisualStyleBackColor = true;
            this.btn_decrypt.Click += new System.EventHandler(this.btn_decrypt_Click);
            // 
            // ckbox_dp
            // 
            this.ckbox_dp.AutoSize = true;
            this.ckbox_dp.Location = new System.Drawing.Point(400, 118);
            this.ckbox_dp.Name = "ckbox_dp";
            this.ckbox_dp.Size = new System.Drawing.Size(48, 16);
            this.ckbox_dp.TabIndex = 7;
            this.ckbox_dp.Text = "显示";
            this.ckbox_dp.UseVisualStyleBackColor = true;
            this.ckbox_dp.CheckedChanged += new System.EventHandler(this.ckbox_dp_CheckedChanged);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 262);
            this.Controls.Add(this.ckbox_dp);
            this.Controls.Add(this.btn_decrypt);
            this.Controls.Add(this.btn_encrypt);
            this.Controls.Add(this.lab_sfile);
            this.Controls.Add(this.lab_pwd);
            this.Controls.Add(this.txtbox_pwd);
            this.Controls.Add(this.btn_sfile);
            this.Controls.Add(this.txtbox_sfile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtbox_sfile;
        private System.Windows.Forms.Button btn_sfile;
        private System.Windows.Forms.TextBox txtbox_pwd;
        private System.Windows.Forms.Label lab_pwd;
        private System.Windows.Forms.Label lab_sfile;
        private System.Windows.Forms.Button btn_encrypt;
        private System.Windows.Forms.Button btn_decrypt;
        private System.Windows.Forms.CheckBox ckbox_dp;
        private System.Windows.Forms.OpenFileDialog sourcefiledialog;
    }
}