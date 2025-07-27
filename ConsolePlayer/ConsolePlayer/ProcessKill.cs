using INIFiles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePlayer
{
    class ProcessKill
    {
        private ErrorLogs Log = new ErrorLogs();
        private string inifile = Environment.ExpandEnvironmentVariables(@"C:\Users\%USERNAME%\AppData\Roaming\ConsolePlayer\000.ini");

        public int NewId()
        {
            return Process.GetCurrentProcess().Id;
        }

        public void PidStart()
        {
            INIFile inif = new INIFile(inifile);
            int Startpid = NewId();
            string pid = Startpid.ToString();
            inif.Write("Settings", "start", pid);  //irás
            Console.Title = "ConsolePlayer - PID: " + pid;
        }

        public void PidNew0()
        {
            INIFile inif = new INIFile(inifile);
            int Startpid = NewId();
            string pid = Startpid.ToString();
            inif.Write("Settings", "NewPid", pid);  //irás
            Console.Title = "ConsolePlayer - PID: " + pid;
        }

        public int PIdChack()
        {
            INIFile inif = new INIFile(inifile);
            return int.Parse(inif.Read("Settings", "NewPid"));
        }

        public void Kill(int pid) 
        {      
            try
            {
                Process process = Process.GetProcessById(pid);
                process.Kill(); // Leállítja a folyamatot
            }
            catch (Exception ex)
            {
                string exMess = ex.Message + " " + "[rutin:  Kill()]";
                Log.Errors(exMess);
            }
        }

        public void WindowsSize() 
        {
            int szelesseg = Console.WindowWidth; // Megtartjuk a jelenlegi szélességet
            int magassag = 3;

            if (Console.BufferHeight < magassag)  // Ellenőrizzük, hogy a buffer elég nagy-e
                Console.BufferHeight = magassag;

            Console.SetWindowSize(szelesseg, magassag);  // Beállítjuk az ablak magasságát 3-ra
        }
    }
}
