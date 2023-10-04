using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GreenTrutle_crossplatform.tools;

public class CastRay
{
    public static List<Vector2> pointAToPointB(Vector2 A, Vector2 B)
    {
        int x1 = (int)A.X;
        int y1 = (int)A.Y;
        int x2 = (int)B.X;
        int y2 = (int)B.Y;
        List<Vector2> line = new List<Vector2>();
        int dx = Math.Abs(x2 - x1);
        int dy = Math.Abs(y2 - y1);
        int sx = x1 < x2 ? 1 : -1;
        int sy = y1 < y2 ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            line.Add(new Vector2(x1, y1));
            if (x1 == x2 && y1 == y2) break;
            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x1 += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y1 += sy;
            }
        }
        return line;
    }
}