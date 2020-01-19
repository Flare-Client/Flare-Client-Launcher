using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;

namespace Flare_Client_Launcher
{
    public class BootMethods
    {
        public static DirectoryInfo flareFiles = new DirectoryInfo(Environment.CurrentDirectory + "/FlareFiles");
        public static FileInfo flareVerFile = new FileInfo(flareFiles.FullName + "/downloadedVer.txt");
        public static FileInfo flareExe = new FileInfo(flareFiles.FullName + "/FlareFromLauncher.exe");

        public static bool launchFlare()
        {
            if (!flareExe.Exists)
            {
                return false;
            }
            Environment.CurrentDirectory = flareFiles.FullName;
            Process.Start(flareExe.FullName);
            return true;
        }
        public static bool isMinecraftRunning()
        {
            return Process.GetProcessesByName("Minecraft.Windows").Length>0;
        }
        public static bool isFlareFresh()
        {
            return !flareFiles.Exists;
        }
        public static bool isFlareDownloaded()
        {
            foreach(FileInfo fi in flareFiles.EnumerateFiles())
            {
                if (fi.Name.StartsWith("FlareFromLauncher"))
                {
                    return true;
                }
            }
            return false;
        }
        public static string getLatestVersion()
        {
            WebClient wc = new WebClient();
            wc.Headers.Add("user-agent", " Mozilla/5.0 (Windows NT 6.1; WOW64; rv:25.0) Gecko/20100101 Firefox/25.0");
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<RootObject> data = serializer.Deserialize<List<RootObject>>(wc.DownloadString("https://api.github.com/repos/Flare-Client/Flare-Client/releases"));
            return data[0].tag_name;
        }
        public static bool downloadFlare(string ver)
        {
            WebClient wc = new WebClient();
            wc.Headers.Add("user-agent", " Mozilla/5.0 (Windows NT 6.1; WOW64; rv:25.0) Gecko/20100101 Firefox/25.0");
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<RootObject> data = serializer.Deserialize<List<RootObject>>(wc.DownloadString("https://api.github.com/repos/Flare-Client/Flare-Client/releases"));
            foreach(RootObject ro in data)
            {
                if (ro.tag_name == ver)
                {
                    downloadFile(flareFiles.FullName + "/FlareFromLauncher.exe", ro.assets[0].browser_download_url);
                    return true;
                }
            }
            return false;
        }
        public static void downloadFile(string dest, string url)
        {
            WebClient wc = new WebClient();
            wc.Headers.Add("user-agent", " Mozilla/5.0 (Windows NT 6.1; WOW64; rv:25.0) Gecko/20100101 Firefox/25.0");
            wc.DownloadFile(url, dest);
        }

        public static void launchMinecraft()
        {
            Process.Start("minecraft://");
        }

        public static void saveVersion(string version)
        {
            if (!flareVerFile.Exists)
            {
                flareVerFile.Create().Close();
            }
            string toWrite = version + "|This file contains the latest downloaded flare version, this way we dont have to ask GitHub for the file each launch. Delete this file to force a re download or delete the FlareFromLauncher.exe";
            byte[] bytes = Encoding.UTF8.GetBytes(toWrite);
            flareVerFile.OpenWrite().Write(bytes, 0, bytes.Length);
        }
        public static string getLatestSavedVersion()
        {
            if (!flareVerFile.Exists)
            {
                return "None";
            }
            string read = File.ReadLines(flareVerFile.FullName).First();
            return read.Split('|')[0];
        }
    }
}
