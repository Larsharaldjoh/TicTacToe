using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicTacToe
{
	class Square
	{
		public Rectangle rectangle;
		public int squareIndex;
		public bool marked;
		public bool player1;

		public Square(int x, int y, int height, int width, int squareNum)
		{
			squareIndex = squareNum;
			rectangle = new Rectangle(x, y, height, width);
		}

	}
}
