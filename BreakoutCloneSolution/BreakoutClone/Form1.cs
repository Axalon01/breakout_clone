
using System.Drawing.Drawing2D;
using System.Reflection.PortableExecutable;

namespace BreakoutClone
{
	public partial class Form1 : Form
	{
		private GameManager gameManager;
		private Ball ball;
		private Paddle paddle;
        private readonly Random rng = new Random();
        public Form1()
		{
			InitializeComponent();
			InitializeCustomComponents();
			SetInitialWindowSizeAndPosition();

			//Hook up to the GameOver event
			gameManager.GameOverTriggered += (s, e) =>
				{
					GameTimer.Stop();
					Application.Exit();
				};
		}

		private void SetInitialWindowSizeAndPosition()
		{
			this.FormBorderStyle = FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.ClientSize = new Size(800, 600);

			gameManager = new GameManager();
			ball = new Ball();
			paddle = new Paddle();

			this.BackColor = Color.Black;
            this.DoubleBuffered = true;
        }

        private void InitializeCustomComponents()
		{
			this.KeyDown += new KeyEventHandler(OnKeyDown);
			this.KeyUp += new KeyEventHandler(KeyIsUp);
			this.KeyPreview = true; // Ensures the form receives key events
		}

		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Space)
			{
				ball.isLaunched = true;

				//Launch ball straight up
				ball.BallYSpeed = -Ball.InitialBallSpeed;

				//Randomize X direction
				int direction = rng.Next(0, 2) == 0 ? 1 : -1;
				ball.BallXSpeed = direction * Ball.InitialBallSpeed;
			}
			
			if (e.KeyCode == Keys.Escape)
			{
				TogglePause();
			}

			if (e.KeyCode == Keys.Left)
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
			if (e.KeyCode == Keys.Left)
			{
				gameManager.goLeft = false;
			}
			else if (e.KeyCode == Keys.Right)
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
			if (gameManager.IsPaused)
			
				return;

				UpdateBallPosition();
				//UpdateScoreText();
				CheckBallCollisions();
				AdjustPaddle();
				gameManager.CheckGameOver();

				this.Invalidate();
			
		}

		private void AdjustPaddle()
		{
			//Keep the paddle inside the screen
			paddle.paddleX = Math.Max(0, Math.Min(paddle.paddleX, this.ClientSize.Width
				- paddle.paddleWidth));

			if (gameManager.goLeft && paddle.paddleX > 0)
			{
				paddle.paddleX -= gameManager.PlayerSpeed;
			}

			if (gameManager.goRight && paddle.paddleX + paddle.paddleWidth < this.ClientSize.Width)
			{
				paddle.paddleX += gameManager.PlayerSpeed;
			}
		}

		private void CheckBallCollisions()
		{
			//Bounce off left wall
			if (ball.BallX < 0)
			{
				ball.BallX = 0;
                ball.BallXSpeed = Math.Abs(ball.BallXSpeed);
            }

			//Bounce off right wall
			if (ball.BallX + ball.BallSize > this.ClientSize.Width)
			{
				ball.BallX = this.ClientSize.Width - ball.BallSize;
				ball.BallXSpeed = -Math.Abs(ball.BallXSpeed);
			}

			//Bounce off top of screen
			if (ball.BallY < 0)
			{
				ball.BallY = 0;
				ball.BallYSpeed = Math.Abs(ball.BallYSpeed);
			}

			if (ball.BallY + ball.BallSize > this.ClientSize.Height + gameManager.BottomBoundaryOffset)
			{

                gameManager.lives--;
                ResetBallPosition();
            }

			//Check collision with paddle
			CheckCollision(ball.BallX, ball.BallY, ball.BallSize, paddle.paddleX, paddle.paddleY,
				paddle.paddleWidth, paddle.paddleHeight, paddle.paddleOffset);
		}

		private void ResetBallPosition()
		{
			ball.isLaunched = false;

            //Center the paddle on the screen
            paddle.paddleX = (this.ClientSize.Width / 2) - (paddle.paddleWidth / 2);

            //Center the ball on the paddle
            ball.BallX = paddle.paddleX + (paddle.paddleWidth / 2) - (ball.BallSize / 2);
            ball.BallY = paddle.paddleY - ball.BallSize;

            //Speeds reset to 0 until player launches again
            ball.BallXSpeed = 0;
            ball.BallYSpeed = 0;
        }

		private void CheckCollision(int ballX, int ballY, int ballSize, int paddleX, int paddleY,
			int paddleWidth, int paddleHeight, int offset)
		{
			Rectangle ballRect = new Rectangle(ball.BallX, ball.BallY, ball.BallSize, ball.BallSize);
			Rectangle paddleRect = new Rectangle(paddle.paddleX, paddle.paddleY, paddle.paddleWidth, paddle.paddleHeight);

			if (ballRect.IntersectsWith(paddleRect))
			{
				//Ball hit the top of the paddle
				ball.BallY = paddle.paddleY - ball.BallSize - paddle.paddleOffset;
				ball.BallYSpeed = -Math.Abs(ball.BallYSpeed);

                //Tweak X speed depending on where it hit the paddle
                int paddleCenter = paddle.paddleX + (paddle.paddleWidth / 2);
                int ballCenterX = ball.BallX + (ball.BallSize / 2);
                int offsetFromCenter = ballCenterX - paddleCenter;

                //Cause the ball trajectory to change depending on where it hit
                if (offsetFromCenter < 0)

                    ball.BallXSpeed = -Math.Abs(ball.BallXSpeed);
                else
                    ball.BallXSpeed = Math.Abs(ball.BallXSpeed);
            }
		}

		//private void UpdateScoreText()
		//{
		//	throw new NotImplementedException();
		//}

		private void UpdateBallPosition()
		{
			if (ball.isLaunched == true)
			{
				ball.BallX += ball.BallXSpeed;	//Figure out how to send a ball into a randon X direction later
				ball.BallY += ball.BallYSpeed;
			}
			else
			{
				//Follow the paddle before launching
				ball.BallX = paddle.paddleX + (paddle.paddleWidth / 2) - (ball.BallSize / 2);
				ball.BallY = paddle.paddleY - ball.BallSize;
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
				e.Graphics.FillEllipse(glowBrush, ball.BallX - 3, ball.BallY - 3, ball.BallSize + 6, ball.BallSize + 6);
			}

			e.Graphics.FillEllipse(Brushes.GhostWhite, ball.BallX, ball.BallY, ball.BallSize, ball.BallSize);
			e.Graphics.DrawEllipse(Pens.Black, ball.BallX, ball.BallY, ball.BallSize, ball.BallSize);

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
				//Top left corner
				path.AddArc(x, y, radius, radius, 180, 90);
				//Top edge + top-right corner
				path.AddArc(x + width - radius, y, radius, radius, 270, 90);
				//Right edge + bottom-right corner
				path.AddArc(x + width - radius, y + height - radius, radius, radius, 0, 90);
				//Bottom edge + bottom-left corner
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
