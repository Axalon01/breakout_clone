using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakoutClone
{
    public class Brick
    {
        public int BrickX { get; set; }
        public int BrickY { get; set; }
        public int BrickWidth { get; set; }
        public int BrickHeight { get; set; }
        public Brush Brush { get; set; }

        public void Draw(Graphics g)
        {
            g.FillRectangle(Brush, BrickX, BrickY, BrickWidth, BrickHeight);
            using (Pen pen = new Pen(Color.Black, 2))
            {
                g.DrawRectangle(pen, BrickX, BrickY, BrickWidth, BrickHeight);
            }
        }
    }
}