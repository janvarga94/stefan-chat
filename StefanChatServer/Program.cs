using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StefanChat
{
    class Program
    {

        public static TcpClient CurrentClient;

        static void Main(string[] args)
        {
            var readThread = new Thread(read);
                readThread.Start();

            var writeThread = new Thread(write);
                writeThread.Start();

            Console.WriteLine(File.ReadAllText("banner.txt"));
        }

        public static void read() {
            var server = new TcpListener(IPAddress.Any, 30000);
            server.Start();
            
            while (true)
            {
                try
                {
                    TcpClient client = server.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();
                    CurrentClient = client;

                    byte[] bytes = new byte[256];

                    int count;
                    while ((count = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        var data = Encoding.ASCII.GetString(bytes, 0, count);
                        Console.WriteLine("Client: " + data);
                    }
                }
                catch
                {

                }
            }
        }

        public static void write()
        {
            var message = Console.ReadLine();
            var msg = Encoding.ASCII.GetBytes(message);
            try
            {
                CurrentClient.GetStream().Write(msg, 0, msg.Length);             
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.WriteLine("You: " + message);
            }
            catch
            {
       
            }
            write();
        }
    }
}
