using System;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using static Scriptool.Program;
using QRCoder;
using static QRCoder.PayloadGenerator;
using System.Drawing;

namespace Scriptool
{
    class GeneraQRcode
    {

        public static string WiFiPW = null; //Password della rete WiFi (l'ho messa public così da poterla svuotare dopo aver generato il codice qr)
        public static PhoneNumber numTel = null; //numero di telefono (stesso motivo per sopra)
        public static string numTelString = null; //numero di telefono in string (stesso motivo sopra)


        public static void ScegliQRcode()
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator(); //inizializza il generatore di codici QR
            if (lingua == "IT")
            {
                Console.WriteLine(": Seleziona il tipo di codice QR da generare:\n\n" +   //scrivi i tipi di codici QR in italiano
                                  " 1) Testo/Link\n" +
                                  " 2) Accesso automatico ad una rete WiFi\n" +
                                  " 3) Chiama un numero di telefono\n" +
                                  "\n" +
                                  " 4)Ritorna al Menu principale\n");
            }
            else if (lingua == "EN")
            {
                Console.WriteLine(": Select the type of QR code you want to generate:\n" +  //scrivi i tipi di codici QR in inglese
                                   " 1) Text/Link\n" +
                                   " 2) Autoconnect to a WiFi\n" +
                                   " 3) Dial a phone number\n" +
                                   "\n" +
                                   " 4)Go back to the main Menu\n");
            }
            Console.Write("  ");  //spazi per la formattazione
            string opz = Console.ReadKey().Key.ToString();  //salva l'opzione (il tipo di codice QR) scelto

            if (opz == "D1" || opz == "NumPad1") //-------------QR CODE TESTO/LINK--------------
            {
                if (lingua == "IT")
                {
                    Console.Write(") Inserisci il testo/link da convertire: ");
                }
                else if (lingua == "EN")
                {
                    Console.Write(") Write the text/link you want to convert: ");
                }
                string testo_Link = Console.ReadLine();  //prende il testo/link da convertire inserito dall'utente
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(testo_Link, QRCodeGenerator.ECCLevel.Q);  //object contenente dati da convertire in codice QR
                QRCode qrCode = new QRCode(qrCodeData);  //converte i dati in un codice QR
                Bitmap qrCodeImage = qrCode.GetGraphic(20);  //renderizza l'immagine del codice QR
                SalvaQRcode(qrCodeImage, testo_Link); //richiama la funzione SalvaQRcode passando i parametri necessari
            }
            else if (opz == "D2" || opz == "NumPad2") //---------------QR CODE WIFI---------------
            {
                string WiFiSSID; //nome della rete
                WiFi WiFiGen; // object contenente tutti i dati da convertire in codice QR
                if (lingua == "IT")
                {
                    Console.Write(") Inserisci il nome della rete WiFi a cui connettersi: ");
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
                    Console.Write(") Write the name of the WiFi network to connect to: ");
                    WiFiSSID = Console.ReadLine();
                    Console.Write("     Write the password of the WiFi network(it won't be stored): ");
                    WiFiPW = Console.ReadLine();
                    if (WiFiSSID == "")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Unvalid SSID, press Enter to go back to the main Menu");
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
            else if (opz == "D3" || opz == "NumPad3")  //----------NUMERO DI TELEFONO-------------
            {
                if (lingua == "IT")
                {
                    Console.Write(") Scrivi il numero di telefono che verrà chiamato(non verrà conservato): +");
                    numTelString = Console.ReadLine();
                    numTel = new PhoneNumber(numTelString);
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(numTel, QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(qrCodeData);
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);
                    SalvaQRcode(qrCodeImage, numTelString);

                }
                else if (lingua == "EN")
                {
                    Console.Write(") Type the phone number that will be dialed(it won't be stored): +");
                    numTelString = Console.ReadLine();
                    numTel = new PhoneNumber(numTelString);
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(numTel, QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(qrCodeData);
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);
                    SalvaQRcode(qrCodeImage, numTelString);
                }
            }
            else if (opz == "D4" || opz == "NumPad4")
            {
                MenuPrint();
            }
            else //se l'user scrive un tipo di qr code non esistente
            {
                Console.ForegroundColor = ConsoleColor.Red;
                if (lingua == "IT")
                {
                    Console.WriteLine("Tipo di codice QR non valido, premere Invio per tornare al Menu principale");
                }
                else if (lingua == "EN")
                {
                    Console.WriteLine("Unvalid type of QR code, press Enter to go back to the main Menu");
                }
                Console.ReadKey();
                MenuPrint();
            }
        }


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
                }
            }
            WiFiPW = null;  //svuota la password
            numTel = null;  //svuota il numero di tel
            numTelString = null; //svuota la string numero di tel
            Console.ReadLine();
            MenuPrint();

        }
    }
}