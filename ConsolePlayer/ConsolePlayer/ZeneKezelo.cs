using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace ConsolePlayer
{
    class ZeneKezelo
    {
        public static ErrorLogs Log = new ErrorLogs();
        private string jasonfolder = Environment.ExpandEnvironmentVariables(@"C:\Users\%USERNAME%\AppData\Roaming\ConsolePlayer\Tracklist.json");    
        private void AddMusic(string open, string listaNev, bool manual = true)
        {
            try
            {
                List<TrackList> trackListak = AlbumLoad();
                TrackList kivalasztottLista = trackListak.FirstOrDefault(t => t.trackListName == listaNev); // Megnézzük linqe lekérdezéssel létezik-e a track lista
                if (kivalasztottLista == null)
                {
                    kivalasztottLista = new TrackList           // Ha nem létezik, létrehozzuk
                    {
                        trackListName = listaNev,             //listaNev (album neve) kézzel adom meg a string értékét
                        tracks = new List<Track>()           //itt hozzá férek a zenkékhez (Track objektumaihoz)
                    };
                    trackListak.Add(kivalasztottLista);    //bemenetről kapott tracklista név hozzá adása
                }   
                int nextId = IdCount(kivalasztottLista.tracks);
                Track ujTrack = new Track            //itt a zene szám adatai címe (pl Kisgrofo.mp4) hossza és út vonala
                {
                    id = nextId,
                    title = MusicName(open),      //zene cím
                    path = open,                 //mappája
                    length = MusicLenght(open)  // zene hossza
                };
                kivalasztottLista.tracks.Add(ujTrack); // itt a zenét hozzá fűzöm a listába
                File.WriteAllText(jasonfolder, JsonConvert.SerializeObject(trackListak, Formatting.Indented));   //List<TrackList>) JSON formátumú szöveggé alakítja.             
                if (manual == true)
                {
                    Console.WriteLine("Zene hozzáadva a " + listaNev + " listához!");
                }
                else
                {             
                    string title = MusicName(open);
                    Console.WriteLine("OK => " + title);                
                }
            }
            catch (Exception ex)
            {
                string exMess = ex.Message + " " + "[rutin: AddMusic()]";            
                Console.WriteLine("Hiba történt: Zene hozzáadáskor");
                Log.Errors(exMess);
            }
        }

        public List<TrackList> AlbumLoad() //itt kezelem Track listámat (vagy nem tudom albumok ahol több zene van)
        {
            if (!File.Exists(jasonfolder))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(jasonfolder));
                File.WriteAllText(jasonfolder, "[]");
            }
            string jsonText = File.ReadAllText(jasonfolder);
            return JsonConvert.DeserializeObject<List<TrackList>>(jsonText) ?? new List<TrackList>(); //Newtonsoft.Json könyvtárból való utasítás JSON szöveget lisa objektumba akarja tenni
        }

        public List<Track> AlbumOpen(string album)
        {
            var trackLists = AlbumLoad();
            foreach (var lista in trackLists)
            {
                if (lista.trackListName == album)
                {
                    if (lista.trackListName.Trim().Equals(album.Trim(), StringComparison.OrdinalIgnoreCase)) //szóközök és egyéb nem kívánatos karakterek eltávolítása
                    {
                        return lista.tracks;
                    }
                }
            }
            return new List<Track>(); // ha nem található ilyen című album
        }

        public bool AlbumFolder(string albumcime)  //albumok tartalmát ki listázhatom
        {
            try
            {
                    var album = AlbumOpen(albumcime);
                    if (album.Count == 0)
                    {
                        Console.WriteLine("Nincs ilyen nevű mappa, vagy üres / nem lett kiválasztva!");
                        return true;             //true esetén  PlaySearch() nem hajtódik végre a benne tárolt kód
                    }
                    else
                    {
                        Console.WriteLine("Talált zenék:");
                        foreach (var zene in album)
                        {
                            Console.WriteLine(zene.id + ": " + zene.title + " " + zene.length);
                        }
                        return false;
                    }     
            }
            catch (Exception ex)
            {
                string exMess = ex.Message + " " + "[rutin: AlbumOpen()]";
                Console.WriteLine("Hiba történt: Album megnyitásakor!");
                Log.Errors(exMess);
                return true;                         //true esetén  PlaySearch() nem hajtódik végre a benne tárolt kód
            }    
        }

        public void MusicSearch() //zenék keresése ez majd más funkcióknál is kelleni fog!!
        {
            var trackLists = AlbumLoad();
            Console.Write("Zene keresés Címre Y/N:  ");
            string OK = Console.ReadLine();
            string musicname = "";
            bool info = false;
            try
            {
                if (OK == "y")
                {
                    Console.Write("Zene szám címe: ");
                    musicname = Console.ReadLine();
                    foreach (var lista in trackLists)
                    {
                        for (int i = 0; i < lista.tracks.Count; i++)
                        {
                            if (lista.tracks[i].title.IndexOf(musicname, StringComparison.OrdinalIgnoreCase) >= 0) // pontosabban keres részszövegket is kidob
                            {
                                Console.WriteLine(lista.tracks[i].id + ": " + lista.tracks[i].title + " " + lista.tracks[i].length);
                                info = true;
                            }
                        }
                    }
                    if (info == false)
                    {
                        Console.WriteLine("Ez a zene cím nem található! (" + musicname + ")");
                    }
                }     
            }
            catch (Exception ex)
            {
                string exMess = ex.Message + " " + "[rutin: MusicSearch()]";
                Console.WriteLine("Hiba történt: keresés");
                Log.Errors(exMess);
            }      
        }

        private void MusicDelet() 
        {
            var trackLists = AlbumLoad();
            bool Delet = false;
            int id = 0;
            try
            {
                Console.Write("Törlöd a számot? Y/N: ");
                string OK = Console.ReadLine();
                if (OK == "y")
                {
                    Console.Write("Delet zene: ");
                    id = int.Parse(Console.ReadLine());
                    foreach (var lista in trackLists)
                    {
                        for (int i = 0; i < lista.tracks.Count; i++)
                        {
                            if (lista.tracks[i].id == id)
                            {
                                lista.tracks.RemoveAt(i);
                                Delet = true;
                                for (int y = 0; y < lista.tracks.Count; y++)
                                {
                                    lista.tracks[i].id = i + 1;
                                }
                                Console.WriteLine("Törölt elm: " + lista.trackListName);
                                break;  
                            }
                        }
                    }

                    if (Delet == true)
                    {
                        File.WriteAllText(jasonfolder, JsonConvert.SerializeObject(trackLists, Formatting.Indented));
                    }
                }
            }
            catch (Exception ex)
            {
                string exMess = ex.Message + " " + "[rutin: MusicDelet()]";
                Console.WriteLine("Hiba történt: delet");
                Log.Errors(exMess);
            }
        }

        public void AlbumDelet(string deletalbum)   //album törlése
        {
            try
            {
                List<TrackList> trackListak = AlbumLoad(); //listák beolvasása
                for (int i = 0; i < trackListak.Count; i++)
                {
                    if (trackListak[i].trackListName == deletalbum) //keresem albumot
                    {
                        trackListak.RemoveAt(i);
                        break;
                    }
                }
                string ujJson = JsonConvert.SerializeObject(trackListak, Formatting.Indented);               // fájl vissza írása
                File.WriteAllText(jasonfolder, ujJson);
                Console.WriteLine("Album törölve: " + deletalbum);
            }
            catch (Exception ex)
            {
                string exMess = ex.Message + " " + "[rutin: deletalbum()]";
                Console.WriteLine("Hiba történt: Album delet");
                Log.Errors(exMess);
            }
        }

        private string MusicLenght(string open)
        {
            TagLib.File file = TagLib.File.Create(open);
            string hossz = file.Properties.Duration.ToString();
            hossz = hossz.Remove(hossz.Length - 8, 8);
            return hossz;
        }

        private string MusicName(string open) 
        {
            string[] Filetomb = { ".mp3", ".mp4", ".wav", ".midi", ".amr", ".flac", ".ogg" };    //kiterjesztések tömb 
            string MusicName = "";
            for (int i = 0; i < Filetomb.Length; i++)
            {
                if (open.Contains(Filetomb[i]) )                        //vane benne támogatott kiterjeztés
                {
                    int perjel = open.LastIndexOf('\\');              // utolsó perjel. száma 0-tól számol 
                    return MusicName = open.Substring(perjel + 1);   // utolsó perjelig levágom +1 az utolsó perjel 
                }
            }
            Console.WriteLine("Fájl nem volt felismerhető");
            return MusicName ="ERRor";
        }

        private void ADDMusicFoldre(string StMappa) //hozzá adok egy mappát és tartama egy uj Track lesz
        {
            try
            {   
                foreach (var mappa in Directory.EnumerateFiles(StMappa, "*", SearchOption.TopDirectoryOnly))
                {
                    int perjel = StMappa.LastIndexOf('\\');              // utolsó perjel. száma 0-tól számol 
                    string TrackListCim = StMappa.Substring(perjel + 1);   // utolsó perjelig levágom +1 az utolsó perjel 
                    AddMusic(mappa, TrackListCim, false);
                }
            }
            catch (Exception ex)
            {
                string exMess = ex.Message + " " + "[rutin:  ADDMusicFoldre()]";
                Console.WriteLine("Hiba történt: mappa hzzá adás");
                Log.Errors(exMess);
            }
        }

        public void TrackListakListazasa()  //ki listázza az albumokat
        {
            int FolderId = -1;
            var trackListak = AlbumLoad();  //lista betőltőből ki nyerem a track cimeket
            if (trackListak.Count == 0)
            {
                Console.WriteLine("Nincs elérhető Track lista.");
                return;
            }
            Console.WriteLine("Elérhető mappák:");
            foreach (var lista in trackListak)
            {
                FolderId ++;
                Console.WriteLine("=>" + FolderId + ";" + lista.trackListName);
            }
        }

        public int IdCount(List<Track> zeneszam) //kézzel felvett zene id hozzá adás
        {
            return zeneszam.Any() ? zeneszam.Max(t => t.id) + 1 : 1;
        }

        public class Track   // zene szám adatai 
        {
            public int id { get; set; }
            public string title { get; set; }
            public string path { get; set; }
            public string length { get; set; }
        }

        public class TrackList  //adott track lista neve
        {
            public string trackListName { get; set; }
            public List<Track> tracks { get; set; }
        }

        public void Start() 
        {
            Console.WriteLine("\t1 - Track lista bővités");      // ki kéne listázni a létező trackeket 
            Console.WriteLine("\t2 - Mappa hozzáadás Tracknek");
            Console.WriteLine("\t3 - Összes tack lista");
            Console.WriteLine("\t4 - Album törlés");
            Console.WriteLine("\t5 - Zene törlés");
            Console.WriteLine("\tx - Vissza a fő menűbe");
            int Exit = 0;
            do
            {
                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Write("Tallozás: ");      
                        string open0 = Console.ReadLine();
                        Console.Write("Add mappa: ");    
                        string tracklist = Console.ReadLine();
                        AddMusic(open0,tracklist);
                        break;
                    case "2":
                        Console.Write("Tallozás: ");      
                        string open1 = Console.ReadLine();
                        ADDMusicFoldre(open1); 
                        break;
                    case "3":
                        TrackListakListazasa();
                          Console.Write("Album megtekintése Y/N: ");
                         string ok = Console.ReadLine();
                         if (ok == "y")
                         {
                             Console.Write("Album címe: ");
                             string albumcime = Console.ReadLine();
                             AlbumFolder(albumcime);
                         }                  
                        break;
                    case "4":
                        Console.Write("Delet album: ");
                        string delet = Console.ReadLine();
                        AlbumDelet(delet);
                        break;
                    case "5":
                        MusicSearch();
                        MusicDelet();       
                        break;
                    case "x":
                        Cls STmenu = new Cls();
                        STmenu.Start();
                        Exit++;
                        break;
                }
            } while (Exit < 1);
        }
    }
}
