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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;


namespace TLTCImport
{
    public partial class AuthorizationForm : Form
    {
        private PictureBox pictureBox;

        private string apiDevKey;

        private TextBox tbApiDevKey;

        private Button btnLogin;

        private Label lblLoginResult;

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
            //panel.BackColor = Color.FromArgb(16, 16, 16);
            Controls.Add(panel);

            //Изображение Testlink
            pictureBox = new PictureBox();
            pictureBox.Size = new Size(520, 112);
            pictureBox.ImageLocation = Application.ExecutablePath.Replace("TLTCImport.dll", "Content\\testlink.png");
            pictureBox.Location = new Point((panel.Width - pictureBox.Width) / 2, 5);

            //Текстбок Api Key
            tbApiDevKey = new TextBox();
            tbApiDevKey.Font = font;
            tbApiDevKey.Text = "Api Key";
            tbApiDevKey.BackColor = Color.WhiteSmoke;
            tbApiDevKey.Size = new Size(105, 30);
            tbApiDevKey.Location = new Point((panel.Width - tbApiDevKey.Width) / 2, 190);
           
            //Кнопка Login
            btnLogin = new Button();
            btnLogin.Font = font;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.BackColor = Color.Gray;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Size = new Size(105, 30);
            btnLogin.Location = new Point((panel.Width - btnLogin.Width) / 2, 260);
            btnLogin.Text = "Login";
            btnLogin.Click += new EventHandler(btnLogin_Click);

            //Текст отображающий сообщение о результатах входа в систему
            lblLoginResult = new Label();
            lblLoginResult.Font = font;
            //lblLoginResult.BackColor = Color.FromArgb(16, 16, 16);
            lblLoginResult.Size = new Size(350, 30);
            lblLoginResult.Location = new Point((panel.Width - lblLoginResult.Width) / 2, 295);
            lblLoginResult.TextAlign = ContentAlignment.MiddleCenter;

            //Картинка отвечающая за загрузку 
            pbLoading = new PictureBox();
            pbLoading.Size = new Size(30, 30);
            pbLoading.ImageLocation = Application.ExecutablePath.Replace("TLTCImport.dll", "Content\\better-loading.gif");
            pbLoading.Location = new Point((panel.Width - 30) / 2, 330);
            pbLoading.Enabled = true;
            pbLoading.Visible = false;

            //Добавление элементов на экран
            panel.Controls.Add(tbApiDevKey);
            panel.Controls.Add(btnLogin);
            panel.Controls.Add(lblLoginResult);
            panel.Controls.Add(pictureBox);
            panel.Controls.Add(pbLoading);
            this.AcceptButton = btnLogin;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {           
            apiDevKey = tbApiDevKey.Text;
            pbLoading.Visible = true;
            if (TlReportTcResult.Authorization(apiDevKey))
            {
                pbLoading.Visible = false;
                lblLoginResult.Text = "";

                var frm = new Form1();
                frm.Location = Location;
                frm.StartPosition = FormStartPosition.Manual;
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
