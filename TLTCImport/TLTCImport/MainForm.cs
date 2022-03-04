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
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace TLTCImport
{
    public partial class MainForm : Form
    {
        const string defaultMainNodeName = "TLTC Import";

        private Panel leftPanel;
        private Panel topPanel;

        private Label lblSoftware, lblCurrentTestPlan, lblTestRun, lblJson;  
        
        private Button btnAutoMode, btnManualMode;    

        private ComboBox cbProjectNames, cbTestPlanName;    

        private bool beSearchTest = false;
        private string pathJsonFile = "../../../JsonFiles/";

        //Поля для word импорта
        private bool outWordDocumentMode = false;
        ImportWithWordDocumnet wordImporter;

        Dictionary<string, List<string>> testNames = new Dictionary<string, List<string>>();

        XElement testCasesXml;
        XElement testCasesXmlExport;

        public MainForm()
        {
            InitializeComponent();
            
            var font = new Font("Century Gothic", 12);
            
            leftPanel = new Panel();
            leftPanel.Dock = DockStyle.Left;
            leftPanel.Size = new Size(210, leftPanel.Size.Height);
            leftPanel.BackColor = Color.DarkGray;

            topPanel = new Panel();
            topPanel.Dock = DockStyle.Top;
            topPanel.BackColor = Color.DarkGray;

            //Текст Программный продукт
            lblSoftware = new Label();
            lblSoftware.BackColor = Color.Gray;
            lblSoftware.Name = "lblSoftware";
            lblSoftware.Text = "Программный продукт:";
            lblSoftware.Size = new Size(200, 30);
            lblSoftware.Location = new Point(5, 5);
            lblSoftware.TextAlign = ContentAlignment.MiddleLeft;
            Controls.Add(lblSoftware);

            //Комбобокс для выбора программного продукта
            cbProjectNames = new ComboBox();
            cbProjectNames.Size = new Size(200, 30);
            cbProjectNames.Location = new Point(5, 35);
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
            lblCurrentTestPlan.Location = new Point(5, 60);
            lblCurrentTestPlan.TextAlign = ContentAlignment.MiddleLeft;
            Controls.Add(lblCurrentTestPlan);

            //Комбобокс для выбора Текущего тестового-плана:
            cbTestPlanName = new ComboBox();
            cbTestPlanName.Size = new Size(200, 30);
            cbTestPlanName.Location = new Point(5, 90);
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
            lblTestRun.Location = new Point(5, 120);
            lblTestRun.TextAlign = ContentAlignment.MiddleLeft;
            Controls.Add(lblTestRun);

            //Кнопка Автоматизированный режим
            btnAutoMode = new Button();
            btnAutoMode.FlatStyle = FlatStyle.Flat;
            btnAutoMode.BackColor = Color.DarkGray;            
            btnAutoMode.FlatAppearance.BorderSize = 0;
            btnAutoMode.Name = "AutoMode";
            btnAutoMode.Size = new Size(200, 30);
            btnAutoMode.Location = new Point(5, 150);
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
            btnManualMode.Location = new Point(5, 180);
            btnManualMode.Text = "Ручной режим";
            btnManualMode.Click += new EventHandler(btnManualMode_Click);
            btnManualMode.Enabled = false;
            Controls.Add(btnManualMode);

            //Текст отображающий сообщение о добавлении файла
            lblJson = new Label();
            lblJson.BackColor = Color.DarkGray;
            lblJson.Size = new Size(200, 30);
            lblJson.Location = new Point(5, 220);
            lblJson.TextAlign = ContentAlignment.MiddleCenter;
            Controls.Add(lblJson);

            //checkBoxForMainFolderName = new CheckBox();
            //checkBoxForMainFolderName.BackColor = Color.FromArgb(40, 40, 40);
            //checkBoxForMainFolderName.Name = "chbForMainFolderName";
            //checkBoxForMainFolderName.Size = new Size(20, 20);
            //checkBoxForMainFolderName.Location = new Point(185, 35);
            //checkBoxForMainFolderName.CheckedChanged += new EventHandler(chbForMainFolderName_CheckedChanged);
            //Controls.Add(checkBoxForMainFolderName);

            //textBoxForMainNodeName = new TextBox();
            //textBoxForMainNodeName.Name = "tbForMainNodeName";
            //textBoxForMainNodeName.Size = new Size(400, 30);
            //textBoxForMainNodeName.Location = new Point(215, 35);
            //textBoxForMainNodeName.Text = defaultMainNodeName;
            //textBoxForMainNodeName.Enabled = false;
            //Controls.Add(textBoxForMainNodeName);

            //btnSetDllPath = new Button();
            //btnSetDllPath.FlatStyle = FlatStyle.Flat;
            //btnSetDllPath.BackColor = Color.FromArgb(40, 40, 40);
            //btnSetDllPath.FlatAppearance.BorderSize = 0;
            //btnSetDllPath.Name = "SetDllPath";
            //btnSetDllPath.Size = new Size(200, 30);
            //btnSetDllPath.Location = new Point(5, 70);
            //btnSetDllPath.Text = "Open DLL file";
            //btnSetDllPath.Click += new EventHandler(btnSetDllPath_Click);
            //Controls.Add(btnSetDllPath);

            //lblDllPath = new Label();
            //lblDllPath.Name = "DllPath";
            //lblDllPath.Location = new Point(210, 70);
            //lblDllPath.Size = new Size(400, 30);
            //Controls.Add(lblDllPath);            

            //btnFindTests = new Button();
            //btnFindTests.FlatStyle = FlatStyle.Flat;
            //btnFindTests.BackColor = Color.FromArgb(40, 40, 40);
            //btnFindTests.FlatAppearance.BorderSize = 0;
            //btnFindTests.Name = "FindTests";
            //btnFindTests.Size = new Size(200, 30);
            //btnFindTests.Location = new Point(5, 140);
            //btnFindTests.Text = "Find Tests";
            //btnFindTests.Click += new EventHandler(btnFindTests_Click);
            //btnFindTests.Enabled = false;
            //Controls.Add(btnFindTests);

            //lblTotalTests = new Label();
            //lblTotalTests.Name = "TotalTests";
            //lblTotalTests.Location = new Point(215, 140);
            //lblTotalTests.Size = new Size(400, 30);
            //Controls.Add(lblTotalTests);

            //lblForModeWithWord = new Label();
            //lblForModeWithWord.BackColor = Color.FromArgb(40, 40, 40);
            //lblForModeWithWord.Name = "lblForModeWithWord";
            //lblForModeWithWord.Text = "Use Word-mode";
            //lblForModeWithWord.Size = new Size(180, 30);
            //lblForModeWithWord.Location = new Point(5, 105);
            //Controls.Add(lblForModeWithWord);

            //checkBoxForModeWithWord = new CheckBox();
            //checkBoxForModeWithWord.BackColor = Color.FromArgb(40, 40, 40);
            //checkBoxForModeWithWord.Name = "chbForModeWithWord";
            //checkBoxForModeWithWord.Size = new Size(20, 20);
            //checkBoxForModeWithWord.Location = new Point(185, 105);
            //checkBoxForModeWithWord.CheckedChanged += new EventHandler(chbForModeWithWord_CheckedChanged);
            //Controls.Add(checkBoxForModeWithWord);

            //btnImport = new Button();
            //btnImport.FlatStyle = FlatStyle.Flat;
            //btnImport.BackColor = Color.FromArgb(40, 40, 40);
            //btnImport.FlatAppearance.BorderSize = 0;
            //btnImport.Name = "ImportTests";
            //btnImport.Size = new Size(200, 30);
            //btnImport.Location = new Point(5, 210);
            //btnImport.Text = "Import Tests";
            //btnImport.Click += new EventHandler(btnImportTests_Click);
            //btnImport.Enabled = false;
            //Controls.Add(btnImport);

            //lblImportComplete = new Label();
            //lblImportComplete.Text = "Import complete";
            //lblImportComplete.Location = new Point(210, 210);
            //lblImportComplete.Size = new Size(400, 30);
            //lblImportComplete.Enabled = false;
            //Controls.Add(lblImportComplete);            

            //Добавление панелей на экран
            Controls.Add(leftPanel);           
            Controls.Add(topPanel);
        }

        private void SetProjectNames(ComboBox comboBox)
        {
            foreach(var item in TlReportTcResult.GetAllProjects())
            {
                comboBox.Items.Add(item.name);
            }
        }

        private void SetTestPlanName(ComboBox comboBox, int projectId)
        {
            foreach (var item in TlReportTcResult.GetAllProjectTestPlans(projectId))
            {
                comboBox.Items.Add(item.name);
                cbTestPlanName.Enabled = true;
            }
        }

        private void cbProjectNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            int projectId = TlReportTcResult.GetProjectIdByName(cbProjectNames.SelectedItem.ToString());
            if (projectId != 0)
                SetTestPlanName(cbTestPlanName, projectId);
            else MessageBox.Show("Ошибка! Проект не выбран или не существует!");
        }

        private void cbTestPlanName_SelectedIndexChanged(object sender, EventArgs e)
        {
            int testPlanId = TlReportTcResult.GetTestPlanIdByName(cbProjectNames.SelectedItem.ToString(), cbTestPlanName.SelectedItem.ToString());
            if (testPlanId != 0)
            {
                btnAutoMode.Enabled = true;
                btnManualMode.Enabled = true;
            }
            else MessageBox.Show("Ошибка! Проект не выбран или не существует!");
        }

        //private void InitializeImportTestCasesFile()
        //{
        //    if(checkBoxForMainFolderName.Checked) testCasesXml = new XElement("testsuite", new XAttribute("name", textBoxForMainNodeName.Text));
        //    else testCasesXml = new XElement("testsuite", new XAttribute("name", defaultMainNodeName));
        //}

        //private void chbForMainFolderName_CheckedChanged(object sender, EventArgs e)
        //{
        //    textBoxForMainNodeName.Enabled = checkBoxForMainFolderName.Checked;
        //}

        //private void chbForModeWithWord_CheckedChanged(object sender, EventArgs e)
        //{
        //    outWordDocumentMode = checkBoxForModeWithWord.Checked;
        //}      

        private void btnManualMode_Click(object sender, EventArgs e)
        {
         
        }

        private void btnAutoMode_Click(object sender, EventArgs e)
        {           
            //MessageBox.Show("Выберите файл в формете .json. Потом написать инструкцию");

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "json files (*.json)|*.json";

            DialogResult result = fileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string urlUploadedFile = fileDialog.FileName;
                string[] names = urlUploadedFile.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                string nameFile = names[names.Length - 1].Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[0];

                CopyFileInProject(urlUploadedFile, nameFile);

                lblJson.Font = new Font(Label.DefaultFont, FontStyle.Bold);
                lblJson.ForeColor = Color.Green;
                lblJson.Text = "Файл \"" + nameFile + ".json" + "\" успешно добавлен";

                JsonToXml jsonToXml = new JsonToXml();
                jsonToXml.TransformJsonToXml(nameFile);
            }
            else
            {
                lblJson.Font = new Font(Label.DefaultFont, FontStyle.Bold);
                lblJson.ForeColor = Color.Red;
                lblJson.Text = "Файл не был добавлен";
            }
        }               

        private void CopyFileInProject(string urlUploadedFile, string nameFile)
        {
            if (File.Exists(pathJsonFile + nameFile + ".json"))
            {
                //MessageBox.Show("Файл уже загружен в систему! Вы уверены, что хотите его изменить?", "Изменение существующего файла", MessageBoxButtons.YesNo);
                File.Delete(pathJsonFile + nameFile + ".json");
                File.Copy(urlUploadedFile, pathJsonFile + nameFile + ".json");
            }
            else             
                File.Copy(urlUploadedFile, pathJsonFile + nameFile + ".json");            
        }
       

        private void GetTestData(ComboBox comboBox, int projectId)
        {
            foreach (var item in TlReportTcResult.GetAllProjectTestPlans(projectId))
            {
                comboBox.Items.Add(item.name);
                cbTestPlanName.Enabled = true;
            }
        }

