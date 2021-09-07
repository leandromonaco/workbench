using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorComponents.ChartJs.Model
{
    public class Chart
    {
        public ChartType Type { get; set; }
        public string ChartJsType
        {
            get
            {
                switch (Type)
                {
                    case ChartType.BarVertical:
                        return "bar";
                    case ChartType.BarHorizontal:
                        return "horizontalBar";
                    case ChartType.Line:
                        return "line";
                    case ChartType.Pie:
                        return "pie";
                    case ChartType.Doughnut:
                        return "doughnut";
                    default:
                        return "";
                }
            }
        }
        public string Id { get; set; }
        public List<string> Labels { get; set; }
        public List<ChartDataset> Datasets { get; set; }
        public bool IsStacked { get; set; }
    }
}
