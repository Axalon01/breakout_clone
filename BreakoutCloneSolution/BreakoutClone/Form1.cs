
using System.Drawing.Drawing2D;
using System.Reflection.PortableExecutable;

namespace BreakoutClone
{
	public partial class Form1 : Form
	{
		private GameManager gameManager;
		private Ball ball;
		private Paddle paddle;
		private Brick brick;
        private readonly Random rng = new Random();
		private Brick[,] bricks;
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
			this.ClientSize = new Size(927, 768);

            gameManager = new GameManager();
            ball = new Ball();
            paddle = new Paddle();
			brick = new Brick();

            paddle.PaddleX = (this.ClientSize.Width - paddle.PaddleWidth) / 2;
			paddle.PaddleY = (this.ClientSize.Height - paddle.PaddleHeight) - 30; //30px above the bottom

			ball.BallX = (this.ClientSize.Width - ball.BallSize) / 2;
			ball.BallY = paddle.PaddleY - ball.BallSize;

			this.BackColor = Color.Black;
            this.DoubleBuffered = true;

			InitializeBricks();
        }

        private void InitializeCustomComponents()
		{
			this.KeyDown += new KeyEventHandler(OnKeyDown);
			this.KeyUp += new KeyEventHandler(KeyIsUp);
			this.KeyPreview = true; // Ensures the form receives key events
		}

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (!ball.isLaunched)
            {
                if (e.KeyCode == Keys.Space)
                {
                    ball.isLaunched = true;

                    // Launch ball straight up
                    ball.BallYSpeed = -Ball.InitialBallSpeed;

                    // Randomize X direction
                    int direction = rng.Next(0, 2) == 0 ? 1 : -1;
                    ball.BallXSpeed = direction * Ball.InitialBallSpeed;
                }
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
				CheckBallCollisions();
				AdjustPaddle();
				CheckBrickCollision();

                gameManager.CheckGameOver();

				this.Invalidate();
			
		}

		private void AdjustPaddle()
		{
			//Keep the paddle inside the screen
			paddle.PaddleX = Math.Max(0, Math.Min(paddle.PaddleX, this.ClientSize.Width
				- paddle.PaddleWidth));

			if (gameManager.goLeft && paddle.PaddleX > 0)
			{
				paddle.PaddleX -= gameManager.PlayerSpeed;
			}

			if (gameManager.goRight && paddle.PaddleX + paddle.PaddleWidth < this.ClientSize.Width)
			{
				paddle.PaddleX += gameManager.PlayerSpeed;
			}
		}

		private void CheckBrickCollision()
		{

			for (int row = 0; row < bricks.GetLength(0); row++)
			{
				for (int col = 0; col < bricks.GetLength(1); col++)
				{
					Brick brick = bricks[row, col];
					if (brick == null || brick.IsDestroyed) continue;

					Rectangle ballRect = new Rectangle(ball.BallX, ball.BallY, ball.BallSize, ball.BallSize);
					Rectangle brickRect = new Rectangle(brick.BrickX, brick.BrickY, brick.BrickWidth, brick.BrickHeight);

					if (ballRect.IntersectsWith(brickRect))
					{
						// Calculate horizontal overlap
						int overlapX = Math.Min(ballRect.Right - brickRect.Left, brickRect.Right - ballRect.Left);

						// Calculate vertical overlap
						int overlapY = Math.Min(ballRect.Bottom - brickRect.Top, brickRect.Bottom - ballRect.Top);

						// Decide which side to bounce
						if (overlapX < overlapY)
						{
							// Hit from left or right ¨ flip X
							ball.BallXSpeed = -ball.BallXSpeed;
						}
						else
						{
							// Hit from top or bottom ¨ flip Y
							ball.BallYSpeed = -ball.BallYSpeed;
						}

						brick.IsDestroyed = true;
						gameManager.Score += brick.PointValue;
					}
				}
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

                gameManager.Lives--;
                ResetBallPosition();
            }

			//Check collision with paddle
			CheckPaddleCollision(ball.BallX, ball.BallY, ball.BallSize, paddle.PaddleX, paddle.PaddleY,
				paddle.PaddleWidth, paddle.PaddleHeight, paddle.paddleOffset);
		}

