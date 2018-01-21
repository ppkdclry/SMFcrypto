namespace CryptoTool.Froms
{
    partial class ProBarForms
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProBarForms));
            this.pB_task = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // pB_task
            // 
            this.pB_task.BackColor = System.Drawing.SystemColors.Desktop;
            this.pB_task.Location = new System.Drawing.Point(10, 10);
            this.pB_task.Margin = new System.Windows.Forms.Padding(0);
            this.pB_task.Name = "pB_task";
            this.pB_task.Size = new System.Drawing.Size(400, 25);
            this.pB_task.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pB_task.TabIndex = 0;
            // 
            // ProBarForms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(420, 45);
            this.ControlBox = false;
            this.Controls.Add(this.pB_task);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProBarForms";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ProBarForms";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.SystemColors.Control;
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ProgressBar pB_task;
    }
}