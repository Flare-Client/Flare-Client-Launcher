using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Flare_Client_Launcher
{
    public partial class Splash : Form
    {
        static Splash instance;

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void updateStatus(string status)
        {
            instance.statusLabel.Text = status;
            instance.Refresh();
        }

        public Splash()
        {
            InitializeComponent();
            instance = this;
        }

        private void Splash_Load(object sender, EventArgs e)
        {
            Timer brightTimer = new Timer();
            byte brightness = 40;
            bool dec = true;
            brightTimer.Tick+=(object s, EventArgs a)=>
            {
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
                this.Refresh();
            };
            brightTimer.Interval = 100;
            brightTimer.Start();

            loadFlare();
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

        public void loadFlare()
        {
            this.CenterToScreen();
            this.TopMost = true;
            updateStatus("Loading...");
            Timer countDown = new Timer();
            byte count = 4;
            countDown.Tick += (object s, EventArgs a) =>
            {
                count--;
                this.Refresh();
                updateStatus("Launching in " + count);
                this.Refresh();
                if (count == 0)
                {
                    count = 4;
                    //MessageBox.Show(BootMethods.getLatestVersion());
                    updateStatus("Checking for Minecraft...");
                    if (!BootMethods.isMinecraftRunning())
                    {
                        updateStatus("Launching Minecraft...");
                        BootMethods.launchMinecraft();
                        return;
                    }
                    updateStatus("Checking for Flare...");
                    if (BootMethods.isFlareFresh())
                    {
                        updateStatus("First time? <3");
                        BootMethods.flareFiles.Create();
                    }
                    if (BootMethods.getLatestSavedVersion() == "None")
                    {
                        downloadHLFlare();
                    }
                    else
                    {
                        if (BootMethods.getLatestVersion() != BootMethods.getLatestSavedVersion())
                        {
                            downloadHLFlare();
                        }
                    }
                    if (!BootMethods.launchFlare())
                    {
                        MessageBox.Show("Flare failed to launch. Please try again later.");
                    }
                    SetForegroundWindow(BootMethods.flare.MainWindowHandle);
                    Application.Exit();
                    countDown.Stop();
                }
            };
            countDown.Interval = 1000;
            countDown.Start();
        }
    }
}
