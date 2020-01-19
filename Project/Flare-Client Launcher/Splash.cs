﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Flare_Client_Launcher
{
    public partial class Splash : Form
    {
        static byte brightness = 40;
        static Splash instance;
        bool retry = true;
        public static void updateStatus(string status)
        {
            instance.statusLabel.Text = status;
        }

        public Splash()
        {
            InitializeComponent();
            instance = this;
        }

        private void Splash_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.Timer brightTimer = new System.Windows.Forms.Timer();
            bool dec = false;
            brightTimer.Tick += (object sen, EventArgs a) =>
            {
                this.Refresh();
                if (brightness > 40)
                {
                    dec = true;
                }
                else if (brightness < 20)
                {
                    dec = false;
                }
                if (dec)
                {
                    brightness--;
                }
                else
                {
                    brightness++;
                }
                this.BackColor = Color.FromArgb(brightness, brightness, brightness);
            };
            brightTimer.Interval = 100;
            brightTimer.Start();
            while (retry)
            {
                loadFlare();
            }
        }
        public static void downloadHLFlare()
        {
            updateStatus("Downloading Flare...");
            string latest = BootMethods.getLatestVersion();
            if (!BootMethods.downloadFlare(latest))
            {
                updateStatus("Failed to download Flare!");
                return;
            }
            BootMethods.saveVersion(latest);
        }
        public static uint ver2uint(string ver)
        {
            string noExcess = ver.Replace("v", "").Replace(".","");
            return uint.Parse(noExcess);
        }

        public void loadFlare()
        {
            this.CenterToScreen();
            updateStatus("Loading...");
            for (byte b = 3; b > 0; b--)
            {
                this.Refresh();
                updateStatus("Launching in " + b);
                this.Refresh();
                Thread.Sleep(1000);
            }
            updateStatus("Checking for Minecraft...");
            if (!BootMethods.isMinecraftRunning())
            {
                MessageBox.Show("Please run Flare with Minecraft open!");
                return;
            }
            updateStatus("Checking for Flare...");
            if (BootMethods.isFlareFresh())
            {
                MessageBox.Show("First time? <3");
                BootMethods.flareFiles.Create();
            }
            if (!BootMethods.isFlareDownloaded())
            {
                if (BootMethods.getLatestSavedVersion() == "None")
                {
                    downloadHLFlare();
                }
                else
                {
                    if (ver2uint(BootMethods.getLatestVersion()) > ver2uint(BootMethods.getLatestSavedVersion()))
                    {
                        downloadHLFlare();
                    }
                }
            }
            if (!BootMethods.launchFlare())
            {
                MessageBox.Show("Flare failed to launch. Please try again later.");
            }
            retry = false;
            Application.Exit();
        }
    }
}