using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GreenTrutle_crossplatform.tools;

public class Node : ICloneable ,IComparable<Node>
{
    public float gScore { get; set; }
    public Vector2 position;
    public Node? parent { get; set; }
    public float hCost { get; set; }
    public float fScore { get; set; }

    public Node(Vector2 vector2)
    {
        position = vector2;
        gScore = 1;
    }
    public Node(Vector2 vector2,Node? parent)
    {
        this.parent = parent;
        position = vector2;
        gScore = 1;
    }

    public Node  Invert()
    {
        return _reverse(this);
    }
    Node _reverse(Node node)
    {
        // Initialize current, previous and next pointers
        Node current = node;
        Node prev = null;
        Node next = null;
 
        while (current != null) {
            // Store next
            next = current.parent;
            // Reverse current node's pointer
            current.parent = prev;
            // Move pointers one position ahead.
            prev = current;
            current = next;
        }
        return prev;
    }

    public object Clone()
    {
        return (Node)this.MemberwiseClone();
    }

    public int CompareTo(Node other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return fScore.CompareTo(other.fScore);
    }
}