using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGrid : MonoBehaviour
{
    public LayerMask wallMask; // layer to mark as 'walls' - aka not let anything pathfind through
    public Vector2 worldSize; // width and height of the graph in world space
    public float nodeRadius; // radius of each node
    public float distanceBetweenNodes; // distance between nodes    
    public List<PathNode> FinalPath; // completed path to follow

    private PathNode[,] nodeArray; // array of nodes that the A* algorithm can use
    private float nodeDiameter; // diameter of each node
    private int arraySizeX; // X size of the Node Array
    private int arraySizeY; // Y size of the Node Array

    public bool debugPath = true; // if we want to show the ghost's path
    public bool debugFullGrid = false; // if we want to show the entire grid the ghost uses for pathfinding
    public Color debugColor = Color.yellow; // the standard debug colour for paths

    private void Start()
    {
        // calculate the size of each node
        nodeDiameter = nodeRadius * 2;

        // calculate the grid's world size
        arraySizeX = Mathf.RoundToInt(worldSize.x / nodeDiameter);
        arraySizeY = Mathf.RoundToInt(worldSize.y / nodeDiameter);

        CreateNodeGrid();
    }

    /*
     * Setup the node grid with a multidimensional array of nodes and determine whether they are walls or not
     */
    private void CreateNodeGrid()
    {
        nodeArray = new PathNode[arraySizeX, arraySizeY];
        Vector3 bottomLeft = Vector3.zero - Vector3.right * worldSize.x / 2 - Vector3.up * worldSize.y / 2;

        for (int x = 0; x < arraySizeX; x++)
        {
            for (int y = 0; y < arraySizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                bool isWall = true;

                // check if the node is in the same location as a wall
                if (Physics2D.OverlapCircle(new Vector2(worldPoint.x, worldPoint.y), nodeRadius, wallMask))
                {
                    isWall = false;
                }

                nodeArray[x, y] = new PathNode(isWall, worldPoint, x, y);
            }
        }
    }

    /* 
     * Return the neighbour nodes of any given node
     */
    public List<PathNode> GetNeighbouringNodes(PathNode _neighbourNode)
    {
        List<PathNode> neighbourList = new List<PathNode>(); // a list of all available neighbours
        int checkedX; // position checked within an X range
        int checkedY; // position checked within an Y range

        // check the right side
        checkedX = _neighbourNode.arrayPosX + 1;
        checkedY = _neighbourNode.arrayPosY;
        if (checkedX >= 0 && checkedX < arraySizeX)
        {
            if (checkedY >= 0 && checkedY < arraySizeY)
            {
                neighbourList.Add(nodeArray[checkedX, checkedY]);
            }
        }

        // check the left side
        checkedX = _neighbourNode.arrayPosX - 1;
        checkedY = _neighbourNode.arrayPosY;
        if (checkedX >= 0 && checkedX < arraySizeX)
        {
            if (checkedY >= 0 && checkedY < arraySizeY)
            {
                neighbourList.Add(nodeArray[checkedX, checkedY]);
            }
        }

        // check the top side
        checkedX = _neighbourNode.arrayPosX;
        checkedY = _neighbourNode.arrayPosY + 1;
        if (checkedX >= 0 && checkedX < arraySizeX)
        {
            if (checkedY >= 0 && checkedY < arraySizeY)
            {
                neighbourList.Add(nodeArray[checkedX, checkedY]);
            }
        }

        // check the bottom side
        checkedX = _neighbourNode.arrayPosX;
        checkedY = _neighbourNode.arrayPosY - 1;
        if (checkedX >= 0 && checkedX < arraySizeX)
        {
            if (checkedY >= 0 && checkedY < arraySizeY)
            {
                neighbourList.Add(nodeArray[checkedX, checkedY]);
            }
        }

        return neighbourList;
    }

    /*
     * Returns the closest node to a Vector position
     */
    public PathNode NodeFromWorldPoint(Vector3 worldPos)
    {
        float xPos = ((worldPos.x + worldSize.x / 2) / worldSize.x);
        float yPos = ((worldPos.y + worldSize.y / 2) / worldSize.y);

        xPos = Mathf.Clamp01(xPos);
        yPos = Mathf.Clamp01(yPos);

        int nodeX = Mathf.RoundToInt((arraySizeX - 1) * xPos);
        int nodeY = Mathf.RoundToInt((arraySizeY - 1) * yPos);

        return nodeArray[nodeX, nodeY];
    }

    /*
     * Draw all the nodes for debugging.
     */
    private void OnDrawGizmos()
    {
        if (debugPath)
        {
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(worldSize.x, worldSize.y, 1));

            if (nodeArray != null)
            {
                foreach (PathNode n in nodeArray)
                {
                    if (n.isWall)
                    {
                        Gizmos.color = Color.green;

                        if (FinalPath != null)
                        {
                            if (FinalPath.Contains(n))
                            {
                                Gizmos.color = debugColor; // these are the path nodes that we want to see
                            }
                        }
                    }
                    else
                    {
                        Gizmos.color = Color.red;
                    }

                    if (debugFullGrid || Gizmos.color == debugColor)
                    {
                        Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter - distanceBetweenNodes));
                    }
                }
            }
        }
    }
}
