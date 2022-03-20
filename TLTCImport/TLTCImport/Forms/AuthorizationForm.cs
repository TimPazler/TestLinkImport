using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace TLTCImport
{
    public partial class AuthorizationForm : Form
    {
        private string apiDevKey;

        private PictureBox pictureBox, pbLoading;

        private Panel panel;

        private TextBox tbApiDevKey;

        private Button btnLogin;

        private Label lblLoginResult, lbApiKey;
        private Label lblhelp, lblProductVersion;

        private LinkLabel llHelp;

        private Font font;        

        //Добавить поле для указания url тестлинка
        public AuthorizationForm()
        {
            InitializeComponent();

            //Текст и логотип
            font = new Font("Century Gothic", 12);        
            panel = new Panel();
            panel.Dock = DockStyle.Fill;
            Controls.Add(panel);

            //Изображение Testlink
            pictureBox = new PictureBox();
            pictureBox.Size = new Size(200, 65);
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.Image = Image.FromFile("../../../Content/TestlinkMini.png");
            pictureBox.Location = new Point((panel.Width - pictureBox.Width) / 2, 15);
            panel.Controls.Add(pictureBox);

            //Текст отображающий сообщение о результатах входа в систему
            lbApiKey = new Label();
            lbApiKey.Font = font;
            lbApiKey.Text = "Укажите Api Key:";
            lbApiKey.ForeColor = Color.Black;
            lbApiKey.Size = new Size(350, 30);
            lbApiKey.Location = new Point((panel.Width - lbApiKey.Width) / 2, 80);
            lbApiKey.TextAlign = ContentAlignment.MiddleCenter;

            //Текстбок Api Key
            tbApiDevKey = new TextBox();
            tbApiDevKey.Font = font;
            tbApiDevKey.PasswordChar = '*';
            tbApiDevKey.BackColor = Color.WhiteSmoke;
            tbApiDevKey.Size = new Size(150, 30);
            tbApiDevKey.Location = new Point((panel.Width - tbApiDevKey.Width) / 2, 115);

            //Кнопка Login
            btnLogin = new Button();
            btnLogin.Font = font;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.BackColor = Color.Gray;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Size = new Size(105, 30);
            btnLogin.Location = new Point((panel.Width - btnLogin.Width) / 2, 150);
            btnLogin.Text = "Login";
            btnLogin.Click += new EventHandler(btnLogin_Click);

            //Текст отображающий сообщение о результатах входа в систему
            lblLoginResult = new Label();
            lblLoginResult.Font = font;
            lblLoginResult.ForeColor = Color.Black;
            lblLoginResult.Size = new Size(350, 30);
            lblLoginResult.Location = new Point((panel.Width - lblLoginResult.Width) / 2, 185);
            lblLoginResult.TextAlign = ContentAlignment.MiddleCenter;

            //Картинка отвечающая за загрузку 
            pbLoading = new PictureBox();
            pbLoading.Size = new Size(25, 25);
            pbLoading.SizeMode = PictureBoxSizeMode.Zoom;
            pbLoading.Image = Image.FromFile("../../../Content/load.gif");
            pbLoading.Location = new Point((panel.Width) / 2 - 13, 190);
            pbLoading.Enabled = true;
            pbLoading.Visible = false;

            //Текст отображающий сообщение ссылку на профиль для просьбы о помощи
            lblhelp = new Label();
            lblhelp.Font = new Font("Century Gothic", 9);
            lblhelp.ForeColor = Color.Black;
            lblhelp.Size = new Size(82, 30);
            lblhelp.Text = "Проблема?";
            lblhelp.Location = new Point(5, 220);
            lblhelp.TextAlign = ContentAlignment.MiddleLeft;

            //Гипперссылка на телегу
            llHelp = new LinkLabel();
            llHelp.Font = new Font("Century Gothic", 9);
            llHelp.ForeColor = Color.Black;
            llHelp.Text = "Пиши, решим!";
            llHelp.Location = new Point(85, 226);
            llHelp.LinkClicked += llHelp_LinkClicked;

            //Текст отображающий версию продукта
            lblProductVersion = new Label();
            lblProductVersion.Font = new Font("Century Gothic", 9);
            lblProductVersion.ForeColor = Color.Gray;
            lblProductVersion.Size = new Size(82, 30);
            lblProductVersion.Text = "v0.16.5";
            lblProductVersion.Location = new Point(215, 220);
            lblProductVersion.TextAlign = ContentAlignment.MiddleRight;

            //Меню
            ToolStripMenuItem fileItem = new ToolStripMenuItem("Где взять Api Key?");
            menuStrip1.Items.Add(fileItem);          
            ToolStripMenuItem aboutItem = new ToolStripMenuItem("О программе");
            aboutItem.Click += aboutItem_Click;
            menuStrip1.Items.Add(aboutItem);
            Controls.Add(menuStrip1);

            //Добавление элементов на экран
            panel.Controls.Add(lblhelp);
            panel.Controls.Add(llHelp);
            panel.Controls.Add(lblProductVersion);
            panel.Controls.Add(pbLoading);
            panel.Controls.Add(tbApiDevKey);
            panel.Controls.Add(btnLogin);
            panel.Controls.Add(lblLoginResult);
            panel.Controls.Add(lbApiKey);
            this.AcceptButton = btnLogin;
        }

        void aboutItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("О программе");
        }

        private void llHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("cmd", $"/c start https://t.me/TimPazler"));
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            apiDevKey = "bf9a98dc1791a5c7115b9e4322b4f3bc";

            //apiDevKey = tbApiDevKey.Text;
            lblLoginResult.Text = "";
            Thread.Sleep(500);
            pbLoading.Visible = true;
            if (TestLinkResult.Authorization(apiDevKey))
            {
                pbLoading.Visible = false;
                lblLoginResult.Text = "";

                //Открытие нового окна
                var frm = new MainForm();
                frm.Location = Location;
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.FormClosing += delegate { Show(); };
                frm.Show();
                Hide();
            }
            else
            {
                pbLoading.Visible = false;
                lblLoginResult.Text = "Неправильный Api ключ";
            }
            
        }

    }
}
