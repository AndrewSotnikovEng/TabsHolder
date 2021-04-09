﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Installer
{
    [Serializable()]
    public class Build
    {
        [System.Xml.Serialization.XmlElement("VersionName")]
        public string VersionName { get; set; }
        [System.Xml.Serialization.XmlElement("Url")]
        public string Url { get; set; }
        public Build(string versionName, string url)
        {
            VersionName = versionName;
            Url = url;
        }

        public Build()
        {

        }


    }
}
