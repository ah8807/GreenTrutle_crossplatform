using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using GreenTrutle_crossplatform.interfaces;
using GreenTrutle_crossplatform.Physics;
using Microsoft.Xna.Framework;

namespace GreenTrutle_crossplatform.scene.Objects;

public class Wall: DrawableGameObject, IParticle, ICustomCollider,IStatic, ICombinedGameObject
{
        public List<DrawableGameObject> game_objects { get; set; }
        private bool? vertical;
        
        public Rectangle aabb { get; set; }

        public Wall()
        {
            game_objects = new List<DrawableGameObject>();
            vertical = null;
        }

        public bool AddBrick(Brick brick)
        {
            if (!vertical.HasValue)
            {
                if (!game_objects.Any())
                {
                    game_objects.Add(brick);
                    aabb = brick.aabb;
                    position = brick.position;
                    return true;
                }

                vertical = game_objects[0].position.X == brick.position.X;
                if (canAdd(brick))
                {
                game_objects.Add(brick);
                GetBoundingBox();
                return true;
                }
                vertical = null;
                return false;
            }
            if (vertical.Value)
            {
                if (brick.position.X == game_objects[0].position.X&&canAdd(brick))
                {
                    game_objects.Add(brick);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (brick.position.Y == game_objects[0].position.Y&&canAdd(brick))
                {
                    game_objects.Add(brick);
                }
                else
                {
                    return false;
                }
            }
            GetBoundingBox();
            return true;
        }

        private bool canAdd(Brick brick)
        {
            Rectangle rect = (brick as IParticle).getRect();
            Rectangle boundingBox = (this as IParticle).getRect();
            if ((bool)vertical)
            {

                if (boundingBox.Top == rect.Bottom || boundingBox.Bottom == rect.Top)
                {
                    return true;
                }

                return false;
            }
            if (brick.position.Y==this.position.Y && (boundingBox.Right == rect.Left || boundingBox.Left == rect.Right))
            {
                return true;
            }

            return false;
            
        }

        public void RemoveBrick(Brick brick)
        {
            game_objects.Remove(brick);
        }

        public void GetBoundingBox()
        {
            if (vertical.HasValue)
            {
                game_objects.Sort((a, b) =>
                    {
                        if ((bool)vertical)
                            return ((a as IPosition).position.Y.CompareTo((b as IPosition).position.Y));
                        return ((a as IPosition).position.X.CompareTo((b as IPosition).position.X));
                    });
                aabb = new Rectangle(0,0,!(bool)vertical?(game_objects[0] as IParticle).aabb.Width * game_objects.Count: (game_objects[0] as IParticle).aabb.Width ,
                    (bool)vertical? (game_objects[0] as IParticle).aabb.Height * game_objects.Count: (game_objects[0] as IParticle).aabb.Height);
                float y=0;
                float x=0;
                foreach (var brick in game_objects)
                {
                    x += brick.position.X;
                    y += brick.position.Y;
                }
                position = new Vector2(x/game_objects.Count, y/game_objects.Count);
            } 
        }
        public Wall CreateWall(List<object> objects)
        {
            for (int i = 0; i < 3; i++)
            {
                if (vertical.HasValue)
                {
                    if(i<=1)
                    objects.Sort((a, b) =>
                    {
                        if ((bool)vertical)
                            return ((a as IPosition).position.Y.CompareTo((b as IPosition).position.Y));
                        return ((a as IPosition).position.X.CompareTo((b as IPosition).position.X));
                    });
                    else
                    {
                        objects.Sort((a, b) =>
                        {
                            if ((bool)vertical)
                                return ((b as IPosition).position.Y.CompareTo((a as IPosition).position.Y));
                            return ((b as IPosition).position.X.CompareTo((a as IPosition).position.X));
                        });
                    }
                }

                foreach (var obj in objects.ToList())
                {
                    if (obj is Brick)
                    {
                        if (AddBrick((Brick)obj))
                            objects.Remove(obj);
                    }
                }
            }

            return this;
        }

        public bool collidingWithItem(object item)
        {
            return false;
        }

        public void collidedWithItem(object item)
        {

        }

}