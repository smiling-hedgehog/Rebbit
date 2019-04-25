using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rebbit01
{
    class SoundParser
    {
        private string query;
        private HtmlWorker htm;
        public SoundParser(string strQuery)
        {
            query = strQuery;
            htm = new HtmlWorker();
        }
        public SoundParser()
        {

        }

        private void isLastPage(string source, ref bool flag)
        {
            var parser = new HtmlParser();
            var document = parser.Parse(source);
            var myList = document.QuerySelectorAll("span.pager__link");
            if (myList.Length > 0)
            {
                if (myList.Last().TextContent == "Следующая")
                {
                    flag = true;
                }
                else flag = false;
            }
            else flag = false;

        }

        public SoundNode BuildNode()
        {
            Stopwatch sWatch = new Stopwatch();

            sWatch.Start();
            string source;
            var parser = new HtmlParser();

            SoundNode head = new SoundNode(query);
            bool flag = true;
            int i = 1;

            while (flag)
            {
                source = htm.getHtml(htm.getQueryString(query, "http://zaycev.net/search.html?page=" + i + "&query_search="));
                var document = parser.Parse(source);
                // var myList = document.QuerySelectorAll("div.musicset-track__track-name");
                //Regex regex = new Regex(@"[/]\w*[/]\w*[/]\w*[.]\w*", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                var UrlList = document.QuerySelectorAll("div.musicset-track:nth-child(n) > div:nth-child(1) > div:nth-child(4) > a:nth-child(1)").Where(item => item.Attributes["href"] != null).Select(x => x.GetAttribute("href")).ToList();//ссылка
                var ArtistList = document.QuerySelectorAll("div.musicset-track:nth-child(n) > div:nth-child(1) > div:nth-child(2) > a:nth-child(1)").ToList();//артист

                var durationSong = document.QuerySelectorAll("div.musicset-track:nth-child(n) > div:nth-child(2)").ToList();//время
                var nameTrack = document.QuerySelectorAll("div.musicset-track:nth-child(n) > div:nth-child(1) > div:nth-child(4) > a:nth-child(1)").ToList();//name track


                var res = ArtistList.Zip(nameTrack.Zip(durationSong, (s2, s3) => s2.TextContent + " " + s3.TextContent), (s1, s2) => s1.TextContent + " " + s2);


                var result = UrlList.Zip(res, (first, second) => new { first, second });


                foreach (var s in result/*myList*/)

                {

                    //MatchCollection matches = regex.Matches(s.InnerHtml);
                    //  if (matches.Count > 0)
                    //{
                    //  foreach (Match match in matches)
                    //{
                    SoundNode tmp = new SoundNode(s.second/*TextContent*/);
                    tmp.setAttr(s.first/*match.Value*/);
                    head.Nodes.Add(tmp);
                    //}

                    //}
                }

                i++;
                isLastPage(source, ref flag);



            }
            sWatch.Stop();
            TimeSpan tSpan;
            tSpan = sWatch.Elapsed;
            MessageBox.Show(tSpan.ToString());
            return head;


        }

        public string getMp3Link(string urlPage)
        {
            string source;
            HtmlWorker htm = new HtmlWorker();
            var parser = new HtmlParser();
            source = htm.getHtml("http://www.zaycev.net" + urlPage);
            if (isContentGoodForDownload(source))
            {
                var document = parser.Parse(source);

                var myList = document.QuerySelectorAll("a.button-download__link").Where(item => item.Attributes["href"] != null).Select(x => x.GetAttribute("href")).ToList();
                List<string> list = new List<string>();


                foreach (var sm in myList)
                {

                    //MessageBox.Show(sm);
                    list.Add(sm);

                }
                return list[0].ToString();
            }
            else
                return null;

        }



        private bool isContentGoodForDownload(string source)
        {
            bool retFlag = false;
            var parser = new HtmlParser();
            var document = parser.Parse(source);
            var myList = document.QuerySelector(".audiotrack-download").Attributes;
            foreach (var i in myList)
            {
                if (i.Name == "style")
                {
                    if (i.Value == "display: flex;")
                    {
                        // Console.WriteLine("style none");
                        retFlag = false;

                    }
                    else if (i.Value == "display: none;")
                    {
                        retFlag = true;
                    }
                    else
                        retFlag = false;

                }
            }
            return retFlag;
        }
    }
}
