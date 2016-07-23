using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace TestSocket
{
    class Server
    {
        Thread serverThread;
        Thread clientThread;
        Socket serverSocket;
        Socket clientSocket;
        public void ServerStart()
        {
            IPAddress ipaddr = IPAddress.Parse("192.168.1.101");
            IPEndPoint ipe = new IPEndPoint(ipaddr, 8003);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(ipe);
            serverSocket.Listen(20);

            while(true)
            {
                try
                {
                    clientSocket = serverSocket.Accept();
                    clientThread = new Thread(new ThreadStart(Receivedata));
                    clientThread.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine("server listen error: " + e.Message);
                }
            }
        }

        public void Receivedata()
        {
            bool keepalive = true;
            Socket s = clientSocket;
            Byte[] buffer = new Byte[1024];

            IPEndPoint client = (IPEndPoint)s.RemoteEndPoint;
            Console.WriteLine("client :" + client .Address + ":" + client.Port);
            string welcome = "welcome to my test socket!";
            byte[] data = new byte[1024];
            data = Encoding.ASCII.GetBytes(welcome);
            s.Send(data, data.Length, SocketFlags.None);
            
            while(keepalive)
            {
                int buflen = 0;
                try
                {
                    buflen = s.Available;
                    s.Receive(buffer, 0, buflen, SocketFlags.None);
                    if(buflen == 0)
                    {
                        continue;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("receive Error :" + e.Message);
                    return;
                }

                client = (IPEndPoint)s.RemoteEndPoint;
                string clientCommand = System.Text.Encoding.ASCII.GetString(buffer).Substring(0, buflen);

                Console.WriteLine(clientCommand + " : " + client.Address + " : " + client.Port);
            }
        }
    }
}
