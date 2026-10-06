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
    class Program
    {
        static void Main(string[] args)
        {

          
        
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var arlist1 = new ArrayList();
            HtmlWeb hw = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc = hw.Load("https://www.texaslottery.com/export/sites/lottery/Games/Lotto_Texas/Winning_Numbers/index.html_1481334353.html");
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
                ////h3[@class='sans']
                var div3 = doc.DocumentNode.SelectNodes("//h3");
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
              //  string newvalue = "", newvalue2 = "";
                try
                {
                    foreach (var fixturesTable in doc.DocumentNode.SelectNodes("//table[1]/tbody"))
                    {
                        foreach (var oppositionNode in fixturesTable.SelectNodes("tr"))
                        {
                            //var value = oppositionNode.SelectSingleNode("td[1]").InnerText.Trim();

                            var value2 = oppositionNode.SelectSingleNode("td[2]").InnerText.Trim();
                            var value3 = oppositionNode.SelectSingleNode("td[3]").InnerText.Trim();
                            if (value2 != "&nbsp;" && loop <= 4)
                            {
                                if (loop == 1)
                                {
                                    bool containsInt = value3.Any(char.IsDigit);
                                    if (containsInt)
                                    { sb.Append(value3 + "|"); }
                                    else
                                    {
                                        sb.Append("0" + "|");
                                    }
                                    string myvalue = value2;
                                    string mynewvalue = myvalue.Replace(" Million", "M");
                                    sb.Append(mynewvalue + "|");
                                }
                                else
                                {
                                    sb.Append(value3 + "|");
                                    sb.Append(value2 + "|");
                                }
                                loop++;
                            }
                            //else if(value1 != "&nbsp;")
                            //{
                            //    newvalue2= oppositionNode.SelectSingleNode("td[2]").InnerText.Trim();
                            //    newvalue = oppositionNode.SelectSingleNode("td[3]").InnerText.Trim();
                            //}
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
                            var value2 = oppositionNode.SelectSingleNode("td[3]").InnerText.Trim();
                            if (value2 != "&nbsp;")
                            {
                                city += value2 + ", ";
                            }
                        }
                    }
                    city = city.Substring(0, city.Length - 2);
                }
                catch { city = "none"; }
                sb.Append(city);
                //bool containsInt = newvalue2.Any(char.IsDigit);
                //if (containsInt)
                //{ sb.Append(newvalue + "/$2"); }
                //else
                //{
                //    sb.Append(newvalue);
                //}
                list.Add(sb.ToString());
            }
            System.IO.File.WriteAllLines(@"D:\lines.txt", list.ToArray());
            Console.WriteLine("Lines written to file successfully.");








            //var HeaderNames = doc.DocumentNode.SelectNodes("//table[@class='large-only']");
            //var div3 = doc.DocumentNode.SelectNodes("//*[@id='content']/table/*/tr[1]/td[1]");
            //var value = div3.FirstOrDefault().InnerText;

            //HtmlNodeCollection tables = doc.DocumentNode.SelectNodes("//table");

            //// Iterate all rows in the first table
            //HtmlNodeCollection rows = tables[0].SelectNodes("tdody");
            //for (int i = 0; i <= rows.Count - 1; i++)
            //{
            //    // Iterate all columns in this row
            //    HtmlNodeCollection cols = rows[i].SelectNodes("td");
            //    if (cols != null)
            //    {
            //        for (int j = 0; j <= cols.Count - 1; j++)
            //        {
            //            // Get the value of the column and print it
            //            string value = cols[j].InnerText;
            //            Console.WriteLine(value);
            //        }
            //    }
            //}
            // Using LINQ to parse HTML table smartly 
            //var HTMLTableTRList = from table in doc.DocumentNode.SelectNodes("//table").Cast<HtmlNode>()
            //                      from row in table.SelectNodes("tbody").Cast<HtmlNode>()
            //                      from cell in row.SelectNodes("th|td").Cast<HtmlNode>()
            //                      select new { Table_Name = table.Id, Cell_Text = cell.InnerText };

            //// now showing output of parsed HTML table
            //foreach (var cell in HTMLTableTRList)
            //{
            //    Console.WriteLine("{0}: {1}", cell.Table_Name, cell.Cell_Text);
            //}

        }
    }
  
    public class Row
    {
        public string Title { get; set; }
    }
}
