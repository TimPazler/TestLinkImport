using System;
using System.Collections.Generic;
using System.Text;

namespace TLTCImport
{
    partial class AuthorizationForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AuthorizationForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.instruction = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lbApiKey = new System.Windows.Forms.Label();
            this.tbApiDevKey = new System.Windows.Forms.TextBox();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.pbLoading = new System.Windows.Forms.PictureBox();
            this.lblLoginResult = new System.Windows.Forms.Label();
            this.lblProductVersion = new System.Windows.Forms.Label();
            this.lblhelp = new System.Windows.Forms.Label();
            this.llHelp = new System.Windows.Forms.LinkLabel();
            this.tbUrlTestLink = new System.Windows.Forms.TextBox();
            this.lbUrlTestLink = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLoading)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.instruction,
            this.aboutItem,
            this.toolStripMenuItem1,
            this.toolStripMenuItem2});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(255, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // instruction
            // 
            this.instruction.Name = "instruction";
            this.instruction.Size = new System.Drawing.Size(116, 20);
            this.instruction.Text = "Где взять Api Key?";
            this.instruction.Click += new System.EventHandler(this.instruction_Click);
            // 
            // aboutItem
            // 
            this.aboutItem.Name = "aboutItem";
            this.aboutItem.Size = new System.Drawing.Size(94, 20);
            this.aboutItem.Text = "О программе";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(12, 20);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(12, 20);
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.Silver;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnLogin.ForeColor = System.Drawing.Color.Black;
            this.btnLogin.Location = new System.Drawing.Point(74, 271);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(105, 30);
            this.btnLogin.TabIndex = 1;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // lbApiKey
            // 
            this.lbApiKey.AutoSize = true;
            this.lbApiKey.BackColor = System.Drawing.Color.Transparent;
            this.lbApiKey.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lbApiKey.ForeColor = System.Drawing.Color.Black;
            this.lbApiKey.Location = new System.Drawing.Point(9, 205);
            this.lbApiKey.Name = "lbApiKey";
            this.lbApiKey.Size = new System.Drawing.Size(142, 21);
            this.lbApiKey.TabIndex = 2;
            this.lbApiKey.Text = "Укажите Api Key:";
            this.lbApiKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbApiDevKey
            // 
            this.tbApiDevKey.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tbApiDevKey.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tbApiDevKey.Location = new System.Drawing.Point(9, 229);
            this.tbApiDevKey.Name = "tbApiDevKey";
            this.tbApiDevKey.PasswordChar = '*';
            this.tbApiDevKey.Size = new System.Drawing.Size(230, 29);
            this.tbApiDevKey.TabIndex = 3;
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::TLTCImport.Properties.Resources.TestlinkMini;
            this.pictureBox.Location = new System.Drawing.Point(28, 38);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(200, 65);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 4;
            this.pictureBox.TabStop = false;
            // 
            // pbLoading
            // 
            this.pbLoading.Image = ((System.Drawing.Image)(resources.GetObject("pbLoading.Image")));
            this.pbLoading.Location = new System.Drawing.Point(115, 307);
            this.pbLoading.Name = "pbLoading";
            this.pbLoading.Size = new System.Drawing.Size(25, 25);
            this.pbLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbLoading.TabIndex = 5;
            this.pbLoading.TabStop = false;
            this.pbLoading.Visible = false;
            // 
            // lblLoginResult
            // 
            this.lblLoginResult.AutoSize = true;
            this.lblLoginResult.BackColor = System.Drawing.Color.Transparent;
            this.lblLoginResult.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblLoginResult.ForeColor = System.Drawing.Color.Black;
            this.lblLoginResult.Location = new System.Drawing.Point(13, 313);
            this.lblLoginResult.Name = "lblLoginResult";
            this.lblLoginResult.Size = new System.Drawing.Size(226, 19);
            this.lblLoginResult.TabIndex = 6;
            this.lblLoginResult.Text = "Данные указаны не правильно";
            this.lblLoginResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblLoginResult.Visible = false;
            // 
            // lblProductVersion
            // 
            this.lblProductVersion.AutoSize = true;
            this.lblProductVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblProductVersion.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblProductVersion.ForeColor = System.Drawing.Color.Gray;
            this.lblProductVersion.Location = new System.Drawing.Point(222, 345);
            this.lblProductVersion.Name = "lblProductVersion";
            this.lblProductVersion.Size = new System.Drawing.Size(33, 17);
            this.lblProductVersion.TabIndex = 7;
            this.lblProductVersion.Text = "v0.4";
            this.lblProductVersion.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // lblhelp
            // 
            this.lblhelp.AutoSize = true;
            this.lblhelp.BackColor = System.Drawing.Color.Transparent;
            this.lblhelp.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblhelp.ForeColor = System.Drawing.Color.Black;
            this.lblhelp.Location = new System.Drawing.Point(0, 345);
            this.lblhelp.Name = "lblhelp";
            this.lblhelp.Size = new System.Drawing.Size(80, 17);
            this.lblhelp.TabIndex = 8;
            this.lblhelp.Text = "Проблема?";
            this.lblhelp.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // llHelp
            // 
            this.llHelp.AutoSize = true;
            this.llHelp.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.llHelp.Location = new System.Drawing.Point(76, 345);
            this.llHelp.Name = "llHelp";
            this.llHelp.Size = new System.Drawing.Size(95, 17);
            this.llHelp.TabIndex = 9;
            this.llHelp.TabStop = true;
            this.llHelp.Text = "Пиши, решим!";
            this.llHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llHelp_LinkClicked);
            // 
            // tbUrlTestLink
            // 
            this.tbUrlTestLink.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tbUrlTestLink.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tbUrlTestLink.Location = new System.Drawing.Point(9, 143);
            this.tbUrlTestLink.Name = "tbUrlTestLink";
            this.tbUrlTestLink.Size = new System.Drawing.Size(230, 29);
            this.tbUrlTestLink.TabIndex = 11;
            // 
            // lbUrlTestLink
            // 
            this.lbUrlTestLink.AutoSize = true;
            this.lbUrlTestLink.BackColor = System.Drawing.Color.Transparent;
            this.lbUrlTestLink.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lbUrlTestLink.ForeColor = System.Drawing.Color.Black;
            this.lbUrlTestLink.Location = new System.Drawing.Point(9, 119);
            this.lbUrlTestLink.Name = "lbUrlTestLink";
            this.lbUrlTestLink.Size = new System.Drawing.Size(230, 21);
            this.lbUrlTestLink.TabIndex = 10;
            this.lbUrlTestLink.Text = "Укажите URL адрес TestLink:";
            this.lbUrlTestLink.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.DarkGray;
            this.label1.Location = new System.Drawing.Point(9, 175);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(212, 17);
            this.label1.TabIndex = 12;
            this.label1.Text = "Например: http://testms.name.ru/";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AuthorizationForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(255, 364);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbUrlTestLink);
            this.Controls.Add(this.lbUrlTestLink);
            this.Controls.Add(this.llHelp);
            this.Controls.Add(this.lblhelp);
            this.Controls.Add(this.lblProductVersion);
            this.Controls.Add(this.lblLoginResult);
            this.Controls.Add(this.pbLoading);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.tbApiDevKey);
            this.Controls.Add(this.lbApiKey);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.menuStrip1);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "AuthorizationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Авторизация в TestLink";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLoading)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblLoginResult;
        private System.Windows.Forms.TextBox tbApiDevKey;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.PictureBox pbLoading;
        private System.Windows.Forms.Label lbApiKey;
        private System.Windows.Forms.Label lblhelp;
        private System.Windows.Forms.Label lblProductVersion;
        private System.Windows.Forms.LinkLabel llHelp;
        private System.Windows.Forms.ToolStripMenuItem instruction;
        private System.Windows.Forms.ToolStripMenuItem aboutItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.TextBox tbUrlTestLink;
        private System.Windows.Forms.Label lbUrlTestLink;
        private System.Windows.Forms.Label label1;
    }
}
