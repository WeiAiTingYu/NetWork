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

namespace Service
{
    public partial class Service : Form
    {
        private int port;
        private IPAddress ip;
        private TcpListener tcpListener;
        private TcpClient[] tcpClients;
        private ushort maxnum;
        private short num=0;
        private string time;
        private NetworkStream stream;
        private byte[] buffer;
        private volatile bool stop;
        private string[] Online;
        public Service()
        {
            maxnum = 10;
            buffer = new byte[4096];
            Online = new string[maxnum];
            time = DateTime.Now.ToString("HH:mm:ss");
            port = 12345;//配置端口
            //ip = IPAddress.Parse("127.0.0.1");//配置ip
            ip = IPAddress.Any;
            Thread service = new Thread(StartService);//服务线程托管
            service.IsBackground = true;
            service.Start();
            Thread.Sleep(300);
            //while (service.IsAlive) ;
            // StartService();
            TextBox.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }
        private void StartService()//服务线程
        {
            tcpListener = new TcpListener(ip, port);
            tcpClients = new TcpClient[maxnum];
            tcpListener.Start();//开始监听连接请求
            while(!stop){
                try
                {
                    tcpClients[num] = tcpListener.AcceptTcpClient();//客户端对象
                    //textBox1.Text += time+"客户端上线" + num+"\r\n";
                    Thread comThread = new Thread(new ParameterizedThreadStart(Communicate));//会话线程托管
                    int n = num;
                    num++;
                    comThread.IsBackground = true;
                    comThread.Start(n);

                }
                catch
                {
                   
                }

            }
        }
        private void Communicate(object a)//通信进程
        {
            int n = (int)a;
            try
            {
                int i=0;
              NetworkStream  stream = tcpClients[n].GetStream();
              while((i = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                string data = Encoding.Default.GetString(buffer, 0, i);
                    string[] msg = data.Split('|');
                    switch (msg[0])
                    {
                        case "close":
                            {
                                num--;
                                Online[n] = "";

                                textBox1.Text += time +msg[1]+ "下线" + "当前连接数" + (num) + "\r\n";
                               
                                break;
                            }
                        case "ON":
                            {
                                Online[n] = msg[1];
                                textBox1.Text += time+msg[1] + "上线"  + "当前连接数" + (num-1) + "\r\n";
                                string msgb = "OK|";
                                byte []b = new byte[10] ;
                                b = Encoding.Default.GetBytes(msgb);
                                stream.Write(b, 0, b.Length);
                                stream.Flush();
                                break;
                            }
                        case "T":
                            {
                                textBox1.Text += time + msg[1]+"对"+msg[2] + "说" + msg[3] + "\r\n";
                                int j;
                                bool find=false;
                                for(j=0;j< Online.Length; j++)
                                {
                                    if (Online[j] == msg[2])
                                    {
                                        find = true;
                                        break;
                                    }

                                }
                                if (find)
                                {
                                    string backmsg = msg[1] + "|" + msg[3];
                                    
                                    byte[] buffer = new byte[1024];
                                    buffer = System.Text.Encoding.Default.GetBytes(data);//编码
                                    try
                                    {
                                        NetworkStream lsstream = tcpClients[j].GetStream();
                                        lsstream.Write(buffer, 0, buffer.Length);
                                        lsstream.Flush();
                                    }
                                    catch { }
                                }
                                else
                                {
                                    MessageBox.Show("该用户未在线");
                                }
                                break;
                            }
                        default:MessageBox.Show(data);break;
                    }
            }
            }
            catch (Exception e)
            {
                MessageBox.Show( "服务器端提示:"+e.Message);
            }
        }

        private void Service_FormClosed(object sender, FormClosedEventArgs e)
        {
            stop = true;
            if(tcpListener!=null)
            tcpListener.Stop();
        }

        private void Service_Load(object sender, EventArgs e)
        {
            textBox2.Text = ip.ToString();
            textBox3.Text = "12345";
        }
    }
}
