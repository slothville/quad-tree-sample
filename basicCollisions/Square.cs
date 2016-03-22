using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using System.Text;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;


namespace basicCollisions
{
	public class Square
	{

		public int WindowHeight{get;set;}
		public int WindowWidth{get;set;}
		public Texture2D Texture { get; set; }
		public int SquareSize { get; set; }
		public int SquareWidth { get; set; }
		public int SquareHeight { get; set; }
		public int DirectionX{ get; set;}
		public int DirectionY{ get; set;}
		public Rectangle square;
		

		private int y0;
		private int x0;
		Random rnd;

		private int FixedDirectionX=1;
		private int FixedDirectionY=1;

		public List<Rectangle> intersections;

		/// <summary>
		/// Gets or sets the velocity [px/s].
		/// </summary>
		/// <value>The velocity.</value>
		private Vector2 Velocity{ get; set;}
		private Vector2 Position{ get; set;} 

		private double delta{ get; set;}


		public int MarginX
		{
			get{ return Program.Game.GraphicsDevice.Viewport.Width - SquareSize;}
		}

		public int MarginY
		{
			get{ return Program.Game.GraphicsDevice.Viewport.Height - SquareSize;}
		}

		private  Color Color { get; set;}

		public Square (Texture2D texture)
		{

			Texture=texture;
			Color = Color.Yellow;
			rnd = new Random (unchecked((int) DateTime.Now.Ticks));
			WindowWidth=Program.Game.Window.ClientBounds.Width;
			WindowHeight=Program.Game.Window.ClientBounds.Height;
			SquareSize=25;
			SquareWidth=25;
			SquareHeight=25;
			RandomPlace (this.MarginX,this.MarginY);

			FixedDirectionX = DirectionX = (rnd.Next(0,2)*2)-1;
			FixedDirectionY = DirectionY = (rnd.Next(0,2)*2)-1;
			square = new Rectangle(x0,y0,SquareSize,SquareSize);
			Position = new Vector2 (x0,y0);
			this.intersections = new List<Rectangle> ();
			this.Velocity = new Vector2 (66,66);
			this.delta = 0;

		
		}





		public void Draw(SpriteBatch spriteBatch)
		{
				//spriteBatch.DrawBorders(Texture,square,Color);
				spriteBatch.Draw(Texture,square,Color);
		}

		public void Draw2(SpriteBatch spriteBatch)
		{
			//spriteBatch.DrawBorders(Texture,square,Color);
			spriteBatch.Draw(Texture,this.Position,null,null,
			new Vector2(0,0),0f,new Vector2(SquareWidth,SquareHeight),
			Color,SpriteEffects.None,0f);
		}
			

		private void RandomPlace(int marginx,int marginy)
		{

				x0=(int)rnd.Next(0, marginx);
				y0=(int)rnd.Next (0,marginy);
				while(x0%5 != 0)
				{
					x0=(int)rnd.Next(0, marginx);
				}

				while(y0%5 != 0)
				{
					y0=(int)rnd.Next(0, marginy);
				}
			
		}


		//Check collision with screen boundaries
		public void CheckBounds(Rectangle bounds)
		{

			if (this.square.Left <= bounds.Left) {
				//Left side collision
				FixedDirectionX = 1;

			} else if (this.square.Right >= bounds.Right) {
				//Right side collision
				FixedDirectionX = -1;

			} 

			if (this.square.Top <= bounds.Top) {
				//Top collision
				FixedDirectionY = 1;

			} else if (this.square.Bottom >= bounds.Bottom) {
				//Bottom collision
				FixedDirectionY = -1;
			}


		}
			
		public void CheckCollisions(List<Rectangle> externals)
		{
		
			IEnumerable<Rectangle> quads = externals.Where (x => !this.square.Equals (x)).Where (x => this.square.Intersects (x));

			if(this.intersections.Count>0)
			{
			this.intersections.Clear ();
			}

			if (quads.Any ()) {


				foreach (Rectangle x in quads) {

					this.intersections.Add (Rectangle.Intersect (this.square, x));

					int leftResult = Math.Abs (this.square.Left - x.Right);
					int rightResult = Math.Abs (this.square.Right - x.Left);
					int topResult = Math.Abs (this.square.Top - x.Bottom);
					int bottomResult = Math.Abs (this.square.Bottom - x.Top);
					bool topRight = Math.Abs (topResult - rightResult) == 0;
					bool topLeft = Math.Abs (topResult - leftResult) == 0;
					bool bottomRight = Math.Abs (bottomResult - rightResult) == 0;
					bool bottomLeft = Math.Abs (bottomResult - leftResult) == 0;

					if (leftResult <= this.square.Width && topResult <= this.square.Width) {

						if (leftResult < topResult) {

							FixedDirectionX = 1;

						} else if (topResult < leftResult) {

							FixedDirectionY = 1;

						} else if (topLeft) {

							FixedDirectionX = 1;
							FixedDirectionY = 1;

						}

					} else if (rightResult <= this.square.Width && topResult <= this.square.Width) {

						if (rightResult < topResult) {

							FixedDirectionX = -1;

						} else if (topResult < rightResult) {

							FixedDirectionY = 1;

						} else if (topRight) {

							FixedDirectionX = -1;
							FixedDirectionY = 1;

						}


					} else if (leftResult <= this.square.Width && bottomResult <= this.square.Width) {

						if (leftResult < bottomResult) {

							FixedDirectionX = 1;

						} else if (bottomResult < leftResult) {

							FixedDirectionY = -1;

						} else if (bottomLeft) {

							FixedDirectionX = 1;
							FixedDirectionY = -1;

						}
					} else if (rightResult <= this.square.Width && bottomResult <= this.square.Width) {

						if (rightResult < bottomResult) {

							FixedDirectionX = -1;

						} else if (bottomResult < rightResult) {

							FixedDirectionY = -1;

						} else if (bottomRight) {

							FixedDirectionX = -1;
							FixedDirectionY = -1;

						}

					}

				

				}

			} 


		}

		public void CheckCollisions(List<Square> externals)
		{
		
			List<Rectangle> quads = externals.Select (x=> x.square).ToList();
			this.CheckCollisions (quads);

		}

		//Apply new direction
		public void FixDirection()
		{
			this.DirectionX = FixedDirectionX;
			this.DirectionY = FixedDirectionY;
		}

		//Move with actual values
		public void Move(double elapsedMiliSeconds)
		{

//			elapsedMiliSeconds = (elapsedMiliSeconds >= 10 && elapsedMiliSeconds <= 1000d / 59.5d) ? 1000d / 59.5d : elapsedMiliSeconds;
//			elapsedMiliSeconds = elapsedMiliSeconds >= 1000d / 33d ? 1000d / 60d : elapsedMiliSeconds;
//			this.delta += MathHelper.Clamp((float)elapsedMiliSeconds,0f,1000f /40f);
			this.delta+= elapsedMiliSeconds;
//			Console.WriteLine (delta);

			double X = (this.delta*(double)this.Velocity.X/1000d);

			double Y = (this.delta*(double)this.Velocity.Y/1000d);



			if (X >= 1f) 
			{ 
				

				square.X += (int)X*this.DirectionX;
				square.Y += (int)Y*this.DirectionY;


				this.delta = 0;
			}




		}
	
	}
}