//        private void btnFindTests_Click(object sender, EventArgs e)
//        {
//            if (!beSearchTest) //Если поиска тестов еще не было, то осуществляется поиск и блокируется возможность изменения введенных данных
//            {
//                if (outWordDocumentMode)
//                {
//                    wordImporter = new ImportWithWordDocumnet();
//                    wordImporter.ReadAndWrightTestCasesOutWordDocument();
//                }

//                testNames.Clear();

//                InitializeImportTestCasesFile(); //Инициализирует xml файл, который будет отправляться через запрос для осуществления импорта
//                ExportTestCases(); //Экспорт уже имеющихся кейсов перед началом составления xml файла для импорта (для проверки на наличие уже имеющихся кейсов)


//                var assembly = Assembly.LoadFile(lblDllPath.Text);

//                int testsCount = 0, testsCountForCreate = 0;

//                System.Diagnostics.Debug.WriteLine("Types count = " + assembly.GetTypes().Length);

//                foreach (Type t in assembly.GetTypes())
//                {
//#if (DEBUG)
///*
//                    if (testsCountForCreate == 1)
//                    {
//                        break;
//                    }
//*/
//#endif
//                    //Создание тест-кейса чисто на основе названия реестра
//                    {
//                        System.Diagnostics.Debug.WriteLine(t.FullName);

