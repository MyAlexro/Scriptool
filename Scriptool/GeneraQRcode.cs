using System;
using System.IO;
using QRCoder;
using System.Drawing;
using System.Diagnostics;
using static QRCoder.PayloadGenerator;
using static Scriptool.MainClass;

namespace Scriptool
{
    class GeneraQRcode
    {
        //li ho messi qua così da poterli nullare
        static string bufferPrecedente;
        static string testo_Link;
        static string WiFiPW = null; //Password della rete WiFi
        static PhoneNumber numTel = null; //numero di telefono
        static string numTelString = null; //numero di telefono in string
        static QRCodeGenerator qrGenerator;
        static QRCodeData qrCodeData;
        static QRCode qrCode;
        static Bitmap qrCodeImage;

        static readonly char[] InvalidChars = Path.GetInvalidFileNameChars(); //lettere non valide con cui nominare un file
        static bool containsInvalid = false; //serve per settare se il nome del codice qr contiene caratteri non validi

        public static void ScegliQRcode()
        {
                bufferPrecedente = "       _____           _       _              _      \n" +
                "      /  ___|         (_)     | |            | |     \n" +
                "      \\ `--.  ___ _ __ _ _ __ | |_ ___   ___ | |     \n" +
                "       `--. \\/ __| '__| | '_ \\| __/ _ \\ / _ \\| |     \n" +
                "      /\\__/ / (__| |  | | |_) | || (_) | (_) | |     \n" +
                "      \\____/ \\___|_|  |_| .__/ \\__\\___/ \\___/|_|     \n" +
                "                        | |                          \n" +
                "                        |_|                           \t \n" +
                "________________________________________________________________________________________________________________________\n" +
                "\n\n\n\n\n";

            if (lingua == "IT") //ho fatto così se no la scritta scriptool e le opzioni del menu principale si sarebbero cancellati
            {
                bufferPrecedente += " 1) Genera un codice QR\n" +
                " 2) Genera una password\n" +
                " 3) Scarica un video da Youtube\n" +
                " 4)\n" +
                " 5)\n" +
                " 6)\n" +
                " 7) Impostazioni\n" +
                " 8) Esci\n" +
                "\n1: Seleziona il tipo di codice QR da generare:\n";
                options = new string[]
                {" 1) Testo/Link", " 2) Accesso automatico ad una rete WiFi", " 3) Chiama un numero di telefono" , "<-Ritorna indietro" };
                MainClass.PrintOptMenu(options, bufferPrecedente, "GeneraQRcode");
            }
            else if (lingua == "EN")
            {
                bufferPrecedente += " 1) Generate a QR code\n" +
                          " 2) Generate a password\n" +
                          " 3) Download a video from Youtube\n" +
                          " 4)\n" +
                          " 5)\n" +
                          " 6)\n" +
                          " 7) Settings\n" +
                          " 8) Exit\n" +
                          "\n1: Select the type of QR code you want to generate:\n";
                options = new string[]
                {" 1) Text/Link" , " 2) Autoconnect to a WiFi", " 3) Dial a phone number\n" , "<-Go back\n" //opzione 4
                };
                MainClass.PrintOptMenu(options, bufferPrecedente, "GeneraQRcode"); //la chiama per printare i vari tipi di codici qr
            }

        }

        //--------genera un codice qr che restituisce un link o del testo-----------
        public static void TestoLinkQRcode()
        {
            if (lingua == "IT")
            {
                Console.Write("  1: Inserisci il testo/link da convertire: ");
            }
            else if (lingua == "EN")
            {
                Console.Write("  1: Write the text/link you want to convert: ");
            }
            testo_Link = @Console.ReadLine();
            qrGenerator = new QRCodeGenerator();
            qrCodeData = qrGenerator.CreateQrCode(testo_Link, QRCodeGenerator.ECCLevel.Q);  //object contenente dati da convertire in codice QR
            qrCode = new QRCode(qrCodeData);  //converte i dati in un codice QR
            qrCodeImage = qrCode.GetGraphic(20);  //renderizza l'immagine del codice QR
            SalvaQRcode(qrCodeImage, testo_Link); //richiama la funzione SalvaQRcode passando i parametri necessari
            testo_Link = null;
            Console.WriteLine("testo:" + testo_Link);
        }

