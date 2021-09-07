using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamAlignment.Core.Model.Presentation
{
    public class BarChart
    {
        public BarChartOrientation Orientation { get; set; }
        public string ChartJSOrientation
        {
            get
            {
                switch (Orientation)
                {
                    case BarChartOrientation.Vertical:
                        return "bar";
                    case BarChartOrientation.Horizontal:
                        return "horizontalBar";
                    default:
                        return "bar";
                }
            }
        }
        public string Id { get; set; }
        public List<string> Labels { get; set; }
        public List<ChartDataset> Datasets { get; set; }
        public bool IsStacked { get; set; }
    }
}
