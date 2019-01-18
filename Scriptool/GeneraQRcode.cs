using System;
using System.IO;
using QRCoder;
using System.Drawing;
using static QRCoder.PayloadGenerator;
using static Scriptool.MainClass;

namespace Scriptool
{
    class GeneraQRcode
    {

        public static string WiFiPW = null; //Password della rete WiFi (l'ho messa public così da poterla svuotare dopo aver generato il codice qr)
        public static PhoneNumber numTel = null; //numero di telefono (stesso motivo per sopra)
        public static string numTelString = null; //numero di telefono in string (stesso motivo sopra)
        public static QRCodeGenerator qrGenerator = new QRCodeGenerator(); //inizializza il generatore di codici QR

        public static void ScegliQRcode()
        {
            string titolo = "       _____           _       _              _      \n" +
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
                titolo += " 1) Genera un codice QR\n" +
                " 2) Genera una password\n" +
                " 3)\n" +
                " 4)\n" +
                " 5)\n" +
                " 6)\n" +
                " 7) Impostazioni\n" +
                " 8) Esci\n" +
                "\n1: Seleziona il tipo di codice QR da generare:\n";
                options = new string[]
                {" 1) Testo/Link", " 2) Accesso automatico ad una rete WiFi", " 3) Chiama un numero di telefono\n" , " <-Ritorna al Menu principale\n" //opzione 4
                };
                MainClass.PrintOptMenu(options, titolo, "GeneraQRcode");
            }
            else if (lingua == "EN")
            {
                titolo += " 1) Generate a QR code\n" +
                          " 2) Generate a password\n" +
                          " 3)\n" +
                          " 4)\n" +
                          " 5)\n" +
                          " 6)\n" +
                          " 7) Settings\n" +
                          " 8) Exit\n" +
                          "\n1: Select the type of QR code you want to generate:\n";
                options = new string[]
                {" 1) Text/Link" , " 2) Autoconnect to a WiFi", " 3) Dial a phone number\n" , " <-Go back to the main Menu\n" //opzione 4
                };
                MainClass.PrintOptMenu(options, titolo, "GeneraQRcode"); //la chiama per printare i vari tipi di codici qr
            }

        }
        
        //genera un codice qr che restituisce un link o del testo
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
            string testo_Link = Console.ReadLine();  //prende il testo/link da convertire inserito dall'utente
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(testo_Link, QRCodeGenerator.ECCLevel.Q);  //object contenente dati da convertire in codice QR
            QRCode qrCode = new QRCode(qrCodeData);  //converte i dati in un codice QR
            Bitmap qrCodeImage = qrCode.GetGraphic(20);  //renderizza l'immagine del codice QR
            SalvaQRcode(qrCodeImage, testo_Link); //richiama la funzione SalvaQRcode passando i parametri necessari
        }

        //genera un codice qr che si autoconnette ad una rete wifi
        public static void WiFiQRcode()
        {
            string WiFiSSID; //nome della rete
            WiFi WiFiGen; // object contenente tutti i dati da convertire in codice QR
            if (lingua == "IT")
            {
                Console.Write("  2: Inserisci il nome della rete WiFi a cui connettersi: ");
                WiFiSSID = Console.ReadLine();
                Console.Write("     Inserisci la password della rete WiFi(non verrà conservata):");
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
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(WiFiGen, QRCodeGenerator.ECCLevel.Q); //object contenente tutti i dati da convertire 
                    QRCode qrCode = new QRCode(qrCodeData); //converte i dati
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);  //renderizza l'mmagine 
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
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(WiFiGen, QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(qrCodeData);
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);
                    SalvaQRcode(qrCodeImage, WiFiSSID);
                }
            }
        }

        //genera il codice qr che chiama un numero di tel
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
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(numTel, QRCodeGenerator.ECCLevel.Q); //converte l'object in dati del qrcode
            QRCode qrCode = new QRCode(qrCodeData); //genera il codice qr
            Bitmap qrCodeImage = qrCode.GetGraphic(20); //prende l'immagine 
            SalvaQRcode(qrCodeImage, numTelString); //ho messo numTelString come NomeQRcode
        }

        //torna indietro
        public static void Indietro()
        {
            MenuPrint();
        }



        //salva il codice qr
        public static void SalvaQRcode(Bitmap qrCodeImage, string NomeQRcode)
        {
            if (!Directory.Exists(defaultPath)) //se la cartella dove salvare i codici QR non esiste(perchè è stata eliminata o rinominata) non genera il codice e da questo errore
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
            else if (Directory.Exists(defaultPath))  //se invece esiste
            {
                if (QRcodeFormat == "JPEG") //se l'impostazione per il formato con cui formare il QR code è JPEG
                {
                    qrCodeImage.Save($"{defaultPath}/{NomeQRcode}.jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                else if (QRcodeFormat == "PNG") //se invece è uguale a PNG
                {
                    qrCodeImage.Save($"{defaultPath}/{NomeQRcode}.png", System.Drawing.Imaging.ImageFormat.Png);
                }
                if (apriQRcode == "Y")  //se la preferenza/impostazione apriQRcode è uguale a Y apre il qr e scrive 
                {
                    System.Diagnostics.Process.Start($"{defaultPath}/{NomeQRcode}.jpeg");
                    if (lingua == "IT")
                    {
                        Console.WriteLine("Codice QR generato e salvato nella cartella predefinita. Premere invio per tornare al Menu principale");
                    }
                    else if (lingua == "EN")
                    {
                        Console.WriteLine("QR code generated and saved to the defined path. Press Enter to go back to the main Menu");
                    }
                }
                else  // scrive solo che è stato creato sul desktop
                {
                    if (lingua == "IT")
                    {
                        Console.WriteLine("Codice QR generato e salvato nella cartella predefinita. Premere invio per tornare al Menu principale");
                    }
                    else if (lingua == "EN")
                    {
                        Console.WriteLine("QR code created and saved to the defined path. Press Enter to go back to the main Menu");
                    }
                    WiFiPW = null;  //svuota la password
                    numTel = null;  //svuota il numero di tel
                    numTelString = null; //svuota la string numero di tel
                    Console.ReadLine();
                    MenuPrint();
                }
            }
        }
    }
}