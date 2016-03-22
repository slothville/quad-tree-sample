#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

#endregion

namespace basicCollisions
{

	
	public class GameScreen
	{
			
		protected ContentManager content;
		
		[XmlIgnore]
		public Type Type;
		
		public GameScreen()
		{
			Type=this.GetType();
		}
		
		#region Game Methods
		
		
		
		public virtual void LoadContent (ContentManager Content)
		{
			
			this.content=new ContentManager(Content.ServiceProvider,"Content");
			
		}
		
		public virtual void UnloadContent ()
		{
			content.Unload();
		}
		
		public virtual void Update (GameTime gameTime){}
		
		public virtual void Draw (SpriteBatch spriteBatch){}

		//public virtual void HandleInput(GameTime gameTime, InputState input) { }
		
		#endregion		
		
	}
}

