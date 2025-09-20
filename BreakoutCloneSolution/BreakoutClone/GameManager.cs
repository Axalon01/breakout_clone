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
		public int score { get; set; } = 0;
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
		//To make the ball go off screen before disappearing
		public int BottomBoundaryOffset { get; private set; } = 30;
        private bool gameOverTriggered = false;

        public void CheckGameOver()
		{
			if (gameOverTriggered) return;

			if (lives == 0)
			{
				gameOverTriggered = true;
				GameOver("Out of lives. Better luck next time.");
			}
			else if (score == 864)
			{
				gameOverTriggered = true;
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
