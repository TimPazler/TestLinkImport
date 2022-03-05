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

namespace TLTCImport
{
    public partial class MainForm : Form
    {
        private Panel leftPanel;
        private Panel topPanel;

        private Label lblSoftware, lblCurrentTestPlan, lblTestRun;
        public Label lblMessageAddJson, lblMessageRecognitionJson;
        public Label lblMessageDataTransferXmlFile;

        private int projectId, testPlanId;
        private string projectName, testPlanName;


        private Button btnAutoMode, btnManualMode;    

        private ComboBox cbProjectNames, cbTestPlanName;    

        private string pathJsonFile = "../../../Files/";

        //Поля для word импорта
        private bool outWordDocumentMode = false;
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
            lblMessageAddJson = new Label();
            lblMessageAddJson.BackColor = Color.DarkGray;
            lblMessageAddJson.Size = new Size(200, 50);
            lblMessageAddJson.Location = new Point(5, 220);
            lblMessageAddJson.TextAlign = ContentAlignment.MiddleCenter;
            Controls.Add(lblMessageAddJson);

            //Текст отображающий сообщение о распознании файла Json
            lblMessageRecognitionJson = new Label();
            lblMessageRecognitionJson.BackColor = Color.DarkGray;
            lblMessageRecognitionJson.Size = new Size(200, 50);
            lblMessageRecognitionJson.Location = new Point(5, 280);
            lblMessageRecognitionJson.TextAlign = ContentAlignment.MiddleCenter;
            Controls.Add(lblMessageRecognitionJson);

            //Текст отображающий сообщение что Данные перенесены в xml файл
            lblMessageDataTransferXmlFile = new Label();
            lblMessageDataTransferXmlFile.BackColor = Color.DarkGray;
            lblMessageDataTransferXmlFile.Size = new Size(200, 50);
            lblMessageDataTransferXmlFile.Location = new Point(5, 350);
            lblMessageDataTransferXmlFile.TextAlign = ContentAlignment.MiddleCenter;
            Controls.Add(lblMessageDataTransferXmlFile);                     

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
            var count = 0;
            foreach (var item in TlReportTcResult.GetAllProjectTestPlans(projectId))
            {
                comboBox.Items.Add(item.name);
                count++;
            }

            if (count > 0)
            {
                Thread.Sleep(250);
                cbTestPlanName.Enabled = true;
            }
            //добавить проверку что не выбрано другое значение в выпадающем списке.
        }

        private void cbProjectNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            projectId = TlReportTcResult.GetProjectIdByName(cbProjectNames.SelectedItem.ToString());
            projectName = cbProjectNames.SelectedItem.ToString();
            if (projectId != 0)
                SetTestPlanName(cbTestPlanName, projectId);
            else MessageBox.Show("Ошибка! Проект не выбран или не существует!");
        }

        private void cbTestPlanName_SelectedIndexChanged(object sender, EventArgs e)
        {
            testPlanId = TlReportTcResult.GetTestPlanIdByName(cbProjectNames.SelectedItem.ToString(), cbTestPlanName.SelectedItem.ToString());
            testPlanName = cbTestPlanName.SelectedItem.ToString();

            if (testPlanId != 0)
            {
                btnAutoMode.Enabled = true;
                btnManualMode.Enabled = true;
            }
            else MessageBox.Show("Ошибка! Проект не выбран или не существует!");
        }
        
        private void btnManualMode_Click(object sender, EventArgs e)
        {
         
        }

        private void btnAutoMode_Click(object sender, EventArgs e)
        {
            int countValuesCases = 0;
            //MessageBox.Show("Выберите файл в формете .json. Потом написать инструкцию");

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "json files (*.json)|*.json";

            DialogResult result = fileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string urlUploadedFile = fileDialog.FileName;
                string[] names = urlUploadedFile.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                string nameFile = names[names.Length - 1].Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[0];

                //Перенос файла в проект
                CopyFileInProject(urlUploadedFile, nameFile);

                lblMessageAddJson.Font = new Font(DefaultFont, FontStyle.Bold);
                lblMessageAddJson.ForeColor = Color.Green;
                lblMessageAddJson.Text = "✔ Файл \"" + nameFile + ".json" + "\" успешно добавлен";

                JsonToXml jsonToXml = new JsonToXml();
                var jsonFileCorrect = jsonToXml.JsonValidation(nameFile);
                
                //Проверка коректен ли файл
                if (jsonFileCorrect)
                {                   
                    lblMessageRecognitionJson.Font = new Font(DefaultFont, FontStyle.Bold);
                    lblMessageRecognitionJson.ForeColor = Color.Green;
                    lblMessageRecognitionJson.Text = "✔ Json файл распознан!";

                    //Запись значений из json в словарь
                    var valuesCases = jsonToXml.GetDataJson(nameFile, ref jsonFileCorrect);

                    //Если файл корректен, то перенос результатов в xml файл
                    jsonToXml.FillXmlFile(projectName, testPlanName, testPlanId, valuesCases, out countValuesCases);

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
                else
                {
                    lblMessageAddJson.Font = new Font(DefaultFont, FontStyle.Bold);
                    lblMessageAddJson.ForeColor = Color.DarkRed;
                    lblMessageAddJson.Text = "✖ Ошибка! Json файл не корректен. Попробуйте загрузить другой файл!";
                }                                            
            }
            else
            {
                lblMessageAddJson.Font = new Font(Label.DefaultFont, FontStyle.Bold);
                lblMessageAddJson.ForeColor = Color.Red;
                lblMessageAddJson.Text = "✖ Файл не был добавлен";

                lblMessageRecognitionJson.Text = "";
                lblMessageDataTransferXmlFile.Text = "";
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
    }
}
