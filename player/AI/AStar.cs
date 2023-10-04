using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using GreenTrutle_crossplatform.interfaces;
using GreenTrutle_crossplatform.Physics;
using GreenTrutle_crossplatform.scene;
using GreenTrutle_crossplatform.scene.Objects;
using GreenTrutle_crossplatform.tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Physics2D.Fluids;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace GreenTrutle_crossplatform.player.AI;

public class AStar
{
    public List<Vector2> directions = new List<Vector2>();
    public List<Vector2> wayPoints = new List<Vector2>();
    private DrawableGameObject player;
    private DrawableGameObject ai;
    
    private int gridWidth;
    private int gridHeight;

    // Open and closed lists for A* algorithm
    private List<Node> openList;
    private List<Node> closedList;
    
    public AStar(DrawableGameObject player,DrawableGameObject ai,int width, int height)
    {
        this.player = player;
        this.ai = ai;
        
        gridWidth = width;
        gridHeight = height;
    }
    

    public bool Find(Vector2 playerPosition)
    {
        directions.Clear();
        wayPoints.Clear();
        Vector2 start = World.worldScordsToTile(ai.position);
        Vector2 end = World.worldScordsToTile(playerPosition);
        Node? nodes = newFindPath(start, end);
        if (nodes == null) return false;
        Node tmp = (Node)nodes.Clone();
        List<Vector2> tmp_dir = new List<Vector2>();
        while (tmp != null)
        {
            if(tmp.parent != null)
                tmp_dir.Add(tmp.parent.position-tmp.position); 
            wayPoints.Add(tmp.position);
            tmp = tmp.parent;
        }
        bool init = false;
        Vector2 prev = default(Vector2);
        foreach (Vector2 item in tmp_dir)
        {
            if (!init)
                init = true;
            else if (prev.Equals(item))
                continue;
        
            prev = item;
            directions.Add(item);
        }
        directions.Add(tmp_dir[tmp_dir.Count - 1]);
        
        List<Vector2> points = new List<Vector2>();
        for (int i = 0; i < wayPoints.Count-2; i++)
        {
            if (tmp_dir[i] != tmp_dir[i + 1])
            {
                points.Add(World.tileToWorldCords(wayPoints[i]));
            }
        }

        points.Add(World.tileToWorldCords(wayPoints[wayPoints.Count - 1]));

        wayPoints = points;
        return true;

        // for        // {
        //     Vector2 curr=new Vector2(tmp_directions[i][0],tmp_directions[i][1]);
        //     Vector2 next=new Vector2(tmp_directions[i+1][0],tmp_directions[i+1][1]);
        //     tmp_dir.Add(next-curr);
        // }
        //
        // bool init = false;
        // Vector2 prev = default(Vector2);
        // foreach (Vector2 item in tmp_dir)
        // {
        //     if (!init)
        //         init = true;
        //     else if (prev.Equals(item))
        //         continue;
        //
        //     prev = item;
        //     directions.Add(item);
        // }
        // /*
        // if(tmp_dir.Any())
        //     directions.Add(tmp_dir[0]);
        // for(int i = 0 ; i<tmp_dir.Count-1 ; i++)
        // {
        //     if (!(tmp_dir[i] == tmp_dir[i + 1]))
        //     {
        //         directions.Add(tmp_dir[i]);
        //     }
        // }
        // if(tmp_dir.Any())
        //     directions.Add(tmp_dir[tmp_dir.Count-1]);
        //     */(int i = 0 ; i<tmp_directions.Count-1 ; i++)

    }

    private Vector2 findClosestPoint(Vector2 position)
    {
        Vector2 new_pos=new Vector2(position.X, position.Y);
        new_pos.Floor();
        // Node curNode = new Node(new_pos);
        // List<Node> children = getNeighorNodes(curNode);
        // children.Add(curNode);
        // children.Sort((a,b) => Vector2.Distance(position, a.position).CompareTo(Vector2.Distance(position, b.position)));
        // return children[0].position;
        return new_pos;
    }
    
