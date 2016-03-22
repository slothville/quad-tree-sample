#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

#endregion

namespace basicCollisions
{
	public class PlayScreen:GameScreen
	{

		Texture2D Texture;
		Observer observer;
		bool EnterDown =false;
		public int WindowHeight{get;set;}
		public int WindowWidth{get;set;}



		public PlayScreen()
        {
			
			WindowWidth=Program.Game.GraphicsDevice.Viewport.Width;
			WindowHeight=Program.Game.GraphicsDevice.Viewport.Height;

        }
		
		
		
		public override void LoadContent (ContentManager Content)
		{
			base.LoadContent(Content);
			
			if(Texture==null)
			{
			Texture=content.Load<Texture2D>("dragonball");
			}
				
			observer = new Observer (Texture);

			
	
		}
		
		public override void UnloadContent ()
		{
			base.UnloadContent();
		}
		
		public override void Update (GameTime gameTime)
		{

			if(Keyboard.GetState ().IsKeyDown (Keys.Enter) && !EnterDown)
			{
				EnterDown = true;
				observer.AddSquare ();
			}else if(!Keyboard.GetState ().IsKeyDown (Keys.Enter))
			{
				EnterDown = false;
			}

			observer.Update (gameTime);
		}
		
		public override void Draw (SpriteBatch spriteBatch)
		{
				
			observer.Draw (spriteBatch);
		}

	}
}


