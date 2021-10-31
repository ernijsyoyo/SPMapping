using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

/// <summary>
/// Class for sending UDP traffic
/// </summary>
public class NetworkManagerUDP : Singleton<NetworkManagerUDP>
{
    // prefs
    public string IP = "192.168.0.172";  // define in init
    public int port = 8050;  // define in init

    // "connection" things
    IPEndPoint remoteEndPoint;
    UdpClient client;

    public bool test = false;
    private void Update()
    {
        if (test)
        {
            test = false;
            sendTestMessage();
        }
    }

    // start from unity3d
    public void Start()
    {
        print("UDP endpoint: " + IP + " : " + port);
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
        client = new UdpClient();
    }


    public void sendTestMessage()
    {
        sendString("Dipship UDP");
    }

    // sendData
    public void sendString(string message)
    {
        print("Destination - " + IP + ":" + port + "; Sending message - " + message);
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            client.Send(data, data.Length, remoteEndPoint);
        }
        catch (System.Exception err)
        {
            print(err.ToString());
        }
    }
}