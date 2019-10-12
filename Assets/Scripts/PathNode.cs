using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public int arrayPosX; // X position in the Node Array
    public int arrayPosY; // Y position in the Node Array

    public bool isWall; // if this node is a wall
    public Vector3 worldPos; // position of the node in world space

    public PathNode parentNode; // what the previous node in a path was

    public int movementCost; // movement cost of moving to the next node
    public int distanceCost; // distance to the target from this node

    public int totalCost // get the total cost of this node
    {
        get { return movementCost + distanceCost; }
    }

    /*
     * Construct a Node of a path
     */
    public PathNode(bool _isWall, Vector3 _worldPos, int _arrayPosX, int _arrayPosY)
    {
        isWall = _isWall;
        worldPos = _worldPos;
        arrayPosX = _arrayPosX;
        arrayPosY = _arrayPosY;
    }
}
