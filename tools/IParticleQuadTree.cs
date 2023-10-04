using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GreenTrutle_crossplatform.interfaces;
using Microsoft.Xna.Framework;
using Vector2 = System.Numerics.Vector2;

namespace GreenTrutle_crossplatform.tools;

public class QuadTree
{
    private int capacity;
    private Rectangle bound;
    private int x;
    private int y;
    private int width;
    private int height;
    private List<IParticle> particles;
    private bool devided = false;
    private Dictionary<String, QuadTree> subTrees;
    private string lUp = "lUp";
    private string rUp = "rUp";
    private string lDown = "lDown";
    private string rDown = "rDown";

    public QuadTree(Rectangle bound,int capacity) {
        this.bound = bound;
        this.x=this.bound.X;
        this.y=this.bound.Y;
        this.width = this.bound.Width;
        this.height = this.bound.Height;
        this.particles = new List<IParticle>();
        this.capacity = capacity;
        this.devided = false;
        subTrees = new Dictionary<string, QuadTree>();
    }
    

    public void addPoint(IParticle particle){
        if(!this.isInbound(particle.getRect()))
            return;
        if(!this.isFull()){
            particles.Add(particle); 
        }else {
            if (!this.devided) {
                this.devide();
            }
            subTrees[lUp].addPoint(particle);
            subTrees[rUp].addPoint(particle);
            subTrees[lDown].addPoint(particle);
            subTrees[rDown].addPoint(particle);
        }
    }

    private void devide() {
        subTrees.Add(lUp,new QuadTree(new Rectangle(x,y, (int)Math.Ceiling(width/2f),(int)Math.Ceiling(height/2f)),capacity));

        subTrees.Add(rUp,new QuadTree(new Rectangle(x+width/2,y, (int)Math.Ceiling(width/2f),(int)Math.Ceiling(height/2f)),capacity));

        subTrees.Add(lDown,new QuadTree(new Rectangle(x,y+height/2, (int)Math.Ceiling(width/2f),(int)Math.Ceiling(height/2f)),capacity));

        subTrees.Add(rDown,new QuadTree(new Rectangle(x+width/2,y+height/2, (int)Math.Ceiling(width/2f),(int)Math.Ceiling(height/2f)),capacity));
        devided = true;
    }

    private bool isFull() {
        return particles.Count == capacity;
    }

    private bool isInbound(Rectangle point)
    {
        return bound.Intersects(point);
    }



    public List<IParticle> search(IParticle obj){
        List<IParticle> arr = new List<IParticle>();
        if(!bound.Intersects(obj.getRect()))
            return arr;
        for (int i = 0; i < particles.Count; i++) {
            if(particles[i].getRect().Intersects(obj.getRect())&&particles[i]!=obj)
                arr.Add(this.particles[i]);
            if(arr.Count > 0)
                return arr;
        }
        if((this.devided))
        {
            arr.AddRange(subTrees[rUp].search(obj));
            arr.AddRange(subTrees[lUp].search(obj));
            arr.AddRange(subTrees[rDown].search(obj));
            arr.AddRange(subTrees[lDown].search(obj));
        }
        return arr;
    }    
    public List<IParticle> searchWithRect(IParticle obj,Rectangle rect){
        List<IParticle> arr = new List<IParticle>();
        if(!bound.Intersects(rect))
            return arr;
        for (int i = 0; i < particles.Count; i++) {
            if(particles[i].getRect().Intersects(obj.getRect())&&particles[i]!=obj)
                arr.Add(this.particles[i]);
            if(arr.Count > 0)
                return arr;
        }
        if((this.devided))
        {
            arr.AddRange(subTrees[rUp].searchWithRect(obj, rect));
            arr.AddRange(subTrees[lUp].searchWithRect( obj, rect));
            arr.AddRange(subTrees[rDown].searchWithRect( obj, rect));
            arr.AddRange(subTrees[lDown].searchWithRect( obj, rect));
        }
        return arr;
    }
    public List<IParticle> searchAll(IParticle obj){
        List<IParticle> arr = new List<IParticle>();
        if(!bound.Intersects(obj.getRect()))
            return arr;
        for (int i = 0; i < particles.Count; i++) {
             if(particles[i].getRect().Intersects(obj.getRect())&&particles[i]!=obj)
                arr.Add(this.particles[i]);
        }
        if((this.devided))
        {
            arr.AddRange(subTrees[rUp].searchAll(obj));
            arr.AddRange(subTrees[lUp].searchAll(obj));
            arr.AddRange(subTrees[rDown].searchAll(obj));
            arr.AddRange(subTrees[lDown].searchAll(obj));
        }
        return arr;
    }
}
