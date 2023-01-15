using System;
using System.Collections.Generic;
using System.Linq;
using GreenTrutle_crossplatform.scene;
using Microsoft.Xna.Framework;

namespace GreenTrutle_crossplatform.player.AI;

public class AStar
{
    public static List<int[]> findPath(int[][] maze, DrawableGameObject player,DrawableGameObject ai, List<int[]> tresures){
        List<int[]> path = new List<int[]>();
        List<Vector2> Q = new List<Vector2>();
        int n = maze.GetLength(0);
        int m = maze.GetLength(1);
        Vector2 start = ai.position;
        int[] end = player.position;

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
            if(maze[curNode[0]][curNode[1]]==-4&&!tresures.Any()){
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
                if(maze[i][j]==1){
                    continue;
                }
                int cost=0;
                if(maze[curNode[0]][curNode[1]]!=-2&&maze[curNode[0]][curNode[1]]!=-4&&maze[curNode[0]][curNode[1]]!=-3){
//                  cost=maze[curNode[0]][curNode[1]];
                    cost=1;
                }
                int t_gScore = gScore[curNode[0],curNode[1]]+cost;
                //če si našev nobo bolj optimalno pot do soseda ogliča v katerem se nahajaš popravi vrenosti cen v gScore in fScore za tega soseda
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