//                        string namespaceCurrentType = t.FullName;
//                        string stringIndexOf = "Карта_Тестирования.Suites";

//                        if (namespaceCurrentType.Contains(stringIndexOf)) //&& namespaceCurrentType == "ЭБ.Карта_Тестирования.Suites.РРО_и_ОБАС.Costs")
//                        {
//                            testsCount++;

//                            //Выборка последнего слова в пространстве имен (с конца до первой точки)
//                            int intStartIndexOfForTestName = namespaceCurrentType.Length - 1;

//                            for (; intStartIndexOfForTestName >= 0; intStartIndexOfForTestName--)
//                            {
//                                if (namespaceCurrentType[intStartIndexOfForTestName] == '.')
//                                {
//                                    break;
//                                }
//                            }

//                            string testName = namespaceCurrentType.Substring(intStartIndexOfForTestName + 1);
//                            string description = "";

//                            int intStartIndexOfForTestSuites = namespaceCurrentType.IndexOf(stringIndexOf);
//                            string directory = namespaceCurrentType.Substring(intStartIndexOfForTestSuites + stringIndexOf.Length + 1);//, namespaceCurrentType.Length - intStartIndexOfForTestSuites - stringIndexOf.Length - testName.Length - 1);
//                            //Удаление последнего слова в пространстве имен (вместе с точкой) (оно является названием реестра и используется как название теста)
//                            int lengthForRemove = directory.Length - testName.Length - 1;