    public Node newFindPath(Vector2 start, Vector2 end)
    {
        Node start_node = new Node(start,null);
        start_node.gScore = start_node.hCost = start_node.fScore = 0;

        List<Node> queue = new List<Node>();
        Dictionary<Vector2,Node> visited = new Dictionary<Vector2, Node>();
        
        queue.Add(start_node);

        while (queue.Any())
        {
            Node curr_node = getNodeWithMinFScore(queue);
            queue.Remove(curr_node);
            List<Node> children_plus_one = getNeighorNodes(curr_node);
            foreach (Node child in children_plus_one)
            {
                child.parent = curr_node;
                if (child.position == end)
                {
                    child.parent = curr_node;
                    return child.Invert();;
                }

                if (!canMoveTo(child))
                {
                    continue;
                }
                
                // gScore - dolžina poti od začetka do trenutnega noda 
                child.gScore = curr_node.gScore+1;
                // hScore - ocenjena  najkrajša pot of curNode do cilja
                child.hCost = manhatanDistance(child.position, end);
                // fScore - celotan pot od začetka do cilja
                child.fScore = child.gScore + child.hCost;
                
                Node sameNode = queue.Find(node => node.position == child.position);
                if (sameNode != null)
                {
                    if (sameNode.gScore > child.gScore)
                    {
                        sameNode.gScore = child.gScore;
                        sameNode.fScore = child.fScore;
                    }
                    continue;
                }
                
                visited.TryGetValue(child.position, out sameNode);
                if (sameNode != null)
                {
                    if (sameNode.gScore > child.gScore)
                    {
                        sameNode.gScore = child.gScore;
                        sameNode.fScore = child.fScore;
                        queue.Add(sameNode);
                    }
                    continue;
                }
                queue.Add(child);
            }
            visited.TryAdd(curr_node.position, curr_node);
        }

        return null;
    }

    public List<Node> getNeighorNodes(Node curr_node)
    {
        List<Node> children = new List<Node>();
        // children.Add(new Node(curr_node.position+new Vector2(-0.5f*tileSize,-0.5f*tileSize)));
        // children.Add(new Node(curr_node.position+new Vector2(-0.5f*tileSize,+0.5f*tileSize)));
        // children.Add(new Node(curr_node.position+new Vector2(+0.5f*tileSize,-0.5f*tileSize)));
        // children.Add(new Node(curr_node.position+new Vector2(+0.5f*tileSize,+0.5f*tileSize)));
        children.Add(new Node(curr_node.position+new Vector2(1,0)));
        children.Add(new Node(curr_node.position+new Vector2(-1,0)));
        children.Add(new Node(curr_node.position+new Vector2(0,1)));
        children.Add(new Node(curr_node.position+new Vector2(0,-1)));
        return children;
    }

    private bool canMoveTo(Node child)
    {
        IParticle clone = (IParticle)ai.Clone();
        clone.position = child.position;
        Vector2 tile;
        int width = clone.aabb.Width / World.tileSize.Width;
        int height = clone.aabb.Width / World.tileSize.Height;
        
        Vector2[] dirs = new[]
        {
            new Vector2((int)clone.position.X - width / 2, (int)clone.position.Y), //left
            new Vector2((int)clone.position.X + width / 2/2, (int)clone.position.Y), //right
            new Vector2((int)clone.position.X, (int)clone.position.Y - height / 2), //up
            new Vector2((int)clone.position.X, ((int)clone.position.Y + height / 2)), //down
        };
        int Left = (int)(clone.position.X - width / 2);
        int Right = (int)(clone.position.X + width / 2);
        int Up = (int)((int)clone.position.Y - height / 2);
        int Down = (int)((int)clone.position.Y + height / 2);
        bool cantMove=false;
        for (int x = Left ; x<=Right ; x++)
        {
            for (int y = Up; y <= Down; y++)
            {
                if (World.grid.TryGetValue(new Vector2(x,y), out cantMove) && cantMove)
                    return false;
            }
        }
        
        

        return true;
    }

    private float manhatanDistance(Vector2 position1, Vector2 position2)
    {
        return Math.Abs(position1.X - position2.X) + Math.Abs(position1.Y - position2.Y);
    }

    private Node? getNodeWithMinFScore(List<Node> queue)
    {
        float min_fScore = float.MaxValue;
        Node? min_node=null;
        foreach (var node in queue)
        {
            float node_fScore = node.fScore;
            if ( node_fScore< min_fScore)
            {
                min_fScore=node_fScore;
                min_node = node;
            }
        }
        return min_node;
    }

