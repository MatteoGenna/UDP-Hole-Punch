using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Text;

namespace udpHolePunch
{
    internal class Program
    {
        static readonly Int32 port = 5000;       //replace port
        static readonly string IP = "127.0.0.1"; //replace with ip to connect
        static IPEndPoint iPEndPoint = new (IPAddress.Any, port);
        static UdpClient udpClient = new (port);

        static void Main(string[] args)
        {
            config();
            Console.WriteLine("Config done - Ready to exchange datagrams\n");

            while (true)
            {
                string message = Console.ReadLine();
                send(message);
            }
            

        }

        static void config()
        {
            //udpClient.AllowNatTraversal(true); //Windows only?

            try
            {
                //udpClient.Connect(IP, port);



            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        public void reciver()
        {
            var recive = Task.Run(() =>{
                return udpClient.ReceiveAsync();
            });

            var reciveAwaiter = recive.GetAwaiter();
            //callback
            reciveAwaiter.OnCompleted(() =>
            {
                var result = reciveAwaiter.GetResult();
                byte[] Buffer = result.Buffer;
                string message = Encoding.ASCII.GetString(Buffer) ;
                Console.WriteLine(" < " + message +" \n");
                //Console.WriteLine(" - send back the confirm message");
                //send(message);
                Console.WriteLine(" - re-call Reciver()");
                reciver();
            });
        }

        static void send(string i)
        {
            var sender = Task.Run(() =>
            {
                string message = " > " + i;
                Byte[] sendBytes = Encoding.ASCII.GetBytes(message);
                try
                {
                    //Console.Write(message + "\n\n");
                    udpClient.Send(sendBytes, sendBytes.Length, IP, port);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                
            });
            
        }

    }
}