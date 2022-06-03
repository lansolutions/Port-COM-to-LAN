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
        public SerialPort PortaBalança;

        public decimal Peso = 0;
        public PortaCOM(out decimal Peso)
        {
            CapturarPesoBalança();
            Peso = this.Peso;
        }

        public void CapturarPesoBalança()
        {
            try
            {
                string[] Configuracao = new ConfiguracaoDosParametros().DadosParametros.Where(x => x.Nome == "PORTA COMx BALANCA").Select(x => x.Valor).FirstOrDefault().Split(';');

                string Porta = Configuracao[0];
                int BaudRate = Convert.ToInt32(Configuracao[1]);
                int Paridade = Convert.ToInt32(Configuracao[2]);
                int DataBits = Convert.ToInt32(Configuracao[3]);
                int BitsParada = Convert.ToInt32(Configuracao[4]);

                PortaBalança = new SerialPort(Porta, BaudRate, (Parity)Paridade, DataBits, (StopBits)BitsParada);

                PortaBalança.DataReceived += DadosRecebidos;

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
        }


        public void DadosRecebidos(object sender, SerialDataReceivedEventArgs e)
        {
            var hexString = PortaBalança.ReadExisting();
            hexString = hexString.Substring(hexString.Length - 6, 5);
            Peso = decimal.Parse(hexString) / 1000.0M;
            PortaBalança.Close();
        }

    }
}
