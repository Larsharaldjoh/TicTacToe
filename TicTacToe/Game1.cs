using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace TicTacToe
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		SpriteFont font;

		Texture2D gridTex;
		Color gridColor;
		Vector2 gridSize;
		Point squareSize;
		List<Square> squares;

		bool playerOne;
		int hover;
		bool playerOneOwns;
		bool marked;
		bool gameOver;
		bool playerOneWins;
		MouseState mouseState;
		List<int> xSquares;
		List<int> oSquares;


		private const float _delay = 100; // milliseconds
		private float _remainingDelay = _delay;


		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			graphics.IsFullScreen = false;
			Window.IsBorderless = false;
			graphics.PreferredBackBufferWidth = 800;
			graphics.PreferredBackBufferHeight = 600;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{


			IsMouseVisible = true;
			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			font = Content.Load<SpriteFont>("Roboto");

			playerOne = true;
			xSquares = new List<int>();
			oSquares = new List<int>();

			squares = new List<Square>();
			squareSize = new Point(GraphicsDevice.Viewport.Width/3, GraphicsDevice.Viewport.Height/3);
			gridSize = new Vector2(3, 3);
			gridTex = new Texture2D(GraphicsDevice, 1, 1);
			gridTex.SetData(new Color[] { Color.White });
			gridColor = Color.Black;

			GenerateGrid();
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
				this.Exit();

			if (xSquares.Count > 2 && !gameOver)
			{
				if (xSquares.Contains(7) && xSquares.Contains(5) && xSquares.Contains(3) || xSquares.Contains(9) && xSquares.Contains(5) && xSquares.Contains(1) || 
					xSquares.Contains(9) && xSquares.Contains(8) && xSquares.Contains(7) || xSquares.Contains(6) && xSquares.Contains(5) && xSquares.Contains(4) || 
					xSquares.Contains(3) && xSquares.Contains(2) && xSquares.Contains(1) || xSquares.Contains(7) && xSquares.Contains(4) && xSquares.Contains(1) || 
					xSquares.Contains(8) && xSquares.Contains(5) && xSquares.Contains(2) || xSquares.Contains(9) && xSquares.Contains(6) && xSquares.Contains(3))
				{
					gameOver = true;
					playerOneWins = true;
				}
			} 
			if (oSquares.Count > 2 && !gameOver)
			{
				if (oSquares.Contains(7) && oSquares.Contains(5) && oSquares.Contains(3) || oSquares.Contains(9) && oSquares.Contains(5) && oSquares.Contains(1) || 
					oSquares.Contains(9) && oSquares.Contains(8) && oSquares.Contains(7) || oSquares.Contains(6) && oSquares.Contains(5) && oSquares.Contains(4) || 
					oSquares.Contains(3) && oSquares.Contains(2) && oSquares.Contains(1) || oSquares.Contains(7) && oSquares.Contains(4) && oSquares.Contains(1) || 
					oSquares.Contains(8) && oSquares.Contains(5) && oSquares.Contains(2) || oSquares.Contains(9) && oSquares.Contains(6) && oSquares.Contains(3))
				{
					gameOver = true;
					playerOneWins = false;
				}
			} 


			mouseState = Mouse.GetState();

			Point mousePosition = Mouse.GetState().Position;
			foreach (Square square in squares)
			{
				if (intersects(square.rectangle, mousePosition))
				{
					hover = square.squareIndex;
					marked = square.marked;
					playerOneOwns = square.player1;

					if (mouseState.LeftButton == ButtonState.Pressed && square.marked != true && !gameOver)
					{
						if (playerOne == true)
						{
							xSquares.Add(square.squareIndex);
							square.marked = true;
							square.player1 = playerOne;
							playerOne = false;
						}
						else
						{
							oSquares.Add(square.squareIndex);
							square.marked = true;
							square.player1 = playerOne;
							playerOne = true;
						}
					}
				}
			}


			var timer = (float)gameTime.ElapsedGameTime.Milliseconds;
			_remainingDelay -= timer;
			if (_remainingDelay <= 0)
			{

				_remainingDelay = _delay;
			}



			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			drawGrid();

			cursorCoordinates();

			spriteBatch.Begin();
			spriteBatch.DrawString(font, "Hovering over square: " + hover.ToString(), new Vector2(20, 10), Color.White);
			if (marked)
				if (playerOneOwns)
					spriteBatch.DrawString(font, "Marked by player one", new Vector2(20, 25), Color.White);
				else
					spriteBatch.DrawString(font, "Marked by player two", new Vector2(20, 25), Color.White);
			else
				spriteBatch.DrawString(font, "Not marked", new Vector2(20, 25), Color.White);
			string oSquare = "";
			string xSquare = "";
			foreach(int index in oSquares)
			{
				oSquare += index.ToString();
			}
			foreach(int index in xSquares)
			{
				xSquare += index.ToString();
			}
			spriteBatch.DrawString(font, "X squares: " + xSquare, new Vector2(20, 80), Color.White);
			spriteBatch.DrawString(font, "O squares: " + oSquare, new Vector2(20, 95), Color.White);

			if (gameOver)
				if (playerOneWins)
					spriteBatch.DrawString(font, "Player one wins!!!", new Vector2(325, 285), Color.Pink);
				else
					spriteBatch.DrawString(font, "Player two wins!!!", new Vector2(325, 285), Color.Teal);
			spriteBatch.End();


			base.Draw(gameTime);
		}


		private void cursorCoordinates()
		{
			Point mousePosition = Mouse.GetState().Position;
			spriteBatch.Begin();
			spriteBatch.DrawString(font, "X:" + mousePosition.X.ToString(), new Vector2(20, 45), Color.White);
			spriteBatch.DrawString(font, "Y:" + mousePosition.Y.ToString(), new Vector2(20, 60), Color.White);
			spriteBatch.End();

		}

		private void drawGrid()
		{
			spriteBatch.Begin();
			foreach (Square square in squares)
			{
				if (square.marked && square.player1)
					gridColor = Color.Red;
				else if (square.marked && !square.player1)
					gridColor = Color.Blue;
				else
					gridColor = Color.Black;
				
				spriteBatch.Draw(gridTex, square.rectangle, gridColor);
				DrawBorder(square.rectangle, 2, Color.Yellow);
			}
			spriteBatch.End();
		}

		private void DrawBorder(Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor)
		{
			// Draw top line
			spriteBatch.Draw(gridTex, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), borderColor);

			// Draw left line
			spriteBatch.Draw(gridTex, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);

			// Draw right line
			spriteBatch.Draw(gridTex, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder),
											rectangleToDraw.Y,
											thicknessOfBorder,
											rectangleToDraw.Height), borderColor);
			// Draw bottom line
			spriteBatch.Draw(gridTex, new Rectangle(rectangleToDraw.X,
											rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder,
											rectangleToDraw.Width,
											thicknessOfBorder), borderColor);
		}

		private void GenerateGrid()
		{
			int count = 0;
			for (int iy = (int)gridSize.Y - 1; iy > -1; iy--)
			{
				for (int ix = (int)gridSize.X - 1; ix > -1; ix--)
				{
					count++;
					Square tempRec = new Square(ix * squareSize.X, iy * squareSize.Y, squareSize.X, squareSize.Y, count);
					squares.Add(tempRec);
				}
			}
		}

		public bool intersects(Rectangle square, Point point)
		{
			return ((point.X >= square.X && point.X <= square.X + square.Width) && (point.Y >= square.Y && point.Y <= square.Y + square.Height));
		}
	}
}
