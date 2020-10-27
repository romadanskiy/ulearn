using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryTasks;
using System.Drawing;

namespace GeometryPainting
{
    //Напишите здесь код, который заставит работать методы segment.GetColor и segment.SetColor
    public static class SegmentExtensions
    {
        public static  Dictionary<Segment, Color> ColorsDictionary = new Dictionary<Segment, Color>();

        public static Color GetColor(this Segment segment)
        {
            if (!ColorsDictionary.ContainsKey(segment))
                return Color.Black;
            return ColorsDictionary[segment];
        }

        public static void SetColor(this Segment segment, Color color)
        {
            ColorsDictionary[segment] = color;
        }
    }
}
