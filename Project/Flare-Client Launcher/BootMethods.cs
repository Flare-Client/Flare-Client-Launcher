using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Threading.Tasks;
using System.Net;

namespace Flare_Client_Launcher
{
    public class BootMethods
    {
        public static DirectoryInfo flareFiles = new DirectoryInfo(Environment.CurrentDirectory + "/FlareFiles");
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
    }
}
