using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace basicCollisions
{
	public class QuadTree
	{
	
		private int MaxLevel = 4;
		private int MaxItems = 3;
		private int Level;
		public Rectangle Bounds;
		private QuadTree[] Nodes;
		private QuadTree[] refNodes;
		private List<Rectangle> Items;
		//private List<Rectangle> quadList;
		//private HashSet<Rectangle> mayCollide;

		private bool HasChilds
		{
			get
			{
				return Nodes[0]!=null;
			}
		}

		private bool HadChilds
		{
			get
			{
				return refNodes[0]!=null;
			}
		}

		public QuadTree (int parentLevel,Rectangle parentBounds)
		{
			this.Level = parentLevel;
			this.Bounds = parentBounds;
			this.Nodes = new QuadTree[4];
			this.refNodes = new QuadTree[4];
			this.Items = new List<Rectangle> ();

		}
			

		public void Clear()
		{
			this.Items.Clear ();


			try
			{

			for(int i=0;i<this.Nodes.Length;i++)
			{
				if(this.Nodes[i] != null)
				{
					this.Nodes [i].Clear ();
					this.Nodes [i] = null;	
				}
			}
			}catch(Exception e)
			{
				Console.WriteLine (e.ToString());
			}
		}
			
		private void Split()
		{

			//Right UP
			this.Nodes[0]= new QuadTree(this.Level+1,new Rectangle(this.Bounds.X+this.Bounds.Width/2,this.Bounds.Y,this.Bounds.Width/2,this.Bounds.Height/2));
			//Left UP
			this.Nodes[1]= new QuadTree(this.Level+1,new Rectangle(this.Bounds.X,this.Bounds.Y,this.Bounds.Width/2,this.Bounds.Height/2));
			//Left DOWN
			this.Nodes[2]= new QuadTree(this.Level+1,new Rectangle(this.Bounds.X,this.Bounds.Y+this.Bounds.Height/2,this.Bounds.Width/2,this.Bounds.Height/2));
			//Right DOWN
			this.Nodes[3]= new QuadTree(this.Level+1,new Rectangle(this.Bounds.X+this.Bounds.Width/2,this.Bounds.Y+this.Bounds.Height/2,this.Bounds.Width/2,this.Bounds.Height/2));

		}

		private void Split2()
		{
			if (!this.HadChilds) {
				//Right UP
				this.Nodes [0] = this.refNodes[0] = new QuadTree (this.Level + 1, new Rectangle (this.Bounds.X + this.Bounds.Width / 2, this.Bounds.Y, this.Bounds.Width / 2, this.Bounds.Height / 2));
				//Left UP
				this.Nodes [1] = this.refNodes[1] = new QuadTree (this.Level + 1, new Rectangle (this.Bounds.X, this.Bounds.Y, this.Bounds.Width / 2, this.Bounds.Height / 2));
				//Left DOWN
				this.Nodes [2] = this.refNodes[2] = new QuadTree (this.Level + 1, new Rectangle (this.Bounds.X, this.Bounds.Y + this.Bounds.Height / 2, this.Bounds.Width / 2, this.Bounds.Height / 2));
				//Right DOWN
				this.Nodes [3] = this.refNodes[3] = new QuadTree (this.Level + 1, new Rectangle (this.Bounds.X + this.Bounds.Width / 2, this.Bounds.Y + this.Bounds.Height / 2, this.Bounds.Width / 2, this.Bounds.Height / 2));
			
			} else if(this.HadChilds){
			
				this.Nodes [0] = this.refNodes [0];
				this.Nodes [1] = this.refNodes [1];
				this.Nodes [2] = this.refNodes [2];
				this.Nodes [3] = this.refNodes [3];

			}
		}

		private IEnumerable<int> GetIndex(Rectangle item) 
		{
			int[] index = new int[4]{-1,-1,-1,-1};


			int verticalMidpoint = this.Bounds.Center.X;
			int horizontalMidpoint = this.Bounds.Center.Y;

			// Object can completely fit within the top quadrants
			bool topQuadrant = item.Bottom < horizontalMidpoint;
			// Object can completely fit within the bottom quadrants
			bool bottomQuadrant = item.Top > horizontalMidpoint;

			bool leftQuadrant = item.Right < verticalMidpoint;
			bool rightQuadrant = item.Left > verticalMidpoint;

			bool bottonAndTop = (item.Top < horizontalMidpoint) && (item.Bottom > horizontalMidpoint);
			bool leftAndRight = (item.Left < verticalMidpoint) && (item.Right > verticalMidpoint);

			if(bottonAndTop && leftAndRight)
			{
				for(int i=0;i<index.Length;i++)
				{
					index [i] = i;
				}
			}else if(bottonAndTop && !leftAndRight)
			{
				if(leftQuadrant)
				{
					index [0] = 1;
					index [1] = 2;
				}
				else if(rightQuadrant)
				{
					index [0] = 0;
					index [1] = 3;
				}
			}else if(!bottonAndTop && leftAndRight)
			{
				if(topQuadrant)
				{
					index [0] = 0;
					index [1] = 1;
				}
				else if(bottomQuadrant)
				{
					index [0] = 2;
					index [1] = 3;
				}
			}else if(!bottonAndTop && !leftAndRight)
			{
				if(leftQuadrant)
				{
					if (topQuadrant) {
						index [0] = 1;
					} else if (bottomQuadrant) {
						index [0] = 2;
					}

				}else if(rightQuadrant)
				{
					if (topQuadrant) {
						index [0] = 0;
					} else if (bottomQuadrant) {
						index [0] = 3;
					}

				}
			}

			return index.Where(x=> x!=-1);
		}
			
		public void Insert(Rectangle item)
		{

			try
			{

				if (this.HasChilds) 
				{

					this.Items.Add(item);

					int i= this.Items.Count-1;

					while(this.Items.Count > 0)
					{

						IEnumerable<int> index = this.GetIndex (this.Items[i]);


						foreach(int j in index)
						{
							this.Nodes[j].Insert(this.Items[i]);
						}
						this.Items.RemoveAt(i);
						i--;

					}


				} if(!this.HasChilds)
				{
					this.Items.Add(item);

					if (this.Items.Count > this.MaxItems && this.Level < this.MaxLevel) 
					{

						this.Split();


						int i= this.Items.Count-1;

						while(this.Items.Count > 0)
						{

							IEnumerable<int> index = this.GetIndex (this.Items[i]);


							foreach(int j in index)
							{
								this.Nodes[j].Insert(this.Items[i]);
							}
							this.Items.RemoveAt(i);
							i--;

						}


					}

				}


			}catch(Exception e)
			{
				Console.WriteLine (e.ToString ());
			}
		}
			
		public IEnumerable<Rectangle> Retrieve(Rectangle item)
		{
			HashSet<Rectangle> mayCollide = new HashSet<Rectangle> ();

			this.RetrieveHelper (mayCollide,item);

			return mayCollide.Where(x=> !x.Equals(item));
		}

		private void RetrieveHelper(HashSet<Rectangle> items,Rectangle item)
		{

			if (!this.HasChilds) 
			{

				items.UnionWith (this.Items);

			} 
			else if(this.HasChilds)
			{
				IEnumerable<int> index = this.GetIndex (item);

				foreach(int i in index)
				{
					this.Nodes [i].RetrieveHelper (items, item);
				}

			}

		}

		public List<Rectangle> GetQuads()
		{

			List<Rectangle> quadList = new List<Rectangle> ();

//			if (this.quadList == null) {
//				this.quadList = new List<Rectangle> ();
//			} else 
//			{
//				this.quadList.Clear ();
//			}

			this.AddQuads (quadList);
			return quadList;
		}

		private void AddQuads(List<Rectangle> list)
		{

			if(this.HasChilds)
			{
				foreach(QuadTree node in this.Nodes)
				{
					node.AddQuads (list);
				}
			}

			list.Add (this.Bounds);
		}
			
	
	}
}

