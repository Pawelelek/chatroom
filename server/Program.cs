using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace server
{
    class ChatServer
    {
        const short port = 4040;
        const string address = "127.0.0.1";
        TcpListener listener = null;
        public ChatServer()
        {
            listener = new TcpListener(IPAddress.Parse(address), port);
        }
        //const string JOIN_CMD = "$<join>";
        //const string LEAVE_CMD = "$<leave>";
        //UdpClient server = new UdpClient(port);
        //HashSet<IPEndPoint> members = new HashSet<IPEndPoint>();
        //IPEndPoint clientEndPoint = null;
        //private void AddMember(IPEndPoint member)
        //{
        //    members.Add(member);
        //    Console.WriteLine("Member was added!!!");
        //}
        //private void RemoveMember(IPEndPoint member)
        //{
        //    members.Remove(member);
        //    //members.Clear();
        //    Console.WriteLine("Member was deleted!!!");
        //}
        //private void SendToAll(byte[] data)
        //{
        //    foreach (IPEndPoint member in members)
        //    {
        //        server.SendAsync(data, data.Length, member);
        //    }
        //}
        public void Start()
        {
            listener.Start();
            Console.WriteLine("Waiting for connection...");
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Connected...");
            NetworkStream ns = client.GetStream();
            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);
            while (true)
            {
                //byte[] data = server.Receive(ref clientEndPoint);
                //string message = Encoding.Unicode.GetString(data);
                string message = sr.ReadLine();
                Console.WriteLine($" {message} at {DateTime.Now.ToShortTimeString()}" +
                $" from {client.Client.LocalEndPoint}");
                sw.WriteLine("Thanks!!!");
                sw.Flush();
                //Console.WriteLine($" {message} at {DateTime.Now.ToShortTimeString()}" +
                //$" from {clientEndPoint}");
                //if (JOIN_CMD == message)
                //{
                //    AddMember(clientEndPoint);
                //}
                //else if (LEAVE_CMD == message)
                //{
                //    RemoveMember(clientEndPoint);
                //}
                //else
                //{
                //    SendToAll(data);
                //}
            }
        }



    }
    internal class Program
    {
        
        static void Main(string[] args)
        {
            ChatServer server = new ChatServer();
            server.Start();
           

                //switch (message)
                //{
                //    case JOIN_CMD:
                        
                //        break;
                //    default:
                //        break;
                //}
            
        }
    }
}
