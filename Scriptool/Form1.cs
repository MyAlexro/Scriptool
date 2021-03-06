﻿using System;
using System.Windows.Forms;
using Scriptool; //così da poter accedere a tutte le cose(public) di Program.cs

namespace Scriptool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            string path = folderBrowserDialog1.SelectedPath;
            if (path != "")
            {
                if (Impostazioni.QR_VideoPath == "QRpath")
                {
                    MainClass.defaultQrPath = path;
                }
                else if(Impostazioni.QR_VideoPath == "Videopath")
                {
                    MainClass.defaultVideoPath = path;
                }
            }
            this.Close();
        }
    }
}
