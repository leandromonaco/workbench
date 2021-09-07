using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamAlignment.Core.Model.Presentation
{
    public class ChartDataset
    {
        public string Label { get; set; }
        public List<double> Data { get; set; }
        public string BackgroundColor { get; set; }
        public string BorderColor { get; set; }
        public int BorderWidth { get; set; }
        public string Type { get; set; }
    }
}
