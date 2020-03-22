using System;
using System.Net.Sockets;
using System.IO;
namespace TcpLearn
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("tcp client started");
            while (true)
            {
                Console.WriteLine("Input adress");
                string adress = Console.ReadLine();
                var Adress_File = adress.Split(' ');
                var mess = generateRequest(Adress_File[0]);
                try
                {
                    var port = 80;
                    var serverAdress = getHost(getHost(Adress_File[0]));
                    var client = new TcpClient(getHost(Adress_File[0]), port);
                    var data = System.Text.Encoding.ASCII.GetBytes(mess);
                    NetworkStream stream = client.GetStream();
                    stream.Write(data, 0, data.Length);
                    stream.Flush();
                    Console.WriteLine("Sent {0}", mess);
                    var responceData = new byte[1000];
                    int bytesRead = stream.Read(responceData, 0, responceData.Length);
                    var responceMessage = System.Text.Encoding.ASCII.GetString(responceData, 0, bytesRead);
                    Console.WriteLine("Responce message : " +  "\n" + responceMessage);
                    saveToFile(returnHtml(responceMessage),Adress_File[1]);
                    stream.Close();
                    client.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        static string generateRequest(string refer)
        {
            int indexOfFileStart = refer.IndexOf("/");
            int hostLenght;
            string file;
            if (indexOfFileStart == -1) 
            {
               file = "/";
               hostLenght = refer.Length;
            }
            else 
            {
                file = refer.Substring(indexOfFileStart);
                hostLenght = indexOfFileStart;
            }
            string host = refer.Substring(0, hostLenght);
            string toReturn = String.Format("GET {0} HTTP/1.0\nHOST: {1}\n\n", file, host);
            return toReturn;
        }
        static void saveToFile(string message,string file)
        {
            using (StreamWriter sw = new StreamWriter("../" + "../" + file))
            {
                sw.Write(message);
            }
        }
        static string getHost(string refer)
        {
            int indexOfFileStart = refer.IndexOf("/");
            if (indexOfFileStart == -1) return refer;
            else return refer.Substring(0, indexOfFileStart);
        }
        static string returnHtml(string refer)
        {
            int index = refer.IndexOf("\r\n\r\n") + 4;
            return refer.Substring(index);
        }
    }
}
