using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using TestLinkApi;
using TLTCImport.FolderStorageTestLink;

namespace TLTCImport
{
    public partial class MainForm : Form
    {
        private Panel leftPanel;
        private Panel topPanel;

        private Label lblSoftware, lblCurrentTestPlan, lblTestRun;
        public Label lblMessageAddJson, lblMessageRecognitionJson;
        public Label lblMessageDataTransferXmlFile, lblAddCasesTestlink;
        public Label lblNotAllTestCasesRecognized;
      
        public UcTreeView treeView;
        public ImageList imageList;
        public TreeNode tNSubfolder, tNTestCase;

        private Button btnExpandTree, btnCollapseTree;
        private Button btnAutoMode, btnManualMode;       

        private ComboBox cbProjectNames, cbTestPlanName;
        
        private int projectId, testPlanId;
        private string projectName, testPlanName;
        private string pathFile = "../../../Files/";
        private string pathContent = "../../../Content/";
        private int IconFoldersAndSubfolders = 0, IconTestCases = 1;
        void aboutItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("О программе");
        }

        public MainForm()
        {
            InitializeComponent();

            this.Closing += MainForm_Closing;

            var font = new Font("Century Gothic", 12);

            //Панель слева
            leftPanel = new Panel();
            leftPanel.Dock = DockStyle.Left;
            leftPanel.Size = new Size(210, leftPanel.Size.Height);
            leftPanel.BackColor = Color.DarkGray;

            //Панель справа сверху
            topPanel = new Panel();
            topPanel.Dock = DockStyle.Top;
            topPanel.BackColor = Color.DarkGray;

            //Текст Программный продукт
            lblSoftware = new Label();
            lblSoftware.BackColor = Color.Gray;
            lblSoftware.Name = "lblSoftware";
            lblSoftware.Text = "Программный продукт:";
            lblSoftware.Size = new Size(200, 30);
            lblSoftware.Location = new Point(5, 30);
            lblSoftware.TextAlign = ContentAlignment.MiddleLeft;
            Controls.Add(lblSoftware);

            //Комбобокс для выбора программного продукта
            cbProjectNames = new ComboBox();
            cbProjectNames.Size = new Size(200, 30);
            cbProjectNames.Location = new Point(5, 60);
            cbProjectNames.DropDownStyle = ComboBoxStyle.DropDown;
            cbProjectNames.SelectedIndexChanged += cbProjectNames_SelectedIndexChanged;
            SetProjectNames(cbProjectNames);
            Controls.Add(cbProjectNames);

            //Текст Текущий тест-план:
            lblCurrentTestPlan = new Label();
            lblCurrentTestPlan.BackColor = Color.Gray;
            lblCurrentTestPlan.Name = "lblCurrentTestPlan";
            lblCurrentTestPlan.Text = "Текущий тест-план: ";
            lblCurrentTestPlan.Size = new Size(200, 30);
            lblCurrentTestPlan.Location = new Point(5, 85);
            lblCurrentTestPlan.TextAlign = ContentAlignment.MiddleLeft;
            Controls.Add(lblCurrentTestPlan);

            //Комбобокс для выбора Текущего тестового-плана:
            cbTestPlanName = new ComboBox();
            cbTestPlanName.Size = new Size(200, 30);
            cbTestPlanName.Location = new Point(5, 115);
            cbTestPlanName.DropDownStyle = ComboBoxStyle.DropDown;
            cbTestPlanName.SelectedIndexChanged += cbTestPlanName_SelectedIndexChanged;
            cbTestPlanName.Enabled = false;
            Controls.Add(cbTestPlanName);

            //Текст Прогон тестов:
            lblTestRun = new Label();
            lblTestRun.BackColor = Color.Gray;
            lblTestRun.Name = "lblTestRun";
            lblTestRun.Text = "Прогон тестов:";
            lblTestRun.Size = new Size(200, 30);
            lblTestRun.Location = new Point(5, 145);
            lblTestRun.TextAlign = ContentAlignment.MiddleLeft;
            Controls.Add(lblTestRun);

            //Кнопка Автоматизированный режим
            btnAutoMode = new Button();
            btnAutoMode.FlatStyle = FlatStyle.Flat;
            btnAutoMode.BackColor = Color.DarkGray;
            btnAutoMode.FlatAppearance.BorderSize = 0;
            btnAutoMode.Name = "AutoMode";
            btnAutoMode.Size = new Size(200, 30);
            btnAutoMode.Location = new Point(5, 175);
            btnAutoMode.Text = "Авто режим";
            btnAutoMode.Click += new EventHandler(btnAutoMode_Click);
            btnAutoMode.Enabled = false;
            Controls.Add(btnAutoMode);

            //Кнопка Ручной режим
            btnManualMode = new Button();
            btnManualMode.FlatStyle = FlatStyle.Flat;
            btnManualMode.BackColor = Color.DarkGray;
            btnManualMode.FlatAppearance.BorderSize = 0;
            btnManualMode.Name = "ManualMode";
            btnManualMode.Size = new Size(200, 30);
            btnManualMode.Location = new Point(5, 205);
            btnManualMode.Text = "Ручной режим";
            btnManualMode.Click += new EventHandler(btnManualMode_Click);
            btnManualMode.Enabled = false;
            Controls.Add(btnManualMode);

            //Кнопка Раскрыть дерево
            btnExpandTree = new Button();
            btnExpandTree.FlatStyle = FlatStyle.Flat;
            btnExpandTree.BackColor = Color.DarkGray;
            btnExpandTree.FlatAppearance.BorderSize = 0;
            btnExpandTree.Name = "UncoverTree";
            btnExpandTree.Size = new Size(120, 30);
            btnExpandTree.Location = new Point(210, 85);
            btnExpandTree.Text = "Раскрыть дерево";
            btnExpandTree.Click += new EventHandler(btnExpandTree_Click);
            btnExpandTree.Enabled = false;
            Controls.Add(btnExpandTree);

            //Кнопка Закрыть дерево
            btnCollapseTree = new Button();
            btnCollapseTree.FlatStyle = FlatStyle.Flat;
            btnCollapseTree.BackColor = Color.DarkGray;
            btnCollapseTree.FlatAppearance.BorderSize = 0;
            btnCollapseTree.Name = "UncoverTree";
            btnCollapseTree.Size = new Size(120, 30);
            btnCollapseTree.Location = new Point(330, 85);
            btnCollapseTree.Text = "Закрыть дерево";
            btnCollapseTree.Click += new EventHandler(btnCollapseTree_Click);
            btnCollapseTree.Enabled = false;
            Controls.Add(btnCollapseTree);

            //Текст отображающий сообщение о добавлении файла
            lblMessageAddJson = new Label();
            lblMessageAddJson.BackColor = Color.DarkGray;
            lblMessageAddJson.Size = new Size(200, 50);
            lblMessageAddJson.Location = new Point(5, 245);
            lblMessageAddJson.TextAlign = ContentAlignment.MiddleCenter;
            Controls.Add(lblMessageAddJson);

            //Текст отображающий сообщение о распознании файла Json
            lblMessageRecognitionJson = new Label();
            lblMessageRecognitionJson.BackColor = Color.DarkGray;
            lblMessageRecognitionJson.Size = new Size(200, 50);
            lblMessageRecognitionJson.Location = new Point(5, 285);
            lblMessageRecognitionJson.TextAlign = ContentAlignment.MiddleCenter;
            Controls.Add(lblMessageRecognitionJson);

            //пока не используется (для вывода сообщения на экран )
            ////Текст отображающий сообщение что Данные перенесены в xml файл
            //lblMessageDataTransferXmlFile = new Label();
            //lblMessageDataTransferXmlFile.BackColor = Color.DarkGray;
            //lblMessageDataTransferXmlFile.Size = new Size(200, 50);
            //lblMessageDataTransferXmlFile.Location = new Point(5, 350);
            //lblMessageDataTransferXmlFile.TextAlign = ContentAlignment.MiddleCenter;
            //Controls.Add(lblMessageDataTransferXmlFile);

            //Текст отображающий сообщение что Результаты прогона перенесены в TestLink
            lblAddCasesTestlink = new Label();
            lblAddCasesTestlink.BackColor = Color.DarkGray;
            lblAddCasesTestlink.Size = new Size(200, 100);
            lblAddCasesTestlink.Location = new Point(5, 325);
            lblAddCasesTestlink.TextAlign = ContentAlignment.MiddleCenter;
            Controls.Add(lblAddCasesTestlink);

            //Текст отображающий сообщение о том почему не все кейсы были распознаны
            lblNotAllTestCasesRecognized = new Label();
            lblNotAllTestCasesRecognized.BackColor = Color.DarkGray;
            lblNotAllTestCasesRecognized.Font = new Font(Label.DefaultFont, FontStyle.Bold);
            lblNotAllTestCasesRecognized.ForeColor = Color.IndianRed;
            lblNotAllTestCasesRecognized.Size = new Size(200, 200);
            lblNotAllTestCasesRecognized.Location = new Point(5, 375);
            lblNotAllTestCasesRecognized.TextAlign = ContentAlignment.MiddleCenter;
            Controls.Add(lblNotAllTestCasesRecognized);

            //Дерево, содеражащее в себе все папки и тест кейсы            
            treeView = new UcTreeView();
            imageList = new ImageList();
            imageList.Images.Add(Image.FromFile(pathContent + "folder.gif"));
            imageList.Images.Add(Image.FromFile(pathContent + "leaf.gif"));
            treeView.ImageList = imageList;
            treeView.ImageIndex = IconFoldersAndSubfolders;
            treeView.BorderStyle = BorderStyle.None;
            treeView.Location = new Point(220, 135);
            treeView.Size = new Size(580, 470);

            //Создание пустой папки дерева
            tNSubfolder = new TreeNode("Пусто");
            treeView.Nodes.Add(tNSubfolder);
            Controls.Add(treeView);

            //Добавление панелей на экран
            Controls.Add(leftPanel);
            Controls.Add(topPanel);

            //Меню
            ToolStripMenuItem fileItem = new ToolStripMenuItem("Загрузить json файл");
            MainFormMenu.Items.Add(fileItem);

            ToolStripMenuItem aboutItem1 = new ToolStripMenuItem("Справка");
            MainFormMenu.Items.Add(aboutItem1);

            ToolStripMenuItem aboutItem = new ToolStripMenuItem("О программе");
            aboutItem.Click += aboutItem_Click;
            MainFormMenu.Items.Add(aboutItem);
            Controls.Add(MainFormMenu);
        }       

