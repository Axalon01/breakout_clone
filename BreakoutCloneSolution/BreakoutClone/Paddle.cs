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
		public int paddleHeight { get; set; } = 30;

		// To prevent the ball from getting stuck in the paddle
		public const int paddleOffset = 5;

		//Paddle position
		public int paddleX { get; set; } = 300;
		public int paddleY { get; set; } = 500;
	}
}