        //------------genera un codice qr che si autoconnette ad una rete wifi---------
        public static void WiFiQRcode()
        {
            string WiFiSSID; //nome della rete
            WiFi WiFiGen; // object contenente tutti i dati da convertire in codice QR
            if (lingua == "IT")
            {
                Console.Write("  2: Inserisci il nome della rete WiFi a cui connettersi: ");
                WiFiSSID = Console.ReadLine();
                Console.Write("     Inserisci la password della rete WiFi(non verrà conservata): ");
                WiFiPW = Console.ReadLine();
                if (WiFiSSID == "")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("     SSID non valido, premere invio per tornare al Menu principale");
                    Console.ReadKey();
                    MenuPrint();
                }
                else
                {
                    WiFiGen = new WiFi(WiFiSSID, WiFiPW, WiFi.Authentication.WPA); // !!!!!WiFi.Authentication.WPA include sia WPA che WPA2!!!!!
                    qrGenerator = new QRCodeGenerator();
                    qrCodeData = qrGenerator.CreateQrCode(WiFiGen, QRCodeGenerator.ECCLevel.Q); //object contenente tutti i dati da convertire 
                    qrCode = new QRCode(qrCodeData); //converte i dati
                    qrCodeImage = qrCode.GetGraphic(20);  //renderizza l'mmagine 
                    SalvaQRcode(qrCodeImage, WiFiSSID);   //richiama la funzione SalvaQRcode passando i parametri necessari (passa il WiFISSID cosi salva il codice QR con il nome della rete)
                }
            }
            else if (lingua == "EN")
            {
                Console.Write("  2: Write the name of the WiFi network to connect to: ");
                WiFiSSID = Console.ReadLine();
                Console.Write("     Write the password of the WiFi network(it won't be stored): ");
                WiFiPW = Console.ReadLine();
                if (WiFiSSID == "")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("     Unvalid SSID, press Enter to go back to the main Menu");
                    Console.ReadKey();
                    MenuPrint();
                }
                else
                {
                    WiFiGen = new WiFi(WiFiSSID, WiFiPW, WiFi.Authentication.WPA);
                    qrGenerator = new QRCodeGenerator();
                    qrCodeData = qrGenerator.CreateQrCode(WiFiGen, QRCodeGenerator.ECCLevel.Q);
                    qrCode = new QRCode(qrCodeData);
                    qrCodeImage = qrCode.GetGraphic(20);
                    SalvaQRcode(qrCodeImage, WiFiSSID);
                }
            }
        }

        //--------genera il codice qr che chiama un numero di tel--------
        public static void PhoneNumQRcode()
        {
            if (lingua == "IT")
            {
                Console.Write("  3: Scrivi il numero di telefono che verrà chiamato(non verrà conservato): +");
            }
            else if (lingua == "EN")
            {
                Console.Write("  3: Type the phone number that will be dialed(it won't be stored): +");
            }
            numTelString = Console.ReadLine();
            numTel = new PhoneNumber(numTelString); //inizializza l'object PhoneNumber dalla string numTelString
            qrGenerator = new QRCodeGenerator();
            qrCodeData = qrGenerator.CreateQrCode(numTel, QRCodeGenerator.ECCLevel.Q); //converte l'object in dati del qrcode
            qrCode = new QRCode(qrCodeData); //genera il codice qr
            qrCodeImage = qrCode.GetGraphic(20); //prende l'immagine 
            SalvaQRcode(qrCodeImage, numTelString); //ho messo numTelString come NomeQRcode
        }

        //-----torna indietro-------
        public static void Indietro()
        {
            MenuPrint();
        }



        //---------salva il codice qr-----------
        public static void SalvaQRcode(Bitmap qrCodeImage, string NomeQRcode)
        {
            if (!Directory.Exists(defaultQrPath)) //se non esiste la path dove salvare i codici
            {
                Console.ForegroundColor = ConsoleColor.Red;
                if (lingua == "IT")
                {
                    Console.WriteLine("Impossibile trovare la cartella dove salvare i codici QR. Premere Invio per tornare al Menu principale");
                }
                else if (lingua == "EN")
                {
                    Console.WriteLine("The folder to save QR codes cannot be found. Press Enter to go back to the main Menu");
                }
                Console.ReadKey();
                MenuPrint();
            }
            else if (Directory.Exists(defaultQrPath))  //se invece esiste
            {
                for (int i = 0; i <= InvalidChars.Length - 1; i++)
                {
                    if (NomeQRcode.Contains(InvalidChars[i].ToString())) //controlla se nel nome ci sono caratteri non validi
                    {
                        containsInvalid = true;
                        break;
     
                    }
                }
                if (QRcodeFormat == "JPEG") //se l'impostazione per il formato con cui formare il QR code è JPEG
                {
                    if (containsInvalid || NomeQRcode.Length >= 248)//se contiene caratteri non validi o il nome del codice QR è troppo lungo
                    {
                        qrCodeImage.Save($"{defaultQrPath}/InvalidName.jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    else
                    {
                        qrCodeImage.Save($"{defaultQrPath}/{NomeQRcode}.jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                }
                else if (QRcodeFormat == "PNG") //se invece l'impostazione è PNG
                {
                    if (containsInvalid)//se quindi contiene caratteri non validi
                    {
                        qrCodeImage.Save($"{defaultQrPath}/InvalidName.png", System.Drawing.Imaging.ImageFormat.Png);
                    }
                    else
                    {
                        qrCodeImage.Save($"{defaultQrPath}/{NomeQRcode}.png", System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
                if (lingua == "IT")
                {
                    Console.WriteLine("\nCodice QR generato e salvato nella cartella predefinita. Premere invio per tornare al Menu principale");
                }
                else if (lingua == "EN")
                {
                    Console.WriteLine("\nQR code generated and saved to the defined folder. Press Enter to go back to the main Menu");
                }
                if (apriQRcode == "Y")  //se l'impostazione apriQRcode è uguale a Y apre il codice QR
                {
                    Process.Start($"{defaultQrPath}/{NomeQRcode}.jpeg");
                }
                //li metto a null per far sì che il garbage collector svuoti la memoria cancellandoli
                WiFiPW = null;
                numTel = null;
                numTelString = null;
                testo_Link = null;
                qrCodeData = null;
                qrGenerator = null;
                qrCodeData = null;
                bufferPrecedente = null;
                GC.Collect();
                Console.ReadLine();
                MenuPrint();
            }
        }
    }
}