        private void TreeCreate(Folder[][] foldersAndSubfolders)
        {
            //Удаление пустой папки
            treeView.Nodes.Remove(tNSubfolder);
            int j = 0;
           
            foreach (var folders in foldersAndSubfolders)
            {
                foreach (var valueFolder in folders)
                {
                    //Добавление папок
                    tNSubfolder = new TreeNode(valueFolder.nameFolder);
                    treeView.Nodes.Add(tNSubfolder);

                    //Добавление тесткейсов в подпапки
                    for (int i = 0; i < valueFolder.subfolders.Length; i++)
                    {
                        tNTestCase = new TreeNode(valueFolder.subfolders[i].nameSubfolder);
                        AddTestCases(valueFolder.subfolders[i].testCases, tNTestCase);
                        treeView.Nodes[j].Nodes.Add(tNTestCase);
                    }
                    j++;
                }
            }

            //Для лечения бага
            treeView.ExpandAll();
            treeView.CollapseAll();
        }

        private string CreateTestCaseFullName(InfoTestCase testCaseFullName)
        {
            var prefixName = TestLinkResult.GetPrefixProjectByName(projectName);      
            return prefixName + "-" + testCaseFullName.externalIdTestCase + ":" + testCaseFullName.nameTestCase;
        }        
        private void AddSubfolders(Subfolder[] subfolderNames, TreeNode tNSubfolder)
        {
            // Добавляем подпапки к папкам
            foreach (var subfolderName in subfolderNames)
                tNSubfolder.Nodes.Add(new TreeNode(subfolderName.nameSubfolder));            
        }

