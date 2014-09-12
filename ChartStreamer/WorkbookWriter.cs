using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;

namespace ChartStreamer
{
    /// <summary>
    /// Description of WorkbookStreamer.
    /// </summary>
    public static class WorkbookWriter
    {
        public static void UpdateWorkbook(this WorkbookJob job)
        {
            
            
            ZipFile zip = new ZipFile(job.Path);
            Dictionary<string,ChartRenderingJob> workToDo = job.ChartJobs.ToDictionary(x=>x.TemplatePath);
            foreach(var chartJob in job.ChartJobs)
            {
                TemporaryDataSource.Process(zip,chartJob);
            }
            
        }
        
        internal sealed class TemporaryDataSource: ICSharpCode.SharpZipLib.Zip.IStaticDataSource
        {
         
            public static void Process( ZipFile zip, ChartRenderingJob chart)
            {
                TemporaryDataSource tds = new TemporaryDataSource(){ ms = new MemoryStream()};
                var currentEntry = zip.GetEntry(chart.TemplatePath);
                using( var input = zip.GetInputStream(currentEntry))
                {
                    ChartWriter.RenderChart(chart,input,tds.ms);
                }
                zip.BeginUpdate();
                zip.Add(tds, currentEntry.Name,currentEntry.CompressionMethod,currentEntry.IsUnicodeText);
                zip.CommitUpdate();

                
            }
            private TemporaryDataSource()
            {
            }
            MemoryStream ms;
            public Stream  GetSource()
            {
                ms.Position = 0;
                return ms;
            }
            
        }
     
    }

    public class WorkbookJob
    {
        public string Path;
        public IEnumerable<ChartRenderingJob> ChartJobs;
    }

}
