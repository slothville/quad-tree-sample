using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using System.Diagnostics;

namespace basicCollisions
{
	public class Observer
	{
		public Texture2D Texture { get; set; }
		private List<Square> squares;
		private Rectangle QuadTreeMaster;

		private QuadTree quad;
		List<Rectangle> quads;


//		List<Rectangle> intersections;


		public int MarginX
		{
			get{ return Program.Game.GraphicsDevice.Viewport.Width;}
		}

		public int MarginY
		{
			get{ return Program.Game.GraphicsDevice.Viewport.Height;}
		}


		public Observer (Texture2D texture)
		{
			this.Texture = texture;
			QuadTreeMaster = new Rectangle (0,0,this.MarginX,this.MarginY);
			squares = new List<Square> ();
			quad = new QuadTree(0, QuadTreeMaster);
			quads = new List<Rectangle> ();
//			intersections = new List<Rectangle> ();


		}


		public void AddSquare()
		{

			squares.Add (new Square (this.Texture));

		}

		public void Update(GameTime gameTime)
		{
//			if(this.intersections.Count>0)
//			{
//			this.intersections.Clear ();
//			}

			if(squares.Count > 0)
			{

				quad.Clear ();
				foreach(Square square in squares)
				{
					if(square!=null)
					{
					quad.Insert (square.square);
					}
				}

				quads.Clear ();
				quads = quad.GetQuads ();

				foreach(Square square in squares)
				{
					square.CheckBounds(QuadTreeMaster);
				}
				squares.ForEach (x=>{x.CheckBounds(QuadTreeMaster);});
//				squares.ForEach (x=>{x.CheckCollisions(squares);});

					

				foreach(Square square in squares)
				{
					List<Rectangle> cols = quad.Retrieve (square.square).ToList();
//					ICollection<Rectangle> cols = quad.Retrieve (square.square);

					if(cols.Count>0)
					{
						square.CheckCollisions (cols);

//						if(square.intersections.Count>0)
//						{
//							this.intersections.AddRange (square.intersections);
//						}

					}
					
				}

				squares.ForEach (x=>{x.FixDirection();});
				squares.ForEach (x=>{x.Move(gameTime.ElapsedGameTime.TotalMilliseconds);});
				//squares.ForEach (x=>{x.Move();});


			


			}
		

		}

		public void Draw(SpriteBatch spriteBatch)
		{


			if(squares.Count > 0)
			{
				if(quads.Count>0)
				{

					foreach(Rectangle quad in quads.OrderByDescending(x=>x.Width))
					{


							//spriteBatch.DrawBorders(Texture,quad,Color.White*0.0f);
						
						

					}
				}

				foreach(Square square in this.squares)
				{
					//spriteBatch.DrawBorders (this.Texture,square.square,Color.Orange);
					//square.Draw (spriteBatch);
					spriteBatch.Draw(this.Texture,square.square,Color.White);

				}

//				if(this.intersections.Count>0){
//					foreach(Rectangle intersection in intersections)
//					{
//						spriteBatch.Draw (Texture,intersection,Color.Red);
//					}
//				}



//				foreach(Rectangle controlpartner in this.controlPartners)
//				{
//					spriteBatch.Draw (Texture, controlpartner, Color.Red);
//				}
//
//				spriteBatch.Draw (Texture, this.controlRectangle.square, Color.Violet);
			
			}

		}
	}
}

