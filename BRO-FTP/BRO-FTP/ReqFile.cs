using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;

namespace BRO_FTP
{
    class Req
    {
        public static void getFile(string file, BinaryWriter writer)
        {
            byte[] package = new byte[0];
            byte[] payloadInfoBytes = new byte[0];
            try
            {
                //string extension = Path.GetExtension(file);
                string payloadInfo = "2 " + file + " 0 ";

                payloadInfoBytes = Encoding.UTF8.GetBytes(payloadInfo);

                //package = File.ReadAllBytes(file);

                writer.Write(IPAddress.HostToNetworkOrder(payloadInfoBytes.Length));
                writer.Write(payloadInfoBytes);

                writer.Flush();


                /*
                int resLength = IPAddress.NetworkToHostOrder(reader.ReadInt32());
                byte[] response = reader.ReadBytes(resLength);

                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string corrpath = path.Substring(0, path.Length - 23);
                string finpath = Path.Combine(corrpath, "Properties\\share_folder");
                string[] files = Directory.GetFiles(finpath);

                File.WriteAllBytes(Path.Join(finpath, payloadInfo[2]), payloadBytes);
                */
            }
            catch (Exception ex)
            {
                Console.WriteLine("error: {0}", ex.Message);
                throw ex;
            }
        }
        public static void listReq(TcpClient client)
        {
            byte[] package = new byte[0];
            byte[] payloadInfoBytes = new byte[0];
            try
            {
                BufferedStream stream = new BufferedStream(client.GetStream());
                BinaryWriter writer = new BinaryWriter(stream);
                BinaryReader reader = new BinaryReader(stream);
                //string extension = Path.GetExtension(file);
                string payloadInfo = "3";

                payloadInfoBytes = Encoding.UTF8.GetBytes(payloadInfo);

                writer.Write(IPAddress.HostToNetworkOrder(payloadInfoBytes.Length));
                writer.Write(payloadInfoBytes);

                writer.Flush();

                int resLength = IPAddress.NetworkToHostOrder(reader.ReadInt32());
                byte[] response = reader.ReadBytes(resLength);

                string toWrite = Encoding.UTF8.GetString(response);
                Console.WriteLine(toWrite);

            }
            catch (Exception ex)
            {
                Console.WriteLine("error: {0}", ex.Message);
                throw ex;
            }
        }

    }
}
