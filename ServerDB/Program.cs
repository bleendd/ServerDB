using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ServerDB
{
    class Program
    {

        public static Hashtable clientsList = new Hashtable();
        
        static void Main(string[] args)
        {
            TcpListener serverSocket = new TcpListener(IPAddress.Any, 55666);
            TcpClient clientSocket = default(TcpClient);
            serverSocket.Start();

            Console.WriteLine("U startua!");

            while (true)
            {
                clientSocket = serverSocket.AcceptTcpClient();
                byte[] bitatPrej = new byte[8192];
                string dataPrejKlientit = null;

                NetworkStream networkStream = clientSocket.GetStream();
                int bytesRead = networkStream.Read(bitatPrej, 0, bitatPrej.Length);
                dataPrejKlientit = System.Text.Encoding.ASCII.GetString(bitatPrej, 0, bytesRead);

                if ( dataPrejKlientit.StartsWith("menaxhimi"))
                {
                    XmlSerializer writer = new XmlSerializer(typeof(List<ClassMenaxhimi>));
                    StreamWriter file = new StreamWriter("../menaxhimi.xml");
                    List<ClassMenaxhimi> lista = new List<ClassMenaxhimi>();
  
                    
                    dataPrejKlientit = dataPrejKlientit.Substring(9);
                    string[] info = dataPrejKlientit.Split('$');
                    for (int i = 0; i < info.Length-1; i=i+6)
                    {
                        ClassMenaxhimi menaxhimi = new ClassMenaxhimi();
                        menaxhimi.Java = int.Parse(info[i]);
                        menaxhimi.Ekipi1 = info[i+1];
                        menaxhimi.Ekipi2 = info[i+2];
                        menaxhimi.GolaE1 = int.Parse(info[i+3]);
                        menaxhimi.GolaE2 = int.Parse(info[i+4]);
                        menaxhimi.Komenti = info[i+5];
                        lista.Add(menaxhimi);
                    }

                    writer.Serialize(file, lista);
                    file.Close();
                }

                if (dataPrejKlientit.StartsWith("regjistrimi")) 
                {
                    XmlSerializer writer = new XmlSerializer(typeof(List<ClassRegjistrimi>));
                    StreamWriter file = new StreamWriter("../regjistrimi.xml");
                    List<ClassRegjistrimi> lista = new List<ClassRegjistrimi>();


                    dataPrejKlientit = dataPrejKlientit.Substring(11);
                    string[] info = dataPrejKlientit.Split('$');
                    for (int i = 0; i < info.Length - 1; i++)
                    {
                        ClassRegjistrimi regjistrimi = new ClassRegjistrimi();
                        regjistrimi.Ekipi = info[i];
                        lista.Add(regjistrimi);
                    }

                    writer.Serialize(file, lista);
                    file.Close();
                }

                // if webservice
                // clientsList.Add(dataPrejKlientit, clientSocket);
                // dergesa(dataPrejKlientit + " u lidh!", dataPrejKlientit, clientSocket, false);
                // Console.WriteLine(dataPrejKlientit + " hyri ne chat!");


            }

        }

        public static void dergesa(string msg, string uName, TcpClient clientSocket, bool flag)
        {
            NetworkStream broadcastStream = clientSocket.GetStream();
            Byte[] broadcastBytes = null;

            if (flag == true)
            {
                broadcastBytes = Encoding.ASCII.GetBytes(uName + " thot: " + msg);
            }
            else
            {
                broadcastBytes = Encoding.ASCII.GetBytes(msg);
            }
            broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
            broadcastStream.Flush();
        }
    }
}
