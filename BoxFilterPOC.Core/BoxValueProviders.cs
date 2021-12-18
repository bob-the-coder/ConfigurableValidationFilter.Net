using System;

namespace BoxFilterExample
{
    public static class BoxValueProviders
    {
        public static Func<Box, int> Height = box => box.Height;
        public static Func<Box, int> Width = box => box.Width;
        public static Func<Box, int> Depth = box => box.Depth;
        public static Func<Box, int> Area = box => box.Width * box.Depth;
        public static Func<Box, int> Volume = box => box.Height / 10 * box.Width / 10 * box.Depth / 10;
        public static Func<Box, double> Density = box => box.Weight / ((double)box.Height / 100 * box.Width / 100 * box.Depth / 100);
        public static Func<Box, int> Weight = box => box.Weight;
        public static Func<Box, string> Color = box => box.Color;
        public static Func<Box, DateTime> ReceivedOn = box => box.ReceivedOn;
        public static Func<Box, bool> IsRecent = box => (int)(DateTime.UtcNow - box.ReceivedOn).TotalDays <= 1;
    }
}
