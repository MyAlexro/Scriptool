using System;
using System.Threading;
using System.IO;
using System.Globalization; //per sapere la lingua di sistema



namespace Scriptool
{
    class MainClass : GeneraQRcode
    {
        static public string scriptoolPath = Path.GetDirectoryName(System.IO.Path.GetFullPath("Scriptool.exe")); //prende la path di dov'è Scriptool.exe (togliendo la parte Scriptool.exe)
        static public string lingua; //lingua di scriptool = ? ("IT","EN")
        static public string apriQRcode;  //aprire il codice QR dopo averlo creato?
        static public string QRcodeFormat; //salva il codice qr con il formato: ?
        static public string defaultPath; //salva il codice qr nella path: ?

        static public string[] options; //inizializza le opzioni (che l'utente dovrà selezionare)
        static public string titolo; //titolo da printare "Scriptool", "Settings" o "Impostazioni"

        [STAThread]
        static void Main(string[] args)
        {
            if (!System.IO.File.Exists($"{scriptoolPath}/Settings.txt"))   //se il txt delle impostazioni non esiste
            {
                Setup();
            }
            StartUp();
            MenuPrint();
        }



        //       ------------SETUP----------

        static void Setup()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            CultureInfo linguaSys = CultureInfo.InstalledUICulture;  //rileva la lingua di sistema
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
            Thread thr = new Thread(() => //l'ho messo in un thread per poter usare Thread.sleep
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
            Console.Title = "Scriptool"; //titolo finestra scriptool
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
            StrRe.Close(); //chiude la stream reader StrRe così da, eventualmente, poterci scrivere dopo
        }


        //     ---------MENU--------
        public static void MenuPrint()
        {
            string titolo = ("       _____           _       _              _      \n" +
                            "      /  ___|         (_)     | |            | |     \n" +
                            "      \\ `--.  ___ _ __ _ _ __ | |_ ___   ___ | |     \n" +
                            "       `--. \\/ __| '__| | '_ \\| __/ _ \\ / _ \\| |     \n" +
                            "      /\\__/ / (__| |  | | |_) | || (_) | (_) | |     \n" +
                            "      \\____/ \\___|_|  |_| .__/ \\__\\___/ \\___/|_|     \n" +
                            "                        | |                          \n" +
                            "                        |_|                           \t \n" +
                            "________________________________________________________________________________________________________________________\n" +
                            "\n\n\n\n\n");

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.CursorSize = 1;
            if (lingua == "IT")
            {
                options = new string[] { " 1) Genera un codice QR", " 2) Genera una password", " 3) Scarica un video da Youtube", " 4)", " 5)", " 6)", " 7) Impostazioni", " 8) Esci" };
                PrintOptMenu(options, titolo, "MenuPrint");
            }
            else if (lingua == "EN")
            {
                options = new string[] { " 1) Generate a QR code", " 2) Generate a password", " 3) Download a video from Youtube", " 4)", " 5)", " 6)", " 7) Settings", " 8) Exit" };
                PrintOptMenu(options, titolo, "MenuPrint");
            }
        }

