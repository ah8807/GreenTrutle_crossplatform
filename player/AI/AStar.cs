using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using GreenTrutle_crossplatform.interfaces;
using GreenTrutle_crossplatform.scene;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace GreenTrutle_crossplatform.player.AI;

public class AStar
{
    public List<Vector2> directions = new List<Vector2>();
    private DrawableGameObject player;
    private DrawableGameObject ai;
    public AStar(DrawableGameObject player,DrawableGameObject ai)
    {
        this.player = player;
        this.ai = ai;
    }

    public void Find(Vector2 playerPosition)
    {
        directions.Clear();
        List<int[]> tmp_directions = findPath(World.grid, playerPosition, ai, new List<int[]>());
        List<Vector2> tmp_dir = new List<Vector2>();
        
        for(int i = 0 ; i<tmp_directions.Count-1 ; i++)
        {
            Vector2 curr=new Vector2(tmp_directions[i][0],tmp_directions[i][1]);
            Vector2 next=new Vector2(tmp_directions[i+1][0],tmp_directions[i+1][1]);
            tmp_dir.Add(next-curr);
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
        /*
        if(tmp_dir.Any())
            directions.Add(tmp_dir[0]);
        for(int i = 0 ; i<tmp_dir.Count-1 ; i++)
        {
            if (!(tmp_dir[i] == tmp_dir[i + 1]))
            {
                directions.Add(tmp_dir[i]);
            }
        }
        if(tmp_dir.Any())
            directions.Add(tmp_dir[tmp_dir.Count-1]);
            */
    }
    public static List<int[]> findPath(int[,] maze, Vector2 playerPosition,DrawableGameObject ai, List<int[]> tresures){
        List<int[]> path = new List<int[]>();
        List<int[]> Q = new List<int[]>();
        int m = maze.GetLength(0);
        int n = maze.GetLength(1);
        int[] start = new[] { (int)ai.position.X, (int)ai.position.Y };
        int[] end = new[] { (int)playerPosition.X, (int)playerPosition.Y };

        //v array dodaj start
        Q.Add(start);
        int[,][] parent = new int[n,m][];

        //array dolžine poti
        int[,] gScore = new int[n,m];
        for (int i = 0; i < n; i++) {
            for (int j = 0; j < m; j++) {
                gScore[i,j] = int.MaxValue;
            }
        }
        gScore[start[0],start[1]]=0;

        //array dolžin optimalne poti do cilja
        int[,] fScore = new int[n,m];
        for (int i = 0; i < n; i++) {
            for (int j = 0; j < m; j++) {
                fScore[i,j] = int.MaxValue;
            }
        }
        fScore[start[0],start[1]]=h(start,tresures,end);

        //ponavljaj dokler array ni prazen
        while(Q.Any()){
            int[] curNode = getMinNode(Q, fScore);
            //če ni zakladov in si na cilju vrni pot
            if(new Vector2(curNode[0],curNode[1])==new Vector2(end[0],end[1])&&!tresures.Any()){
                path=getPath(curNode,parent);
                path.Reverse();
                return path;
            }
            //ostrani trenutno polje
            Q.Remove(curNode);
            //preveri vse možne smeri
            foreach(int[] d in dir) {
                int i = curNode[0] + d[0];
                int j = curNode[1] + d[1];
                //če zadeneš zid nadaljuj
                IParticle newPos = (IParticle)ai.Clone();
                newPos.position = new Vector2(i, j);
                if(World.collision(newPos)){
                    continue;
                }
                int cost=1;
                int t_gScore = gScore[curNode[0],curNode[1]]+cost;
                //če si našev novo bolj optimalno pot do soseda ogliča v katerem se nahajaš popravi vrenosti cen v gScore in fScore za tega soseda
                if(t_gScore<gScore[i,j]){
                    parent[i,j]=curNode;
                    gScore[i,j]=t_gScore;
                    int[] neighbor = new int[]{i,j};
                    fScore[i,j]=t_gScore+h(neighbor,tresures,end);
                    //če tega soseda še nimaš v array vozlišč ga dodaj
                    if(!QContains(neighbor,Q)){
                        Q.Add(neighbor);
                    }
                }
            }


        }
        //do tle pride samo če je nekaj narobe
        return path;
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
    static int h(int[] curNode,List<int[]>tresures,int[]end){
        int h=int.MaxValue;
        if(!tresures.Any()){
            h=distance(curNode,end);
            return h;
        }
        foreach(int[] tresure in tresures){
            int tmp=distance(curNode,tresure);
            if(tmp<h){
                h=tmp;
            }
        }
        return h;
    }

    public static int distance(int[] point1, int[] point2) {
        return Math.Abs((point2[0]-point1[0]))+Math.Abs((point2[1]-point1[1]));
    }
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