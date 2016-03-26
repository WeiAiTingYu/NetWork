using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;
namespace Client
{
    public partial class Main : Form
    {
        public Login login;//登录框对象
        public TcpClient tcpclient;
        public NetworkStream stream;
        public string username;//用户名
        private volatile bool _stop;//是否停止服务
        public Form1 form_dialog;//对话框对象
        public Main(Login loginclient)
        {
            //下面对传入的对话框参数进行赋值
            login = loginclient;
            tcpclient = loginclient.client.tcpClient;
            stream = tcpclient.GetStream();
            username = loginclient.client.username;
            TextBox.CheckForIllegalCrossThreadCalls = false;//取消跨线程检查
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Main_Load(object sender, EventArgs e)
        {
            label3.Text = username;
            groupBox1.MouseDoubleClick += new MouseEventHandler(ItemDoubleClick);
            groupBox2.MouseDoubleClick += new MouseEventHandler(ItemDoubleClick2);
            Thread commuThread = new Thread(listening);//建立通信接收线程
            commuThread.Start();
        }
        public void ItemDoubleClick(object sennder, MouseEventArgs e)
        {
            Form1 f = new Form1(this);
            form_dialog = f;//赋值给本类的对话框
            f.reciveName = label1.Text.ToString();
            f.Text = "正在和" + label1.Text.ToString() + "会话中--";
            f.Show();  
        }
        public void ItemDoubleClick2(object sennder, MouseEventArgs e)
        {
            Form1 f = new Form1(this);
            form_dialog = f;//赋值给本类的对话框
            f.reciveName = label2.Text.ToString();
            f.Text = "正在和" + label2.Text.ToString() + "会话中--";
            f.Show();
        }
        private void listening()
        {
            int length = 0;
            byte[] buffer = new byte[1024*1024*10];
            while (!_stop)
            {
                try
                {

                    length = stream.Read(buffer, 0, buffer.Length);
                    string data = Encoding.Default.GetString(buffer,0,length);
                    string[] msg = data.Split('|');
                    textBox1.Text = msg[1] + "对你说" + msg[3];
                    form_dialog.GetChildAtPoint(new Point(3, 4)).Text +=System.DateTime.Now.ToString("HH:mm:ss")+"  "+msg[1]+"说:"+msg[3]+"\r\n";
                    stream.Flush();
                    //  int i = Login.client.stream
                }
                catch { }
            }

        }
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
           login.client.say("close|"+login.client.username);
            _stop = true;
           stream.Close();
           tcpclient.Close();
            Application.Exit();
        }
    }
}
