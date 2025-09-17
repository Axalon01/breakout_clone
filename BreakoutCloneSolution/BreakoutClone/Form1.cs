
using System.Drawing.Drawing2D;

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
			if (gameManager.showPauseOverlay)
			{
				using (SolidBrush overlayBrush = new SolidBrush(Color.FromArgb(100, Color.Black)))
				{
					e.Graphics.FillRectangle(overlayBrush, this.ClientRectangle);
				}

				//Draw the "Paused" text
			}
		}

		private void DrawRoundedRectangle(Graphics g, Brush brush, int x, int y, int width, int height, int radius)
		{
			using (GraphicsPath path = new GraphicsPath())
			{
				path.AddArc(x, y, radius, radius, 90, 180);
				path.AddArc(x + width - radius, y, radius, radius, 90, 270);
				path.AddArc(x + width - radius, y + height - radius, radius, radius, 0, 90);
				path.AddArc(x, y + height - radius, radius, radius, 90, 90);
				path.CloseFigure();

				//Fill the rounded rectangle
				g.FillPath(brush, path);

				//Draw the outline of the rounded rectangle
				using (Pen pen = new Pen(Color.Black, 2))
				{
					g.DrawPath(pen, path);
				}
			}
		}
	}
}
