using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace basicCollisions
{
	public static class DrawExtensions
	{
		public static void DrawBorders(this SpriteBatch spritebach,Texture2D texture,Rectangle rectangle, Color color)
		{
			int linewidth = 1;
			Rectangle top = new Rectangle (rectangle.X,rectangle.Y,rectangle.Width,linewidth);
			Rectangle bottom = new Rectangle (rectangle.X,rectangle.Bottom-linewidth,rectangle.Width,linewidth);
			Rectangle left = new Rectangle (rectangle.X,rectangle.Y,linewidth,rectangle.Height);
			Rectangle right = new Rectangle (rectangle.Right-linewidth,rectangle.Y,linewidth,rectangle.Height);

			spritebach.Draw (texture, rectangle, color);
			spritebach.Draw (texture, top, Color.Black);
			spritebach.Draw (texture, bottom, Color.Black);
			spritebach.Draw (texture, left, Color.Black);
			spritebach.Draw (texture, right, Color.Black);
		}
	}
}

