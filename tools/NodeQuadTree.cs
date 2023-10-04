using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GreenTrutle_crossplatform.tools;

public class NodeQuadTree
{
    //The node data in this quad
        public List<Node> nodes;
        //The rectangle that bounds this quad
        public Rectangle bounds;
        public int capacity = 4;
        //The four child quads
        public NodeQuadTree LATERAL_LEFT_TOP, LATERAL_RIGHT_TOP, LATERAL_LEFT_BOTTOM, LATERAL_RIGHT_BOTTOM;

        //The constructor
        public NodeQuadTree(Rectangle bounds)
        {
            this.bounds = bounds;
            nodes = new List<Node>();
        }

        public void fillQuadTree(Node[,] grid)
        {
            foreach (var node in  grid)
            {
                insert(node);
            }
        }

        //Insert a node in the quad
        public void insert(Node node)
        {
            //If this quad does not have a node and the node fits in this quad
            if (nodes.Count!=capacity && bounds.Contains(node.position))
            {
                nodes.Add(node);
                return;
            }

            //If this quad does have a node or the node does not fit in this quad
            if (nodes.Count==capacity || !bounds.Contains(node.position))
            {
                //If we do not yet have child quads, subdivide
                if (LATERAL_LEFT_TOP == null)
                {
                    subdivide();
                }

                //Insert the node in the appropriate child
                if (LATERAL_LEFT_TOP.bounds.Contains(node.position))
                {
                    LATERAL_LEFT_TOP.insert(node);
                }
                if (LATERAL_RIGHT_TOP.bounds.Contains(node.position))
                {
                    LATERAL_RIGHT_TOP.insert(node);
                }
                if (LATERAL_LEFT_BOTTOM.bounds.Contains(node.position))
                {
                    LATERAL_LEFT_BOTTOM.insert(node);
                }
                if (LATERAL_RIGHT_BOTTOM.bounds.Contains(node.position))
                {
                    LATERAL_RIGHT_BOTTOM.insert(node);
                }
            }
        }

        //Divide the quad into four child quads
        private void subdivide()
        {
            float halfWidth = bounds.Width / 2;
            float halfHeight = bounds.Height / 2;

            LATERAL_LEFT_TOP = new NodeQuadTree(new Rectangle(bounds.X, bounds.Y, (int)Math.Ceiling(halfWidth), (int)Math.Ceiling(halfHeight)));
            LATERAL_RIGHT_TOP = new NodeQuadTree(new Rectangle(bounds.X + (int)halfWidth, bounds.Y, (int)Math.Ceiling(halfWidth), (int)Math.Ceiling(halfHeight)));
            LATERAL_LEFT_BOTTOM = new NodeQuadTree(new Rectangle(bounds.X, bounds.Y + (int)halfHeight, (int)Math.Ceiling(halfWidth), (int)Math.Ceiling(halfHeight)));
            LATERAL_RIGHT_BOTTOM = new NodeQuadTree(new Rectangle(bounds.X + (int)halfWidth, bounds.Y + (int)halfHeight, (int)Math.Ceiling(halfWidth), (int)Math.Ceiling(halfHeight)));
        }

        //Query the quad for all nodes in a rectangle
        public List<Node> search(Rectangle queryRect)
        {
            List<Node> nodes = new List<Node>();

            //If the query rectangle does not intersect with this quad, return an empty list
            if (!bounds.Intersects(queryRect))
            {
                return nodes;
            }

            //If this quad has a node and the query rectangle contains it, add it to the list
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    if (queryRect.Contains(node.position)) ;
                        nodes.Add(node);
                }
            }

            //If we do not yet have child quads, return the list
            if (LATERAL_LEFT_TOP == null)
            {
                return nodes;
            }

            //Query the appropriate child quads
            nodes.AddRange(LATERAL_LEFT_TOP.search(queryRect));
            nodes.AddRange(LATERAL_RIGHT_TOP.search(queryRect));
            nodes.AddRange(LATERAL_LEFT_BOTTOM.search(queryRect));
            nodes.AddRange(LATERAL_RIGHT_BOTTOM.search(queryRect));

            return nodes;
        }
}


