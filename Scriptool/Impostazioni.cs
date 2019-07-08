using System;
using System.Text;
using System.Windows.Forms;
using static Scriptool.MainClass;

namespace Scriptool
{
    class Impostazioni
    {

        static public string QR_VideoPath = "";
        public static void PrintImpostazioni()  //Reminder: Per aggiungere/togliere un'impostazione bisogna modificare anche l'array string "line" nella funzione StartUp
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            if (lingua == "IT")
            {
                options = new string[]
                { $" 1) Lingua: {lingua}", $" 2) Apri il codice QR dopo la creazione: {apriQRcode}" ,
                  $" 3) Salva i codici QR con il formato: {QRcodeFormat}",$" 4) Salva i codici QR nella cartella: {defaultQrPath}",$" 5) Salva i video nella cartella: {defaultVideoPath}\n\n",
                  $"<-Torna indietro\n\n"
                };
                titolo = "  _____                           _            _             _ \n" +
                         " |_   _|                         | |          (_)           (_) \n" +
                         "   | |  _ __ ___  _ __   ___  ___| |_ __ _ _____  ___  _ __  _  \n" +
                         "   | | | '_ ` _ \\| '_ \\ / _ \\/ __| __/ _` |_  / |/ _ \\| '_ \\| | \n" +
                         "  _| |_| | | | | | |_) | (_) \\__ \\ || (_| |/ /| | (_) | | | | | \n" +
                         " |_____|_| |_| |_| .__/ \\___/|___/\\__\\__,_/___|_|\\___/|_| |_|_| \n" +
                         "                 | |                                            \n" +
                         "                 |_|                                           \t \n" +
                         "                                                   \n" +
                         "                                                   \n" +
                         "                                                   \n" +
                         "                                                   \n" +
                         "Seleziona l'impostazione che vuoi modificare:\n";
            }
            else if (lingua == "EN")
            {
                options = new string[]
                { $" 1) Language: {lingua}", $" 2) Open the QR code after it has been created: {apriQRcode}",
                  $" 3) Save QR codes with the extension: {QRcodeFormat}",$" 4) Save QR codes to the path: {defaultQrPath}",$" 5) Save videos to the path: {defaultVideoPath}\n\n",
                  $"<-Go back\n\n"
                };
                titolo = "   _____      _   _   _                 \n" +
                               "  / ____|    | | | | (_)                \n" +
                               " | (___   ___| |_| |_ _ _ __   __ _ ___ \n" +
                               "  \\___ \\ / _ \\ __| __| | '_ \\ / _` / __|\n" +
                               "  ____) |  __/ |_| |_| | | | | (_| \\__ \\\n" +
                               " |_____/ \\___|\\__|\\__|_|_| |_|\\__, |___/\n" +
                               "                               __/ |    \n" +
                               "                              |___/             \t \n" +
                               "                                                   \n" +
                               "                                                   \n" +
                               "                                                   \n" +
                               "                                                   \n" +
                               "                                                   \n" +
                               $"Select the setting you want to edit:\n";
            }
            PrintOptMenu(options, titolo, "Impostazioni");
        }


        //-------LINGUA?????----------
        public static void SelLingua()
        {
            if (lingua == "IT")
            {
                Console.Write("1: Scrivi il nome della lingua che vuoi impostare(IT/EN): ");
            }
            else if (lingua == "EN")
            {
                Console.Write("1: Type the name of the language that you want to set(IT/EN): ");
            }

            string linguaBuff = Console.ReadLine();
            if (linguaBuff == "IT" || linguaBuff == "it" || linguaBuff == "EN" || linguaBuff == "en") //se l'input è valido
            {
                if (lingua == "IT")
                {
                    Console.Write("Lingua aggiornata, premere Invio per tornare al Menu principale");
                }
                else if (lingua == "EN")
                {
                    Console.Write("Language updated, press Enter to go back to the main Menu");
                }
                lingua = linguaBuff.ToUpper();
                SalvaImpostazioni();
                Console.ReadLine();
                MenuPrint();

            }
            else //se l'input non è valido
            {
                Console.ForegroundColor = ConsoleColor.Red;
                if (lingua == "IT")
                {
                    Console.WriteLine("Lingua non valida, premere Invio");
                }
                else if (lingua == "EN")
                {
                    Console.WriteLine("Invalid language, press Enter");
                }
                Console.ReadKey();
                PrintImpostazioni();
            }
        }

