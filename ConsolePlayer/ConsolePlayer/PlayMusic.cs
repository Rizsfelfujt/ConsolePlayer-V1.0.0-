using INIFiles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsolePlayer
{
    class PlayMusic
    {

        private ZeneKezelo Funkcio = new ZeneKezelo();
        private Player StartPlay = new Player();
        private ProcessKill ProcKiller = new ProcessKill();
        private ErrorLogs Log = new ErrorLogs();

        private void NewMusicST() 
        {
            int pid = ProcKiller.PIdChack();
            ProcKiller.Kill(pid);      
        }

        private void PlayMusic1(string openAlbum) 
        {
                try
                {
                    Console.Write("Add meg a zene ID-d (1/2/..): ");
                    int zeneId = int.Parse(Console.ReadLine());
                    bool End = false;
                    var album = Funkcio.AlbumOpen(openAlbum);
                    if (album == null || album.Count == 0)
                    {
                        Console.WriteLine("Nincs elérhető zene az albumban.");
                        return;
                    }

                    NewMusicST();  //figyelem hogy fute lejátszás és ha igen ki lövöm
                    ProcKiller.WindowsSize();
                    System.Diagnostics.Process.Start("ConsolePlayer.exe");
                    ProcKiller.PidNew0();  //logolom az aktuális processt

                    for (int i = 0; i < album.Count; i++)   //albu  bejárása
                    {
                        var zene = album[i];        // zene betöltése az album listából
                        End = true;
                        if (zene.id == zeneId)     //consolról egyezésvan  kipörgetem a zene tulajdonságaités elindítom
                        {
                            End = false;
                            for (int j = i; j < album.Count; j++)  //haegyezés van j értéke át veszi külső for kezdő értékét ahol megtalálta a zenét 
                            {
                                var playzene = album[j];

                                int id = playzene.id;                   //zene id
                                string path = playzene.path;           //helye
                                string title = playzene.title;         //címe                     
                                string length = playzene.length;       //hossza pl 01:34
                                int close = ProcKiller.NewId();       //bezáráshoz logolt pid idt haszálom
                             
                                Thread t = new Thread(() => StartPlay.Play( path, id, title, length)); //uj szállon elinditom a Player rutint minden paraméterrel 
                                t.Start();                   //elindítja az új szálat
                                t.Join();                   //blokkolja a hívó szálat, amíg a t szál be nem fejeződik.

                                Console.WriteLine();
                                    if (j + 1 < album.Count)
                                    {
                                        var NextZene = album[j + 1];  // ciklus kezdő értékét megnövelem 1 el hogy kitudjam iratni a következő zenét
                                        Console.WriteLine("=> Következő: " + NextZene.id + ": " + NextZene.title + " " + NextZene.length);
                                    }
                                    else
                                    {          
                                        Console.WriteLine("Nincs több zene a mappába! ");
                                        Thread.Sleep(1500);
                                        ProcKiller.Kill(close);
                                    }

                                    Console.Write("Next music? (Y/N): ");
                                    string ok = Console.ReadLine();
                                    if (ok != "y")
                                    {
                                        ProcKiller.Kill(close);
                                    }           
                            }
                        }
                    }
                    if (End == true)
                    {
                        int close = ProcKiller.NewId();       //bezáráshoz logolt pid idt haszálom
                        Console.Clear();
                        Console.WriteLine("Zene [ID:" + zeneId + "] nem található! ");
                        Thread.Sleep(3000);
                        ProcKiller.Kill(close);
                    }
                }
                catch (Exception ex)
                {
                    string exMess = ex.Message + " " + "[rutin: PlayMusic1() ]";
                    Console.WriteLine("Ismeretlen hiba!");
                    Log.Errors(exMess);
                }
         }

        private void MusicSearch(string openAlbum) 
        {       
             try
             {
                  bool info = false;
                  Console.Write("Zene szám címe: ");
                  string musicname = Console.ReadLine();
                  var album = Funkcio.AlbumOpen(openAlbum);
                  Console.WriteLine("------------------------------------");
                  foreach (var zene in album)
                  {
                        if (zene.title.IndexOf(musicname, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            Console.WriteLine("=> "+zene.id + ":" + zene.title + " " + zene.length);
                            info = true;
                        }
                 }
         
                  if (info == true)
                  {
                        PlayMusic1(openAlbum);
                  }

                 if (info == false)
                 {
                     Console.WriteLine("Ez a zene cím nem található! (" + musicname + ")");
                 }
             }
             catch (Exception ex)
             {
                 string exMess = ex.Message + " " + "[rutin: MusicSearch() ]";
                 Console.WriteLine("Ismeretlen hiba!");
                 Log.Errors(exMess);
             }
        }

        private void PlayMenu(bool skip, string openAlbum)
        {
            try
            {
                string ok0;
                string ok1;
                string ok2;
                int Exit = 0;
                if (skip == false)
                {
                    do
                    {
                        Console.Write("Válasszon zeneszámot!  Y/N: ");
                        ok0 = Console.ReadLine();
                        if (ok0 == "y")
                        {
                            PlayMusic1(openAlbum);
                        }
                        Console.Write("Keresés az albumba. Y/N: ");
                        ok1 = Console.ReadLine();
                        if (ok1 == "y")
                        {
                            MusicSearch(openAlbum);
                           
                        }
                        Console.Write("Exit X: ");
                        ok2 = Console.ReadLine();
                        if (ok2 == "x")
                        {
                            Cls STmenu = new Cls();
                            STmenu.Start();
                            Exit++;
                        }
                    } while (Exit < 1);
                }
            }
            catch (Exception ex)
            {
                string exMess = ex.Message + " " + "[rutin: PlayMenu() ]";
                Console.WriteLine("Ismeretlen hiba");
                Log.Errors(exMess);
            }
        }

        public void Start() 
        {
            Funkcio.TrackListakListazasa();
            Console.Write("Album címe: ");
            string openAlbum = Console.ReadLine();
            bool End = Funkcio.AlbumFolder(openAlbum);
            PlayMenu(End, openAlbum);
        }
    }
}





                    //ProcessStartInfo psi = new ProcessStartInfo("ConsolePlayer.exe");
                    //var proc = Process.Start(psi);
                    //inif.Write("Settings", "NewPid", proc.Id.ToString());