		private void ResetBallPosition()
		{
			ball.isLaunched = false;

            //Center the paddle on the screen
            paddle.PaddleX = (this.ClientSize.Width / 2) - (paddle.PaddleWidth / 2);

            //Center the ball on the paddle
            ball.BallX = paddle.PaddleX + (paddle.PaddleWidth / 2) - (ball.BallSize / 2);
            ball.BallY = paddle.PaddleY - ball.BallSize;

            //Speeds reset to 0 until player launches again
            ball.BallXSpeed = 0;
            ball.BallYSpeed = 0;
        }

		private void CheckPaddleCollision(int ballX, int ballY, int ballSize, int paddleX, int paddleY,
			int paddleWidth, int paddleHeight, int offset)
		{
			Rectangle ballRect = new Rectangle(ball.BallX, ball.BallY, ball.BallSize, ball.BallSize);
			Rectangle paddleRect = new Rectangle(paddle.PaddleX, paddle.PaddleY, paddle.PaddleWidth, paddle.PaddleHeight);

			if (ballRect.IntersectsWith(paddleRect))
			{
				//Ball hit the top of the paddle
				ball.BallY = paddle.PaddleY - ball.BallSize - paddle.paddleOffset;
				ball.BallYSpeed = -Math.Abs(ball.BallYSpeed);

                //Tweak X speed depending on where it hit the paddle
                int paddleCenter = paddle.PaddleX + (paddle.PaddleWidth / 2);
                int ballCenterX = ball.BallX + (ball.BallSize / 2);
                int offsetFromCenter = ballCenterX - paddleCenter;

				//Normalize to -1 t0 +1
				float relativeOffset = (float)offsetFromCenter / (paddle.PaddleWidth / 2);

				//Use current speed
				int totalSpeed = ball.CurrentBallSpeed;

                //Calculate new X and Y speeds
                ball.BallXSpeed = (int)(relativeOffset * totalSpeed);

                //Keep the speed constant by adjusting Y
                int xSquared = ball.BallXSpeed * ball.BallXSpeed;
				int ySquared = (totalSpeed * totalSpeed) - xSquared;

				//Safety check (avoid square root of negative if rounding errors happen)
				if (ySquared < 0) ySquared = 0;

				ball.BallYSpeed = -(int)Math.Sqrt(ySquared); //Negative so it always goes upward

				// --- Clamps to prevent boring/infinite loops ---

				//Prevent perfectly verticle bounces
				if (ball.BallXSpeed == 0)
				{
					ball.BallXSpeed = rng.Next(0, 2) == 0 ? 1 : -1; //Tiny nudge left or right
				}

				//Prevent completely horizontal bounces
				if (Math.Abs(ball.BallYSpeed) < 2)
				{
					ball.BallYSpeed = ball.BallYSpeed < 0 ? -2 : 2; //Force at least two upward
				}
            }
		}

