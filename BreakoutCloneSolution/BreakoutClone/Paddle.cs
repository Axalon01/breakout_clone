using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakoutClone
{
	public class Paddle
	{
		//Paddle size
		public int paddleWidth { get; set; } = 30;
		public int paddleHeight { get; set; } = 120;

		//Paddle position
		public int paddleX { get; set; } = 0;
		public int paddleY { get; set; } = 0; //Will need to actually set these later
	}
}