        //--------APRI QRCODE DOPO CREAZIONE?????------------
        public static void ApriQRcodeDopoCreazione()
        {
            if (lingua == "IT")
            {
                Console.Write("2: Aprire i QR code dopo la loro creazione?(Y/N): ");
            }
            else if (lingua == "EN")
            {
                Console.Write("2: Do you want to open the QR codes after they have been created?(Y/N): ");
            }

            string apriQRcodeBuff = Console.ReadLine();
            if (apriQRcodeBuff == "Y" || apriQRcodeBuff == "y" || apriQRcodeBuff == "N" || apriQRcodeBuff == "n") //se l'input è valido
            {
                if (lingua == "IT")
                {
                    Console.WriteLine("Preferenza aggiornata, premere Invio per tornare al Menu principale");
                }
                else if (lingua == "EN")
                {
                    Console.WriteLine("Setting updated, press Enter to go back to the main Menu");
                }
                apriQRcode = apriQRcodeBuff.ToUpper();
                SalvaImpostazioni();
                Console.ReadLine();
                MenuPrint();
            }
            else //se l'input non è valido
            {
                Console.ForegroundColor = ConsoleColor.Red;
                if (lingua == "IT")
                {
                    Console.WriteLine("Impostazione non valida, premere Invio");
                }
                else if (lingua == "EN")
                {
                    Console.WriteLine("Invalid setting, press Enter");
                }
                Console.ReadKey();
                Impostazioni.PrintImpostazioni();
            }
        }


        //------------SALVA QR CODE CON FORMATO??????--------------
        public static void SalvaFormatoQRcode()
        {
            if (lingua == "IT")
            {
                Console.Write("3: Con quale formato vuoi salvare i codici QR?(JPEG/PNG): ");
            }
            else if (lingua == "EN")
            {
                Console.Write("3: What extention do you want to save the QR codes with?(JPEG/PNG): ");
            }

            string QRcodeFormatBuff = Console.ReadLine();
            if (QRcodeFormatBuff == "JPEG" || QRcodeFormatBuff == "jpeg" || QRcodeFormatBuff == "PNG" || QRcodeFormatBuff == "png") //se l'input è valido
            {
                if (lingua == "IT")
                {
                    Console.WriteLine("Formato aggiornato, premere Invio per tornare al Menu principale");
                }
                else if (lingua == "EN")
                {
                    Console.WriteLine("Extension updated, press Enter to go back to the main Menu");
                }
                QRcodeFormat = QRcodeFormatBuff.ToUpper();
                SalvaImpostazioni();
                Console.ReadLine();
                MenuPrint();
            }
            else   //se l'input non è valido
            {
                Console.ForegroundColor = ConsoleColor.Red;
                if (lingua == "IT")
                {
                    Console.WriteLine("Formato non valido, premere Invio");
                }
                else if (lingua == "EN")
                {
                    Console.WriteLine("Invalid extension, press Enter");
                }
                Console.ReadKey();
                Impostazioni.PrintImpostazioni();
            }
        }


        //---------------PATH DOVE SALVARE I CODICI QR???????------------------
        public static void PathSalvaQRcode()
        {
            QR_VideoPath = "QRpath";
            Application.Run(new Form1()); //apre il Form1
            if (Form1.ActiveForm == null) //se il form non è aperto(quindi se è stato chiuso)
            {
                if (lingua == "IT")
                {
                    Console.WriteLine("4: Cartella aggiornata, premere Invio per tornare al Menu principale");
                }
                else if (lingua == "EN")
                {
                    Console.WriteLine("4: Path updated, press Enter to go back to the main Menu");
                }
                SalvaImpostazioni();
                Console.ReadKey();
                MenuPrint();
            }
        }

        //---------------PATH DOVE SALVARE I VIDEO???????------------------
        public static void PathSalvaVideo()
        {
            QR_VideoPath = "Videopath";
            Application.Run(new Form1()); //apre il Form1
            if (Form1.ActiveForm == null) //se il form non è aperto(quindi se è stato chiuso)
            {
                if (lingua == "IT")
                {
                    Console.WriteLine("5: Cartella aggiornata, premere Invio per tornare al Menu principale");
                }
                else if (lingua == "EN")
                {
                    Console.WriteLine("5: Path updated, press Enter to go back to the main Menu");
                }
                SalvaImpostazioni();
                Console.ReadKey();
                MenuPrint();
            }
        }


        //------------TORNA INDIETRO-----------
        public static void Indietro()
        {
            Console.Clear();
            MenuPrint();
        }


        //-----------SALVA IMPOSTAZIONI----------------                                                                                                     ___
        static void SalvaImpostazioni()  //salva le impostazioni, l'ho messo in una funzione a parte così da non dover scrivere ogni volta queste 2 righe  (°<°)meow
        {
            string[] impostazioniText = { lingua, apriQRcode, QRcodeFormat, defaultQrPath, defaultVideoPath };
            System.IO.File.WriteAllLines($"{scriptoolPath}/Settings.txt", impostazioniText, Encoding.UTF8);
        }
    }
}
