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
		public int paddleWidth { get; set; } = 120;
		public int paddleHeight { get; set; } = 15;

		// To prevent the ball from getting stuck in the paddle
		public readonly int paddleOffset = 5;

		//Paddle position
		public int paddleX { get; set; }
		public int paddleY { get; set; }
	}
}
