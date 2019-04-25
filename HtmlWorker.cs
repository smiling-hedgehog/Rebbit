using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace rebbit01
{
    class HtmlWorker
    {
   
        public string getQueryString(string strSearch, string url)
        {
            strSearch = strSearch.Replace(" ", "+");
            return url + strSearch;
        }

        public string getHtml(string url)
        {
            try
            {
                string line;
                string lineRes = "";
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                Stream data = client.OpenRead(url);
                StreamReader reader = new StreamReader(data, Encoding.UTF8);
                while ((line = reader.ReadLine()) != null)
                {
                    lineRes += line;
                }
                data.Close();
                reader.Close();
                return lineRes;
            }
            catch
            {
                return null;
            }

        }

    }
}
