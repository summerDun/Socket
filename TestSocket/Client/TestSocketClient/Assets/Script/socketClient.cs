using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine.UI;
using System;
using System.Text;

public class socketClient : MonoBehaviour {

    Socket clientSocket;
    Thread clientThread;
    public Text info;
    private static int index = 0;
	// Use this for initialization
	void Start () {
        ConnectServer();
        
	}

    public void ConnectServer()
    {

        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("192.168.1.101"), 8003);
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            Debug.Log("connect!");
            clientSocket.Connect(ipep);
        }
        catch (SocketException e)
        {
            info.text = e.Message;
            Debug.Log(" connect error: " + e.Message);
            return;
        }

        Thread.Sleep(20);               //  这里不睡觉的话，就会发现clientSocket.Available为0.

        if (clientSocket.Connected)
        {
            clientThread = new Thread(ReceiveData);
            //Thread.Sleep(20);               //  这里不睡觉的话，就会发现clientSocket.Available为0.放在上面也行.

            clientThread.Start();
        }

    }

    public void Send()
    {
        
        byte[] data = new byte[1024];
        index++;
        string msg = "I love you ! + " + index;
        data = Encoding.ASCII.GetBytes(msg);
        clientSocket.Send(data, data.Length, SocketFlags.None);
    }

    private void ReceiveData()
    {
        byte[] data = new byte[1024];
        //接收服务器信息
        int bufLen = 0;
        try
        {
            bufLen = clientSocket.Available;

            clientSocket.Receive(data, 0, bufLen, SocketFlags.None);

            if (bufLen == 0)
            {
                return;
            }

        }
        catch (Exception ex)
        {
            Debug.Log("Receive error: " + ex.Message);
            return;
        }
        
        string clientcommand = System.Text.Encoding.ASCII.GetString(data).Substring(0, bufLen);

        //info.text += clientcommand;
        Debug.Log(clientcommand);
    }
}
