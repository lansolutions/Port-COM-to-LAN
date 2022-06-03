using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SuperSimpleTcp;

namespace Port_COM_to_LAN
{
    internal class PortaLANClient
    {
        string IpClient;     
        string EnvioDados;

        public PortaLANClient(string Dados)
        {
            IpClient = new ConfiguracaoDosParametros().DadosParametros.Where(x => x.Nome == "ENDEREÇO IPV4 CONTROL CENTER CLIENT").FirstOrDefault().Valor;
            EnvioDados = "Peso;";
            EnvioDados += Dados;
            ForçarSincronizacao();
        }


        public void ForçarSincronizacao()
        {
            try
            {
                using (SimpleTcpClient client = new SimpleTcpClient($"{IpClient}:3700"))
                {
                    client.Connect();

                    client.Send(EnvioDados);

                    client.Events.DataSent += (s, f) => { client.Disconnect(); };                    
                }
            }
            catch (Exception Ex)
            {
                Log.Logger(Ex.ToString());
            }

        }

    }


    internal class PortaLANServer
    {
        public SimpleTcpServer server;
        string RecebimentoDados;

        public PortaLANServer()
        {
            IniciarServidor();
        }
        
       
        private void IniciarServidor()
        {
            string IP = string.Empty;
            
            try
            {
                string localIP = string.Empty;

                using (System.Net.Sockets.Socket socket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    socket.Connect("8.8.8.8", 65530);
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;

                    localIP = endPoint.Address.ToString();
                    localIP = endPoint.Address.ToString();

                    IP = localIP;

                }
            }
            catch (Exception)
            {
                try
                {
                    string nomeMaquina = Dns.GetHostName();
                    IPAddress[] ipLocal = Dns.GetHostAddresses(nomeMaquina);
                    IP = ipLocal.LastOrDefault().ToString();
                }
                catch (Exception Ex)
                {
                    Log.Logger(Ex.ToString());
                }
               
            }

            try
            {
                IP += ":3700";
                server = new SimpleTcpServer(IP);
                server.Events.DataReceived += Events_DataReceived;
                server.Start();
            }
            catch (Exception Ex)
            {
                Log.Logger(Ex.ToString());
            }


            
          

        }
       
        private void Events_DataReceived(object sender, DataReceivedEventArgs e)
        {
            try
            {
                RecebimentoDados = Encoding.UTF8.GetString(e.Data);

                if (RecebimentoDados == "RequisicaoDePeso")
                {
                    new PortaCOM(out decimal Peso);

                    new PortaLANClient(Peso.ToString());
                }
            }
            catch(Exception Ex)
            {
                Log.Logger(Ex.ToString());
            }
           
        }
    }
}
