using System;
using System.Threading;
using System.Net;
using System.ComponentModel;
using static Scriptool.MainClass; //per accedere alle impostazioni

namespace Scriptool
{
    class DownloadYTVideo
    {
        static WebClient clientWeb;
        static int DownloadUrlStart = 0;
        static int DownloadUrlEnd = 0;
        static int StartPoint = 0;
        static string videoTitle;
        static char[] videoIdChar = new char[11];  //crea l'array dove conservare l'id del video
        static string videoId; //la stringa dell'id del video
        static char[] charDecodedResponse;
        static string decodedResponse; //la risposta dalla web request alla page del video
        static char[] urlDownloadChar; //url di download del video in un array di char
        static string UrlDownloadStringDecoded; //la stringa decodata dell'url di download del video
        static char[] videoTitleChar;
        static string encodedVideoTitle;
        static string urlDownloadString;
        static int counter = 0;
        static bool downloadFinished = false;


        public static void Start_GetMetadata()
        {
            Console.CursorVisible = true;
            if (lingua == "IT")
            {
                Console.Write("\n3: Inserire Url del video che si vuole scaricare: ");
            }
            else if (lingua == "EN")
            {
                Console.Write("\n3: Insert the link of the video you want to download: ");
            }
            char[] url = Console.ReadLine().ToCharArray();

            string inputUrl = new string(url);
            if (!inputUrl.Contains("https://www.youtube.")) //se l'url è invalido
            {
                Console.ForegroundColor = ConsoleColor.Red;
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
                counter = 2;
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
                    GetTitle();
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
                for (int i = titleStart; i <= charDecodedResponse.Length; i++)
                {
                    if (charDecodedResponse[i] == '&')
                    {
                        titleEnd = i - 1;
                        break;
                    }
                }
                int titleLength = (titleEnd - titleStart) + 1;
                videoTitleChar = new char[titleLength];
                counter = 0;
                for (int i = titleStart; i <= titleEnd; i++)
                {
                    videoTitleChar[counter] = charDecodedResponse[i];
                    counter++;
                }
                encodedVideoTitle = new string(videoTitleChar);
                videoTitle = WebUtility.UrlDecode(encodedVideoTitle);
                if (lingua == "IT")
                {
                    Console.WriteLine($"Titolo del video: {videoTitle}");
                }
                else if (lingua == "EN")
                {
                    Console.WriteLine($"Video title: {videoTitle}");
                }
                GetDownloadUrl();
            }

        }



        static void GetDownloadUrl()
        {

            if (decodedResponse.Contains("&quality=hd720") || decodedResponse.Contains("quality=hd720")) //se il video ha la qualità HD 720p
            {
                if (lingua == "IT")
                {
                    //  Console.WriteLine($"Il video può essere scaricato in HD 720p");
                }
                else if (lingua == "EN")
                {
                    // Console.WriteLine("The video can be download in HD 720p");
                }
                for (int i = 0; i <= charDecodedResponse.Length - 1; i++)
                {
                    if (charDecodedResponse[i] == 'y' && charDecodedResponse[i + 1] == '=' && charDecodedResponse[i + 2] == 'h' && charDecodedResponse[i + 3] == 'd')
                    {
                        StartPoint = i; //start point da dove iniziare a cercare l'url di download del video
                        break;
                    }
                }
                if (StartPoint != 0)
                {
                    for (int a = StartPoint; a <= charDecodedResponse.Length - 1; a++) //cerca l'inizio dell'url di download del video
                    {
                        if (charDecodedResponse[a] == 'u' && charDecodedResponse[a + 1] == 'r' && charDecodedResponse[a + 2] == 'l' && charDecodedResponse[a + 3] == '=')
                        {
                            DownloadUrlStart = a + 4;
                            break;
                        }
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
                    DownloadVideo();
                }
            }
            else
            {
                if (lingua == "IT")
                {
                    Console.WriteLine($"Il video non può essere scaricato in Hd 720p, premere Invio per tornare al Menu principale");
                }
                else if (lingua == "EN")
                {
                    Console.WriteLine("The video can't be download in HD 720p, press Enter to go back to the main Menu");
                }
                Console.ReadLine();
                MenuPrint();
            }

        }
         


        static void DownloadVideo()
        {
            UrlDownloadStringDecoded = Uri.UnescapeDataString(urlDownloadString); //decoda la stringa 5 volte
            UrlDownloadStringDecoded = Uri.UnescapeDataString(urlDownloadString);
            UrlDownloadStringDecoded = Uri.UnescapeDataString(urlDownloadString);
            UrlDownloadStringDecoded = Uri.UnescapeDataString(urlDownloadString);
            UrlDownloadStringDecoded = Uri.UnescapeDataString(urlDownloadString);
            Uri UriDownloadVideo = new Uri(UrlDownloadStringDecoded); //converte la stringa in Uri
            clientWeb.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompletedAsync); //assegna il metodo DownloadCompleted all'evento DownloadFileCompleted
            try
            {
                clientWeb.DownloadFileAsync(UriDownloadVideo, $"{defaultVideoPath}/{videoTitle}.mp4"); //comincia a scaricare il file
                DateTime startTimeDate = DateTime.Parse(DateTime.Now.ToString()); //prende la data e l'ora locali e li analizza per individuare il formato dell'ora
                string startTime = startTimeDate.ToString("HH:mm:ss"); //prende l'ora, i minuti e i secondi dal DateTime completo
                if (lingua == "IT")
                {
                    Console.WriteLine($"Download del video iniziato {startTime}, verrà salvato nella cartella predefinita. Non chiudere il programma!");
                    Console.Write("Download in corso...");
                }
                else if (lingua == "EN")
                {
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
                    Console.WriteLine("Errore nel scaricare il video, premere Invio per tornare al Menu principale");
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
                clientWeb = null;
                charDecodedResponse = null;
                GC.Collect();
                Console.ReadKey();
                MenuPrint();
            }
        }


        static void DownloadCompletedAsync(object sender, AsyncCompletedEventArgs e)
        {
                downloadFinished = true;
                System.Diagnostics.Debug.WriteLine($"Download completed, downloadFinished = {downloadFinished}");
        }
    }
}

