using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
namespace Client
{
   public class myclient
    {
        public int port;
        public IPAddress ip;
        public TcpClient tcpClient;
        public NetworkStream stream;//数据流对象
        private byte[] bufffer;
        public string username;
        public string data;
       
        public myclient()
        {
             ip = IPAddress.Parse("192.168.84.130");
            //ip = IPAddress.Any;
            port = 12345;
            bufffer = new byte[4096];
            tcpClient = new TcpClient();
        }
        public void say(string msg)
        {
            try
            {
                bufffer = System.Text.Encoding.Default.GetBytes(msg);//编码
                stream.Write(bufffer, 0, bufffer.Length);//发送
                stream.Flush();
            }
            catch { }
        }
        public void read()
        {
            int i;
            try
            {
                while ((i = stream.Read(bufffer, 0, bufffer.Length)) != 0)
                {

                    data = Encoding.Default.GetString(bufffer, 0, i);
                    System.Windows.Forms.MessageBox.Show("服务器返回了一条" + data);
                    stream.Flush();
                }
            } catch { }


        }
    }
}
