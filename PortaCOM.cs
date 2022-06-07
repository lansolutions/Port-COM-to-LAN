using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace Port_COM_to_LAN
{
    internal class PortaCOM
    {
        public decimal Peso { get; set; }

        public string Ip;

        public PortaCOM(string Ip)
        {
            this.Ip = Ip;
            CapturarPesoBalança();
        }
        public decimal CapturarPesoBalança()
        {
            decimal Peso = 0;
           
            try
            {
                string[] Configuracao = new ConfiguracaoDosParametros().DadosParametros.Where(x => x.Nome == "PORTA COMx BALANCA").Select(x => x.Valor).FirstOrDefault().Split(';');

                string Porta = Configuracao[0];
                int BaudRate = Convert.ToInt32(Configuracao[1]);
                int Paridade = Convert.ToInt32(Configuracao[2]);
                int DataBits = Convert.ToInt32(Configuracao[3]);
                int BitsParada = Convert.ToInt32(Configuracao[4]);

                SerialPort PortaBalança = new SerialPort(Porta, BaudRate, (Parity)Paridade, DataBits, (StopBits)BitsParada);

                PortaBalança.DataReceived += (s, e) => DadosRecebidos(PortaBalança);

                if (!PortaBalança.IsOpen)
                {
                    PortaBalança.Open();
                }

                PortaBalança.WriteLine("\u0005\r\n");
            }
            catch (Exception Ex)
            {
                Log.Logger(Ex.ToString());
            }
            
            return Peso;

        }

        public void DadosRecebidos(SerialPort PortaBalança)
        {
            var hexString = PortaBalança.ReadExisting();
            hexString = hexString.Substring(hexString.Length - 6, 5);
            Peso = decimal.Parse(hexString) / 1000.0M; Peso = Math.Round(Peso, 3);
            Log.Logger($"Peso Adquirido: {Peso}");
            PortaBalança.Close();
            new PortaLANClient($"{Peso};{Ip}");

        }
    }
}
