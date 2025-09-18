using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakoutClone
{
	public class GameManager
	{
		Ball ball;

		public int lives { get; set; } = 3;
		private int score { get; set; } = 0;
		public float SetVolume { get; set; } = 0.7f; // Can be changed later, like by the user
		public float PausedVolume
		{
			get
			{
				return SetVolume >= 0.1f ? 0.1f : SetVolume;
			}
		}
		public bool IsPaused { get; set; } = false;
		public bool showPauseOverlay => IsPaused;
		public bool goLeft, goRight;
		public int PlayerSpeed { get; private set; } = 12; //Sets default player speed

		public void CheckGameOver()
		{
			if (lives == 0)
			{
				GameOver("Out of lives. Better luck next time.");
			}
			else if (score == 864)
			{
				GameOver("Congrats! You bashed all those bricks!");
			}
		}

		private void GameOver(string message)
		{
			MessageBox.Show(message);
			//ResetGameState();
			GameOverTriggered?.Invoke(this, EventArgs.Empty);
		}

		public event EventHandler GameOverTriggered;

		private void ResetGameState()
		{
			score = 0;
			lives = 3;
			ball.BallXSpeed = ball.BallYSpeed = Ball.InitialBallSpeed;
		}
	}
}
