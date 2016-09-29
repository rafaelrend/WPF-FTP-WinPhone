using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Windows.Networking.Sockets;
using Windows.Networking;
//http://stackoverflow.com/questions/24150134/code-for-windows-phone-8-socket-networking
namespace TSSClient
{

    class Client
    {
        StreamReader reader;
        StreamWriter writer;
        String GetIPAddress()
        {
            List<string> IpAddress = new List<string>();
            var Hosts = Windows.Networking.Connectivity.NetworkInformation.GetHostNames().ToList();
            foreach (var Host in Hosts)
            {
                string IP = Host.DisplayName;
                IpAddress.Add(IP);
            }
            return IpAddress.Last();
        }
        public async void ConnectLinear(StreamSocket socket, EndpointPair e)//connects, and then returns to the thread
        {
            await socket.ConnectAsync(e);
        }
        public bool Connect(String ipAddress)
        {
            const String port = "8020";
            try
            {
                String myIP = GetIPAddress();
                HostName localIP = new HostName(myIP);
                HostName remoteHost = new HostName(ipAddress);

                StreamSocket socket = new StreamSocket();
                EndpointPair e = new EndpointPair(localIP, port, remoteHost, port);

                ConnectLinear(socket, e);

                if (socket.Information.LocalPort != port)//if there is no connection, control will pass to here, but the socket won't have been set up properly.
                    return false;

                Stream forReader = socket.InputStream.AsStreamForRead();
                Stream forWriter = socket.OutputStream.AsStreamForWrite();

                reader = new StreamReader(forReader);
                writer = new StreamWriter(forWriter);
                writer.AutoFlush = true;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public String SendMessage(String input)//gets a message, returns the response String
        {
            try
            {
                if (reader == null)
                    return "";
                writer.WriteLine(input);
                return reader.ReadLine();
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}