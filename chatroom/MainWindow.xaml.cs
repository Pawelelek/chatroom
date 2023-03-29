using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
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
        UdpClient client = new UdpClient();
        ObservableCollection<MessageInfo> messages = new ObservableCollection<MessageInfo>();
        //const string serverAddress = "127.0.0.1";
        //const short serverPort = 4040;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = messages;
            string serverAddress = ConfigurationManager.AppSettings["ServerAddress"]!; 
            short serverPort = short.Parse(ConfigurationManager.AppSettings["ServerPort"]!); 
            serverEndPoint = new IPEndPoint(IPAddress.Parse(serverAddress), serverPort);
            //sendbtn.IsEnabled = false;
            
        }

        private void SendBtnClick(object sender, RoutedEventArgs e)
        {
            SendMessage(msgTextBox.Text);
        }

        private void JoinBtnClick(object sender, RoutedEventArgs e)
        {
            //if (!sendbtn.IsEnabled)
            //{
            //    sendbtn.IsEnabled = true;
            //}
            SendMessage("$<join>");
            Listen();
        }
        private async void SendMessage(string message)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            await client.SendAsync(data, data.Length, serverEndPoint);
        }
        private async void Listen()
        {
          while (true)
          {
              var res = await client.ReceiveAsync();
              string message = Encoding.Unicode.GetString(res.Buffer);
              messages.Add(new MessageInfo(message));
          }
           
        }
        private void LeaveBtnClick(object sender, RoutedEventArgs e)
        {
            //if (sendbtn.IsEnabled)
            //{
            //    sendbtn.IsEnabled = false;
            //}
            //if (leavebtn.IsEnabled)
            //{
            //    leavebtn.IsEnabled = false;
            //}
            //SendMessage("$<leave>");
        }
    }
    class MessageInfo
    {
        public string Message { get; set; }
        public DateTime Time { get; set; }
        public MessageInfo(string msg)
        {
            Message = msg;
            Time = DateTime.Now;
        }
        public override string ToString()
        {
            return $"{Message}, {Time.ToShortDateString()}";
        }
    }
}
