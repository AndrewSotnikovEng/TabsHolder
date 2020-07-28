using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TabsHolder
{
    class TabItemService
    {
        public static string getUrlTitle(string url)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var webGet = new HtmlWeb();
            var document = webGet.Load(url);
            var title = document.DocumentNode.SelectSingleNode("html/head/title").InnerText;

            return title;
        }
    }
}
