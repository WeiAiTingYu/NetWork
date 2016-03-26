using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Login : Form
    {
        public  myclient client;
        public string username;
        public Login()
        {
            client = new myclient();
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)//登陆
        {
            if(textBox1.Text.ToString()!="")
            {
                try
                {
                    client.tcpClient.Connect(client.ip, client.port);
                    client.stream = client.tcpClient.GetStream();
                    client.username = textBox1.Text.ToString();
                    client.say("ON|" + client.username);
                    this.DialogResult = DialogResult.OK;
                    this.Close();




                }
                catch {
                    MessageBox.Show("服务器连接失败");
                }

            }
            else
            {
                MessageBox.Show("用户名不能为空");
            }
        }
    }
}
