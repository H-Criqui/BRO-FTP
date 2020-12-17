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
    class Send
    {
        public static void sendFile(string file, BinaryWriter writer)
        {
            byte[] package = new byte[0];
            byte[] payloadInfoBytes = new byte[0];
            try
            {
                //string extension = Path.GetExtension(file);
                string payloadInfo = "1 " + file.Length + ' ' + file + " 0 ";

                payloadInfoBytes = Encoding.UTF8.GetBytes(payloadInfo);

                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string corrpath = path.Substring(0, path.Length - 23);
                string finpath = Path.Combine(corrpath, "Properties\\share_folder");

                package = File.ReadAllBytes(finpath);

                writer.Write(IPAddress.HostToNetworkOrder(payloadInfoBytes.Length));
                writer.Write(payloadInfoBytes);

                writer.Flush();

                writer.Write(IPAddress.HostToNetworkOrder(package.Length));
                writer.Write(package);

                writer.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error: {0}", ex.Message);
                throw ex;
            }
        }

        /*
         byte[] FileConstructer(string file)
         {
             byte[] package = new byte[0];
             byte[] payload = new byte[0];
             try 
             {
                 string payloadInfo = "1 " + file.Length + ' ' + file + " 0 ";

                 payload = Encoding.UTF8.GetBytes(payloadInfo);

                 package = File.ReadAllBytes(file);

                 package.CopyTo(payload, payload.Length);
                 return package;
             }
             catch(Exception ex)
             {
                 throw new Exception(ex.Message);
             }
        */

    }
}

