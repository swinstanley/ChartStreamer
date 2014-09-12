using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChartStreamer;
using System.Xml;

namespace ConsoleTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Random r = new Random();
           
            var seriesDefs = new List<ChartSeriesJob>();
            for (int i = 0; i < 4; i++)
            {
                string Name = "This is series " + i;
                var lValues = new List<double>();
                while (lValues.Count < 10)
                    lValues.Add(r.Next());
               
            }
            var catHeadings = new List<string>();
            while (catHeadings.Count< 10)
                catHeadings.Add("Category " + catHeadings.Count);

            ChartRenderingJob job = new ChartRenderingJob(){
                TemplatePath = "xl/charts/chart1.xml",
                Title = "Hello World",
                Series = seriesDefs,
                CategoryHeadings = catHeadings};

            XmlWriter w = XmlTextWriter.Create(Console.Out);
            job.WriteChart(w);

        }
    }
}
