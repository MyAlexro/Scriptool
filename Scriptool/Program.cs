using System;
using System.Text;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using System.Globalization; //per sapere la lingua di sistema
using System.Collections.Generic;
using System.Linq;



namespace Scriptool
{
    class Program : GeneraQRcode
    {

        static public string scriptoolPath = Path.GetDirectoryName(System.IO.Path.GetFullPath("Scriptool.exe")); //prende la path di dov'è Scriptool.exe (togliendo la parte Scriptool.exe)
        static public string lingua; //lingua di scriptool = ? ("IT","EN")
        static public string apriQRcode;  //aprire il codice QR dopo averlo creato?
        static public string QRcodeFormat; //salva il codice qr con il formato: ?
        static public string defaultPath; //salva il codice qr nella path: ?

        [STAThread]
        static void Main(string[] args)
        {
            if (!System.IO.File.Exists($"{scriptoolPath}/Settings.txt"))   //se il txt delle impostazioni non esiste
            {
                Setup();
            }
            StartUp();
            MenuPrint();
            ChooseOpt();
        }



        //       ------------SETUP----------

        static void Setup()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            CultureInfo linguaSys = CultureInfo.InstalledUICulture;
            string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);  //prende la directory del desktop
            Console.WriteLine("Setup");
            using (StreamWriter StrWr = File.CreateText($"{scriptoolPath}/Settings.txt")) //inizializza un stream writer
            {
                //impostazioni predefinite
                if (linguaSys.ToString().Contains("IT")) StrWr.WriteLine("IT"); //LINGUA
                else if (linguaSys.ToString().Contains("EN")) StrWr.WriteLine("EN"); //LINGUA
                StrWr.WriteLine("Y");  //APRI QR CODE
                StrWr.WriteLine("JPEG");  //SALVA QR CON CON L'ESTEINSIONE
                StrWr.WriteLine(DesktopPath); //SALVA IL CODICE QR NELLA PATH
                StrWr.Close();
            }
            Thread thr = new Thread(() =>
            {
                if (System.IO.File.Exists($"{scriptoolPath}/Settings.txt"))
                {
                    if (linguaSys.ToString().Contains("IT")) Console.WriteLine("File Settings.txt creato con successo");
                    else if (linguaSys.ToString().Contains("EN")) Console.WriteLine("File Settings.txt created successfully");
                }

            });
            thr.Start();
            Thread.Sleep(500);
            thr.Abort();
        }


        //      ----------STARTUP-----------

        static void StartUp()
        {
            Console.Clear();
            Console.Title = "Scriptool";
            Console.SetWindowSize(120, 30); //imposta grandezza ottimale della finestra
            Console.SetBufferSize(120, 9000); //imposta grandezza ottimale del buffer della finestra

            string[] line = new string[4]; // !-------PER AGGIUNGERE IMPOSTAZIONI AUMENTARE IL NUMERO DELL'ARRAY--------!
            int counter = 0;
            System.IO.StreamReader StrRe = new System.IO.StreamReader($"{scriptoolPath}/Settings.txt");  //inizializza uno StreamReader
            foreach (string lineNumber in File.ReadLines($"{scriptoolPath}/Settings.txt")) //per ogni linea nel file Settings.txt
            {
                line[counter] = StrRe.ReadLine(); //La linea[1] è uguale a *linea 1 nel file txt*, la linea[2] è uguale a *linea 2 nel file txt*, la linea[3] è uguale a *linea 3 nel file txt*
                counter++;

                // PRENDE I VALORI DAL FILE TXT ALLE VARIABILI(string lingua, string apriQRcode, string QRcodeFormat, string defaultPath)                 
                //LINGUA
                if (line[0] == "IT")
                {
                    lingua = "IT";
                }
                else if (line[0] == "EN")
                {
                    lingua = "EN";
                }
                //APRI QR CODE DOPO L'APERTURA
                if (line[1] == "Y")
                {
                    apriQRcode = "Y";
                }
                else
                {
                    apriQRcode = "N";
                }
                //FORMATO DELL'IMMAGINE DEL QR CODE DA SALVARE
                if (line[2] == "JPEG")
                {
                    QRcodeFormat = "JPEG";
                }
                else
                {
                    QRcodeFormat = "PNG";
                }
                //PATH DOVE SALVARE IL CODICE QR
                if (line[3] != null)
                {
                    defaultPath = line[3];
                }

            }
            StrRe.Close();
        }


        //     ---------MENU--------
        public static void MenuPrint()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("       _____           _       _              _      \n" +
                              "      /  ___|         (_)     | |            | |     \n" +
                             "      \\ `--.  ___ _ __ _ _ __ | |_ ___   ___ | |     \n" +
                              "       `--. \\/ __| '__| | '_ \\| __/ _ \\ / _ \\| |     \n" +
                               "      /\\__/ / (__| |  | | |_) | || (_) | (_) | |     \n" +
                            "      \\____/ \\___|_|  |_| .__/ \\__\\___/ \\___/|_|     \n" +
                               "                        | |                          \n" +
                               "                        |_|                           \t \n" +
                              "________________________________________________________________________________________________________________________\n" +
                              "\n\n\n\n\n");

            Console.CursorSize = 1;
            if (lingua == "IT")
            {
                Console.WriteLine(" 1) Genera un codice QR \t \n" +
                  " 2) Genera una password \t \n" +
                  " 3) \t \n" +
                  " 4) \t \n" +
                  " 5) \t \n" +
                  " 6) \t \n" +
                  " 7) Impostazioni \t \n \n" +
                  " 8) Esci \t \n");
            }
            else if (lingua == "EN")
            {
                Console.WriteLine(" 1) Generate a QR code\t \n" +
                  " 2) Generate a password \t \n" +
                  " 3) \t \n" +
                  " 4) \t \n" +
                  " 5) \t \n" +
                  " 6) \t \n" +
                  " 7) Settings \t \n \n" +
                  " 8) Exit \t \n");
            }
            ChooseOpt();
        }


        //        -----------SCEGLI OPZIONI-----------

        static void ChooseOpt()
        {
            string opz = Console.ReadKey().Key.ToString();

            if (opz == "D1" || opz == "NumPad1") //sono i Keycode dei tasti. Per vederli basta scrivere Keys. e vedere i suggeriti
            {
                GeneraQRcode.ScegliQRcode();
            }
            else if (opz == "D2" || opz == "NumPad2")
            {
                GeneraPW();
            }
            else if (opz == "D3" || opz == "NumPad3")
            {
                Option3();
            }
            else if (opz == "D4" || opz == "NumPad4")
            {
                Option4();
            }
            else if (opz == "D5" || opz == "NumPad5")
            {
                Option5();
            }
            else if (opz == "D6" || opz == "NumPad6")
            {
                Option6();
            }
            else if (opz == "D7" || opz == "NumPad7")
            {
                Impostazioni();
            }
            else if (opz == "D8" || opz == "NumPad8")
            {
                Esci();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                if (lingua == "IT")
                {
                    Console.WriteLine("Funzione invalida, premere Invio");
                }
                else if (lingua == "EN")
                {
                    Console.WriteLine("Invalid function, press Enter");
                }
                Console.ReadKey();
                MenuPrint(); //chiamo solo MenuPrint() perchè in essa viene già chiamata ChooseOpt
            }
        }







        //               -----------FUNZIONI-----------

        //----Genera codice QR (sopra)----



        //----Genera password----
        static void GeneraPW()
        {
            string characs = "ABCDEFGHIJKLMONPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz123456789èé&?!@"; //caratteri disponibili per generare la password
            Random r = new Random();
            if (lingua == "IT")
            {
                Console.Write(": Scrivi la lunghezza della password(1-255): ");
            }
            else if (lingua == "EN")
            {
                Console.Write(": Write the length of the password(1-255): ");
            }

            try //ho messo il try per prevenire il crash del programma se l'input è maggiore del limite e quindi causa un Overflow; e se l'input non è numerico quindi causa un FormatException
            {
                int lunghezza = Convert.ToByte(Console.ReadLine()); //converte l'input in byte

                if (lunghezza == 0)  //se la lunghezza è 0
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    if (lingua == "IT")
                    {
                        Console.Write("Lunghezza non valida, premere Invio per ritornare al Menu principale");
                    }
                    else if (lingua == "EN")
                    {
                        Console.Write("Unvalid length, press Enter to go back to the main Menu");
                    }
                    Console.ReadLine();
                    Console.Clear();
                    MenuPrint();
                }
                else  //se no prosegue a generare la PW
                {
                    char[] password = new char[lunghezza];
                    for (int i = 0; i < lunghezza; i++)
                    {
                        password[i] = characs[r.Next(characs.Length)]; //la posizione i di password è uguale a un carattere casuale di characs
                    }
                    if (lingua == "IT")
                    {
                        Console.Write("\nPassword generata: ");
                        Console.WriteLine(password); //va scritto in un writeline diverso se no non funziona, idk
                        Console.WriteLine("\nPremi Invio per tornare al Menu principale");
                    }
                    else if (lingua == "EN")
                    {
                        Console.Write("\nGenerated password: ");
                        Console.WriteLine(password);
                        Console.WriteLine("\nPress Enter to go back to the main Menu");
                    }
                    Console.ReadKey();
                    MenuPrint();
                }

            }
            catch (OverflowException) //l'input è maggiore del limite e quindi causa un Overflow
            {
                Console.ForegroundColor = ConsoleColor.Red;
                if (lingua == "IT")
                {
                    Console.Write("Lunghezza non valida, premere Invio per ritornare al Menu principale");
                }
                else if (lingua == "EN")
                {
                    Console.Write("Unvalid length, press Enter to go back to the main Menu");
                }
                Console.ReadLine();
                Console.Clear();
                MenuPrint();  //richiama la funzione MenuPrint()
            }
            catch (FormatException) //l'input non è numerico quindi causa un FormatException
            {
                Console.ForegroundColor = ConsoleColor.Red;
                if (lingua == "IT")
                {
                    Console.Write("Input non valido, premere Invio per ritornare al Menu principale");
                }
                else if (lingua == "EN")
                {
                    Console.Write("Unvalid input format, press Enter to go back to the main Menu");
                }
                Console.ReadLine();
                Console.Clear();
                MenuPrint();  //richiama la funzione MenuPrint()
            }
        }

        //_____________________________________________________________________________________________________________________________________________________________//
        //OPTION 3
        static void Option3()
        {
            Console.WriteLine(": ");
        }

        //_____________________________________________________________________________________________________________________________________________________________//
        //OPTION 4
        static void Option4()
        {
            Console.WriteLine(": ");
        }

        //_____________________________________________________________________________________________________________________________________________________________//
        //OPTION 5
        static void Option5()
        {
            Console.WriteLine(": ");
        }

        //_____________________________________________________________________________________________________________________________________________________________//
        //OPTION 6
        static void Option6()
        {
            Console.WriteLine(": ");
        }


        static void Impostazioni()  //Reminder: Per aggiungere/togliere un'impostazione bisogna modificare anche l'array string line nella funzione StartUp
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            if (lingua == "IT")
            {
                Console.WriteLine(@"  _____                           _            _             _ ");
                Console.WriteLine(@" |_   _|                         | |          (_)           (_)");
                Console.WriteLine(@"   | |  _ __ ___  _ __   ___  ___| |_ __ _ _____  ___  _ __  _ ");
                Console.WriteLine(@"   | | | '_ ` _ \| '_ \ / _ \/ __| __/ _` |_  / |/ _ \| '_ \| |");
                Console.WriteLine(@"  _| |_| | | | | | |_) | (_) \__ \ || (_| |/ /| | (_) | | | | |");
                Console.WriteLine(@" |_____|_| |_| |_| .__/ \___/|___/\__\__,_/___|_|\___/|_| |_|_|");
                Console.WriteLine(@"                 | |                                          ");
                Console.WriteLine("                 |_|                                           \t \n" +
                                   "                                                   \n" +
                                   "                                                   \n" +
                                   "                                                   \n" +
                                   "                                                   \n" +
                                   "                                                   \n");
                Console.WriteLine($"Scrivi il numero dell'impostazione che vuoi modificare:\n");
                Console.WriteLine($" 1) Lingua: {lingua}");
                Console.WriteLine($" 2) Apri codice QR dopo la creazione: {apriQRcode}");
                Console.WriteLine($" 3) Salva QR code con formato: {QRcodeFormat}");
                Console.WriteLine($" 4) Salva i codici QR nella cartella: {defaultPath}\n");
                Console.WriteLine($" 5)Torna indietro\n\n");
            }
            else if (lingua == "EN")
            {
                Console.WriteLine(@"   _____      _   _   _                 ");
                Console.WriteLine(@"  / ____|    | | | | (_)                ");
                Console.WriteLine(@" | (___   ___| |_| |_ _ _ __   __ _ ___ ");
                Console.WriteLine(@"  \___ \ / _ \ __| __| | '_ \ / _` / __|");
                Console.WriteLine(@"  ____) |  __/ |_| |_| | | | | (_| \__ \");
                Console.WriteLine(@" |_____/ \___|\__|\__|_|_| |_|\__, |___/");
                Console.WriteLine(@"                               __/ |    ");
                Console.WriteLine("                              |___/             \t \n" +
                                   "                                                   \n" +
                                   "                                                   \n" +
                                   "                                                   \n" +
                                   "                                                   \n" +
                                   "                                                   \n");
                Console.WriteLine($"Type the number of the setting you want to edit:\n");
                Console.WriteLine($" 1) Language: {lingua}");
                Console.WriteLine($" 2) Open the QR code after it has been created: {apriQRcode}");
                Console.WriteLine($" 3) Save the QR code with the extension: {QRcodeFormat}");
                Console.WriteLine($" 4) Save QR codes to path: {defaultPath}\n");
                Console.WriteLine($" 5)Go back\n\n");
            }

            //LINGUA?
            string opz = Console.ReadKey().Key.ToString();
            if (opz == "D1" || opz == "NumPad1")
            {
                if (lingua == "IT")
                {
                    Console.Write(": Scrivi il nome della lingua che vuoi impostare(IT/EN): ");
                }
                else if (lingua == "EN")
                {
                    Console.Write(": Type the name of the language that you want to set(IT/EN): ");
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
                    Impostazioni();
                }
            }
            //APRIRE I QR CODE DOPO LA CREAZIONE?
            else if (opz == "D2" || opz == "NumPad2")
            {
                if (lingua == "IT")
                {
                    Console.Write(": Aprire i QR code dopo la loro creazione?(Y/N): ");
                }
                else if (lingua == "EN")
                {
                    Console.Write(": Do you want to open the QR codes after they have been created?(Y/N): ");
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
                    Impostazioni();
                }
            }
            //CON QUALE FORMATO VUOI SALVARE I CODICI QR?
            else if (opz == "D3" || opz == "NumPad3")
            {
                if (lingua == "IT")
                {
                    Console.Write(": Con quale formato vuoi salvare i codici QR?(JPEG/PNG): ");
                }
                else if (lingua == "EN")
                {
                    Console.Write(": What extention do you want to save the QR codes with?(JPEG/PNG): ");
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
                    Impostazioni();
                }
            }
            //DOVE VUOI SALVARE I CODICI QR?
            else if (opz == "D4" || opz == "NumPad4")
            {
                Application.Run(new Form1()); //apre il Form1
                if (Form1.ActiveForm == null) //se il form non è aperto(quindi se è stato chiuso)
                {
                    if (lingua == "IT")
                    {
                        Console.WriteLine(".\nCartella aggiornata, premere Invio per tornare al Menu principale");
                    }
                    else if (lingua == "EN")
                    {
                        Console.WriteLine(".\nPath updated, press Enter to go back to the main Menu");
                    }
                    SalvaImpostazioni();
                    Console.ReadKey();
                    MenuPrint();
                }
            }
            //TORNA INDIETRO
            else if (opz == "D5" || opz == "NumPad5")
            {
                Console.Clear();
                MenuPrint();
            }
        }

        static void SalvaImpostazioni()  //salva le impostazioni, l'ho messo in una funzione a parte così da non dover scrivere ogni volta queste 2 righe
        {
            string[] impostazioniText = { lingua, apriQRcode, QRcodeFormat, defaultPath };
            System.IO.File.WriteAllLines($"{scriptoolPath}/Settings.txt", impostazioniText, Encoding.UTF8);
        }

        static void Esci()
        {
            Environment.Exit(0);
        }
    }
}

