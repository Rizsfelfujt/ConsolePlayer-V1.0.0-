using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;


namespace ConsolePlayer
{
    class PlayAlbum
    {
      private  ErrorLogs Log = new ErrorLogs();
      private  ZeneKezelo Funkcio = new ZeneKezelo();
      private  Player StartPlay = new Player();
      private  ProcessKill ProcKiller = new ProcessKill();

      private void NewMusicST2()
      {
          int pid = ProcKiller.PIdChack();
          ProcKiller.Kill(pid);

      }

      private void WindowSet() 
      {
          ProcKiller.WindowsSize();
          System.Diagnostics.Process.Start("ConsolePlayer.exe");
          ProcKiller.PidNew0();
      }

        private void PlayOptions()
        {       
            List<ConsolePlayer.ZeneKezelo.TrackList> trackListak = Funkcio.AlbumLoad();
            int start = 0;
            int close = ProcKiller.NewId();  
            try
            {
                    Console.Write("Add meg az album sorszámát: ");
                    start = int.Parse(Console.ReadLine());
                    NewMusicST2(); //figyelem hogy fute lejátszás és ha igen ki lövöm
                    WindowSet();
                    foreach (var track in trackListak[start].tracks)
                    {
                          if (!File.Exists(track.path))
                          {
                                Console.WriteLine("Album nem létezik.");
                                Thread.Sleep(3000);
                                ProcKiller.Kill(close);
                          }
                          else
                          {              
                                string path = track.path;
                                string title = track.title;
                                int id = track.id;
                                string length = track.length;
                                Thread t = new Thread(() => StartPlay.Play( path, id, title, length));
                                t.Start();
                                t.Join(); 
                          }                        
                    }
                    Console.Clear();
                    Console.WriteLine("Lejátszás végetért!");
                    Thread.Sleep(6000);
                    ProcKiller.Kill(close);
             }
             catch (Exception ex)
             {
                    string exMess = ex.Message + " " + "[rutin:  Playoption()]";
                    Console.WriteLine("Hibás bemeneti paraméter");
                    Log.Errors(exMess);
             }
         }

        public void Start() 
        {
            Console.WriteLine("\t1 - Albumok megtekintése.");
            Console.WriteLine("\t2 - Exit");
            int Exit = 0;
            do
            {
                switch (Console.ReadLine())
                {
                    case "1":
                         Funkcio.TrackListakListazasa();                  
                         PlayOptions();
                        break;
                    case "2":
                        Cls STmenu = new Cls();
                        STmenu.Start();
                        Exit++;
                        break;
                }
            } while (Exit < 1);
        }
    }
}