        public static int PrintOptMenu(string[] opt, string staticText, string MethodId)  //opt per le opzioni da printare, staticText per scrivere un eventuale testo che non deve essere cancellato
        {                                                                                 // MethodIdentifier per indentificare da quale metodo viene chiamato
            Console.CursorVisible = false;  //nascone il cursor (barretta bianca) per evitare glitch grafici
            int currentOpt = 0; //
            do
            {
                Console.Clear();
                Console.WriteLine(staticText);
                for (int i = 0; i < opt.Length; i++)
                {
                    if (i == currentOpt)
                    {
                        Console.ForegroundColor = ConsoleColor.White;  //per l'opzione selezionata
                    }
                    Console.WriteLine(opt[i]);
                    Console.ForegroundColor = ConsoleColor.DarkGreen;  //per il resto delle impostazioni
                }
                switch (Console.ReadKey().Key.ToString())
                {
                    case "UpArrow":
                        if (currentOpt > 0) currentOpt--;  //per salire nel menu
                        else if (currentOpt == 0) currentOpt = opt.Length - 1;  //se si è alla prima opzione e si va in su si va all'ultima opzione
                        break;
                    case "DownArrow":
                        if (currentOpt < opt.Length - 1) currentOpt++;
                        else if (currentOpt == opt.Length - 1) currentOpt = 0;  //se si è all'ultima opzione e si va ancora in giù si va alla prima opzione
                        break;
                    case "Enter":

                        InitOpt(currentOpt, MethodId); //se si preme Enter chiama Init(ialize)Opt con l'opzione scelta(currentOpt) e il nome del metodo da cui viene chiamato (MethodId)
                        break;
                }
            }
            while (true);
        }

        //Inizializza l'opzione scelta
        static void InitOpt(int chosenOpt, string MethodId) //opzione scelta(currentOpt) | nome del metodo da cui viene chiamato (MethodId)
        {
            Console.CursorVisible = true;
            if (MethodId == "MenuPrint")  //se InitOpt viene chiamato da MenuPrint
            {
                //i numeri delle impostazioni sono sbalzati perchè iniziano da 1 nel menu ma da 0 nel codice
                switch (chosenOpt)
                {
                    case 0:
                        GeneraQRcode.ScegliQRcode(); //opt 1
                        break;
                    case 1:
                        GeneraPW();  //opt 2
                        break;
                    case 2:
                        Option3();  //opt 3
                        break;
                    case 3:
                        Option4();  //opt 4
                        break;
                    case 4:
                        Option5();  //opt 5
                        break;
                    case 5:
                        Option6();  //opt 6
                        break;
                    case 6:
                        Impostazioni.PrintImpostazioni();  //opt 7
                        break;
                    case 7:
                        Esci();  //opt 8
                        break;
                }


            }
            else if (MethodId == "GeneraQRcode") //se viene chiamato da GeneraQRcode
            {
                switch (chosenOpt)
                {
                    case 0:
                        GeneraQRcode.TestoLinkQRcode();  //opt 1
                        break;
                    case 1:
                        GeneraQRcode.WiFiQRcode();   //opt 2
                        break;
                    case 2:
                        GeneraQRcode.PhoneNumQRcode();  //opt 3
                        break;
                    case 3:
                        GeneraQRcode.Indietro(); //opt 4
                        break;
                }
            }
            else if (MethodId == "Impostazioni") // se invece viene chiamato da "Impostazioni"
            {
                switch (chosenOpt)
                {
                    case 0:
                        Impostazioni.SelLingua(); //imp 1
                        break;
                    case 1:
                        Impostazioni.ApriQRcodeDopoCreazione(); //imp 2
                        break;
                    case 2:
                        Impostazioni.SalvaFormatoQRcode(); //imp 3
                        break;
                    case 3:
                        Impostazioni.PathSalvaQRcode(); //imp 4
                        break;
                    case 4:
                        Impostazioni.Indietro(); //imp 5
                        break;
                }
            }
        }


        //               -----------FUNZIONI-----------

        //----Genera codice QR----



        //----Genera password----
        static void GeneraPW()
        {
            string characs = "ABCDEFGHIJKLMONPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz123456789èé&?!@"; //caratteri disponibili per generare la password
            Random r = new Random();
            if (lingua == "IT")
            {
                Console.Write("\n2: Scrivi la lunghezza della password(1-255): ");
            }
            else if (lingua == "EN")
            {
                Console.Write(" 2: Write the length of the password(1-255): ");
            }

            try //ho messo il try per prevenire il crash del programma se l'input è maggiore del limite e quindi causa un Overflow e se l'input non è numerico quindi causa un FormatException
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


        static void Esci()
        {
            Environment.Exit(0);
        }
    }
}