    // public List<int[]> findPath(NodeQuadTree quadTree, Vector2 playerPosition,DrawableGameObject ai, List<int[]> tresures){
    //     List<Node> path = new List<Node>();
    //     List<Node> Q = new List<Node>();
    //
    //     Node start = getClosestNode(quadTree,ai.position);
    //     Node end = getClosestNode(quadTree,playerPosition);
    //
    //     //v array dodaj start
    //     Q.Add(start);
    //     int[,][] parent = new int[gridHeight,gridWidth][];
    //
    //     //array dolžine poti
    //     int[,] gScore = new int[gridHeight,gridWidth];
    //     for (int i = 0; i < gridHeight; i++) {
    //         for (int j = 0; j < gridWidth; j++) {
    //             gScore[i,j] = int.MaxValue;
    //         }
    //     }
    //     gScore[start[0],start[1]]=0;
    //
    //     //array dolžin optimalne poti do cilja
    //     int[,] fScore = new int[gridHeight,gridWidth];
    //     for (int i = 0; i < gridHeight; i++) {
    //         for (int j = 0; j < gridWidth; j++) {
    //             fScore[i,j] = int.MaxValue;
    //         }
    //     }
    //     fScore[start[0],start[1]]=h(start,tresures,end);
    //
    //     //ponavljaj dokler array ni prazen
    //     while(Q.Any()){
    //         int[] curNode = getMinNode(Q, fScore);
    //         //če ni zakladov in si na cilju vrni pot
    //         if(new Vector2(curNode[0],curNode[1])==new Vector2(end[0],end[1])&&!tresures.Any()){
    //             path=getPath(curNode,parent);
    //             path.Reverse();
    //             return path;
    //         }
    //         //ostrani trenutno polje
    //         Q.Remove(curNode);
    //         //preveri vse možne smeri
    //         foreach(int[] d in dir) {
    //             int i = curNode[0] + d[0];
    //             int j = curNode[1] + d[1];
    //             //če zadeneš zid nadaljuj
    //             IParticle newPos = (IParticle)ai.Clone();
    //             newPos.position = new Vector2(i, j);
    //             if(World.collision(newPos)){
    //                 continue;
    //             }
    //             int cost=1;
    //             int t_gScore = gScore[curNode[0],curNode[1]]+cost;
    //             //če si našev novo bolj optimalno pot do soseda ogliča v katerem se nahajaš popravi vrenosti cen v gScore in fScore za tega soseda
    //             if(t_gScore<gScore[i,j]){
    //                 parent[i,j]=curNode;
    //                 gScore[i,j]=t_gScore;
    //                 int[] neighbor = new int[]{i,j};
    //                 fScore[i,j]=t_gScore+h(neighbor,tresures,end);
    //                 //če tega soseda še nimaš v array vozlišč ga dodaj
    //                 if(!QContains(neighbor,Q)){
    //                     Q.Add(neighbor);
    //                 }
    //             }
    //         }
    //
    //
    //     }
    //     //do tle pride samo če je nekaj narobe
    //     return path;
    // }

    private static Node? getClosestNode(NodeQuadTree quadTree, Vector2 position)
    {
        float mindDistance = float.MaxValue;
        Node minNode = null;
        List<Node> possibleNodes=quadTree.search(new Rectangle((int)(Math.Round(position.X) - 1), (int)(Math.Round(position.Y) - 1), 2, 2));
        foreach (Node node in possibleNodes)
        {
            float dx = Math.Abs(position.X - node.position.X);
            float dy = Math.Abs(position.Y - node.position.Y);
            float distance = (float)Math.Pow(dx, 2)+(float)Math.Pow(dy, 2);
            if (distance < mindDistance)
            {
                minNode = node;
                mindDistance = distance;
            }
        }

        return minNode;
    }

    public static List<int[]> getPath(int[]curNode,int[,][] parent){
        List<int[]> path = new List<int[]>();
        path.Add(curNode);
        while(parent[curNode[0],curNode[1]] != null){
            curNode=parent[curNode[0],curNode[1]];
            path.Add(curNode);
        }
        path.RemoveAt(path.Count-1);
        return path;
    }
    // static int h(int[] curNode,List<int[]>tresures,int[]end){
    //     int h=int.MaxValue;
    //     if(!tresures.Any()){
    //         h=distance(curNode,end);
    //         return h;
    //     }
    //     foreach(int[] tresure in tresures){
    //         int tmp=distance(curNode,tresure);
    //         if(tmp<h){
    //             h=tmp;
    //         }
    //     }
    //     return h;
    // }

    // public static int distance(int[] point1, int[] point2) {
    //     return Math.Abs((point2[0]-point1[0]))+Math.Abs((point2[1]-point1[1]));
    // }
    private static int[] getMinNode(List<int[]> Q, int[,] fScore)
    {
        int tmpF = int.MaxValue;
        int[] curNode = new int[0];
        foreach(int[] node in Q){
            if(fScore[node[0],node[1]]<tmpF){
                tmpF= fScore[node[0],node[1]];
                curNode=node;
            }
        }
        return curNode;
    }
    static int[][] dir = {new int[]{-1, 0}, new int[]{1, 0}, new int[]{0, -1}, new int[]{0, 1}};
    private static bool QContains(int[] neighbor, List<int[]> q) {
        foreach(int[] node in q){
            if(node[0]==neighbor[0]&&node[1]==neighbor[1])
                return true;
        }
        return false;
    }
    
}