//                            if (lengthForRemove > 0) //Данная ситуация происходит, когда класс располагается в папке stringIndexOf - предполагается, что тогда он создан не для какого-либо раздела в карте тестирования
//                            {
//                                directory = directory.Remove(lengthForRemove);
//                                System.Diagnostics.Debug.WriteLine(directory);
//                                System.Diagnostics.Debug.WriteLine(testName);


//                                if (AddTestCase(testName, description, directory)) testsCountForCreate++;
//                            }
//                        }
//                    }


//                    //Просмотр методов и их атрибутов
//                    /*
//                    foreach (MethodInfo method in t.GetMethods())
//                    {
//                        string methodFullPath = method.DeclaringType.FullName;

//                        if (!methodFullPath.Contains(stringIndexOf))// !methodFullPath.Contains("Suites"))
//                            continue;

//                        methodFullPath = methodFullPath.Substring(methodFullPath.IndexOf(stringIndexOf) + stringIndexOf.Length + 1);

//                        foreach (TestCaseAttribute attr in method.GetCustomAttributes(typeof(TestCaseAttribute)))
//                        {
//                            var args = attr.Arguments;
//                            string argsStr = FormArgsStr(args);

//                            string testName = attr.TestName;
//                            string description = attr.Description;


//                            if (string.IsNullOrEmpty(testName))
//                                testName = method.Name;
//                            if (string.IsNullOrEmpty(description))
//                                description = null;

