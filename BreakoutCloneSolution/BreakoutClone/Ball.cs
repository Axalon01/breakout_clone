using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakoutClone
{
	public class Ball
	{
		public const int InitialBallSpeed = 8;
		public const int BallResetPosition = 0; //Need to figure out the position
		public int BallXSpeed { get; set; } = InitialBallSpeed;
		public int BallYSpeed { get; set; } = InitialBallSpeed;
		public int BallX { get; set; }
		public int BallY { get; set; }
		public int BallSize { get; private set; } = 20;

		public bool isLaunched { get; set; } = false;
	}
}
