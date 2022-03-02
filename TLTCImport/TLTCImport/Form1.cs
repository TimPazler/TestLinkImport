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
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace TLTCImport
{
    public partial class Form1 : Form
    {
        const string defaultMainNodeName = "TLTC Import";

        private Panel leftPanel;
        private Panel topPanel;

        private Label lblSetMainFolderName;
        private TextBox textBoxForMainNodeName;
        private CheckBox checkBoxForMainFolderName;
        
        private Button btnSetDllPath;
        private Button btnFindTests;
        private Label lblDllPath;
        private Label lblTotalTests;

        private ComboBox cbProjectNames;

        private Label lblForModeWithWord;
        private CheckBox checkBoxForModeWithWord;

        private Button btnImport;
        private Label lblImportComplete;

        private bool beSearchTest = false;

        //Поля для word импорта
        private bool outWordDocumentMode = false;
        ImportWithWordDocumnet wordImporter;

        Dictionary<string, List<string>> testNames = new Dictionary<string, List<string>>();

        XElement testCasesXml;
        XElement testCasesXmlExport;

        public Form1()
        {
            InitializeComponent();


            cbProjectNames = new ComboBox();
            cbProjectNames.Size = new Size(200, 30);
            cbProjectNames.Location = new Point(5, 0);
            cbProjectNames.DropDownStyle = ComboBoxStyle.DropDown;
            SetProjectNames(cbProjectNames);
            Controls.Add(cbProjectNames);

            lblSetMainFolderName = new Label();
            lblSetMainFolderName.BackColor = Color.FromArgb(40, 40, 40);
            lblSetMainFolderName.Name = "lblSetMainFolderName";
            lblSetMainFolderName.Text = "Set name main folder in TestLink";
            lblSetMainFolderName.Size = new Size(180, 30);
            lblSetMainFolderName.Location = new Point(5, 35);
            Controls.Add(lblSetMainFolderName);

            checkBoxForMainFolderName = new CheckBox();
            checkBoxForMainFolderName.BackColor = Color.FromArgb(40, 40, 40);
            checkBoxForMainFolderName.Name = "chbForMainFolderName";
            checkBoxForMainFolderName.Size = new Size(20, 20);
            checkBoxForMainFolderName.Location = new Point(185, 35);
            checkBoxForMainFolderName.CheckedChanged += new EventHandler(chbForMainFolderName_CheckedChanged);
            Controls.Add(checkBoxForMainFolderName);

            textBoxForMainNodeName = new TextBox();
            textBoxForMainNodeName.Name = "tbForMainNodeName";
            textBoxForMainNodeName.Size = new Size(400, 30);
            textBoxForMainNodeName.Location = new Point(215, 35);
            textBoxForMainNodeName.Text = defaultMainNodeName;
            textBoxForMainNodeName.Enabled = false;
            Controls.Add(textBoxForMainNodeName);

            btnSetDllPath = new Button();
            btnSetDllPath.FlatStyle = FlatStyle.Flat;
            btnSetDllPath.BackColor = Color.FromArgb(40, 40, 40);
            btnSetDllPath.FlatAppearance.BorderSize = 0;
            btnSetDllPath.Name = "SetDllPath";
            btnSetDllPath.Size = new Size(200, 30);
            btnSetDllPath.Location = new Point(5, 70);
            btnSetDllPath.Text = "Open DLL file";
            btnSetDllPath.Click += new EventHandler(btnSetDllPath_Click);
            Controls.Add(btnSetDllPath);

            lblDllPath = new Label();
            lblDllPath.Name = "DllPath";
            lblDllPath.Location = new Point(210, 70);
            lblDllPath.Size = new Size(400, 30);
            Controls.Add(lblDllPath);            

            btnFindTests = new Button();
            btnFindTests.FlatStyle = FlatStyle.Flat;
            btnFindTests.BackColor = Color.FromArgb(40, 40, 40);
            btnFindTests.FlatAppearance.BorderSize = 0;
            btnFindTests.Name = "FindTests";
            btnFindTests.Size = new Size(200, 30);
            btnFindTests.Location = new Point(5, 140);
            btnFindTests.Text = "Find Tests";
            btnFindTests.Click += new EventHandler(btnFindTests_Click);
            btnFindTests.Enabled = false;
            Controls.Add(btnFindTests);

            lblTotalTests = new Label();
            lblTotalTests.Name = "TotalTests";
            lblTotalTests.Location = new Point(215, 140);
            lblTotalTests.Size = new Size(400, 30);
            Controls.Add(lblTotalTests);


            lblForModeWithWord = new Label();
            lblForModeWithWord.BackColor = Color.FromArgb(40, 40, 40);
            lblForModeWithWord.Name = "lblForModeWithWord";
            lblForModeWithWord.Text = "Use Word-mode";
            lblForModeWithWord.Size = new Size(180, 30);
            lblForModeWithWord.Location = new Point(5, 105);
            Controls.Add(lblForModeWithWord);

            checkBoxForModeWithWord = new CheckBox();
            checkBoxForModeWithWord.BackColor = Color.FromArgb(40, 40, 40);
            checkBoxForModeWithWord.Name = "chbForModeWithWord";
            checkBoxForModeWithWord.Size = new Size(20, 20);
            checkBoxForModeWithWord.Location = new Point(185, 105);
            checkBoxForModeWithWord.CheckedChanged += new EventHandler(chbForModeWithWord_CheckedChanged);
            Controls.Add(checkBoxForModeWithWord);


            btnImport = new Button();
            btnImport.FlatStyle = FlatStyle.Flat;
            btnImport.BackColor = Color.FromArgb(40, 40, 40);
            btnImport.FlatAppearance.BorderSize = 0;
            btnImport.Name = "ImportTests";
            btnImport.Size = new Size(200, 30);
            btnImport.Location = new Point(5, 210);
            btnImport.Text = "Import Tests";
            btnImport.Click += new EventHandler(btnImportTests_Click);
            btnImport.Enabled = false;
            Controls.Add(btnImport);

            lblImportComplete = new Label();
            lblImportComplete.Text = "Import complete";
            lblImportComplete.Location = new Point(210, 210);
            lblImportComplete.Size = new Size(400, 30);
            lblImportComplete.Enabled = false;
            Controls.Add(lblImportComplete);

            leftPanel = new Panel();
            leftPanel.Dock = DockStyle.Left;
            leftPanel.Size = new Size(210, leftPanel.Size.Height);
            leftPanel.BackColor = Color.FromArgb(28, 28, 28);
            Controls.Add(leftPanel);

            topPanel = new Panel();
            topPanel.Dock = DockStyle.Top;
            topPanel.BackColor = Color.FromArgb(28, 28, 28);
            Controls.Add(topPanel);
        }

        private void SetProjectNames(ComboBox comboBox)
        {
            foreach(var item in TlReportTcResult.GetAllProjects())
            {
                comboBox.Items.Add(item.name);
            }
        }

        private void InitializeImportTestCasesFile()
        {
            if(checkBoxForMainFolderName.Checked) testCasesXml = new XElement("testsuite", new XAttribute("name", textBoxForMainNodeName.Text));
            else testCasesXml = new XElement("testsuite", new XAttribute("name", defaultMainNodeName));
        }

        private void chbForMainFolderName_CheckedChanged(object sender, EventArgs e)
        {
            textBoxForMainNodeName.Enabled = checkBoxForMainFolderName.Checked;
        }

        private void chbForModeWithWord_CheckedChanged(object sender, EventArgs e)
        {
            outWordDocumentMode = checkBoxForModeWithWord.Checked;
        }

        private void btnSetDllPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Dll files (*.dll) | *.dll;";

            DialogResult result = fileDialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                lblDllPath.Text = fileDialog.FileName;

                btnFindTests.Enabled = true;
            }
        }

        private void btnFindTests_Click(object sender, EventArgs e)
        {
            if (!beSearchTest) //Если поиска тестов еще не было, то осуществляется поиск и блокируется возможность изменения введенных данных
            {
                if (outWordDocumentMode)
                {
                    wordImporter = new ImportWithWordDocumnet();
                    wordImporter.ReadAndWrightTestCasesOutWordDocument();
                }

                testNames.Clear();

                InitializeImportTestCasesFile(); //Инициализирует xml файл, который будет отправляться через запрос для осуществления импорта
                ExportTestCases(); //Экспорт уже имеющихся кейсов перед началом составления xml файла для импорта (для проверки на наличие уже имеющихся кейсов)


                var assembly = Assembly.LoadFile(lblDllPath.Text);

                int testsCount = 0, testsCountForCreate = 0;

                System.Diagnostics.Debug.WriteLine("Types count = " + assembly.GetTypes().Length);

                foreach (Type t in assembly.GetTypes())
                {
#if (DEBUG)
/*
                    if (testsCountForCreate == 1)
                    {
                        break;
                    }
*/
#endif
                    //Создание тест-кейса чисто на основе названия реестра
                    {
                        System.Diagnostics.Debug.WriteLine(t.FullName);

                        string namespaceCurrentType = t.FullName;
                        string stringIndexOf = "Карта_Тестирования.Suites";

                        if (namespaceCurrentType.Contains(stringIndexOf)) //&& namespaceCurrentType == "ЭБ.Карта_Тестирования.Suites.РРО_и_ОБАС.Costs")
                        {
                            testsCount++;

                            //Выборка последнего слова в пространстве имен (с конца до первой точки)
                            int intStartIndexOfForTestName = namespaceCurrentType.Length - 1;

                            for (; intStartIndexOfForTestName >= 0; intStartIndexOfForTestName--)
                            {
                                if (namespaceCurrentType[intStartIndexOfForTestName] == '.')
                                {
                                    break;
                                }
                            }

                            string testName = namespaceCurrentType.Substring(intStartIndexOfForTestName + 1);
                            string description = "";

                            int intStartIndexOfForTestSuites = namespaceCurrentType.IndexOf(stringIndexOf);
                            string directory = namespaceCurrentType.Substring(intStartIndexOfForTestSuites + stringIndexOf.Length + 1);//, namespaceCurrentType.Length - intStartIndexOfForTestSuites - stringIndexOf.Length - testName.Length - 1);
                            //Удаление последнего слова в пространстве имен (вместе с точкой) (оно является названием реестра и используется как название теста)
                            int lengthForRemove = directory.Length - testName.Length - 1;

                            if (lengthForRemove > 0) //Данная ситуация происходит, когда класс располагается в папке stringIndexOf - предполагается, что тогда он создан не для какого-либо раздела в карте тестирования
                            {
                                directory = directory.Remove(lengthForRemove);
                                System.Diagnostics.Debug.WriteLine(directory);
                                System.Diagnostics.Debug.WriteLine(testName);


                                if (AddTestCase(testName, description, directory)) testsCountForCreate++;
                            }
                        }
                    }


                    //Просмотр методов и их атрибутов
                    /*
                    foreach (MethodInfo method in t.GetMethods())
                    {
                        string methodFullPath = method.DeclaringType.FullName;

                        if (!methodFullPath.Contains(stringIndexOf))// !methodFullPath.Contains("Suites"))
                            continue;

                        methodFullPath = methodFullPath.Substring(methodFullPath.IndexOf(stringIndexOf) + stringIndexOf.Length + 1);

                        foreach (TestCaseAttribute attr in method.GetCustomAttributes(typeof(TestCaseAttribute)))
                        {
                            var args = attr.Arguments;
                            string argsStr = FormArgsStr(args);

                            string testName = attr.TestName;
                            string description = attr.Description;


                            if (string.IsNullOrEmpty(testName))
                                testName = method.Name;
                            if (string.IsNullOrEmpty(description))
                                description = null;

                            //testName = XmlEscapeSymbols(testName);
                            //description = XmlEscapeSymbols(description);

                            AddTestCase(testName, description, methodFullPath, argsStr);

                            testsCount++;
                        }

                        foreach (TestAttribute attr in method.GetCustomAttributes(typeof(TestAttribute)))
                        {
                            string testName = method.Name;
                            string description = attr.Description;

                            //description = XmlEscapeSymbols(description);

                            AddTestCase(testName, description, methodFullPath);

                            testsCount++;
                        }
                    }*/
                }


                //Добавление в xml оставшихся кейсов из word, для которых нет автотестов в сборке
                if (outWordDocumentMode) AddAllAnothTestOutWordToXml();


                btnFindTests.Text = "Change entered values";

                cbProjectNames.Enabled = false;
                checkBoxForMainFolderName.Enabled = false;
                textBoxForMainNodeName.Enabled = false;

                btnSetDllPath.Enabled = false;

                checkBoxForModeWithWord.Enabled = false;

                btnImport.Enabled = true;
                lblTotalTests.Text = "Tests count: " + testsCount + " Tests count for create: " + testsCountForCreate;

                beSearchTest = true;
            }
            else // иначе, если поиск уже был, то позволяет повторно ввести данные
            {
                btnFindTests.Text = "Find Tests";

                cbProjectNames.Enabled = true;
                checkBoxForMainFolderName.Enabled = true;
                textBoxForMainNodeName.Enabled = checkBoxForMainFolderName.Checked;

                btnSetDllPath.Enabled = true;

                checkBoxForModeWithWord.Enabled = true;

                btnImport.Enabled = false;
                lblTotalTests.Text = "";

                btnFindTests.Enabled = true;
                lblImportComplete.Enabled = false;

                beSearchTest = false;
            }
        }

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

        private void btnImportTests_Click(object sender, EventArgs e)
        {
            btnImport.Enabled = false;


            string path = Application.ExecutablePath.Replace("TLTCImport.dll", "Content\\");
            string pathToImport = path + "ImportTestCases.xml";


            testCasesXml.Save(pathToImport);

            TlReportTcResult.ImportTestCases(pathToImport, cbProjectNames.SelectedItem.ToString());


            lblImportComplete.Enabled = true;
        }       

        private void ExportTestCases()
        {
            string path = Application.ExecutablePath.Replace("TLTCImport.dll", "Content\\");
            string pathToExport = path + "\\ExportTestCases.xml";

            int projectId = TlReportTcResult.GetProjectIdByName(cbProjectNames.SelectedItem.ToString());

            var testsuites = TlReportTcResult.GetAllTestProjectSuites(projectId);

            string nameMainFolder = defaultMainNodeName;
            if (checkBoxForMainFolderName.Checked) nameMainFolder = textBoxForMainNodeName.Text;

            foreach (var testSuite in testsuites)
            {
                if (testSuite.name == nameMainFolder)
                {
                    testCasesXmlExport = TlReportTcResult.ExportTestSuite(testSuite.id);
                    testCasesXmlExport.Save(pathToExport);
                    break;
                }
            }
        }

    }
}
