using System;
using System.Drawing;
using System.Windows.Forms;

namespace TLTCImport
{
    public partial class AuthorizationForm : Form
    {
        private PictureBox pictureBox;

        private string apiDevKey;

        private TextBox tbApiDevKey;

        private Button btnLogin;

        private Label lblLoginResult, lbApiKey;

        private PictureBox pbLoading;

        private Font font;        

        private Panel panel;

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
            pictureBox.Size = new Size(300, 65);
            pictureBox.Image = Image.FromFile("../../../Content/TestlinkMini.png");
            pictureBox.Location = new Point((panel.Width - pictureBox.Width) / 2, 5);
            panel.Controls.Add(pictureBox);

            //Текст отображающий сообщение о результатах входа в систему
            lbApiKey = new Label();
            lbApiKey.Font = font;
            lbApiKey.Text = "Укажите Api Key:";
            lbApiKey.ForeColor = Color.Black;
            lbApiKey.Size = new Size(350, 30);
            lbApiKey.Location = new Point((panel.Width - lbApiKey.Width) / 2, 120);
            lbApiKey.TextAlign = ContentAlignment.MiddleCenter;

            //Текстбок Api Key
            tbApiDevKey = new TextBox();
            tbApiDevKey.Font = font;
            tbApiDevKey.PasswordChar = '*';
            tbApiDevKey.BackColor = Color.WhiteSmoke;
            tbApiDevKey.Size = new Size(150, 30);
            tbApiDevKey.Location = new Point((panel.Width - tbApiDevKey.Width) / 2, 155);

            //Кнопка Login
            btnLogin = new Button();
            btnLogin.Font = font;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.BackColor = Color.Gray;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Size = new Size(105, 30);
            btnLogin.Location = new Point((panel.Width - btnLogin.Width) / 2, 200);
            btnLogin.Text = "Login";
            btnLogin.Click += new EventHandler(btnLogin_Click);

            //Текст отображающий сообщение о результатах входа в систему
            lblLoginResult = new Label();
            lblLoginResult.Font = font;
            lblLoginResult.ForeColor = Color.Black;
            lblLoginResult.Size = new Size(350, 30);
            lblLoginResult.Location = new Point((panel.Width - lblLoginResult.Width) / 2, 235);
            lblLoginResult.TextAlign = ContentAlignment.MiddleCenter;

            //Картинка отвечающая за загрузку 
            pbLoading = new PictureBox();
            pbLoading.Size = new Size(25, 25);
            pbLoading.SizeMode = PictureBoxSizeMode.Zoom;
            pbLoading.Image = Image.FromFile("../../../Content/load.gif");
            pbLoading.Location = new Point((panel.Width) / 2 - 13, 250);

            pbLoading.Enabled = true;
            pbLoading.Visible = false;

            //Добавление элементов на экран
            panel.Controls.Add(pbLoading);
            panel.Controls.Add(tbApiDevKey);
            panel.Controls.Add(btnLogin);
            panel.Controls.Add(lblLoginResult);
            panel.Controls.Add(lbApiKey);
            this.AcceptButton = btnLogin;
        }       

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //apiDevKey = "";

            apiDevKey = tbApiDevKey.Text;
            pbLoading.Visible = true;
            if (TlReportTcResult.Authorization(apiDevKey))
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
