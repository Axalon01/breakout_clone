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
		private const float reducedVolume = 0.1f; //10% volume
		private float originalVolume = 0.7f;
		private bool isPaused = false;
		private bool showPauseOverplay = false;
		int playerSpeed = 12; //Sets default player speed
	}
}