//                            //testName = XmlEscapeSymbols(testName);
//                            //description = XmlEscapeSymbols(description);

//                            AddTestCase(testName, description, methodFullPath, argsStr);

//                            testsCount++;
//                        }

//                        foreach (TestAttribute attr in method.GetCustomAttributes(typeof(TestAttribute)))
//                        {
//                            string testName = method.Name;
//                            string description = attr.Description;

//                            //description = XmlEscapeSymbols(description);

//                            AddTestCase(testName, description, methodFullPath);

//                            testsCount++;
//                        }
//                    }*/
//                }


        //        //Добавление в xml оставшихся кейсов из word, для которых нет автотестов в сборке
        //        if (outWordDocumentMode) AddAllAnothTestOutWordToXml();


        //        btnFindTests.Text = "Change entered values";

        //        cbProjectNames.Enabled = false;
        //        checkBoxForMainFolderName.Enabled = false;
        //        textBoxForMainNodeName.Enabled = false;

        //        btnAutoMode.Enabled = false;
        //        btnManualMode.Enabled = false;

        //        checkBoxForModeWithWord.Enabled = false;

        //        btnImport.Enabled = true;
        //        lblTotalTests.Text = "Tests count: " + testsCount + " Tests count for create: " + testsCountForCreate;

        //        beSearchTest = true;
        //    }
        //    else // иначе, если поиск уже был, то позволяет повторно ввести данные
        //    {
        //        btnFindTests.Text = "Find Tests";

        //        cbProjectNames.Enabled = true;
        //        checkBoxForMainFolderName.Enabled = true;
        //        textBoxForMainNodeName.Enabled = checkBoxForMainFolderName.Checked;

        //        btnAutoMode.Enabled = true;
        //        btnManualMode.Enabled = true;

        //        checkBoxForModeWithWord.Enabled = true;

        //        btnImport.Enabled = false;
        //        lblTotalTests.Text = "";

        //        btnFindTests.Enabled = true;
        //        lblImportComplete.Enabled = false;

        //        beSearchTest = false;
        //    }
        //}

        /// <summary>
        /// Сформировать строку с параметрами, используемыми в тесте
        /// </summary>
        /// <param name="args">Массив параметров</param>
        private string FormArgsStr(object[] args) 
        {
            string argsStr = "Параметры: ";
            if (args.Count() > 0)
            {
                for (int i = 0; i < args.Count(); i++)
                {
                    argsStr += $"{args[i]}";
                    if (i != args.Count() - 1)
                    {
                        argsStr += ", ";
                    }
                }
            }
            return argsStr;
        }


        private string XmlEscapeSymbols(string str) 
        {
            if (str == null) 
            {
                return str;
            }
            //str = str.Replace("& ", "&amp; ");
            //str = str.Replace("\"", "&quot;");
            str = str.Replace("\'", "&apos;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            
            return str;
        }

        private bool AddTestCase(string testName, string description, string hierarchy, string argsStr = null)
        {
            if (!testNames.ContainsKey(hierarchy))
            {
                testNames.Add(hierarchy, new List<string>() { testName });
            }
            else
            {
                if (!testNames[hierarchy].Contains(testName))
                    testNames[hierarchy].Add(testName);
                else return false;
            }


            /* //При наличии тесткейса с таким же именем добавляет к имени количество уже имеющихся вхождений и добавляет в список
            else
            {
                if (testNames[hierarchy].Contains(testName))
                {
                    int elementCount = testNames[hierarchy].Select(x => x == testName).Count();

                    testName += " (" + elementCount + ")";
                }

                testNames[hierarchy].Add(testName);
            }*/

            #region old
            /*
            //             XElement testCase = new XElement("testcase", 
            //                 new XAttribute("name", testName),
            //                 new XElement("summary", description));
            */
            #endregion

            XElement testCaseEl;

            if (outWordDocumentMode)
            {
                testCaseEl = wordImporter.GetXmlElementForRegistry(testName);

                if(testCaseEl == null) testCaseEl = new XElement("testcase", new XAttribute("name", "_empty_" + testName));
            }
            else
            {
                testCaseEl = new XElement("testcase", new XAttribute("name", testName));
                if (!string.IsNullOrEmpty(description))
                {
                    testCaseEl.Add(new XElement("summary", description));
                }
                if (!string.IsNullOrEmpty(argsStr))
                {
                    testCaseEl.Add(new XElement("preconditions", argsStr));
                }
            }

            return AddTestCaseToXml(hierarchy, testCaseEl);
        }

        private bool AddTestCaseToXml(string pathToSuite, XElement testCase)
        {
            XElement currentNode = testCasesXml;

            string[] splittedPath = pathToSuite.Split(".");

            foreach (var item in splittedPath)
            {
                string suiteName = item.Replace("_", " ");

                XElement result = currentNode.Descendants("testsuite")
                    .FirstOrDefault(el => (string)el.Attribute("name") == suiteName);

                if (result != null)
                {
                    currentNode = result;
                }
                else
                {
                    XElement newNode = new XElement("testsuite", new XAttribute("name", suiteName));
                    currentNode.Add(newNode);
                    currentNode = newNode;
                }
            }

            XElement resultForTestCase = null;

            if (testCasesXmlExport != null)
                resultForTestCase = testCasesXmlExport.Descendants("testcase")
                        .FirstOrDefault(el => (string)el.Attribute("name") == (string)testCase.Attribute("name"));

            //Если результат поиска уже имеющегося TestCase в рассматриваемой директории не успешен, то добавление нового кейса (иначе ничего не делает - чтобы не создавать кейс, который уже есть) 
            if (resultForTestCase == null)
            {
                currentNode.Add(testCase);
                return true;
            }
            else return false;
        }

        //Добавление всех оставшихся тестов из word, которых не было в файле сборки
        public void AddAllAnothTestOutWordToXml()
        {
            List<XElement> tempList = wordImporter.GetXmlElementForAnothRegistry();

            XElement newNode = new XElement("testsuite", new XAttribute("name", "Прочие реестры"));
            testCasesXml.Add(newNode);

            foreach (XElement tempXml in tempList)
            {
                newNode.Add(tempXml);
            }
        }         

        //private void ExportTestCases()
        //{
        //    string path = Application.ExecutablePath.Replace("TLTCImport.dll", "Content\\");
        //    string pathToExport = path + "\\ExportTestCases.xml";

        //    int projectId = TlReportTcResult.GetProjectIdByName(cbProjectNames.SelectedItem.ToString());

        //    var testsuites = TlReportTcResult.GetAllTestProjectSuites(projectId);

        //    string nameMainFolder = defaultMainNodeName;
        //    if (checkBoxForMainFolderName.Checked) nameMainFolder = textBoxForMainNodeName.Text;

        //    foreach (var testSuite in testsuites)
        //    {
        //        if (testSuite.name == nameMainFolder)
        //        {
        //            testCasesXmlExport = TlReportTcResult.ExportTestSuite(testSuite.id);
        //            testCasesXmlExport.Save(pathToExport);
        //            break;
        //        }
        //    }
        //}    
    }
}
