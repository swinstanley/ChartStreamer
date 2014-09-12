using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections;

namespace ChartStreamer
{
    public static class ChartWriter
    {
          static HashSet<string> chartTypes = new HashSet<string>(new string[]{"areaChart", "area3DChart",
                               "lineChart", "line3DChart",
                               "stockChart", "radarChart",
                               "scatterChart",
                               "pieChart",
                               "pie3DChart",
                               "doughnutChart", "barChart", "bar3DChart", "ofPieChart", "surfaceChart", "surface3DChart", "bubbleChart"});
            const string chartsMl = "http://schemas.openxmlformats.org/drawingml/2006/chart";
            const string drawingMl = "http://schemas.openxmlformats.org/drawingml/2006/main";
        
        public static void WriteChart(this ChartRenderingJob job, XmlWriter writer)
        {
            XmlDocument dChart = new XmlDocument();
            dChart.Load(File.OpenRead(job.TemplatePath));
            

            XmlNamespaceManager nsMan = new XmlNamespaceManager(dChart.NameTable);
            foreach (XmlAttribute a in dChart.DocumentElement.Attributes)
            {
                if (a.Prefix == "xmlns")
                {
                    nsMan.AddNamespace(a.LocalName, a.Value);
                }
            }
            if (!string.IsNullOrWhiteSpace(job.Title))
            {
                dChart["c:chartSpace"]["c:chart"]["c:title"]["c:tx"]["c:rich"]["a:p"]["a:r"]["a:t"].InnerText = job.Title;
            }
            
            //write out the series, we make an assumption that there are enough defined (this is not going to be true

            var series = new Queue<XmlElement>(
                dChart.SelectNodes("/c:chartSpace/c:chart/c:plotArea/*/c:ser",nsMan).Cast<XmlElement>());
            
            foreach (var seriesJob in job.Series)
            {
                var seriesTemplate = series.Dequeue();
                
                UpdateLiteralValues(seriesTemplate["c:val"],seriesJob.Values);
                UpdateLiteralValues(seriesTemplate["c:cat"], job.CategoryHeadings);

                //TODO:
                
            }
            dChart.Save(writer);
        }

        internal static void UpdateLiteralValues (XmlElement valuesElement,IEnumerable values)
        {

            //the code will work the same if it's a string or a numeric
            XmlElement literal = valuesElement.FirstChild as XmlElement;

            int i = 0;
            var points = literal.GetElementsByTagName("c:pt");
                
            foreach (var v in values)
            {
                if (points.Count > i)
                {
                    points[i]["c:v"].InnerText = v.ToString();
                }
                else
                {
                    literal.InnerXml +=string.Format("<c:pt idx=\"{0}\"><c:v>{1}</c:v></c:pt>",i, v);

                }
                i++;
            }
            literal["c:ptCount"].SetAttribute("val", i.ToString());// InnerText = i.ToString();
    

        }
        
    }
}
