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

        //public UcTreeView treeView;
        public ImageList imageList;
        public TreeNode tNFolder, tNSubfolder, tNTestCase;

        private int projectId, testPlanId;
        private string projectName, testPlanName;
        private string pathFile = "../../../Files/";
        private string pathContent = "../../../Content/";
        private int IconFoldersAndfolders = 0, IconTestCases = 1;

        public static Folder[] folders;
        private Dictionary<string, string> manuallySelectedTests = new Dictionary<string, string>();

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
            imageList = new ImageList();
            imageList.Images.Add(Image.FromFile(pathContent + "folder.gif"));
            imageList.Images.Add(Image.FromFile(pathContent + "leaf.gif"));
            treeView.ImageList = imageList;
            treeView.ImageIndex = IconFoldersAndfolders;
            treeView.BorderStyle = BorderStyle.None;

            //Создание пустой папки дерева
            tNSubfolder = new TreeNode("Пусто");
            treeView.Nodes.Add(tNSubfolder);
            Controls.Add(treeView);
        }

        private Folder[] TreeCreate(Folder[] folders)
        {
            //Удаление пустой папки или старых
            treeView.Nodes.Clear();

            //Отображаем все папки на экране
            AddAllFolders(folders);
          
            //Для лечения бага прорисовки тесткейсов
            treeView.Visible = false;

            treeView.ExpandAll();
            treeView.CollapseAll();

            treeView.Visible = true;

            //Добавляем информацию по проекту в массив           
            AddProjectInfoForArrFolders(folders);

            return folders;
        }

        //Добавление всех тесткейсов в папки дерева
        private Dictionary<string, int> AddTestCasesInFolders(Folder[] folders, TreeNode tNTestCase, string nameFolder)
        {
            Dictionary<string, int> countTestCasesInFolder = new Dictionary<string, int>();

            int count = 0;
            foreach (var folder in folders)
            {
                if (tNTestCase.Text.Contains(folder.nameFolder))
                {
                    if (folder.testCases != null)
                    {
                        foreach (var testCase in folder.testCases)
                        {
                            if (testCase != null)
                            {
                                tNTestCase.Nodes.Add(new TreeNodeVirtual(CreateTestCaseFullName(testCase), IconTestCases, IconTestCases));
                                count++;
                            }
                        }
                    }
                }
            }
            countTestCasesInFolder.Add(nameFolder, count);

            return countTestCasesInFolder;
        }

        //Добавление всех папок в дерево
        private void AddAllFolders(Folder[] folders)
        {
            int valueCountTestCases = 0;

            foreach (var folder in folders)
            {
                tNFolder = new TreeNode(folder.nameFolder + $" ({CountingCasesInSubfolders(folder)})");
                treeView.Nodes.Add(tNFolder);

                AddSubfolders(folder, tNFolder);
               
                AddTestCasesInFolders(folders, tNFolder, folder.nameFolder);

                ////Проверка, что кол-во кейсов в папке соотвествует тому сколько их в массиве
                //foreach (var testCases in countTestCasesInFolder)
                //{
                //    if (folder.nameFolder == testCases.Key) 
                //    {
                //        if(folder.testCases.Length != testCases.Value) 
                //        {
                //            tNFolder.Text = folder.nameFolder + $" ({testCases.Value}) ";
                //            MessageBox.Show($"Ошибка! В папке {folder.nameFolder} не хватает тест кейсов! Всего должно быть: {folder.testCases.Length}." +
                //                $"\r\n Попробуйте перезагрузить программу! Или свяжитесь с разработчиком!");
                //        }
                //    }
                //}
            }
        }

        private int CountingCasesInSubfolders(Folder folder)
        {
            int countTestCasesInSubfolder = 0;
            if (folder.testCases != null)
            {
                countTestCasesInSubfolder = folder.testCases.Length;
            }
            else
            {
                for (int i = 0; i < folder.folders.Length; i++)
                {
                    countTestCasesInSubfolder += folder.folders[i].testCases.Length;
                }

                var countTestCasesInFolder = 0;
                if (folder.testCases != null)
                {
                    countTestCasesInFolder = folder.testCases.Length;
                }
                countTestCasesInSubfolder = countTestCasesInSubfolder + countTestCasesInFolder;                
            }

            return countTestCasesInSubfolder;
        }

        //Рекурсия
        //Добавление всех подпапок в дерево
        private void AddSubfolders(Folder folder, TreeNode treeNode)
        {
            var folders = folder.folders;
            foreach (var newFolder in folders)
            {
                //Добавление к имени папки количества тесткейсов, взятых из массива
                tNSubfolder = new TreeNode(newFolder.nameFolder + $" ({newFolder.testCases.Length})");
                treeNode.Nodes.Add(tNSubfolder);

                if (newFolder.folders != null)
                {
                    if (newFolder.folders.Length != 0)
                    {
                        foreach (TreeNode newTreeNode in treeNode.Nodes)
                        {
                            if(newTreeNode.Text.Contains(newFolder.nameFolder))
                                AddSubfolders(newFolder, newTreeNode);
                        }
                    }
                }
                AddTestCasesInFolders(folders, tNSubfolder, newFolder.nameFolder);
            }

        }           
        
        //Рекурсия
        //Добавление во все тест кейсы информацию о проекте (имя и id проекта, префикс)
        public void AddProjectInfoForArrFolders(Folder[] folders)
        {
            string prefixName = TestLinkResult.GetPrefixProjectByName(projectName);

            //Проходим папки первого уровня
            foreach (var folder in folders)
            {
                //если на первом уровне есть тесткейсы
                if (folder.testCases != null)
                {
                    foreach (var testCase in folder.testCases)
                    {
                        if (testCase != null)
                        {
                            if (testCase.nameTestCase != "" || testCase.nameTestCase != null)
                                testCase.project = new Project(projectId, projectName, prefixName);
                        }
                    }
                }
                //иначе смотрим подпапки
                else
                    ///Просмотр подпапок
                    AddProjectInfoForArrFolders(folder.folders);
            }
        }    

        private string CreateTestCaseFullName(InfoTestCase testCaseFullName)
        {
            var prefixName = TestLinkResult.GetPrefixProjectByName(projectName);      
            return prefixName + "-" + testCaseFullName.externalIdTestCase + ":" + testCaseFullName.nameTestCase;
        }                       

        private bool CheckCaseExistsInTestPlan(Dictionary<string, int> allTestCasesTestPlan, Dictionary<string, string> selectedTestsCases)
        {
            //Проверка существования кейса в тест плане
            foreach (var testCaseTestPlan in allTestCasesTestPlan)
            {
                foreach (var testCase in selectedTestsCases)
                {
                    if (testCaseTestPlan.Key == testCase.Key)                    
                        return true;                                      
                }                
            }
            return false;
        }

        //Рекурсия
        private Dictionary<string, string> GetTestCaseIdAndResultFromManuallySelectedTests(Folder[] folders)
        {           
            //Проходим папки первого уровня
            foreach (var folder in folders)
            {
                //если в папке есть тесткейсы
                if (folder.testCases != null)
                {
                    foreach (var testCase in folder.testCases)
                    {
                        if (testCase != null)
                        {
                            if (testCase.nameTestCase != "" || testCase.nameTestCase != null)
                            {
                                if (testCase.typeResult != "null")
                                {
                                    var fullExternalId = testCase.project.prefixName + "-" + testCase.externalIdTestCase;
                                    manuallySelectedTests.Add(fullExternalId, testCase.typeResult);
                                }
                            }
                        }
                    }
                }
                //иначе смотрим подпапки
                else
                    //Просмотр подпапок
                    GetTestCaseIdAndResultFromManuallySelectedTests(folder.folders);
            }           
            return manuallySelectedTests;
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

        //Кнопка Перенос тестов
        private void btCaseTransfer_Click(object sender, EventArgs e)
        {
            //Окно загрузки
            var openLoadForm = OpenFormLoadingScreen("Выполняется перенос результатов в TestLink...");

            //Добавить проверку на то что чекбоксы не пустые
            var allTestCasesTestPlan = TestLinkResult.GetTestCasesToTestPlan(testPlanId, projectName);
            var selectedTestsCases = GetTestCaseIdAndResultFromManuallySelectedTests(folders);

            if (selectedTestsCases.Count != 0)
            {
                //Проверка существования кейса в тест плане
                if (CheckCaseExistsInTestPlan(allTestCasesTestPlan, selectedTestsCases))
                {
                    //Перенос тестов в TestLink
                    TestLinkResult.ImportsRunInfoInTestLink(testPlanId, selectedTestsCases, projectName);
                }
                else
                    MessageBox.Show("Тест кейса не существует в тест плане!");

                //Закрытие окна закрузки и отображение кнопок
                openLoadForm.Close();
                OpenAllElementsMainForm("manual");

                //Очистка словаря
                manuallySelectedTests.Clear();
            }
            else
            {
                //Закрытие окна закрузки и отображение кнопок
                openLoadForm.Close();
                OpenAllElementsMainForm("manual");

                //Очистка словаря
                manuallySelectedTests.Clear();
                
                MessageBox.Show("Ни один тест кейс не был выбран!");
            }                      
        }

        //Кнопка Ручной режим
        private void btnManualMode_Click(object sender, EventArgs e)
        {   
            //Перед ручным режимом блокируем все элементы
            BlockAllElementsMainForm();

            //Окно загрузки
            var openLoadForm = OpenFormLoadingScreen("Получение информации о папках..");

            //OpenLoadForm.Size = this.Size;
            //OpenLoadForm.FormBorderStyle = FormBorderStyle.None;
            //OpenLoadForm.BackColor = Color.Black;//цвет фона
            //OpenLoadForm.Opacity = 0.4;

            //Постройка дерева
            TreeWithTestCases treeWithTestCases = new TreeWithTestCases();
            folders = TreeCreate(treeWithTestCases.FillArrayWithData(projectId));

            //Закрытие окна закрузки и отображение кнопок
            openLoadForm.Close();
            OpenAllElementsMainForm("manual");
        }

        //Кнопка Автоматический режим
        private void btnAutoMode_Click(object sender, EventArgs e)
        {
            bool FileExistence;
            var testCaseTransferResults = (0, false);
            int countValuesCases;
            Dictionary<string, string> valuesCases = new Dictionary<string, string>();

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
                            var openLoadForm = OpenFormLoadingScreen("Выполняется перенос результатов в TestLink...");

                            //Импорт тестов в Тестлинк               
                            testCaseTransferResults = TestLinkResult.ImportsRunInfoInTestLink(testPlanId, valuesCases, projectName);

                            //Закрытие окна закрузки
                            openLoadForm.Close();
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

        private LoadingScreen OpenFormLoadingScreen(string message)
        {
            LoadingScreen OpenLoadForm = new LoadingScreen(message);
            OpenLoadForm.Location = Location;
            OpenLoadForm.StartPosition = FormStartPosition.CenterScreen;
            OpenLoadForm.FormClosing += delegate { Show(); };
            OpenLoadForm.Show();

            return OpenLoadForm;
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
            btnCaseTransfer.Enabled = false;

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
                btnCaseTransfer.Enabled = true;
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
