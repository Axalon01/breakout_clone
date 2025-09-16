using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakoutClone
{
	public class GameManager
	{
		private int lives = 3;
		private int score = 0;
		public float SetVolume { get; set; } = 0.7f; // Can be changed later, like by the user
		public float PausedVolume
		{
			get
			{
				return SetVolume >= 0.1f ? 0.1f : SetVolume;
			}
		}
		public bool IsPaused { get; set; } = false;
		private bool showPauseOverplay = false;
		public bool goLeft, goRight;
		int playerSpeed = 12; //Sets default player speed

		private void CheckGameOver()
		{
			if (lives == 0)
			{
				GameOver("Out of lives. Better luck nenxt time.");
			}
			else if (score == 864)
			{
				GameOver("Congrats! You bashed all those bricks!");
			}
		}

		private void GameOver(string message)
		{
			GameTimer.Stop();
			MessageBox.Show(message);
			ResetGameState();
			GameTimer.Start();
			Application.Exit();
		}
	}
}
