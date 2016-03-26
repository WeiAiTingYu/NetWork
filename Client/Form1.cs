using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Client
{
    public partial class Form1 : Form
    {
        private TcpClient tcpclient;
        private NetworkStream stream;
        private Main maindialog;
        public string reciveName;
        public string username;
        public Form1(Main main)
        {
            reciveName = main.label1.ToString();
            username = main.username;
            maindialog = main;
            tcpclient = main.tcpclient;
            stream = main.stream;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox2.Text.ToString()!="")               
            maindialog.login.client.say("T|"+username+"|" + reciveName + "|"+ textBox2.Text.ToString());
            textBox1.Text += System.DateTime.Now.ToString("HH:mm:ss") + "  " + "我:"+textBox2.Text.ToString()+"\r\n";
            textBox2.Text = "";

        }
        private void communication()
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
 


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
