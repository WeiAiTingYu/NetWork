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
namespace NetWork
{
    public partial class Service : Form
    {
        private int port;
        private IPAddress ip;
        private TcpListener tcpListener;
        public Service()
        {
            port = 12345;//配置端口
            ip = IPAddress.Parse("127.0.0.1");//配置ip
            Thread service = new Thread(StartService);
            service.Start();
           // StartService();
            InitializeComponent();
        }
        private void StartService()//服务线程
        {
            tcpListener = new TcpListener(ip, port);
            tcpListener.Start();//开始监听连接请求
            TcpClient tcpClinet = tcpListener.AcceptTcpClient();//客户端对象
            MessageBox.Show("有客户端链接!");
        }
    }
}
