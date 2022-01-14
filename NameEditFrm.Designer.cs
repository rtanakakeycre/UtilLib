namespace McDbg_Ver2
{
    partial class NameEditFrm
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
            this.m_sTbName1 = new System.Windows.Forms.TextBox();
            this.m_sBtnOk = new System.Windows.Forms.Button();
            this.m_sBtnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_sTbName1
            // 
            this.m_sTbName1.Location = new System.Drawing.Point(12, 12);
            this.m_sTbName1.Name = "m_sTbName1";
            this.m_sTbName1.Size = new System.Drawing.Size(331, 19);
            this.m_sTbName1.TabIndex = 0;
            // 
            // m_sBtnOk
            // 
            this.m_sBtnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.m_sBtnOk.Location = new System.Drawing.Point(268, 37);
            this.m_sBtnOk.Name = "m_sBtnOk";
            this.m_sBtnOk.Size = new System.Drawing.Size(75, 23);
            this.m_sBtnOk.TabIndex = 1;
            this.m_sBtnOk.Text = "OK";
            this.m_sBtnOk.UseVisualStyleBackColor = true;
            // 
            // m_sBtnCancel
            // 
            this.m_sBtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_sBtnCancel.Location = new System.Drawing.Point(187, 37);
            this.m_sBtnCancel.Name = "m_sBtnCancel";
            this.m_sBtnCancel.Size = new System.Drawing.Size(75, 23);
            this.m_sBtnCancel.TabIndex = 2;
            this.m_sBtnCancel.Text = "Cancel";
            this.m_sBtnCancel.UseVisualStyleBackColor = true;
            // 
            // NameEditFrm
            // 
            this.AcceptButton = this.m_sBtnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.m_sBtnCancel;
            this.ClientSize = new System.Drawing.Size(348, 64);
            this.Controls.Add(this.m_sBtnCancel);
            this.Controls.Add(this.m_sBtnOk);
            this.Controls.Add(this.m_sTbName1);
            this.Name = "NameEditFrm";
            this.Text = "名称変更";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox m_sTbName1;
        private System.Windows.Forms.Button m_sBtnOk;
        private System.Windows.Forms.Button m_sBtnCancel;

    }
}