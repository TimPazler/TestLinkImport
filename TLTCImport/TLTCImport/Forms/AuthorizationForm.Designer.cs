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
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            instruction = new System.Windows.Forms.ToolStripMenuItem();
            aboutItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            btnLogin = new System.Windows.Forms.Button();
            lbApiKey = new System.Windows.Forms.Label();
            tbApiDevKey = new System.Windows.Forms.TextBox();
            pictureBox = new System.Windows.Forms.PictureBox();
            pbLoading = new System.Windows.Forms.PictureBox();
            lblLoginResult = new System.Windows.Forms.Label();
            lblProductVersion = new System.Windows.Forms.Label();
            lblhelp = new System.Windows.Forms.Label();
            llHelp = new System.Windows.Forms.LinkLabel();
            tbUrlTestLink = new System.Windows.Forms.TextBox();
            lbUrlTestLink = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbLoading).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { instruction, aboutItem, toolStripMenuItem1, toolStripMenuItem2 });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new System.Drawing.Size(255, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // instruction
            // 
            instruction.Name = "instruction";
            instruction.Size = new System.Drawing.Size(116, 20);
            instruction.Text = "Где взять Api Key?";
            instruction.Click += instruction_Click;
            // 
            // aboutItem
            // 
            aboutItem.Name = "aboutItem";
            aboutItem.Size = new System.Drawing.Size(94, 20);
            aboutItem.Text = "О программе";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(12, 20);
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new System.Drawing.Size(12, 20);
            // 
            // btnLogin
            // 
            btnLogin.BackColor = System.Drawing.Color.Silver;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnLogin.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            btnLogin.ForeColor = System.Drawing.Color.Black;
            btnLogin.Location = new System.Drawing.Point(74, 271);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new System.Drawing.Size(105, 30);
            btnLogin.TabIndex = 1;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // lbApiKey
            // 
            lbApiKey.AutoSize = true;
            lbApiKey.BackColor = System.Drawing.Color.Transparent;
            lbApiKey.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lbApiKey.ForeColor = System.Drawing.Color.Black;
            lbApiKey.Location = new System.Drawing.Point(9, 205);
            lbApiKey.Name = "lbApiKey";
            lbApiKey.Size = new System.Drawing.Size(142, 21);
            lbApiKey.TabIndex = 2;
            lbApiKey.Text = "Укажите Api Key:";
            lbApiKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbApiDevKey
            // 
            tbApiDevKey.BackColor = System.Drawing.Color.WhiteSmoke;
            tbApiDevKey.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            tbApiDevKey.Location = new System.Drawing.Point(9, 229);
            tbApiDevKey.Name = "tbApiDevKey";
            tbApiDevKey.PasswordChar = '*';
            tbApiDevKey.Size = new System.Drawing.Size(230, 29);
            tbApiDevKey.TabIndex = 3;
            // 
            // pictureBox
            // 
            pictureBox.Image = Properties.Resources.TestlinkMini;
            pictureBox.Location = new System.Drawing.Point(28, 38);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new System.Drawing.Size(200, 65);
            pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pictureBox.TabIndex = 4;
            pictureBox.TabStop = false;
            // 
            // pbLoading
            // 
            pbLoading.Image = (System.Drawing.Image)resources.GetObject("pbLoading.Image");
            pbLoading.Location = new System.Drawing.Point(115, 307);
            pbLoading.Name = "pbLoading";
            pbLoading.Size = new System.Drawing.Size(25, 25);
            pbLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pbLoading.TabIndex = 5;
            pbLoading.TabStop = false;
            pbLoading.Visible = false;
            // 
            // lblLoginResult
            // 
            lblLoginResult.AutoSize = true;
            lblLoginResult.BackColor = System.Drawing.Color.Transparent;
            lblLoginResult.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lblLoginResult.ForeColor = System.Drawing.Color.Black;
            lblLoginResult.Location = new System.Drawing.Point(13, 313);
            lblLoginResult.Name = "lblLoginResult";
            lblLoginResult.Size = new System.Drawing.Size(226, 19);
            lblLoginResult.TabIndex = 6;
            lblLoginResult.Text = "Данные указаны не правильно";
            lblLoginResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lblLoginResult.Visible = false;
            // 
            // lblProductVersion
            // 
            lblProductVersion.AutoSize = true;
            lblProductVersion.BackColor = System.Drawing.Color.Transparent;
            lblProductVersion.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lblProductVersion.ForeColor = System.Drawing.Color.Gray;
            lblProductVersion.Location = new System.Drawing.Point(212, 345);
            lblProductVersion.Name = "lblProductVersion";
            lblProductVersion.Size = new System.Drawing.Size(33, 17);
            lblProductVersion.TabIndex = 7;
            lblProductVersion.Text = "v0.5";
            lblProductVersion.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // lblhelp
            // 
            lblhelp.AutoSize = true;
            lblhelp.BackColor = System.Drawing.Color.Transparent;
            lblhelp.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lblhelp.ForeColor = System.Drawing.Color.Black;
            lblhelp.Location = new System.Drawing.Point(0, 345);
            lblhelp.Name = "lblhelp";
            lblhelp.Size = new System.Drawing.Size(80, 17);
            lblhelp.TabIndex = 8;
            lblhelp.Text = "Проблема?";
            lblhelp.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // llHelp
            // 
            llHelp.AutoSize = true;
            llHelp.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            llHelp.Location = new System.Drawing.Point(76, 345);
            llHelp.Name = "llHelp";
            llHelp.Size = new System.Drawing.Size(95, 17);
            llHelp.TabIndex = 9;
            llHelp.TabStop = true;
            llHelp.Text = "Пиши, решим!";
            llHelp.LinkClicked += llHelp_LinkClicked;
            // 
            // tbUrlTestLink
            // 
            tbUrlTestLink.BackColor = System.Drawing.Color.WhiteSmoke;
            tbUrlTestLink.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            tbUrlTestLink.Location = new System.Drawing.Point(9, 143);
            tbUrlTestLink.Name = "tbUrlTestLink";
            tbUrlTestLink.Size = new System.Drawing.Size(230, 29);
            tbUrlTestLink.TabIndex = 11;
            // 
            // lbUrlTestLink
            // 
            lbUrlTestLink.AutoSize = true;
            lbUrlTestLink.BackColor = System.Drawing.Color.Transparent;
            lbUrlTestLink.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lbUrlTestLink.ForeColor = System.Drawing.Color.Black;
            lbUrlTestLink.Location = new System.Drawing.Point(9, 119);
            lbUrlTestLink.Name = "lbUrlTestLink";
            lbUrlTestLink.Size = new System.Drawing.Size(230, 21);
            lbUrlTestLink.TabIndex = 10;
            lbUrlTestLink.Text = "Укажите URL адрес TestLink:";
            lbUrlTestLink.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = System.Drawing.Color.Transparent;
            label1.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label1.ForeColor = System.Drawing.Color.DarkGray;
            label1.Location = new System.Drawing.Point(9, 175);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(212, 17);
            label1.TabIndex = 12;
            label1.Text = "Например: http://testms.name.ru/";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AuthorizationForm
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(255, 364);
            Controls.Add(label1);
            Controls.Add(tbUrlTestLink);
            Controls.Add(lbUrlTestLink);
            Controls.Add(llHelp);
            Controls.Add(lblhelp);
            Controls.Add(lblProductVersion);
            Controls.Add(lblLoginResult);
            Controls.Add(pbLoading);
            Controls.Add(pictureBox);
            Controls.Add(tbApiDevKey);
            Controls.Add(lbApiKey);
            Controls.Add(btnLogin);
            Controls.Add(menuStrip1);
            ForeColor = System.Drawing.Color.White;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MainMenuStrip = menuStrip1;
            Name = "AuthorizationForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Авторизация в TestLink";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbLoading).EndInit();
            ResumeLayout(false);
            PerformLayout();
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
