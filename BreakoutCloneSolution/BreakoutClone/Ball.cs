using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakoutClone
{
	public class Ball
	{
		public const int InitialBallSpeed = 6;
		public const int BallResetPosition = 0; //Need to figure out the position
		public int BallXSpeed { get; set; } = InitialBallSpeed;
		public int BallYSpeed { get; set; } = InitialBallSpeed;
		public int BallX { get; set; } = 300;
		public int BallY { get; set; } = 200; //Need to adjust both of these when I decide on the ball's start position
		public int BallSize { get; private set; } = 20;

		public bool isLaunched { get; set; } = false;
	}
}
