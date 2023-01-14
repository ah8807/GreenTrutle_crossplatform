using GreenTrutle_crossplatform.interfaces;
using GreenTrutle_crossplatform.scene;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Fluids;

namespace GreenTrutle_crossplatform.Physics
{
    internal class PhysicsEngine : DrawableGameComponent
    {
        Level level;
        public PhysicsEngine(Level level) : base(Globals.game){
            this.level = level;
            createWorld();
        }
        public void createWorld()
        {
            World.grid = new int[135, 240];
            Globals.levels.Add(new Level());
            var lines = File.ReadAllLines("../../../labirinth.txt");
            for (int i = 0; i < 135; i++)
            {
                for (int j = 0; j < 240; j++)
                {
                    String line = lines[i].Replace(" ", "");
                    if (line[j] == 49)
                        World.grid[i, j] = 1;
                    else
                        World.grid[i, j] = 0;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 oldPosition = Vector2.One;
            foreach (DrawableGameObject o in level.scene)
            {
                if(o is IMovable)
                {
                    IMovable movable = (IMovable)o;
                    oldPosition = new Vector2(movable.position.X, movable.position.Y);
                    movable.position = Globals.calcMovment(movable, gameTime);
                }
                IParticle particle1 = o is IParticle ? (IParticle)o : null;
                //reši kolizijo z labirintom
                if (particle1 != null)
                {
                    if (World.collision(particle1))
                    {
                        ICustomCollider part1= o is ICustomCollider ? (ICustomCollider)o : null;
                        if (part1!=null && part1.collidingWithItem(new World())){
                            resolve(particle1);
                        }
                    }

                }
                //reši kolizijo z ostalimi elementi
                foreach (DrawableGameObject obj2 in level.scene)
                {
                    IParticle particle2 = obj2 is IParticle ? (IParticle)obj2 : null;

                    if (particle1 != null && particle2 != null && !particle1.Equals(particle2))
                    {
                        if (collision(particle1, particle2))
                        {
                            ICustomCollider customCollider1 = o is ICustomCollider ? (ICustomCollider)o : null;
                            ICustomCollider customCollider2 = obj2 is ICustomCollider ? (ICustomCollider)obj2 : null;

                            if (customCollider1 != null )
                            {
                                if(customCollider1.collidingWithItem(particle2))
                                    resolveCollision(particle1, particle2);
                                customCollider1.collidedWithItem(particle2);
                            }
                            if (customCollider2 != null)
                            {
                                if (customCollider2.collidingWithItem(particle2))
                                    resolveCollision(particle1, particle2);
                                customCollider2.collidedWithItem(particle1);
                            }
                            if(customCollider1==null && customCollider2 == null)
                            {
                                resolveCollision(particle1, particle2);
                            }
                        }
                    }
                }

            }

                base.Update(gameTime);
        }

        private void resolveCollision(IParticle particle1, IParticle particle2)
        {
            
        }

        private void resolve(IParticle particle)
        {
            while (World.collision(particle))
            {
                particle.position = particle.position - Globals.getDirection(particle.velocity);
            }
        }

        private Boolean collision(IParticle particle1, IParticle particle2)
        {
            Rectangle rect1 = particle1.getRect();
            Rectangle rect2 = particle2.getRect();

            return (rect1.X < rect2.Width && rect1.Width > rect2.X &&
    rect1.Y < rect2.Height && rect1.Height > rect2.Y);
        }
    }
}
