using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChartStreamer
{
    [Serializable]
    public  sealed class ChartRenderingJob
    {
        public string TemplatePath{get;set;} //should be relative to a zip root
        public string Title {get;set;}
        public IEnumerable<ChartSeriesJob> Series{get;set;}
        public IEnumerable<string> CategoryHeadings { get; set; }    
    }
    [Serializable]
    public sealed class ChartSeriesJob
    {
        public string Name  {get;set;}
        public IEnumerable<string> Values{get;set;} //passed as string as this is an inevitable conversion anyway
        public Tuple<byte,byte,byte> Color{get;set;}
    }
    
}
