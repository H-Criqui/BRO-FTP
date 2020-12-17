using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Transactions;

namespace BRO_FTP
{    class Listener
    {

        static void TCPListener(bool running)
        {
            TcpListener server = null;

            // creates a TCP listener on local IP and waiting for a connection to be made
            try
            {
                // Local IP and port to listen on
                int port = 4000;
                //IPAddress localAddr = IPAddress.Parse("127.0.0.2"); //Needs your IP to function
                IPAddress[] localAddresses = Dns.GetHostAddresses(Dns.GetHostName());
                IPAddress localAddr = localAddresses[localAddresses.Length-1]; //Needs your IP to function

                // new TCP listener creation
                server = new TcpListener(localAddr, port);

                // Start listening for connections
                server.Start();
                

                // Enter the listening loop waiting for connection

                    //Console.Write("Waiting for a connection... ");

                    // look for client to accept
                    TcpClient client = server.AcceptTcpClient();

                if(client.Connected)
                {
                    //return client;
                    Rec.recFile(client);
                }

            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }

        }

        // Pings an IP and port trying to connect
        static TcpClient Connect(String IPAddress, String portNum) 
        {
           //while (true)
            //{
                try
                {
                    // sets port num and IPAddress
                    int port = Int32.Parse(portNum);
                    TcpClient client = new TcpClient(IPAddress, port);

                    return client;
        
                }
                catch (ArgumentNullException)
                {
                   
                    Console.WriteLine("No connection");
                    Thread.Sleep(5000);
                    throw new Exception();
                }
                catch (SocketException)
                {
                    Console.WriteLine("No connection");
                    Thread.Sleep(5000);
                    throw new Exception();  
                }
            //}
        }


        public static void Main()
        {
            bool running = true;
            // Starts connection thread
            Thread thread = new Thread(() =>
            {
                //Thread.Sleep(1000);
                TCPListener(running);
                //Thread.Sleep(3000);
            });

            thread.Start();
            
            Console.Write("Enter the IP address: ");
            string ipadd = Console.ReadLine();
            TcpClient client;
            
            client = Connect(ipadd, "4000"); // Needs target IP to function
            BufferedStream stream = new BufferedStream(client.GetStream());
            BinaryWriter writer = new BinaryWriter(stream);



            while(running)
            {
                Console.Write("Commands: get, send, list, quit\nEnter Command: ");
                string[] command = Console.ReadLine().Split(' ');
                switch (command[0].ToLower())
                {
                    case "get":
                        Req.getFile(command[1], writer);
                        break;
                    case "send":
                        Send.sendFile(command[1], writer);
                        break;
                    case "list":
                        Req.listReq(client);
                        break;
                    case "quit":
                        running = false;

                        string payloadInfo = "4 ";

                        byte[] payloadInfoBytes = Encoding.UTF8.GetBytes(payloadInfo);

                        writer.Write(IPAddress.HostToNetworkOrder(payloadInfoBytes.Length));
                        writer.Write(payloadInfoBytes);

                        writer.Flush();
                        break;
                }
            }


            

          
            //Thread reqHandler = new Thread(() =>
            //{
            //    while (running)
            //    {
            //        BinaryReader 
            //    }
            //}
        }
    }
}
