using GreenTrutle_crossplatform.interfaces;
using GreenTrutle_crossplatform.scene;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;
using GreenTrutle_crossplatform.scene.Objects;
using GreenTrutle_crossplatform.tools;
using Microsoft.VisualBasic.CompilerServices;
using tainicom.Aether.Physics2D.Fluids;

namespace GreenTrutle_crossplatform.Physics
{
    internal class PhysicsEngine : DrawableGameComponent
    {
        public static QuadTree quadTree;
        private Timer updTimer = new Timer(10);
        EmptyLevel level;

        private bool wait=false;
        public PhysicsEngine(EmptyLevel level) : base(Globals.game){
            this.level = level;
            quadTree = getQuadTree(level.scene);
            World.grid=World.CreateGrid(this,null);
            Globals.eventManager.Subscribe("sceneIStaticUpdate",updateGrid);
            updTimer.repeat += (sender,args)=>
            {
                newCollDetection(gameTime, level.scene);
            };
        }

        public void updateGrid(object o, Dictionary<String, object> args)
        {
            if (!wait)
            {
                wait = true;
                Timer waitT = new Timer(2000);
                waitT.oneTime += (o, args) =>
                {
                    // quadTree = getQuadTree(level.scene);
                    // CreateGrid(this,null);
            quadTree = getQuadTree(level.scene);
            Dictionary<Vector2, bool> tmpg;
            Parallel.Invoke(
                () =>
                {
                    tmpg = World.CreateGrid(this, null);
                    World.grid = tmpg;
                });
                    waitT.Close();
                    wait = false;
                };
            }
        }

        public override void Update(GameTime gameTime)
        {
            updTimer.Enabled = this.Enabled;
            this.gameTime = gameTime;
            //oldCollDetection(gameTime);

            base.Update(gameTime);
        }

        public GameTime gameTime { get; set; }

        public void newCollDetection(GameTime gameTime,Scene scene)
        {
            quadTree = getQuadTree(scene);
            List<IParticle> colidedWith;
            foreach (IParticle p in scene)
            {
                // if (p is IStatic||p is ITrigger)
                //     continue;
                Vector2 oldPosition = Vector2.One;
                if (p is IMovable)
                {
                    IMovable movable = (IMovable)p;
                    oldPosition = new Vector2(movable.position.X, movable.position.Y);
                    movable.position = Globals.calcMovment(movable, gameTime);
                }
                
                colidedWith=quadTree.searchAll(p);
                
                foreach (IParticle item in colidedWith)
                {
                    if (item.position == p.position && item.GetType() == p.GetType() && item is IStatic)
                    {
                        scene.removeItem((DrawableGameObject)item);
                    }
                    ICustomCollider customCollider1 = p is ICustomCollider ? (ICustomCollider)p : null;
                    ICustomCollider customCollider2 = item is ICustomCollider ? (ICustomCollider)item : null;

                    if (customCollider1 != null)
                    {
                        if (!(item is ITrigger) && customCollider1.collidingWithItem(item))
                            resolveCollision(p, item);
                        customCollider1.collidedWithItem(item);
                    }

                    if (customCollider2 != null)
                    {
                        if (!(p is ITrigger) &&customCollider2.collidingWithItem(p))
                            resolveCollision(item, p);
                        customCollider2.collidedWithItem(p);
                    }
                    
                    if (customCollider1 == null && customCollider2 == null)
                    {
                        resolveCollision(p, item);
                    }
                    
                }
            }
        }

        public static QuadTree getQuadTree(Scene scene)
        {
            QuadTree quadTree = new QuadTree(new Rectangle(0, 0, (int)Globals.gameSize.X, (int)Globals.gameSize.Y), 4);
            foreach (IParticle p in scene)
            {
                quadTree.addPoint(p);
            }
            
            return quadTree;
        }

        public void resolveCollision(IParticle p, IParticle item)
        {
            Vector2 overlap = GetMinOveralp(p, item);
            
            if (item is IStatic && p is IStatic)
            {
                p.position += overlap;
                p.position = new Vector2((int)p.position.X, (int)p.position.Y);
                return;
            }else if (p is IStatic)
            {
                item.position-= overlap;
                item.position = new Vector2((int)item.position.X, (int)item.position.Y);
                return;
            }else if (item is IStatic)
            {
                p.position += overlap;
                p.position = new Vector2((int)p.position.X, (int)p.position.Y);
                return;
            }
            else
            {
                p.position += overlap / 2;
                item.position -= overlap / 2;
                p.position = new Vector2((int)p.position.X, (int)p.position.Y);
                item.position = new Vector2((int)item.position.X, (int)item.position.Y);
            }

        }
        private Vector2 GetMinOveralp(IParticle particle, IParticle item)
        {
            Rectangle rect1 = particle.getRect();
            Rectangle rect2 = item.getRect();
            
            Vector2 mtv = Vector2.Zero;
            float minOverlap = float.MaxValue;
            
            // Left
            float overlap = (rect1.Right - rect2.Left);
            if (overlap < minOverlap)
            {
                minOverlap = overlap;
                mtv = new Vector2(-overlap, 0);
            }

            // Right
            overlap = (rect2.Right - rect1.Left);
            if (overlap < minOverlap)
            {
                minOverlap = overlap;
                mtv = new Vector2(overlap, 0);
            }

            // Top
            overlap = (rect1.Bottom - rect2.Top);
            if (overlap < minOverlap)
            {
                minOverlap = overlap;
                mtv = new Vector2(0, -overlap);
            }

            // Bottom
            overlap = (rect2.Bottom - rect1.Top);
            if (overlap < minOverlap)
            {
                minOverlap = overlap;
                mtv = new Vector2(0, overlap);
            }

            return mtv;
        }
        

        private Boolean collision(IParticle particle1, IParticle particle2)
        {
            Rectangle rect1 = particle1.getRect();
            Rectangle rect2 = particle2.getRect();

            return rect1.Intersects(rect2);
        }

        protected override void Dispose(bool disposing)
        {
            updTimer.Close();
            base.Dispose(disposing);
        }
    }
}
