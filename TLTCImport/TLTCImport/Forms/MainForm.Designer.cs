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
            this.referenceItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainFormStatus = new System.Windows.Forms.StatusStrip();
            this.leftPanel = new System.Windows.Forms.Panel();
            this.lblDescriptionTests = new System.Windows.Forms.Label();
            this.btnShowAllTests = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cbBlocked = new System.Windows.Forms.CheckBox();
            this.cbFailed = new System.Windows.Forms.CheckBox();
            this.cbPassed = new System.Windows.Forms.CheckBox();
            this.lblTestRun = new System.Windows.Forms.Label();
            this.pbForlblTestRun = new System.Windows.Forms.PictureBox();
            this.btnManualMode = new System.Windows.Forms.Button();
            this.btnAutoMode = new System.Windows.Forms.Button();
            this.lblSoftware = new System.Windows.Forms.Label();
            this.cbProjectNames = new System.Windows.Forms.ComboBox();
            this.cbTestPlanName = new System.Windows.Forms.ComboBox();
            this.topPanel = new System.Windows.Forms.Panel();
            this.lblCurrentTestPlan = new System.Windows.Forms.Label();
            this.btnCollapseTree = new System.Windows.Forms.Button();
            this.pbForlblCurrentTestPlan = new System.Windows.Forms.PictureBox();
            this.btnExpandTree = new System.Windows.Forms.Button();
            this.pbForlblSoftware = new System.Windows.Forms.PictureBox();
            this.treeView = new TLTCImport.UcTreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCaseTransfer = new System.Windows.Forms.Button();
            this.MainFormMenu.SuspendLayout();
            this.leftPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbForlblTestRun)).BeginInit();
            this.topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbForlblCurrentTestPlan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbForlblSoftware)).BeginInit();
            this.panel1.SuspendLayout();
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
            this.MainFormMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.referenceItem,
            this.aboutItem});
            this.MainFormMenu.Location = new System.Drawing.Point(0, 0);
            this.MainFormMenu.Name = "MainFormMenu";
            this.MainFormMenu.Size = new System.Drawing.Size(1148, 24);
            this.MainFormMenu.TabIndex = 0;
            this.MainFormMenu.Text = "menuStrip1";
            // 
            // referenceItem
            // 
            this.referenceItem.Name = "referenceItem";
            this.referenceItem.Size = new System.Drawing.Size(65, 20);
            this.referenceItem.Text = "Справка";
            // 
            // aboutItem
            // 
            this.aboutItem.Name = "aboutItem";
            this.aboutItem.Size = new System.Drawing.Size(94, 20);
            this.aboutItem.Text = "О программе";
            // 
            // MainFormStatus
            // 
            this.MainFormStatus.Location = new System.Drawing.Point(0, 640);
            this.MainFormStatus.Name = "MainFormStatus";
            this.MainFormStatus.Size = new System.Drawing.Size(1148, 22);
            this.MainFormStatus.TabIndex = 1;
            this.MainFormStatus.Text = "statusStrip1";
            // 
            // leftPanel
            // 
            this.leftPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.leftPanel.BackColor = System.Drawing.Color.Silver;
            this.leftPanel.Controls.Add(this.lblDescriptionTests);
            this.leftPanel.Controls.Add(this.btnShowAllTests);
            this.leftPanel.Controls.Add(this.pictureBox1);
            this.leftPanel.Controls.Add(this.cbBlocked);
            this.leftPanel.Controls.Add(this.cbFailed);
            this.leftPanel.Controls.Add(this.cbPassed);
            this.leftPanel.Controls.Add(this.lblTestRun);
            this.leftPanel.Controls.Add(this.pbForlblTestRun);
            this.leftPanel.Controls.Add(this.btnManualMode);
            this.leftPanel.Controls.Add(this.btnAutoMode);
            this.leftPanel.Location = new System.Drawing.Point(0, 24);
            this.leftPanel.Name = "leftPanel";
            this.leftPanel.Size = new System.Drawing.Size(210, 1357);
            this.leftPanel.TabIndex = 2;
            // 
            // lblDescriptionTests
            // 
            this.lblDescriptionTests.AutoSize = true;
            this.lblDescriptionTests.BackColor = System.Drawing.Color.Gray;
            this.lblDescriptionTests.ForeColor = System.Drawing.Color.White;
            this.lblDescriptionTests.Location = new System.Drawing.Point(5, 286);
            this.lblDescriptionTests.Name = "lblDescriptionTests";
            this.lblDescriptionTests.Size = new System.Drawing.Size(103, 15);
            this.lblDescriptionTests.TabIndex = 7;
            this.lblDescriptionTests.Text = "Описание тестов:";
            this.lblDescriptionTests.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnShowAllTests
            // 
            this.btnShowAllTests.Enabled = false;
            this.btnShowAllTests.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowAllTests.Location = new System.Drawing.Point(3, 316);
            this.btnShowAllTests.Name = "btnShowAllTests";
            this.btnShowAllTests.Size = new System.Drawing.Size(204, 30);
            this.btnShowAllTests.TabIndex = 7;
            this.btnShowAllTests.Text = "Отобразить все тесты продукта";
            this.btnShowAllTests.UseVisualStyleBackColor = true;
            this.btnShowAllTests.Click += new System.EventHandler(this.btnShowAllTests_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Gray;
            this.pictureBox1.Location = new System.Drawing.Point(3, 280);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(204, 30);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // cbBlocked
            // 
            this.cbBlocked.AutoSize = true;
            this.cbBlocked.Checked = true;
            this.cbBlocked.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbBlocked.Enabled = false;
            this.cbBlocked.Location = new System.Drawing.Point(13, 215);
            this.cbBlocked.Name = "cbBlocked";
            this.cbBlocked.Size = new System.Drawing.Size(93, 19);
            this.cbBlocked.TabIndex = 8;
            this.cbBlocked.Text = "Блокирован";
            this.cbBlocked.UseVisualStyleBackColor = true;
            // 
            // cbFailed
            // 
            this.cbFailed.AutoSize = true;
            this.cbFailed.Checked = true;
            this.cbFailed.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbFailed.Enabled = false;
            this.cbFailed.Location = new System.Drawing.Point(101, 189);
            this.cbFailed.Name = "cbFailed";
            this.cbFailed.Size = new System.Drawing.Size(81, 19);
            this.cbFailed.TabIndex = 7;
            this.cbFailed.Text = "Провален";
            this.cbFailed.UseVisualStyleBackColor = true;
            // 
            // cbPassed
            // 
            this.cbPassed.AutoSize = true;
            this.cbPassed.Checked = true;
            this.cbPassed.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbPassed.Enabled = false;
            this.cbPassed.ForeColor = System.Drawing.Color.Transparent;
            this.cbPassed.Location = new System.Drawing.Point(13, 190);
            this.cbPassed.Name = "cbPassed";
            this.cbPassed.Size = new System.Drawing.Size(75, 19);
            this.cbPassed.TabIndex = 6;
            this.cbPassed.Text = "Пройден";
            this.cbPassed.UseVisualStyleBackColor = true;
            // 
            // lblTestRun
            // 
            this.lblTestRun.AutoSize = true;
            this.lblTestRun.BackColor = System.Drawing.Color.Gray;
            this.lblTestRun.ForeColor = System.Drawing.Color.White;
            this.lblTestRun.Location = new System.Drawing.Point(5, 123);
            this.lblTestRun.Name = "lblTestRun";
            this.lblTestRun.Size = new System.Drawing.Size(90, 15);
            this.lblTestRun.TabIndex = 0;
            this.lblTestRun.Text = "Прогон тестов:";
            this.lblTestRun.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbForlblTestRun
            // 
            this.pbForlblTestRun.BackColor = System.Drawing.Color.Gray;
            this.pbForlblTestRun.Location = new System.Drawing.Point(3, 117);
            this.pbForlblTestRun.Name = "pbForlblTestRun";
            this.pbForlblTestRun.Size = new System.Drawing.Size(204, 30);
            this.pbForlblTestRun.TabIndex = 4;
            this.pbForlblTestRun.TabStop = false;
            // 
            // btnManualMode
            // 
            this.btnManualMode.Enabled = false;
            this.btnManualMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManualMode.Location = new System.Drawing.Point(3, 240);
            this.btnManualMode.Name = "btnManualMode";
            this.btnManualMode.Size = new System.Drawing.Size(204, 30);
            this.btnManualMode.TabIndex = 5;
            this.btnManualMode.Text = "Ручной режим";
            this.btnManualMode.UseVisualStyleBackColor = true;
            this.btnManualMode.Click += new System.EventHandler(this.btnManualMode_Click);
            // 
            // btnAutoMode
            // 
            this.btnAutoMode.Enabled = false;
            this.btnAutoMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAutoMode.Location = new System.Drawing.Point(3, 153);
            this.btnAutoMode.Name = "btnAutoMode";
            this.btnAutoMode.Size = new System.Drawing.Size(203, 30);
            this.btnAutoMode.TabIndex = 4;
            this.btnAutoMode.Text = "Авто режим";
            this.btnAutoMode.UseVisualStyleBackColor = true;
            this.btnAutoMode.Click += new System.EventHandler(this.btnAutoMode_Click);
            // 
            // lblSoftware
            // 
            this.lblSoftware.AutoSize = true;
            this.lblSoftware.BackColor = System.Drawing.Color.Gray;
            this.lblSoftware.ForeColor = System.Drawing.Color.White;
            this.lblSoftware.Location = new System.Drawing.Point(5, 10);
            this.lblSoftware.Name = "lblSoftware";
            this.lblSoftware.Size = new System.Drawing.Size(139, 15);
            this.lblSoftware.TabIndex = 0;
            this.lblSoftware.Text = "Программный продукт:";
            this.lblSoftware.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbProjectNames
            // 
            this.cbProjectNames.ForeColor = System.Drawing.Color.Black;
            this.cbProjectNames.FormattingEnabled = true;
            this.cbProjectNames.Location = new System.Drawing.Point(3, 34);
            this.cbProjectNames.Name = "cbProjectNames";
            this.cbProjectNames.Size = new System.Drawing.Size(204, 23);
            this.cbProjectNames.TabIndex = 2;
            this.cbProjectNames.Text = "Выбирите продукт";
            this.cbProjectNames.SelectedIndexChanged += new System.EventHandler(this.cbProjectNames_SelectedIndexChanged);
            // 
            // cbTestPlanName
            // 
            this.cbTestPlanName.Enabled = false;
            this.cbTestPlanName.FormattingEnabled = true;
            this.cbTestPlanName.Location = new System.Drawing.Point(3, 89);
            this.cbTestPlanName.Name = "cbTestPlanName";
            this.cbTestPlanName.Size = new System.Drawing.Size(204, 23);
            this.cbTestPlanName.TabIndex = 3;
            this.cbTestPlanName.SelectedIndexChanged += new System.EventHandler(this.cbTestPlanName_SelectedIndexChanged);
            // 
            // topPanel
            // 
            this.topPanel.BackColor = System.Drawing.Color.Silver;
            this.topPanel.Controls.Add(this.lblSoftware);
            this.topPanel.Controls.Add(this.lblCurrentTestPlan);
            this.topPanel.Controls.Add(this.btnCollapseTree);
            this.topPanel.Controls.Add(this.pbForlblCurrentTestPlan);
            this.topPanel.Controls.Add(this.btnExpandTree);
            this.topPanel.Controls.Add(this.pbForlblSoftware);
            this.topPanel.Controls.Add(this.cbTestPlanName);
            this.topPanel.Controls.Add(this.cbProjectNames);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 24);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(1148, 115);
            this.topPanel.TabIndex = 3;
            // 
            // lblCurrentTestPlan
            // 
            this.lblCurrentTestPlan.AutoSize = true;
            this.lblCurrentTestPlan.BackColor = System.Drawing.Color.Gray;
            this.lblCurrentTestPlan.ForeColor = System.Drawing.Color.White;
            this.lblCurrentTestPlan.Location = new System.Drawing.Point(5, 65);
            this.lblCurrentTestPlan.Name = "lblCurrentTestPlan";
            this.lblCurrentTestPlan.Size = new System.Drawing.Size(117, 15);
            this.lblCurrentTestPlan.TabIndex = 0;
            this.lblCurrentTestPlan.Text = "Текущий тест-план:";
            this.lblCurrentTestPlan.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCollapseTree
            // 
            this.btnCollapseTree.Enabled = false;
            this.btnCollapseTree.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCollapseTree.Location = new System.Drawing.Point(340, 82);
            this.btnCollapseTree.Name = "btnCollapseTree";
            this.btnCollapseTree.Size = new System.Drawing.Size(120, 30);
            this.btnCollapseTree.TabIndex = 6;
            this.btnCollapseTree.Text = "Закрыть дерево";
            this.btnCollapseTree.UseVisualStyleBackColor = true;
            this.btnCollapseTree.Click += new System.EventHandler(this.btnCollapseTree_Click);
            // 
            // pbForlblCurrentTestPlan
            // 
            this.pbForlblCurrentTestPlan.BackColor = System.Drawing.Color.Gray;
            this.pbForlblCurrentTestPlan.Location = new System.Drawing.Point(3, 58);
            this.pbForlblCurrentTestPlan.Name = "pbForlblCurrentTestPlan";
            this.pbForlblCurrentTestPlan.Size = new System.Drawing.Size(204, 30);
            this.pbForlblCurrentTestPlan.TabIndex = 5;
            this.pbForlblCurrentTestPlan.TabStop = false;
            // 
            // btnExpandTree
            // 
            this.btnExpandTree.Enabled = false;
            this.btnExpandTree.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExpandTree.Location = new System.Drawing.Point(216, 82);
            this.btnExpandTree.Name = "btnExpandTree";
            this.btnExpandTree.Size = new System.Drawing.Size(120, 30);
            this.btnExpandTree.TabIndex = 5;
            this.btnExpandTree.Text = "Раскрыть дерево";
            this.btnExpandTree.UseVisualStyleBackColor = true;
            this.btnExpandTree.Click += new System.EventHandler(this.btnExpandTree_Click);
            // 
            // pbForlblSoftware
            // 
            this.pbForlblSoftware.BackColor = System.Drawing.Color.Gray;
            this.pbForlblSoftware.Location = new System.Drawing.Point(3, 3);
            this.pbForlblSoftware.Name = "pbForlblSoftware";
            this.pbForlblSoftware.Size = new System.Drawing.Size(204, 30);
            this.pbForlblSoftware.TabIndex = 6;
            this.pbForlblSoftware.TabStop = false;
            // 
            // treeView
            // 
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.treeView.HideSelection = false;
            this.treeView.LeftPadding = 7;
            this.treeView.Location = new System.Drawing.Point(212, 144);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(936, 427);
            this.treeView.Spacing = 4;
            this.treeView.TabIndex = 5;
            this.treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_NodeMouseClick_1);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.Controls.Add(this.btnCaseTransfer);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 577);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1148, 63);
            this.panel1.TabIndex = 6;
            // 
            // btnCaseTransfer
            // 
            this.btnCaseTransfer.Enabled = false;
            this.btnCaseTransfer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCaseTransfer.Location = new System.Drawing.Point(216, 18);
            this.btnCaseTransfer.Name = "btnCaseTransfer";
            this.btnCaseTransfer.Size = new System.Drawing.Size(120, 30);
            this.btnCaseTransfer.TabIndex = 7;
            this.btnCaseTransfer.Text = "Перенести кейсы";
            this.btnCaseTransfer.UseVisualStyleBackColor = true;
            this.btnCaseTransfer.Click += new System.EventHandler(this.btCaseTransfer_Click);
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1148, 662);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.treeView);
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.MainFormStatus);
            this.Controls.Add(this.MainFormMenu);
            this.Controls.Add(this.leftPanel);
            this.ForeColor = System.Drawing.Color.White;
            this.MainMenuStrip = this.MainFormMenu;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "";
            this.Text = "Перенос результатов прогона в TestLink";
            this.MainFormMenu.ResumeLayout(false);
            this.MainFormMenu.PerformLayout();
            this.leftPanel.ResumeLayout(false);
            this.leftPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbForlblTestRun)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbForlblCurrentTestPlan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbForlblSoftware)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList foldersIcons;
        private System.Windows.Forms.MenuStrip MainFormMenu;
        private System.Windows.Forms.StatusStrip MainFormStatus;
        private System.Windows.Forms.Panel leftPanel;
        private System.Windows.Forms.Label lblSoftware;
        private System.Windows.Forms.ComboBox cbProjectNames;
        private System.Windows.Forms.ComboBox cbTestPlanName;
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Button btnManualMode;
        private System.Windows.Forms.Button btnAutoMode;
        private System.Windows.Forms.Button btnCollapseTree;
        private System.Windows.Forms.Button btnExpandTree;
        private System.Windows.Forms.ToolStripMenuItem referenceItem;
        private System.Windows.Forms.ToolStripMenuItem aboutItem;
        private System.Windows.Forms.Label lblTestRun;
        private System.Windows.Forms.PictureBox pbForlblTestRun;
        private System.Windows.Forms.Label lblCurrentTestPlan;
        private System.Windows.Forms.PictureBox pbForlblCurrentTestPlan;
        private System.Windows.Forms.PictureBox pbForlblSoftware;
        private System.Windows.Forms.CheckBox cbBlocked;
        private System.Windows.Forms.CheckBox cbFailed;
        private System.Windows.Forms.CheckBox cbPassed;
        private UcTreeView treeView;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCaseTransfer;
        private System.Windows.Forms.Label lblDescriptionTests;
        private System.Windows.Forms.Button btnShowAllTests;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