        private void AddTestCases(InfoTestCase[] testCases, TreeNode tNTestCase)
        {
            // Добавляем тесткейсы к подпапкам
            foreach (var testCase in testCases)
                tNTestCase.Nodes.Add(new TreeNodeVirtual(CreateTestCaseFullName(testCase), IconTestCases, IconTestCases));
        }

        private void SetProjectNames(ComboBox comboBox)
        {
            foreach (var item in TestLinkResult.GetAllProjects())            
                comboBox.Items.Add(item.name);            
        }

        private void SetTestPlanName(ComboBox comboBox, int projectId)
        {
            foreach (var item in TestLinkResult.GetAllProjectTestPlans(projectId))
            {
                comboBox.Items.Add(item.name);               
            }

            Thread.Sleep(250);
            cbTestPlanName.Enabled = true;
        }

        private void cbProjectNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (projectId == 0)
            {                
                projectId = TestLinkResult.GetProjectIdByName(cbProjectNames.SelectedItem.ToString());
                projectName = cbProjectNames.SelectedItem.ToString();
                SetTestPlanName(cbTestPlanName, projectId);
            }
            else if (projectId != 0)
            {
                cbTestPlanName.Items.Clear();
                cbTestPlanName.Text = "";

                projectId = TestLinkResult.GetProjectIdByName(cbProjectNames.SelectedItem.ToString());
                projectName = cbProjectNames.SelectedItem.ToString();
                SetTestPlanName(cbTestPlanName, projectId);
            }
        }

        private void cbTestPlanName_SelectedIndexChanged(object sender, EventArgs e)
        {
            testPlanId = TestLinkResult.GetTestPlanIdByName(cbProjectNames.SelectedItem.ToString(), cbTestPlanName.SelectedItem.ToString());
            testPlanName = cbTestPlanName.SelectedItem.ToString();

            if (testPlanId != 0)
            {
                btnAutoMode.Enabled = true;
                btnManualMode.Enabled = true;
            }
        }

        private void btnManualMode_Click(object sender, EventArgs e)
        {   
            //Перед ручным режимом блокируем все элементы
            BlockAllElementsMainForm();

            //Окно загрузки
            LoadingScreen OpenLoadForm = new LoadingScreen("Получение информации о папках..");
            OpenLoadForm.Location = Location;
            OpenLoadForm.StartPosition = FormStartPosition.CenterScreen;
            OpenLoadForm.FormClosing += delegate { Show(); };
            OpenLoadForm.Show();

            //Постройка дерева
            TreeWithTestCases treeWithTestCases = new TreeWithTestCases();
            TreeCreate(treeWithTestCases.NamesFoldersAndSubfolders(projectId));

            //Закрытие окна закрузки и отображение кнопок
            OpenLoadForm.Close();
            OpenAllElementsMainForm();
        }

        private void btnExpandTree_Click(object sender, EventArgs e)
        {
            treeView.ExpandAll();
        }

        private void btnCollapseTree_Click(object sender, EventArgs e)
        {
            treeView.CollapseAll();
        }


        private void btnAutoMode_Click(object sender, EventArgs e)
        {
            bool FileExistence;
            int countSubmittedЕestСases, countValuesCases;
            Dictionary<string, string> valuesCases;
            //MessageBox.Show("Выберите файл в формете .json. Потом написать инструкцию");

            ClearAllMessages();

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "json files (*.json)|*.json";

            DialogResult result = fileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string urlUploadedFile = fileDialog.FileName;
                string[] names = urlUploadedFile.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                string nameFile = names[names.Length - 1].Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[0];

                //Перенос файла в проект
                FileExistence = CopyFileInProject(urlUploadedFile, nameFile);

                //Проверка условия, что мы готовы добавить файл
                if (FileExistence == true)
                {
                    MessagejsonAdd(nameFile);

                    JsonToXml jsonToXml = new JsonToXml();
                    var jsonFileCorrect = jsonToXml.JsonValidation(nameFile);

                    //Проверка коректен ли файл
                    if (jsonFileCorrect)
                    {
                        MessagejsonRecognize();

                        //Запись значений из json в словарь
                        valuesCases = jsonToXml.GetDataJson(nameFile, ref jsonFileCorrect, projectId);

                        //Пока не используется, т.к. не работает
                        //Если файл корректен, то перенос результатов в xml файл
                        jsonToXml.FillXmlFile(projectName, testPlanName, testPlanId, valuesCases, out countValuesCases);

                        //Выводит на экран сообщение
                        //MessageXMLFileFull(countValuesCases);

                        //Перед импортом блокируем кнопки и списки
                        BlockAllElementsMainForm();

                        //Окно загрузки
                        LoadingScreen OpenLoadForm = new LoadingScreen();
                        OpenLoadForm.Location = Location;
                        OpenLoadForm.StartPosition = FormStartPosition.CenterScreen;
                        OpenLoadForm.FormClosing += delegate { Show(); };
                        OpenLoadForm.Show();

                        //Импорт xml файла в Тестлинк               
                        countSubmittedЕestСases = TestLinkResult.ImportsRunInfoInTestLink(pathFile, testPlanId, valuesCases, projectName);

                        //Закрытие окна закрузки
                        OpenLoadForm.Close();
                        OpenAllElementsMainForm();

                        MessageAddCasesTestlink(valuesCases.Count, countSubmittedЕestСases);
                    }
                    else
                        MessagejsonInvalid();
                }
                else
                    MessageFileNotAdd();
            }
            else
                MessageFileNotAdd();

        }
        
        private void MessageFileNotAdd()
        {
            lblMessageAddJson.Font = new Font(Label.DefaultFont, FontStyle.Bold);
            lblMessageAddJson.ForeColor = Color.Red;
            lblMessageAddJson.Text = "✖ Файл не был добавлен";

            lblMessageRecognitionJson.Text = "";
            lblAddCasesTestlink.Text = "";
            lblNotAllTestCasesRecognized.Text = "";
        }

        private void MessagejsonInvalid()
        {
            lblMessageAddJson.Font = new Font(DefaultFont, FontStyle.Bold);
            lblMessageAddJson.ForeColor = Color.DarkRed;
            lblMessageAddJson.Text = "✖ Ошибка! Json файл не корректен. Попробуйте загрузить другой файл!";
        }

        private void MessagejsonAdd(string nameFile)
        {
            lblMessageAddJson.Font = new Font(DefaultFont, FontStyle.Bold);
            lblMessageAddJson.ForeColor = Color.Green;
            lblMessageAddJson.Text = "✔ Файл \"" + nameFile + ".json" + "\" успешно добавлен!";
        }

        private void MessagejsonRecognize()
        {
            lblMessageRecognitionJson.Font = new Font(DefaultFont, FontStyle.Bold);
            lblMessageRecognitionJson.ForeColor = Color.Green;
            lblMessageRecognitionJson.Text = "✔ Json файл распознан!";
        }

        private void MessageXMLFileFull(int countValuesCases)
        {
            if (countValuesCases > 0)
            {
                lblMessageDataTransferXmlFile.Font = new Font(DefaultFont, FontStyle.Bold);
                lblMessageDataTransferXmlFile.ForeColor = Color.Green;
                lblMessageDataTransferXmlFile.Text = "✔ XML файл был создан и заполнен!";
            }
            else
            {
                lblMessageDataTransferXmlFile.Font = new Font(DefaultFont, FontStyle.Bold);
                lblMessageDataTransferXmlFile.ForeColor = Color.DarkRed;
                lblMessageDataTransferXmlFile.Text = "✖ Ошибка! Json файл не содержит информацию о тест кейсах!";
            }
        }

        private void MessageAddCasesTestlink(int countTestCasesJenkins, int countSubmittedЕestСases)
        {
            lblAddCasesTestlink.Font = new Font(DefaultFont, FontStyle.Bold);
            lblAddCasesTestlink.ForeColor = Color.Green;
            lblAddCasesTestlink.Text = $"✔ Информация о прогоне успешно добавлена ​​в Testlink!" +
                $" \r\n Тестов перенесено {countSubmittedЕestСases} из {countTestCasesJenkins}, имеющихся в json файле.";
            if (countSubmittedЕestСases < countTestCasesJenkins)
                lblNotAllTestCasesRecognized.Text = "Причина по которой не все тест кейсы были отправлены, возможно, связана с тем, что " +
                     "названия тестов в Jenkins отличаются от названий кейсов в TestLink!";
        }

        private void BlockAllElementsMainForm()
        {
            MainFormMenu.Enabled = false;
            cbProjectNames.Enabled = false;
            cbTestPlanName.Enabled = false;
            btnAutoMode.Enabled = false;
            btnManualMode.Enabled = false;
            btnExpandTree.Enabled = false;
            btnCollapseTree.Enabled = false;
        }

        private void OpenAllElementsMainForm()
        {
            MainFormMenu.Enabled = true;
            cbProjectNames.Enabled = true;
            cbTestPlanName.Enabled = true;
            btnAutoMode.Enabled = true;
            btnManualMode.Enabled = true;
            btnExpandTree.Enabled = true;
            btnCollapseTree.Enabled = true;
        }

        private void ClearAllMessages()
        {
            lblMessageAddJson.Text = "";
            lblMessageRecognitionJson.Text = "";
            lblAddCasesTestlink.Text = "";
            lblNotAllTestCasesRecognized.Text = "";
        }

        private bool CopyFileInProject(string urlUploadedFile, string nameFile)
        {
            if (File.Exists(pathFile + nameFile + ".json"))
            {
                DialogResult dialogResult = MessageBox.Show("Файл уже загружен в систему! Вы уверены, что хотите его изменить?", "Изменение существующего файла", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    File.Delete(pathFile + nameFile + ".json");
                    File.Copy(urlUploadedFile, pathFile + nameFile + ".json");
                    return true;
                }
                else if (dialogResult == DialogResult.No)
                {
                    MessageFileNotAdd();
                    return false;
                }
            }
            else             
                File.Copy(urlUploadedFile, pathFile + nameFile + ".json");
            return true;
        }

        private void MainForm_Closing(object sender, CancelEventArgs e)
        {
            Application.Exit();

            DeleteAllFiles();
        }

        private void DeleteAllFiles()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(pathFile);

            foreach (FileInfo file in dirInfo.GetFiles())
            {
                file.Delete();
            }
        }
    }
}
