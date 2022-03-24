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
        public Label lblMessageDataTransferXmlFile, lblAddCasesTestlink;
        public Label lblNotAllTestCasesRecognized;
        public Label lblMessageAddJson, lblMessageRecognitionJson;

        public UcTreeView treeView;
        public ImageList imageList;
        public TreeNode tNSubfolder, tNTestCase;

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

            //Комбобокс для выбора программного продукта
            SetProjectNames(cbProjectNames);

            //Текст отображающий сообщение о добавлении файла
            lblMessageAddJson = new Label();
            lblMessageAddJson.BackColor = Color.Silver;
            lblMessageAddJson.Size = new Size(200, 50);
            lblMessageAddJson.Location = new Point(5, 270);
            lblMessageAddJson.TextAlign = ContentAlignment.MiddleCenter;
            leftPanel.Controls.Add(lblMessageAddJson);

            //Текст отображающий сообщение о распознании файла Json
            lblMessageRecognitionJson = new Label();
            lblMessageRecognitionJson.BackColor = Color.Silver;
            lblMessageRecognitionJson.Size = new Size(200, 50);
            lblMessageRecognitionJson.Location = new Point(5, 310);
            lblMessageRecognitionJson.TextAlign = ContentAlignment.MiddleCenter;
            leftPanel.Controls.Add(lblMessageRecognitionJson);

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
            lblAddCasesTestlink.BackColor = Color.Silver;
            lblAddCasesTestlink.Size = new Size(200, 100);
            lblAddCasesTestlink.Location = new Point(5, 350);
            lblAddCasesTestlink.TextAlign = ContentAlignment.MiddleCenter;
            leftPanel.Controls.Add(lblAddCasesTestlink);

            //Текст отображающий сообщение о том почему не все кейсы были распознаны
            lblNotAllTestCasesRecognized = new Label();
            lblNotAllTestCasesRecognized.BackColor = Color.Silver;
            lblNotAllTestCasesRecognized.Font = new Font(Label.DefaultFont, FontStyle.Bold);
            lblNotAllTestCasesRecognized.ForeColor = Color.IndianRed;
            lblNotAllTestCasesRecognized.Size = new Size(200, 200);
            lblNotAllTestCasesRecognized.Location = new Point(5, 400);
            lblNotAllTestCasesRecognized.TextAlign = ContentAlignment.MiddleCenter;
            leftPanel.Controls.Add(lblNotAllTestCasesRecognized);

            //Дерево, содеражащее в себе все папки и тест кейсы            
            treeView = new UcTreeView();
            imageList = new ImageList();
            imageList.Images.Add(Image.FromFile(pathContent + "folder.gif"));
            imageList.Images.Add(Image.FromFile(pathContent + "leaf.gif"));
            treeView.ImageList = imageList;
            treeView.ImageIndex = IconFoldersAndSubfolders;
            treeView.BorderStyle = BorderStyle.None;
            treeView.Location = new Point(220, 145);
            treeView.Size = new Size(850, 470);

            //Создание пустой папки дерева
            tNSubfolder = new TreeNode("Пусто");
            treeView.Nodes.Add(tNSubfolder);
            Controls.Add(treeView);                    
        }

        private void TreeCreate(Folder[] folders)
        {
            //Удаление пустой папки или старых
            treeView.Nodes.Clear();

            int j = 0;

            foreach (var subfolder in folders)
            {
                //Добавление папок
                tNSubfolder = new TreeNode(subfolder.nameFolder);
                treeView.Nodes.Add(tNSubfolder);

                ////Добавление тесткейсов в папки
                //for (int i = 0; i < subfolder.subfolders.Length; i++)
                //{
                //    AddTestCases(subfolder.testCases, tNSubfolder);
                //}
                //treeView.Nodes.Add(tNSubfolder);

                //Добавление тесткейсов в подпапки
                for (int i = 0; i < subfolder.subfolders.Length; i++)
                {
                    tNTestCase = new TreeNode(subfolder.subfolders[i].nameFolder);
                    AddTestCases(subfolder.subfolders[i].testCases, tNTestCase);
                    treeView.Nodes[j].Nodes.Add(tNTestCase);
                }
                j++;
            }

            //Для лечения бага прорисовки тесткейсов
            treeView.Visible = false;

            treeView.ExpandAll();
            treeView.CollapseAll();

            treeView.Visible = true;
        }

        private string CreateTestCaseFullName(InfoTestCase testCaseFullName)
        {
            var prefixName = TestLinkResult.GetPrefixProjectByName(projectName);      
            return prefixName + "-" + testCaseFullName.externalIdTestCase + ":" + testCaseFullName.nameTestCase;
        }        
        private void AddSubfolders(Folder[] folderNames, TreeNode tNSubfolder)
        {
            // Добавляем подпапки к папкам
            foreach (var folderName in folderNames)
                tNSubfolder.Nodes.Add(new TreeNode(folderName.nameFolder));            
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

                //Чекбоксы
                cbPassed.Enabled = true;
                cbBlocked.Enabled = true;
                cbFailed.Enabled = true;
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
            //OpenLoadForm.Size = this.Size;
            OpenLoadForm.FormClosing += delegate { Show(); };
            //OpenLoadForm.FormBorderStyle = FormBorderStyle.None;
            //OpenLoadForm.BackColor = Color.Black;//цвет фона
            //OpenLoadForm.Opacity = 0.4;
            OpenLoadForm.Show();

            //Постройка дерева
            TreeWithTestCases treeWithTestCases = new TreeWithTestCases();
            TreeCreate(treeWithTestCases.NamesFoldersAndSubfolders(projectId));
            //Закрытие окна закрузки и отображение кнопок
            OpenLoadForm.Close();
            OpenAllElementsMainForm("manual");
        }      

        private void btnAutoMode_Click(object sender, EventArgs e)
        {
            bool FileExistence;
            var testCaseTransferResults = (0, false);
            int countValuesCases;
            Dictionary<string, string> valuesCases = new Dictionary<string, string>();
            //MessageBox.Show("Выберите файл в формете .json. Потом написать инструкцию");

            ClearAllMessages();

            if (cbPassed.Checked == true || cbFailed.Checked == true || cbBlocked.Checked == true)
            {

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
                            valuesCases = jsonToXml.GetDataJson(nameFile, ref jsonFileCorrect, projectId, projectName,
                                cbPassed.Checked, cbFailed.Checked, cbBlocked.Checked);


                            //Пока не используется, т.к. не работает
                            //Если файл корректен, то перенос результатов в xml файл
                            //jsonToXml.FillXmlFile(projectName, testPlanName, testPlanId, valuesCases, out countValuesCases);
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

                            //Импорт тестов в Тестлинк               
                            testCaseTransferResults = TestLinkResult.ImportsRunInfoInTestLink(testPlanId, valuesCases, projectName);

                            //Закрытие окна закрузки
                            OpenLoadForm.Close();
                            OpenAllElementsMainForm("auto");

                            //если прерван перенос тесткейсов
                            if (testCaseTransferResults.Item2 == true)
                                MessageAddCasesInterrupted(valuesCases.Count, testCaseTransferResults.Item1);
                            else
                                MessageAddCasesTestlink(valuesCases.Count, testCaseTransferResults.Item1);
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
            else
                MessageBox.Show("Хотя бы один чекбокс должен быть отмечен!");
        }

        private void btnExpandTree_Click(object sender, EventArgs e)
        {
            treeView.ExpandAll();
        }

        private void btnCollapseTree_Click(object sender, EventArgs e)
        {
            treeView.CollapseAll();
        }

        private void MessageFileNotAdd()
        {
            lblMessageAddJson.Font = new Font(Label.DefaultFont, FontStyle.Bold);
            lblMessageAddJson.ForeColor = Color.DarkRed;
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

        private void MessageAddCasesInterrupted(int countTestCasesJenkins, int countSubmittedЕestСases)
        {
            lblAddCasesTestlink.Font = new Font(DefaultFont, FontStyle.Bold);
            lblAddCasesTestlink.ForeColor = Color.DarkRed;
            lblAddCasesTestlink.Text = $"✖ Перенос тест кейсов ​​в Testlink прерван!" +
                $" \r\n Тестов перенесено {countSubmittedЕestСases} из {countTestCasesJenkins}, имеющихся в json файле.";            
        }

        private void MessageAddCasesTestlink(int countTestCasesJenkins, int countSubmittedЕestСases)
        {
            lblAddCasesTestlink.Font = new Font(DefaultFont, FontStyle.Bold);
            lblAddCasesTestlink.ForeColor = Color.Green;
            lblAddCasesTestlink.Text = $"✔ Информация о прогоне успешно добавлена ​​в Testlink!" +
                $" \r\n Тестов перенесено {countSubmittedЕestСases} из {countTestCasesJenkins}, имеющихся в json файле.";
            if (countSubmittedЕestСases < countTestCasesJenkins)
                lblNotAllTestCasesRecognized.Text = "Не все тест кейсы были отправлены? \r\n" +
                     "Возможно, названия кейсов в Jenkins отличаются от названий в TestLink!";
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

            //Чекбоксы
            cbPassed.Enabled = false;
            cbBlocked.Enabled = false;
            cbFailed.Enabled = false;
        }

        private void OpenAllElementsMainForm(string testTransferMode)
        {
            MainFormMenu.Enabled = true;
            cbProjectNames.Enabled = true;
            cbTestPlanName.Enabled = true;
            btnAutoMode.Enabled = true;
            btnManualMode.Enabled = true;
            
            if (testTransferMode == "manual")
            {
                btnExpandTree.Enabled = true;
                btnCollapseTree.Enabled = true;
            }

            //Чекбоксы
            cbPassed.Enabled = true;
            cbBlocked.Enabled = true;
            cbFailed.Enabled = true;
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
