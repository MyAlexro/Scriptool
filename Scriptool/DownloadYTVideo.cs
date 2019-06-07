using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using static Scriptool.MainClass; //per accedere alle impostazioni

namespace Scriptool
{
    class DownloadYTVideo
    {
        public static string urlInput = "";
        static WebClient clientWeb;
        static int DownloadUrlStart = 0;
        static int DownloadUrlEnd = 0;
        static string videoTitle;
        static char[] videoIdChar;  //crea l'array dove conservare l'id del video
        public static string videoId; //la stringa dell'id del video
        public static char[] charDecodedResponse;
        public static string decodedResponse; //la risposta dalla web request alla page del video
        public static char[] urlDownloadChar; //url di download del video in un array di char
        public static string UrlDownloadStringDecoded; //la stringa decodata dell'url di download del video
        static char[] videoTitleChar;
        public static string urlDownloadString;
        static int counter = 0;
        public static bool downloadFinished = false;
        public static int lastPos = 0; //posizione dell'ultimo url non giusto
        public static Dictionary<String, bool> availableQualities; //contenitore di qualità in cui può essere scaricato il video
        public static string chosenQuality; //qualità scelta dall'user
        static char[] invalidChars = Path.GetInvalidFileNameChars(); //caratteri non valide con cui nominare un file
        static int n = 0; //var per una funzione di debug e per individ. num di video con lo stesso nome
        static double rawFileSize;
        static double fileSize;

