using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorComponents.ChartJs.Model
{
    public class ColorHelper
    {
        public static string GetColor(ColorCode colorCode, double transparency)
        {
            switch (colorCode)
            {
                case ColorCode.Green:
                    return $"rgba(0,100,0, {transparency})";
                case ColorCode.Red:
                    return $"rgba(139,0,0, {transparency})";
                case ColorCode.Yellow:
                    return $"rgba(255,140,0, {transparency})";
                case ColorCode.Grey:
                    return $"rgba(128,128,128, {transparency})";
                case ColorCode.Blue:
                    return $"rgba(45,149,236, {transparency})";
                case ColorCode.Orange:
                    return $"rgba(226,113,29, {transparency})";
                default:
                    return $"rgba(0,0,0, {transparency})";
            }
        }

    }

    public enum ColorCode
    {
        Green,
        Red,
        Yellow,
        Grey,
        Blue,
        Orange
    }
}
