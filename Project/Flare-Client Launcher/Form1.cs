using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Flare_Client_Launcher
{
    public partial class Form1 : Form
    {
        WebClient Client = new WebClient();
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Client.DownloadFileCompleted += new AsyncCompletedEventHandler(FileDownloadComplete);
            Uri DownloadsURL = new Uri("https://github.com/Flare-Client/Flare-Client/releases/latest/download/Flare_Client.exe");
            Client.DownloadFileAsync(DownloadsURL, "Flare_Client.exe");
            Client.DownloadProgressChanged += (s, o) =>
            {
                progressBar.Value = o.ProgressPercentage;
            };
        }
        private void FileDownloadComplete(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Finished Downloading Latest Release...");
            string currDir = Directory.GetCurrentDirectory();
            if(File.Exists(currDir + "/Flare_Client.exe"))
            {
                Process.Start(currDir + "/Flare_Client.exe");
            } else
            {
                MessageBox.Show("Unable to Launch Flare-Client, do you have a valid internet connection?");
            }
        }
    }
}
