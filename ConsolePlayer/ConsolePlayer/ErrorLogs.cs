using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePlayer
{
    class ErrorLogs
    {

        public void Errors(string exception) 
        {
            string clock = DateTime.Now.ToString("yyyy.MM.dd.HH.mm");  //rövid dátum
            string ErrLog = Environment.ExpandEnvironmentVariables(@"C:\Users\%USERNAME%\AppData\Roaming\ConsolePlayer\ConsoleLog.txt");
            StreamWriter w = new StreamWriter(ErrLog, true, Encoding.Default);
            w.WriteLine(clock + ": " + exception);
            w.Close();
        }
    }
}