        public static void Start_GetMetadata()
        {
            if (lingua == "IT")
            {
                Console.Write("\n3: Inserire l'url del video che si vuole scaricare: ");
            }
            else if (lingua == "EN")
            {
                Console.Write("\n3: Insert the url of the video you want to download: ");
            }
            urlInput = Console.ReadLine();
            char[] url = urlInput.ToCharArray();
            counter = 2;
            videoIdChar = new char[11];
            for (int i = 0; i <= url.Length - 1; i++) //lenght-1 perchè così arriva fino al blocco N°66 dell'array, se no arrivava al blocco N°67 dell'array (che non esiste)
            {
                if (url[i] == 'v' && url[i + 1] == '=') //l'id parte da "v=", ho messo di controllare "v" e "=" perchè nel link del video ce ne potrebbero essere 2 di "="
                {
                    for (int a = 0; a <= videoIdChar.Length - 1; a++)
                    {
                        videoIdChar[a] = url[i + counter];
                        counter++;
                    }
                }
            }
            clientWeb = new WebClient(); //inizializza un webclient
            videoId = new string(videoIdChar); //converte l'array di char in stringa
            string getInfoUrl = $"https://www.youtube.com/get_video_info?video_id={videoId}"; //crea l'url da cui prendere le info
            try
            {
                string encodedResponse = clientWeb.DownloadString(getInfoUrl.ToString());  //prende le informazioni dall'url
                decodedResponse = Uri.UnescapeDataString(encodedResponse);
                decodedResponse = Uri.UnescapeDataString(encodedResponse);
                decodedResponse = Uri.UnescapeDataString(encodedResponse);
                decodedResponse = Uri.UnescapeDataString(encodedResponse);
                decodedResponse = Uri.UnescapeDataString(encodedResponse); //decoda la risposta del server 6 volte perchè una volta non basta per convertire tutti i simboli url in testo(idk perchè)
                charDecodedResponse = decodedResponse.ToCharArray(); //converte la risposta in array char
                if (decodedResponse.Contains("reason=Invalid+parameters"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.CursorVisible = false;
                    if (lingua == "IT")
                    {
                        Console.WriteLine($"Url non valido, premere Invio per tornare al Menu principale");
                    }
                    else if (lingua == "EN")
                    {
                        Console.WriteLine("Invalid Url, press Enter to go back to the main Menu");
                    }
                    Console.ReadLine();
                    MenuPrint();
                }
                else
                {
                    GetTitle();
                }
            }
            catch (WebException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                if (lingua == "IT")
                {
                    Console.WriteLine("Connessione internet assente, premere Invio per tornare al Menu principale");
                }
                else if (lingua == "EN")
                {
                    Console.WriteLine("No internet connection, press Enter to go back to the main Menu");
                }
                Console.ReadLine();
                MenuPrint();
            }
        }



        static void GetTitle()
        {
            int titleStart = 0;
            int titleEnd = 0;
            for (int i = 0; i <= charDecodedResponse.Length; i++)
            {
                if (charDecodedResponse[i] == 't' && charDecodedResponse[i + 1] == 'l' && charDecodedResponse[i + 2] == 'e' && charDecodedResponse[i + 3] == '=')
                {
                    titleStart = i + 4;
                    break;
                }
            }

            if (titleStart != 0)
            {
                try
                {
                    for (int i = titleStart; i <= charDecodedResponse.Length; i++)
                    {
                        if (charDecodedResponse[i] == '&')
                        {
                            titleEnd = i - 1;
                            break;
                        }
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    titleEnd = charDecodedResponse.Length - 1;
                }
                int titleLength = (titleEnd - titleStart) + 1;
                videoTitleChar = new char[titleLength];
                counter = 0;
                for (int i = titleStart; i <= titleEnd; i++)
                {
                    videoTitleChar[counter] = charDecodedResponse[i];
                    counter++;
                }
                videoTitle = WebUtility.UrlDecode(new string(videoTitleChar));
                if (lingua == "IT")
                {
                    Console.WriteLine($"Titolo del video: {videoTitle}");
                }
                else if (lingua == "EN")
                {
                    Console.WriteLine($"Video title: {videoTitle}");
                }
                GetQualities();
            }

        }


        static void GetQualities()
        {
            string bufferPrecedente = "       _____           _       _              _      \n" +
                "      /  ___|         (_)     | |            | |     \n" +
                "      \\ `--.  ___ _ __ _ _ __ | |_ ___   ___ | |     \n" +
                "       `--. \\/ __| '__| | '_ \\| __/ _ \\ / _ \\| |     \n" +
                "      /\\__/ / (__| |  | | |_) | || (_) | (_) | |     \n" +
                "      \\____/ \\___|_|  |_| .__/ \\__\\___/ \\___/|_|     \n" +
                "                        | |                          \n" +
                "                        |_|                           \t \n" +
                "________________________________________________________________________________________________________________________\n" +
                "\n\n\n\n\n";
            if (lingua == "IT") //ho fatto così se no la scritta scriptool e le opzioni del menu principale si sarebbero cancellati ogni volta che si muove opzione
            {
                bufferPrecedente += " 1) Genera un codice QR\n" +
                " 2) Genera una password\n" +
                " 3) Scarica un video da Youtube\n" +
                " 4)\n" +
                " 5)\n" +
                " 6)\n" +
                " 7) Impostazioni\n" +
                " 8) Esci\n" +
                $"\n3: Inserire Url del video che si vuole scaricare: {urlInput}\n\n" +
                $" Titolo del video: {videoTitle}\n\n" +
                $"  Seleziona la qualità del video:";
            }
            else if (lingua == "EN")
            {
                bufferPrecedente += " 1) Generate a QR code\n" +
                " 2) Generate a password\n" +
                " 3) Download a video from Youtube\n" +
                " 4)\n" +
                " 4)\n" +
                " 5)\n" +
                " 6)\n" +
                " 7) Settings\n" +
                " 8) Exit\n" +
                $"\n3: Insert the link of the video you want to download: {urlInput}\n\n" +
                $" Video title: {videoTitle}\n\n" +
                $"  Select the quality of the video:";
            }
            /* Individua le qualità in cui può essere scaricato il video e aggiunge la qualità al dictionary*/
            availableQualities = new Dictionary<String, bool>();
            if (decodedResponse.Contains("itag=37") || decodedResponse.Contains("itag=85") || decodedResponse.Contains("itag=96"))
            {
                availableQualities.Add("FullHD", true);
            }
            if (decodedResponse.Contains("itag=22") || decodedResponse.Contains("itag=84") || decodedResponse.Contains("itag=95"))
            {
                availableQualities.Add("HD", true);
            }
            if (decodedResponse.Contains("itag=59") || decodedResponse.Contains("itag=78") || decodedResponse.Contains("itag=83") || decodedResponse.Contains("itag=94"))
            {
                availableQualities.Add("480p", true);
            }
            if (decodedResponse.Contains("itag=18") || decodedResponse.Contains("itag=82") || decodedResponse.Contains("itag=93"))
            {
                availableQualities.Add("360p", true);
            }
            if (decodedResponse.Contains("itag=6") || decodedResponse.Contains("itag=91") || decodedResponse.Contains("itag=132"))
            {
                availableQualities.Add("240p", true);
            }
            if (decodedResponse.Contains("itag=139") || decodedResponse.Contains("itag=140") || decodedResponse.Contains("itag=141") || decodedResponse.Contains("itag=256") || decodedResponse.Contains("itag=258") || decodedResponse.Contains("itag=325") || decodedResponse.Contains("itag=328"))
            {
                availableQualities.Add("AudioOnly", true);
            }
            string[] opz = new string[availableQualities.Count + 1]; //opzioni da passare al PrintOptMenu
            int n = 0;
            System.Diagnostics.Debug.WriteLine($"availableQualities.Count = {availableQualities.Count}");
            if (availableQualities.Count != 0)
            {
                foreach (var value in availableQualities)
                {
                    string buffer = $"  {availableQualities.ElementAt(n).ToString().Replace(", True]", "").Replace("[", "")}";
                    opz[n] = buffer;
                    n++;
                }
            }
            else if (availableQualities.Count == 0)
            {
                if (lingua == "IT")
                {
                    System.Diagnostics.Debug.WriteLine("nessuna qualità trovata");
                    bufferPrecedente += "\n\n  Nessuna qualità trovata";
                }
                else if (lingua == "EN")
                {
                    bufferPrecedente += "\n\n  No qualities found";
                }
            }
            if (lingua == "IT") //aggiunge l'opz di tornare indietro
            {
                opz[opz.Length - 1] = "\n<-Ritorna indietro\n";
            }
            else if (lingua == "EN")
            {
                opz[opz.Length - 1] = "\n<-Go back\n";
            }
            PrintOptMenu(opz, bufferPrecedente, "GetQualities");
        }


        public static void GetDownloadUrl(int chosenOpt)
        {
            for (int a = lastPos; a <= charDecodedResponse.Length - 1; a++) //cerca l'inizio dell'url di download del video
            {
                if (charDecodedResponse[a] == 'u' && charDecodedResponse[a + 1] == 'r' && charDecodedResponse[a + 2] == 'l' && charDecodedResponse[a + 3] == '=')
                {
                    DownloadUrlStart = a + 4;
                    break;
                }
            }
            if (DownloadUrlStart != 0)
            {
                for (int b = DownloadUrlStart; b <= charDecodedResponse.Length - 1; b++) //cerca la fine dell'url di download del video
                {
                    if (charDecodedResponse[b] == ',' || charDecodedResponse[b] == '&')
                    {
                        DownloadUrlEnd = b - 1;
                        break;
                    }
                }
            }
            if (DownloadUrlEnd != 0)
            {
                int urlLenght = (DownloadUrlEnd - DownloadUrlStart) + 1; //calcola la lunghezza dell'url di download del video, +1 perchè c'è un errore da qualche parte
                counter = 0;                                        //nella ricerca dell'inizio o della fine dell'url di...
                urlDownloadChar = new char[urlLenght]; //inizializza l'array di char con la lunghezza dell'url
                for (int c = DownloadUrlStart; c <= DownloadUrlEnd; c++)
                {
                    urlDownloadChar[counter] = charDecodedResponse[c]; //passa l'url di download(...) dai metadati del video all'array di char
                    counter++;
                }
                urlDownloadString = new string(urlDownloadChar); //converte l'array in string
                UrlDownloadStringDecoded = Uri.UnescapeDataString(urlDownloadString); //decoda il link per scaricare il video 5 volte
                UrlDownloadStringDecoded = Uri.UnescapeDataString(urlDownloadString);
                UrlDownloadStringDecoded = Uri.UnescapeDataString(urlDownloadString);
                UrlDownloadStringDecoded = Uri.UnescapeDataString(urlDownloadString);
                UrlDownloadStringDecoded = Uri.UnescapeDataString(urlDownloadString);

                CheckIfUrlValid(chosenOpt);
            }
        }


        static void CheckIfUrlValid(int chosenOpt) //controlla se l'url trovato corrisponde alla qualità scelta
        {
            n++;
            System.Diagnostics.Debug.WriteLine($"CheckIfUrlValid execution number {n}");
            chosenQuality = availableQualities.ElementAt(chosenOpt).ToString().Replace(", True]", "").Replace("[", "");
            if (chosenQuality == "FullHD")
            {
                if (UrlDownloadStringDecoded.Contains("itag=37") || UrlDownloadStringDecoded.Contains("itag=85") || UrlDownloadStringDecoded.Contains("itag=96"))
                {
                    DownloadVideo();
                }
                else
                {
                    lastPos = DownloadUrlEnd;
                    GetDownloadUrl(chosenOpt);
                }
            }
            else if (chosenQuality == "HD")
            {
                if (UrlDownloadStringDecoded.Contains("itag=22") || UrlDownloadStringDecoded.Contains("itag=84") || UrlDownloadStringDecoded.Contains("itag=95"))
                {
                    DownloadVideo();
                }
                else
                {
                    lastPos = DownloadUrlEnd;
                    GetDownloadUrl(chosenOpt);
                }
            }
            else if (chosenQuality == "480p")
            {
                if (UrlDownloadStringDecoded.Contains("itag=59") || UrlDownloadStringDecoded.Contains("itag=78") || UrlDownloadStringDecoded.Contains("itag=83") || UrlDownloadStringDecoded.Contains("itag=94"))
                {
                    DownloadVideo();
                }
                else
                {
                    lastPos = DownloadUrlEnd;
                    GetDownloadUrl(chosenOpt);
                }
            }
            else if (chosenQuality == "360p")
            {
                if (UrlDownloadStringDecoded.Contains("itag=18") || UrlDownloadStringDecoded.Contains("itag=82") || UrlDownloadStringDecoded.Contains("itag=93"))
                {
                    DownloadVideo();
                }
                else
                {
                    lastPos = DownloadUrlEnd;
                    GetDownloadUrl(chosenOpt);
                }
            }
            else if (chosenQuality == "240p")
            {
                if (UrlDownloadStringDecoded.Contains("itag=6") || UrlDownloadStringDecoded.Contains("itag=91") || UrlDownloadStringDecoded.Contains("itag=132"))
                {
                    DownloadVideo();
                }
                else
                {
                    lastPos = DownloadUrlEnd;
                    GetDownloadUrl(chosenOpt);
                }
            }
            else if (chosenQuality == "AudioOnly")
            {
                if (UrlDownloadStringDecoded.Contains("itag=139") || UrlDownloadStringDecoded.Contains("itag=140") || UrlDownloadStringDecoded.Contains("itag=141") || UrlDownloadStringDecoded.Contains("itag=256") || UrlDownloadStringDecoded.Contains("itag=258") || UrlDownloadStringDecoded.Contains("itag=325") || UrlDownloadStringDecoded.Contains("itag=328"))
                {
                    DownloadVideo();
                }
                else
                {
                    lastPos = DownloadUrlEnd;
                    GetDownloadUrl(chosenOpt);
                }
            }
        }



        static void DownloadVideo()
        {
            Uri UriDownloadVideo = new Uri(UrlDownloadStringDecoded); //converte la stringa in Uri
            clientWeb.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompletedAsync); //assegna il metodo DownloadCompleted all'evento DownloadFileCompleted
            try
            {
                for (int i = 0; i <= invalidChars.Length - 1; i++) //toglie i caratteri non validi nel titolo
                {
                    if (videoTitle.Contains(invalidChars[i]))
                    {
                        System.Diagnostics.Debug.WriteLine("Carattere non valido nel titolo trovato e rimpiazzato");
                        videoTitle = videoTitle.Replace(invalidChars[i].ToString(), "");
                    }
                }

                n = 1;
                if (!File.Exists($@"{defaultVideoPath}\{videoTitle}.mp4")) //se il file non esiste già
                {
                    clientWeb.DownloadFileAsync(UriDownloadVideo, $@"{defaultVideoPath}\{videoTitle}.mp4"); //comincia a scaricare il file
                }
                else
                {
                    foreach (var file in defaultVideoPath) //se il video esiste già aggiunge  n  al nome (nomeVideo1, nomeVideo2 ecc.)
                    {
                        if (!File.Exists($@"{defaultVideoPath}\{videoTitle}({n}).mp4"))
                        {
                            clientWeb.DownloadFileAsync(UriDownloadVideo, $@"{defaultVideoPath}\{videoTitle}({n}).mp4");
                            videoTitle += $"({n})";
                            break;
                        }
                        else
                        {
                            n++;
                        }
                    }
                }
                DateTime startTimeDate = DateTime.Parse(DateTime.Now.ToString()); //prende la data e l'ora locali e li analizza per individuare il formato dell'ora
                string startTime = startTimeDate.ToString("HH:mm:ss"); //prende l'ora, i minuti e i secondi dal DateTime completo
                if (lingua == "IT")
                {
                    Console.WriteLine($"Dimensione del video: {GetVideoSize(UriDownloadVideo)}\n" +
                        $"Tempo stimato per il download: {GetEstimatedFinishTime($@"{defaultVideoPath}\{videoTitle}.mp4")}");
                    Console.WriteLine($"Download del video iniziato {startTime}, verrà salvato nella cartella predefinita. Non chiudere il programma!");
                    Console.Write("Download in corso...");
                }
                else if (lingua == "EN")
                {
                    Console.WriteLine($"Size of the video: {GetVideoSize(UriDownloadVideo)}\nEstimated download time: {GetEstimatedFinishTime($@"{defaultVideoPath}\{videoTitle}.mp4")}");
                    Console.WriteLine($"Download of the video started {startTime}, it'll be saved in the defined folder. Please don't close the program!");
                    Console.Write("Downlading...");
                }
                while (downloadFinished == false)
                {
                    Console.Write(".");
                    Thread.Sleep(500);
                }
            }
            catch (WebException)
            {
                if (lingua == "IT")
                {
                    Console.WriteLine("Errore nello scaricare il video, premere Invio per tornare al Menu principale");
                }
                else if (lingua == "EN")
                {
                    Console.WriteLine("An error occured while trying to download the video, press Enter to go back to the main Menu");
                }
                Console.Read();
                MenuPrint();
            }

            if (downloadFinished == true)
            {
                FileInfo FileSize = new FileInfo($@"{defaultVideoPath}\{videoTitle}.mp4");
                if (FileSize.Length <= 1)
                {
                    if (lingua == "IT")
                    {
                        Console.WriteLine("\nE' stato impossibile scaricare il video, premere Invio per tornare al Menu principale");
                    }
                    else if (lingua == "EN")
                    {
                        Console.WriteLine($"\nIt was impossible to download the video, press Enter to go back to the main Menu");
                    }
                    File.Delete($@"{defaultVideoPath}\{videoTitle}.mp4");
                }
                else
                {
                    DateTime finishTimeDate = DateTime.Parse(DateTime.Now.ToString());
                    string finishTime = finishTimeDate.ToString("HH:mm:ss");
                    if (lingua == "IT")
                    {
                        Console.WriteLine($"\nDownload video completato {finishTime}, premere Invio per tornare al Menu principale");
                    }
                    else if (lingua == "EN")
                    {
                        Console.WriteLine($"\nDownload completed {finishTime}, press Enter to go back to the main Menu");
                    }
                }

                Console.ReadLine();
                videoId = "";
                downloadFinished = false;
                charDecodedResponse = null;
                decodedResponse = "";
                urlDownloadChar = null;
                UrlDownloadStringDecoded = null;
                urlDownloadString = null;
                chosenQuality = "";
                lastPos = 0;
                GC.Collect();
                MenuPrint();
            }
        }

        static void DownloadCompletedAsync(object sender, AsyncCompletedEventArgs e)
        {
            downloadFinished = true;
            System.Diagnostics.Debug.WriteLine($"Download completed, downloadFinished = {downloadFinished}");
        }

        private static string GetVideoSize(Uri uriPath) //prende il peso del video
        {
            var webRequest = HttpWebRequest.Create(uriPath); 
            webRequest.Method = "HEAD";

            using (var webResponse = webRequest.GetResponse())
            {
                rawFileSize = Convert.ToDouble(webResponse.Headers.Get("Content-Length"));
                fileSize = Math.Round(rawFileSize / 1024.0 / 1024.0, 2);
                if (fileSize / 1024 >= 1)
                {
                    fileSize = Math.Round(rawFileSize / 1024.0 / 1024.0 / 1024.0, 2);
                    return (fileSize).ToString() + " GB";
                }
                fileSize = Math.Round(rawFileSize / 1024.0 / 1024.0, 2);
                return fileSize.ToString() + " MB";
            }
        }

        private static string GetEstimatedFinishTime(string FilePath) //stima il tempo per terminare il download del video
        {
            int difference;
            Thread.Sleep(1000);
            var currentSize1 = new System.IO.FileInfo(FilePath).Length;
            Thread.Sleep(1000);
            var currentSize2 = new System.IO.FileInfo(FilePath).Length;
            difference = Convert.ToInt32(currentSize2 - currentSize1);
            double EstimatedFinishTime;
            EstimatedFinishTime = (rawFileSize / difference);
            return Math.Round(EstimatedFinishTime, 0).ToString() + "s";
        }
    }
}

