using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakoutClone
{
    public class Brick
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Brush Brush { get; set; }

        public void Draw(Graphics g)
        {
            g.FillRectangle(Brush, X, Y, Width, Height);
            using (Pen pen = new Pen(Color.Black, 2))
            {
                g.DrawRectangle(pen, X, Y, Width, Height);
            }
        }
    }
}