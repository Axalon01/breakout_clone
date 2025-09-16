
namespace BreakoutClone
{
	public partial class Form1 : Form
	{
		private GameManager gameManager; //Reference to the GameManager class
		private Ball ball;
		private Paddle paddle;
		public Form1()
		{
			InitializeComponent();
			InitializeCustomComponents();
			SetInitialWindowSizeAndPosition();
		}

		private void SetInitialWindowSizeAndPosition()
		{
			this.FormBorderStyle = FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.ClientSize = new Size(800, 600);

			gameManager = new GameManager(); //Create the game manager object
		}

		private void InitializeCustomComponents()
		{
			this.KeyDown += new KeyEventHandler(OnKeyDown);
			this.KeyUp += new KeyEventHandler(KeyIsUp);
			this.KeyPreview = true; // Ensures the form receives key events
		}

		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Space)
			{
				TogglePause();
			}
			else if (e.KeyCode == Keys.Left)
			{
				gameManager.goLeft = true;
			}
			else if (e.KeyCode == Keys.Right)
			{
				gameManager.goRight = true;
			}
		}
		
		// Key up event to stop paddle
		private void KeyIsUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Down)
			{
				gameManager.goLeft = false;
			}
			else if (e.KeyCode == Keys.Up)
			{
				gameManager.goRight = false;
			}
		}

		private void TogglePause()
		{
			gameManager.IsPaused = !gameManager.IsPaused;

			if (gameManager.IsPaused)
			{
				GameTimer.Stop();
				//waveOutEvent.Volume = gameManager.PausedVolume;
			}
			else
			{
				GameTimer.Start();
				//waveOutEvent.Volume = gameManager.SetVolume;
			}

			this.Invalidate(); //Force the form to repaint
		}

		private void GameTimerEvent(object sender, EventArgs e)
		{
			if (IsPaused)
			{
				return;

				UpdateBallPosition();
				UpdateScoreText();
				CheckBallCollisions();
				AdjustPaddle();
				CheckGameOver();

				this.Invalidate();
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			//Enable anti-aliasing for smoother graphics
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			//Draw the ball with a glow
			using (SolidBrush glowBrush = new SolidBrush(Color.FromArgb(100, Color.GhostWhite)))
			{
				e.Graphics.FillEllipse(glowBrush, ball.ballX - 3, ball.ballY - 3, ball.ballSize + 6, ball.ballSize + 6);
			}

			e.Graphics.FillEllipse(Brushes.GhostWhite, ball.ballX, ball.ballY, ball.ballSize, ball.ballSize);
			e.Graphics.DrawEllipse(Pens.Black, ball.ballX, ball.ballY, ball.ballSize, ball.ballSize);

			//Draw the paddle with rounded corners
			DrawRoundedRectangle(e.Graphics, Brushes.Silver, paddle.paddleX, paddle.paddleY, paddle.paddleWidth, paddle.paddleHeight, 10);

			//Draw semi-transparent overlay when paused


			//Draw the "Paused" text
		}
	}
}
