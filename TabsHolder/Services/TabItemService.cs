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
            string title = null;
            try
            {
                var webGet = new HtmlWeb();
                var document = webGet.Load(url);
                title = document.DocumentNode.SelectSingleNode("html/head/title").InnerText;
            } catch (NullReferenceException e) {
                title = "Not possible to extrat title";
            } catch (UriFormatException e)
            {
                title = "Not possible to extrat title";
            } catch (ArgumentException e)
            {
                title = "Not possible to extrat title";
            }


            return title;
        }
    }
}
