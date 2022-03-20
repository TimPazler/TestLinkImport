namespace TLTCImport
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.foldersIcons = new System.Windows.Forms.ImageList(this.components);
            this.MainFormMenu = new System.Windows.Forms.MenuStrip();
            this.MainFormStatus = new System.Windows.Forms.StatusStrip();
            this.SuspendLayout();
            // 
            // foldersIcons
            // 
            this.foldersIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.foldersIcons.ImageSize = new System.Drawing.Size(16, 16);
            this.foldersIcons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // MainFormMenu
            // 
            this.MainFormMenu.Location = new System.Drawing.Point(0, 0);
            this.MainFormMenu.Name = "MainFormMenu";
            this.MainFormMenu.Size = new System.Drawing.Size(800, 24);
            this.MainFormMenu.TabIndex = 0;
            this.MainFormMenu.Text = "menuStrip1";
            // 
            // MainFormStatus
            // 
            this.MainFormStatus.Location = new System.Drawing.Point(0, 608);
            this.MainFormStatus.Name = "MainFormStatus";
            this.MainFormStatus.Size = new System.Drawing.Size(800, 22);
            this.MainFormStatus.TabIndex = 1;
            this.MainFormStatus.Text = "statusStrip1";
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 630);
            this.Controls.Add(this.MainFormStatus);
            this.Controls.Add(this.MainFormMenu);
            this.ForeColor = System.Drawing.Color.White;
            this.MainMenuStrip = this.MainFormMenu;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " ";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList foldersIcons;
        private System.Windows.Forms.MenuStrip MainFormMenu;
        private System.Windows.Forms.StatusStrip MainFormStatus;
    }
}

