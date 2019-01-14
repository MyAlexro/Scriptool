using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static Scriptool.Program;
using QRCoder;
using static QRCoder.PayloadGenerator;
using System.Drawing;

namespace Scriptool
{
    class GeneraQRcode
    {

        public static string WiFiPW; //Password della rete WiFi (l'ho messa public così da poterla svuotare dopo aver generato il codice qr)


        public static void ScegliQRcode()
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator(); //inizializza il generatore di codici QR
            if (lingua == "IT")
            {
                Console.WriteLine(": Seleziona il tipo di codice QR da generare:\n\n" +
                                   " 1) Testo/Link\n" +
                                   " 2) Accesso automatico ad una rete WiFi\n" +
                                   " 3) Chiama un numero di telefono\n");
            }
            else if (lingua == "EN")
            {
                Console.WriteLine(": Select the type of QR code you want to generate:\n" +
                                   " 1) Text/Link\n" +
                                   " 2) Autoconnect to a WiFi\n" +
                                   " 3) Dial a phone number\n");
            }
            Console.Write("  ");
            string opz = Console.ReadKey().Key.ToString();
            if (opz == "1" || opz == "NumPad1") //QR CODE TESTO/LINK
            {
                if (lingua == "IT")
                {
                    Console.Write(") Inserisci il testo/link da convertire: ");
                }
                else if (lingua == "EN")
                {
                    Console.Write(") Write the text/link you want to convert: ");
                }
                string testo_Link = Console.ReadLine();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(testo_Link, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20);
                SalvaQRcode(qrCodeImage,testo_Link);
            }
            else if (opz == "2" || opz == "NumPad2") //QR CODE WIFI
            {
                string WiFiSSID;
                WiFi WiFiGen;
                string payload;
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
                        WiFiGen = new WiFi(WiFiSSID, WiFiPW, WiFi.Authentication.WPA); //include WPA2 e WPA
                        payload = WiFiGen.ToString();
                        QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
                        QRCode qrCode = new QRCode(qrCodeData);
                        Bitmap qrCodeImage = qrCode.GetGraphic(20);
                        SalvaQRcode(qrCodeImage, WiFiSSID);
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
                        WiFiGen = new WiFi(WiFiSSID, WiFiPW, WiFi.Authentication.WPA); //include WPA2 e WPA
                        payload = WiFiGen.ToString();
                        QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
                        QRCode qrCode = new QRCode(qrCodeData);
                        Bitmap qrCodeImage = qrCode.GetGraphic(20);
                        SalvaQRcode(qrCodeImage, WiFiSSID);
                    }
                }
            }
            else if (opz == "3" || opz == "NumPad3")  //NUMERO DI TELEFONO
            {
                Console.Write(") Scrivi il numero di telefono che verrà chiamato: ");
                PhoneNumber numTel = new PhoneNumber(Console.ReadLine());
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


        public static void SalvaQRcode(Bitmap qrCodeImage, string Contenuto)
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
                    qrCodeImage.Save($"{defaultPath}/{Contenuto}.jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                else if (QRcodeFormat == "PNG") //se invece è uguale a PNG
                {
                    qrCodeImage.Save($"{defaultPath}/{Contenuto}.png", System.Drawing.Imaging.ImageFormat.Png);
                }
                if (apriQRcode == "Y")  //se la preferenza/impostazione apriQRcode è uguale a Y apre il qr e scrive 
                {
                    System.Diagnostics.Process.Start($"{defaultPath}/{Contenuto}.jpeg");
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
                WiFiPW = "";  //svuota la password
                Console.ReadLine();
                MenuPrint();
            }
        }
    }
}
