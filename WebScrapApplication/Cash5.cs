using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using HtmlAgilityPack;
using System.IO;
using System.Globalization;
using System.Net;
using System.Collections;
using ObjectDumper;
using System.Text.RegularExpressions;

namespace WebScrapApplication
{
    class Cash5
    {
        public void DATA()
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var arlist1 = new ArrayList();
            HtmlWeb hw = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc = hw.Load("https://www.texaslottery.com/export/sites/lottery/Games/Cash_Five/Winning_Numbers/index.html_1431178638.html");
            //("//a[@class='detailsLink']");
            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@class='detailsLink']"))
            {
                // Get the value of the HREF attribute
                string hrefValue = link.GetAttributeValue("href", string.Empty);

                arlist1.Add("https://www.texaslottery.com" + hrefValue);
            }
            // var htmlDoc = new HtmlDocument();
            StringBuilder sb = new StringBuilder();
            string[] lines = { };
            //int i = 0;
            List<string> list = new List<string>();
            foreach (var item in arlist1)
            {
                //large-only
                sb.Clear();
                doc = hw.Load(item.ToString());
                var div3 = doc.DocumentNode.SelectNodes("//h3[@class='sans']");
                var datevalue = div3.FirstOrDefault().InnerText;
                Match match = Regex.Match(datevalue, @"\d{2}\/\d{2}\/\d{4}");
                string date = match.Value;
                sb.Append(date + "|");
                foreach (var winningNumver in doc.DocumentNode.SelectNodes("//ol[@class='winningNumberBalls']"))
                {
                    foreach (var winno in winningNumver.SelectNodes("li"))
                    {
                        string finalwinno = "";
                        var number = winno.SelectSingleNode("span").InnerHtml.Trim();
                        Regex regex = new Regex(@"\b[0-9]{1}\b"); //match pattern for exactly 1 digit between 0 and 9 
                        Match match1 = regex.Match(number);
                        if (match1.Success)
                        {
                            finalwinno = "0" + number;
                        }
                        else
                        {
                            finalwinno = number;
                        }
                        sb.Append(finalwinno + "|");
                    }

                }

                //rt-responsive-table-0
                //large-only
                //[@class='large-only']/tbody
                int loop = 1;
                string newvalue = "", newvalue2 = "";
                try
                {
                    foreach (var fixturesTable in doc.DocumentNode.SelectNodes("//table[1]/tbody"))
                    {
                        foreach (var oppositionNode in fixturesTable.SelectNodes("tr"))
                        {
                            //var value = oppositionNode.SelectSingleNode("td[1]").InnerText.Trim();

                            var value1 = oppositionNode.SelectSingleNode("td[2]").InnerText.Trim();
                            var value2 = oppositionNode.SelectSingleNode("td[3]").InnerText.Trim();
                            if (value1 != "&nbsp;" && loop <= 3)
                            {
                                sb.Append(value2 + "|");
                                sb.Append(value1 + "|");
                                loop++;
                            }
                            else if (value1 != "&nbsp;")
                            {
                                newvalue2 = oppositionNode.SelectSingleNode("td[2]").InnerText.Trim();
                                newvalue = oppositionNode.SelectSingleNode("td[3]").InnerText.Trim();
                            }
                        }
                    }
                }
                catch { }
                string city = "";
                try
                {
                    foreach (var fixturesTable in doc.DocumentNode.SelectNodes("//table[2]/tbody"))
                    {
                        foreach (var oppositionNode in fixturesTable.SelectNodes("tr"))
                        {
                            var value2 = oppositionNode.SelectSingleNode("td[4]").InnerText.Trim();
                            if (value2 != "&nbsp;")
                            {
                                city += value2 + ", ";
                            }
                        }
                    }
                    city = city.Substring(0, city.Length - 2);
                }
                catch { city = "none"; }
                sb.Append(city + "|");
                bool containsInt = newvalue2.Any(char.IsDigit);
                if (containsInt)
                { sb.Append(newvalue + "/$2"); }
                else
                {
                    sb.Append(newvalue);
                }
                list.Add(sb.ToString());
            }
            System.IO.File.WriteAllLines(@"D:\lines.txt", list.ToArray());
            Console.WriteLine("Lines written to file successfully.");
        }
    }
}
