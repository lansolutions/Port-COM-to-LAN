using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Port_COM_to_LAN
{
    
    public class Log 
    {            
        public static void Logger(string Logs)
        {
            try
            {              
                string NovoLog = $"{DateTime.Now}: {Logs}{Environment.NewLine}";
                
                LogTxt(NovoLog);
            }
            catch
            {
               
            }
        }
        private static void LogTxt(string Log)
        {
            if (!Directory.Exists(@"C:\LanSolutions\PortaComToLan\Logs\"))
            {
                Directory.CreateDirectory(@"C:\LanSolutions\PortaComToLan\Logs\");
            }

            string PathLog = @"C:\LanSolutions\PortaComToLan\Logs\";

            string Data = DateTime.Now.ToString("dd" + "MM" + "yyyy");

            if (!Directory.Exists($@"{PathLog}{Data}\"))
            {
                Directory.CreateDirectory($@"{PathLog}{Data}\");
            }

            string PathLogHoje = PathLog + Data + @"\";

            if (!File.Exists(PathLogHoje + "Log.txt"))
            {
                File.Create(PathLogHoje + "Log.txt").Dispose();                
            }

            try
            {
                using (StreamWriter outputFile = new StreamWriter(PathLogHoje + "Log.txt", append: true))
                {
                    outputFile.Write(Log);
                }
            }
            catch
            {

            }



        }
    }
}
