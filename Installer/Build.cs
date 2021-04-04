using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Installer
{
    public class Build
    {
        public string VersionName { get; set; }
        public string Url { get; set; }
        public Build(string versionName, string url)
        {
            VersionName = versionName;
            Url = url;
        }


    }
}
