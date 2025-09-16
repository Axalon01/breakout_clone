using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakoutClone
{
	public class Ball
	{
		private const int InitialBallSpeed = 6;
		private const int BallResetPosition = 0; //Need to figure out the position
		private int ballXSpeed = InitialBallSpeed;
		private int ballYSpeed = InitialBallSpeed;
		public int ballX { get; set; } = 300;
		public int ballY { get; set; } = 200; //Need to adjust both of these when I decide on the ball's start position
		public int ballSize { get; set; } = 20;
	}
}
