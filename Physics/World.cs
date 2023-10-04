using System;
using System.Collections.Generic;
using System.Linq;
using GreenTrutle_crossplatform.interfaces;
using GreenTrutle_crossplatform.scene;
using GreenTrutle_crossplatform.scene.Objects;
using Microsoft.Xna.Framework;

namespace GreenTrutle_crossplatform.Physics;

public class World
{
    public static Dictionary<Vector2, bool> grid = new Dictionary<Vector2, bool>();
    public static Rectangle tileSize = new Rectangle(0, 0, 2, 2);
    public static Vector2 worldScordsToTile(Vector2 position)
    {
        float scaleX = Globals.gameSize.X / (Globals.gameSize.X/tileSize.Width);
        float scaleY = Globals.gameSize.Y / (Globals.gameSize.Y/tileSize.Height);
        // Divide the world position by the size of a tile to get the tile coordinates
        // Vector2 tilePos = (position + new Vector2(1, 1)) / new Vector2(scaleX,scaleY);
        Vector2 tilePos = position / new Vector2(scaleX,scaleY);
        // Round the tile position to the nearest integer
        tilePos.X = (int)Math.Ceiling(tilePos.X);
        tilePos.Y = (int)Math.Ceiling(tilePos.Y);
        return tilePos;
    }

    public static void getObjectTiles(DrawableGameObject o)
    {
        
    }
    
    public static Dictionary<Vector2,bool> CreateGrid(object o, Dictionary<string, object> args)
    {
        Dictionary<Vector2,bool> grid = new Dictionary<Vector2,bool>();
        for (int x = 0; x < Globals.gameSize.X; x+=2)
        {
            for (int y = 0; y < Globals.gameSize.Y; y+=2)
            {
                Vector2 pos = new Vector2(x, y)+new Vector2(tileSize.Width/2, tileSize.Height/2);
                IParticle tile = (IParticle)new Wall();
                tile.position = pos;
                tile.aabb = new Rectangle(0, 0, 2, 2);
                grid[worldScordsToTile(pos)] = PhysicsEngine.quadTree.searchAll(tile).OfType<IStatic>().Any();
                // // Create additional nodes in the middle and edges of the cell
                // grid[x, y].center_node = new Node(grid[x, y].position + new Vector2(0.5f,0.5f));
                // grid[x, y].top_edge_middle_node = new Node(grid[x, y].position + new Vector2(0.25f,0.0f));
                // grid[x, y].left_edge_middle_node = new Node(grid[x, y].position + new Vector2(0.25f,0.0f));
                // // Calculate movement costs between nodes
                // grid[x, y].center_node.gCost *= 0.5f;
                // grid[x, y].top_edge_middle_node.gCost *= 0.5f;
                // grid[x, y].left_edge_middle_node.gCost *= 0.5f;
            }
        }

        return grid;
    }

    public static Vector2 tileToWorldCords(Vector2 tilePos)
    {
        Vector2 tileSize = new Vector2(World.tileSize.Width, World.tileSize.Width);
        return (tilePos * tileSize) - (tileSize/ 2);
    }
}