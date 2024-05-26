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
            components = new System.ComponentModel.Container();
            foldersIcons = new System.Windows.Forms.ImageList(components);
            MainFormMenu = new System.Windows.Forms.MenuStrip();
            referenceItem = new System.Windows.Forms.ToolStripMenuItem();
            aboutItem = new System.Windows.Forms.ToolStripMenuItem();
            MainFormStatus = new System.Windows.Forms.StatusStrip();
            leftPanel = new System.Windows.Forms.Panel();
            lblDescriptionTests = new System.Windows.Forms.Label();
            btnShowAllTests = new System.Windows.Forms.Button();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            cbBlocked = new System.Windows.Forms.CheckBox();
            cbFailed = new System.Windows.Forms.CheckBox();
            cbPassed = new System.Windows.Forms.CheckBox();
            lblTestRun = new System.Windows.Forms.Label();
            pbForlblTestRun = new System.Windows.Forms.PictureBox();
            btnManualMode = new System.Windows.Forms.Button();
            btnAutoMode = new System.Windows.Forms.Button();
            lblSoftware = new System.Windows.Forms.Label();
            cbProjectNames = new System.Windows.Forms.ComboBox();
            cbTestPlanName = new System.Windows.Forms.ComboBox();
            topPanel = new System.Windows.Forms.Panel();
            lbl_InfoSuccessTree = new System.Windows.Forms.Label();
            lblCurrentTestPlan = new System.Windows.Forms.Label();
            btnCollapseTree = new System.Windows.Forms.Button();
            pbForlblCurrentTestPlan = new System.Windows.Forms.PictureBox();
            btnExpandTree = new System.Windows.Forms.Button();
            pbForlblSoftware = new System.Windows.Forms.PictureBox();
            treeView = new UcTreeView();
            panel1 = new System.Windows.Forms.Panel();
            btnCaseTransfer = new System.Windows.Forms.Button();
            btRemoveSelection = new System.Windows.Forms.Button();
            MainFormMenu.SuspendLayout();
            leftPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbForlblTestRun).BeginInit();
            topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbForlblCurrentTestPlan).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbForlblSoftware).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // foldersIcons
            // 
            foldersIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            foldersIcons.ImageSize = new System.Drawing.Size(16, 16);
            foldersIcons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // MainFormMenu
            // 
            MainFormMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { referenceItem, aboutItem });
            MainFormMenu.Location = new System.Drawing.Point(0, 0);
            MainFormMenu.Name = "MainFormMenu";
            MainFormMenu.Size = new System.Drawing.Size(1148, 24);
            MainFormMenu.TabIndex = 0;
            MainFormMenu.Text = "menuStrip1";
            // 
            // referenceItem
            // 
            referenceItem.Name = "referenceItem";
            referenceItem.Size = new System.Drawing.Size(65, 20);
            referenceItem.Text = "Справка";
            // 
            // aboutItem
            // 
            aboutItem.Name = "aboutItem";
            aboutItem.Size = new System.Drawing.Size(94, 20);
            aboutItem.Text = "О программе";
            // 
            // MainFormStatus
            // 
            MainFormStatus.Location = new System.Drawing.Point(0, 640);
            MainFormStatus.Name = "MainFormStatus";
            MainFormStatus.Size = new System.Drawing.Size(1148, 22);
            MainFormStatus.TabIndex = 1;
            MainFormStatus.Text = "statusStrip1";
            // 
            // leftPanel
            // 
            leftPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            leftPanel.BackColor = System.Drawing.Color.Silver;
            leftPanel.Controls.Add(lblDescriptionTests);
            leftPanel.Controls.Add(btnShowAllTests);
            leftPanel.Controls.Add(pictureBox1);
            leftPanel.Controls.Add(cbBlocked);
            leftPanel.Controls.Add(cbFailed);
            leftPanel.Controls.Add(cbPassed);
            leftPanel.Controls.Add(lblTestRun);
            leftPanel.Controls.Add(pbForlblTestRun);
            leftPanel.Controls.Add(btnManualMode);
            leftPanel.Controls.Add(btnAutoMode);
            leftPanel.Location = new System.Drawing.Point(0, 24);
            leftPanel.Name = "leftPanel";
            leftPanel.Size = new System.Drawing.Size(210, 1357);
            leftPanel.TabIndex = 2;
            // 
            // lblDescriptionTests
            // 
            lblDescriptionTests.AutoSize = true;
            lblDescriptionTests.BackColor = System.Drawing.Color.Gray;
            lblDescriptionTests.ForeColor = System.Drawing.Color.White;
            lblDescriptionTests.Location = new System.Drawing.Point(5, 286);
            lblDescriptionTests.Name = "lblDescriptionTests";
            lblDescriptionTests.Size = new System.Drawing.Size(103, 15);
            lblDescriptionTests.TabIndex = 7;
            lblDescriptionTests.Text = "Описание тестов:";
            lblDescriptionTests.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnShowAllTests
            // 
            btnShowAllTests.Enabled = false;
            btnShowAllTests.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnShowAllTests.Location = new System.Drawing.Point(3, 316);
            btnShowAllTests.Name = "btnShowAllTests";
            btnShowAllTests.Size = new System.Drawing.Size(204, 30);
            btnShowAllTests.TabIndex = 7;
            btnShowAllTests.Text = "Отобразить все тесты продукта";
            btnShowAllTests.UseVisualStyleBackColor = true;
            btnShowAllTests.Click += btnShowAllTests_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = System.Drawing.Color.Gray;
            pictureBox1.Location = new System.Drawing.Point(3, 280);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(204, 30);
            pictureBox1.TabIndex = 8;
            pictureBox1.TabStop = false;
            // 
            // cbBlocked
            // 
            cbBlocked.AutoSize = true;
            cbBlocked.Checked = true;
            cbBlocked.CheckState = System.Windows.Forms.CheckState.Checked;
            cbBlocked.Enabled = false;
            cbBlocked.Location = new System.Drawing.Point(13, 215);
            cbBlocked.Name = "cbBlocked";
            cbBlocked.Size = new System.Drawing.Size(93, 19);
            cbBlocked.TabIndex = 8;
            cbBlocked.Text = "Блокирован";
            cbBlocked.UseVisualStyleBackColor = true;
            // 
            // cbFailed
            // 
            cbFailed.AutoSize = true;
            cbFailed.Enabled = false;
            cbFailed.Location = new System.Drawing.Point(101, 189);
            cbFailed.Name = "cbFailed";
            cbFailed.Size = new System.Drawing.Size(81, 19);
            cbFailed.TabIndex = 7;
            cbFailed.Text = "Провален";
            cbFailed.UseVisualStyleBackColor = true;
            // 
            // cbPassed
            // 
            cbPassed.AutoSize = true;
            cbPassed.Checked = true;
            cbPassed.CheckState = System.Windows.Forms.CheckState.Checked;
            cbPassed.Enabled = false;
            cbPassed.ForeColor = System.Drawing.Color.Transparent;
            cbPassed.Location = new System.Drawing.Point(13, 190);
            cbPassed.Name = "cbPassed";
            cbPassed.Size = new System.Drawing.Size(75, 19);
            cbPassed.TabIndex = 6;
            cbPassed.Text = "Пройден";
            cbPassed.UseVisualStyleBackColor = true;
            // 
            // lblTestRun
            // 
            lblTestRun.AutoSize = true;
            lblTestRun.BackColor = System.Drawing.Color.Gray;
            lblTestRun.ForeColor = System.Drawing.Color.White;
            lblTestRun.Location = new System.Drawing.Point(5, 123);
            lblTestRun.Name = "lblTestRun";
            lblTestRun.Size = new System.Drawing.Size(90, 15);
            lblTestRun.TabIndex = 0;
            lblTestRun.Text = "Прогон тестов:";
            lblTestRun.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbForlblTestRun
            // 
            pbForlblTestRun.BackColor = System.Drawing.Color.Gray;
            pbForlblTestRun.Location = new System.Drawing.Point(3, 117);
            pbForlblTestRun.Name = "pbForlblTestRun";
            pbForlblTestRun.Size = new System.Drawing.Size(204, 30);
            pbForlblTestRun.TabIndex = 4;
            pbForlblTestRun.TabStop = false;
            // 
            // btnManualMode
            // 
            btnManualMode.Enabled = false;
            btnManualMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnManualMode.Location = new System.Drawing.Point(3, 240);
            btnManualMode.Name = "btnManualMode";
            btnManualMode.Size = new System.Drawing.Size(204, 30);
            btnManualMode.TabIndex = 5;
            btnManualMode.Text = "Ручной режим";
            btnManualMode.UseVisualStyleBackColor = true;
            btnManualMode.Click += btnManualMode_Click;
            // 
            // btnAutoMode
            // 
            btnAutoMode.Enabled = false;
            btnAutoMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAutoMode.Location = new System.Drawing.Point(3, 153);
            btnAutoMode.Name = "btnAutoMode";
            btnAutoMode.Size = new System.Drawing.Size(203, 30);
            btnAutoMode.TabIndex = 4;
            btnAutoMode.Text = "Авто режим";
            btnAutoMode.UseVisualStyleBackColor = true;
            btnAutoMode.Click += btnAutoMode_Click;
            // 
            // lblSoftware
            // 
            lblSoftware.AutoSize = true;
            lblSoftware.BackColor = System.Drawing.Color.Gray;
            lblSoftware.ForeColor = System.Drawing.Color.White;
            lblSoftware.Location = new System.Drawing.Point(5, 10);
            lblSoftware.Name = "lblSoftware";
            lblSoftware.Size = new System.Drawing.Size(139, 15);
            lblSoftware.TabIndex = 0;
            lblSoftware.Text = "Программный продукт:";
            lblSoftware.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbProjectNames
            // 
            cbProjectNames.ForeColor = System.Drawing.Color.Black;
            cbProjectNames.FormattingEnabled = true;
            cbProjectNames.Location = new System.Drawing.Point(3, 34);
            cbProjectNames.Name = "cbProjectNames";
            cbProjectNames.Size = new System.Drawing.Size(204, 23);
            cbProjectNames.TabIndex = 2;
            cbProjectNames.Text = "Выберите продукт";
            cbProjectNames.SelectedIndexChanged += cbProjectNames_SelectedIndexChanged;
            // 
            // cbTestPlanName
            // 
            cbTestPlanName.Enabled = false;
            cbTestPlanName.FormattingEnabled = true;
            cbTestPlanName.Location = new System.Drawing.Point(3, 89);
            cbTestPlanName.Name = "cbTestPlanName";
            cbTestPlanName.Size = new System.Drawing.Size(204, 23);
            cbTestPlanName.TabIndex = 3;
            cbTestPlanName.SelectedIndexChanged += cbTestPlanName_SelectedIndexChanged;
            // 
            // topPanel
            // 
            topPanel.BackColor = System.Drawing.Color.Silver;
            topPanel.Controls.Add(btRemoveSelection);
            topPanel.Controls.Add(lbl_InfoSuccessTree);
            topPanel.Controls.Add(lblSoftware);
            topPanel.Controls.Add(lblCurrentTestPlan);
            topPanel.Controls.Add(btnCollapseTree);
            topPanel.Controls.Add(pbForlblCurrentTestPlan);
            topPanel.Controls.Add(btnExpandTree);
            topPanel.Controls.Add(pbForlblSoftware);
            topPanel.Controls.Add(cbTestPlanName);
            topPanel.Controls.Add(cbProjectNames);
            topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            topPanel.Location = new System.Drawing.Point(0, 24);
            topPanel.Name = "topPanel";
            topPanel.Size = new System.Drawing.Size(1148, 115);
            topPanel.TabIndex = 3;
            // 
            // lbl_InfoSuccessTree
            // 
            lbl_InfoSuccessTree.AutoSize = true;
            lbl_InfoSuccessTree.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lbl_InfoSuccessTree.Location = new System.Drawing.Point(510, 89);
            lbl_InfoSuccessTree.Name = "lbl_InfoSuccessTree";
            lbl_InfoSuccessTree.Size = new System.Drawing.Size(0, 21);
            lbl_InfoSuccessTree.TabIndex = 7;
            // 
            // lblCurrentTestPlan
            // 
            lblCurrentTestPlan.AutoSize = true;
            lblCurrentTestPlan.BackColor = System.Drawing.Color.Gray;
            lblCurrentTestPlan.ForeColor = System.Drawing.Color.White;
            lblCurrentTestPlan.Location = new System.Drawing.Point(5, 65);
            lblCurrentTestPlan.Name = "lblCurrentTestPlan";
            lblCurrentTestPlan.Size = new System.Drawing.Size(117, 15);
            lblCurrentTestPlan.TabIndex = 0;
            lblCurrentTestPlan.Text = "Текущий тест-план:";
            lblCurrentTestPlan.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCollapseTree
            // 
            btnCollapseTree.Enabled = false;
            btnCollapseTree.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCollapseTree.Location = new System.Drawing.Point(340, 82);
            btnCollapseTree.Name = "btnCollapseTree";
            btnCollapseTree.Size = new System.Drawing.Size(120, 30);
            btnCollapseTree.TabIndex = 6;
            btnCollapseTree.Text = "Закрыть дерево";
            btnCollapseTree.UseVisualStyleBackColor = true;
            btnCollapseTree.Click += btnCollapseTree_Click;
            // 
            // pbForlblCurrentTestPlan
            // 
            pbForlblCurrentTestPlan.BackColor = System.Drawing.Color.Gray;
            pbForlblCurrentTestPlan.Location = new System.Drawing.Point(3, 58);
            pbForlblCurrentTestPlan.Name = "pbForlblCurrentTestPlan";
            pbForlblCurrentTestPlan.Size = new System.Drawing.Size(204, 30);
            pbForlblCurrentTestPlan.TabIndex = 5;
            pbForlblCurrentTestPlan.TabStop = false;
            // 
            // btnExpandTree
            // 
            btnExpandTree.Enabled = false;
            btnExpandTree.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnExpandTree.Location = new System.Drawing.Point(216, 82);
            btnExpandTree.Name = "btnExpandTree";
            btnExpandTree.Size = new System.Drawing.Size(120, 30);
            btnExpandTree.TabIndex = 5;
            btnExpandTree.Text = "Раскрыть дерево";
            btnExpandTree.UseVisualStyleBackColor = true;
            btnExpandTree.Click += btnExpandTree_Click;
            // 
            // pbForlblSoftware
            // 
            pbForlblSoftware.BackColor = System.Drawing.Color.Gray;
            pbForlblSoftware.Location = new System.Drawing.Point(3, 3);
            pbForlblSoftware.Name = "pbForlblSoftware";
            pbForlblSoftware.Size = new System.Drawing.Size(204, 30);
            pbForlblSoftware.TabIndex = 6;
            pbForlblSoftware.TabStop = false;
            // 
            // treeView
            // 
            treeView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            treeView.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            treeView.HideSelection = false;
            treeView.LeftPadding = 7;
            treeView.Location = new System.Drawing.Point(212, 144);
            treeView.Name = "treeView";
            treeView.Size = new System.Drawing.Size(936, 427);
            treeView.Spacing = 4;
            treeView.TabIndex = 5;
            treeView.NodeMouseClick += treeView_NodeMouseClick;
            // 
            // panel1
            // 
            panel1.BackColor = System.Drawing.Color.Silver;
            panel1.Controls.Add(btnCaseTransfer);
            panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            panel1.Location = new System.Drawing.Point(0, 577);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(1148, 63);
            panel1.TabIndex = 6;
            // 
            // btnCaseTransfer
            // 
            btnCaseTransfer.Enabled = false;
            btnCaseTransfer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCaseTransfer.Location = new System.Drawing.Point(216, 18);
            btnCaseTransfer.Name = "btnCaseTransfer";
            btnCaseTransfer.Size = new System.Drawing.Size(120, 30);
            btnCaseTransfer.TabIndex = 7;
            btnCaseTransfer.Text = "Перенести кейсы";
            btnCaseTransfer.UseVisualStyleBackColor = true;
            btnCaseTransfer.Click += btCaseTransfer_Click;
            // 
            // btRemoveSelection
            // 
            btRemoveSelection.Enabled = false;
            btRemoveSelection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btRemoveSelection.Location = new System.Drawing.Point(1017, 80);
            btRemoveSelection.Name = "btRemoveSelection";
            btRemoveSelection.Size = new System.Drawing.Size(119, 30);
            btRemoveSelection.TabIndex = 8;
            btRemoveSelection.Text = "Снять выделение";
            btRemoveSelection.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(1148, 662);
            Controls.Add(panel1);
            Controls.Add(treeView);
            Controls.Add(topPanel);
            Controls.Add(MainFormStatus);
            Controls.Add(MainFormMenu);
            Controls.Add(leftPanel);
            ForeColor = System.Drawing.Color.White;
            MainMenuStrip = MainFormMenu;
            Name = "MainForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Tag = "";
            Text = "Перенос результатов прогона в TestLink";
            MainFormMenu.ResumeLayout(false);
            MainFormMenu.PerformLayout();
            leftPanel.ResumeLayout(false);
            leftPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbForlblTestRun).EndInit();
            topPanel.ResumeLayout(false);
            topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbForlblCurrentTestPlan).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbForlblSoftware).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
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
        private System.Windows.Forms.Label lbl_InfoSuccessTree;
        private System.Windows.Forms.Button btRemoveSelection;
    }
}