		private void UpdateBallPosition()
		{
			if (ball.isLaunched == true)
			{
				ball.BallX += ball.BallXSpeed;
				ball.BallY += ball.BallYSpeed;
			}
			else
			{
				//Follow the paddle before launching
				ball.BallX = paddle.PaddleX + (paddle.PaddleWidth / 2) - (ball.BallSize / 2);
				ball.BallY = paddle.PaddleY - ball.BallSize;
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			//Enable anti-aliasing for smoother graphics
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			DrawBall(e.Graphics);
			DrawPaddle(e.Graphics);
			DrawPauseOverlay(e.Graphics);
            DrawLives(e.Graphics);
			Drawscore(e.Graphics);
			DrawBricks(e.Graphics);
        }

		private void DrawBall(Graphics g)
		{
            //Draw the ball with a glow
            using (SolidBrush glowBrush = new SolidBrush(Color.FromArgb(100, Color.GhostWhite)))
            {
                g.FillEllipse(glowBrush, ball.BallX - 3, ball.BallY - 3, ball.BallSize + 6, ball.BallSize + 6);
            }

            g.FillEllipse(Brushes.GhostWhite, ball.BallX, ball.BallY, ball.BallSize, ball.BallSize);
            g.DrawEllipse(Pens.Black, ball.BallX, ball.BallY, ball.BallSize, ball.BallSize);
        }

		private void DrawPaddle(Graphics g)
		{
            //Draw the paddle with rounded corners
            DrawRoundedRectangle(g, Brushes.Silver, paddle.PaddleX, paddle.PaddleY, paddle.PaddleWidth, paddle.PaddleHeight, 10);
        }

		private void DrawPauseOverlay(Graphics g)
		{
			//Draw semi-transparent overlay when paused
			if (gameManager.ShowPauseOverlay)
			{
				using (SolidBrush overlayBrush = new SolidBrush(Color.FromArgb(100, Color.Black)))
				{
					g.FillRectangle(overlayBrush, this.ClientRectangle);
				}

                //Draw the "Paused" text
                Font pausedFont = new Font("Impact",28);
                string pausedText = "Paused";
                SizeF textSize = g.MeasureString(pausedText, pausedFont);
                PointF textPosition = new PointF((this.ClientSize.Width - textSize.Width) / 2, (this.ClientSize.Height - textSize.Height) / 2);
                g.DrawString(pausedText, pausedFont, Brushes.White, textPosition);
            }
		}

		private void DrawLives(Graphics g)
		{
			Font livesFont = new Font("Consolas", 14);
			string liveText = $"{gameManager.Lives} ";
            SizeF textSize = g.MeasureString(liveText, livesFont);
			PointF livesPosition = new PointF(this.ClientSize.Width - textSize.Width - 10, 10); //Right and top side padding
			g.DrawString(liveText, livesFont, Brushes.White, livesPosition);
        }

		private void Drawscore(Graphics g)
		{
			Font scoreFont = new Font("Conslas", 14);
			string scoreText = $"{gameManager.Score}";
			SizeF textSize = g.MeasureString(scoreText, scoreFont);
			PointF scorePosition = new PointF(10, 10);
			g.DrawString(scoreText, scoreFont, Brushes.White, scorePosition);
		}
        private static void DrawRoundedRectangle(Graphics g, Brush brush, int x, int y, int width, int height, int radius)
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

		//Loop to draw the whole wall of bricks
		private void InitializeBricks()
		{
            Brush[] rowColors = { Brushes.Red, Brushes.Pink, Brushes.Orange, Brushes.Yellow, Brushes.Green,
				Brushes.Navy, Brushes.Purple, Brushes.Blue};

			int[] pointsPerRow = { 8, 7, 6, 5, 4, 3, 2, 1 };

            int rows = 8;
			int cols = 14;
			int brickWidth = 62;
			int brickHeight = 30;
			int paddingX = 4;
			int paddingY = 4;
			int marginX = 4;
			int marginY = 70;

			//Create 2D array
			bricks = new Brick[rows, cols];

            for (int row = 0; row < rows; row++)
			{
				for (int col = 0; col < cols; col++)
				{
					int x = marginX + col * (brickWidth + paddingX);
					int y = marginY + row * (brickHeight + paddingY);

					Brush brushForThisRow = rowColors[row];


					bricks[row, col] = new Brick
					{
						BrickX = x,
						BrickY = y,
						BrickWidth = brickWidth,
						BrickHeight = brickHeight,
						Brush = brushForThisRow,
						PointValue = pointsPerRow[row],
					};
				}
			}
		}

		private void DrawBricks(Graphics g)
		{
			for (int row = 0; row < bricks.GetLength(0); row++)
			{
				for (int col = 0; col < bricks.GetLength(1); col++)
				{
					Brick brick = bricks[row, col];

					if (brick != null && !brick.IsDestroyed)
					{
						brick.Draw(g);
					}
				}
			}
		}
	}
}
