using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Flare_Client_Launcher
{
    public partial class Splash : Form
    {
        static byte brightness = 40;
        static Splash instance;
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
            this.CenterToScreen();
            updateStatus("Loading...");
            Timer brightTimer = new Timer();
            bool dec = false;
            brightTimer.Tick += (object sen, EventArgs a) =>
            {
                if (brightness > 40)
                {
                    dec = true;
                }else if (brightness < 20)
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
            updateStatus("Checking for Flare...");
            updateStatus("Latest version: " + BootMethods.getLatestVersion());
        }
    }
}
