using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace chatroom
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IPEndPoint serverEndPoint;
        NetworkStream ns = null;
        StreamReader sr = null;
        TcpClient tcpClient;
        StreamWriter sw = null;
        ObservableCollection<MessageInfo> messages = new ObservableCollection<MessageInfo>();

        public MainWindow()
        {
            InitializeComponent();
            tcpClient = new TcpClient();
            this.DataContext = messages;
            string serverAddress = ConfigurationManager.AppSettings["ServerAddress"]!; 
            short serverPort = short.Parse(ConfigurationManager.AppSettings["ServerPort"]!); 
            serverEndPoint = new IPEndPoint(IPAddress.Parse(serverAddress), serverPort);
            sendbtn.IsEnabled = false;
            leavebtn.IsEnabled = false;
        }

        private void SendBtnClick(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(msgTextBox.Text))
            {
                string message = msgTextBox.Text;
                //byte[] data = Encoding.Unicode.GetBytes(message);
                sw.WriteLine(message);
                //ns.Write(data);
                sw.Flush();
            }
        }

        private void ConnectBtnClick(object sender, RoutedEventArgs e)
        {
            if (!sendbtn.IsEnabled)
            {
                sendbtn.IsEnabled = true;
            }
            if (!leavebtn.IsEnabled)
            {
                leavebtn.IsEnabled = true;
            }
            try
            {
                tcpClient.Connect(serverEndPoint);
                ns = tcpClient.GetStream();
                sr = new StreamReader(ns);
                sw = new StreamWriter(ns);
                Listen();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        //private void SendMessage(string message)
        //{
            

        //    //await client.SendAsync(data, data.Length, serverEndPoint);
        //}
        private async void Listen()
        {
            //StreamReader sr = new StreamReader(ns);
          while (true)
          {
              string? message = await sr.ReadLineAsync();
              //var res = await client.ReceiveAsync();
              //string message = Encoding.Unicode.GetString(res.Buffer);
              messages.Add(new MessageInfo(message));
          }
           
        }
        private void DisconnectBtnClick(object sender, RoutedEventArgs e)
        {
            if (sendbtn.IsEnabled)
            {
                sendbtn.IsEnabled = false;
            }
            if (leavebtn.IsEnabled)
            {
                leavebtn.IsEnabled = false;
            }
            ns.Close();
            tcpClient.Close();
            messages.Clear();
            //this.UpdateLayout();
        }
    }
    class MessageInfo
    {
        public string Message { get; set; }
        public DateTime Time { get; set; }
        public MessageInfo(string? msg)
        {
            Message = msg ?? "";
            Time = DateTime.Now;
        }
        public override string ToString()
        {
            return $"{Message}, {Time.ToShortDateString()}";
        }
    }
}
