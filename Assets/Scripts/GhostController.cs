﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer; // the SpriteRenderer componenet on this ghost
    private Animator animator; // the Animator componenet on this ghost
    private BoxCollider2D boxCollider2D; // the BoxCollider2D componenet on this ghost
    private NodeGrid nodeGridReference; // the NodeGrid componenet on this ghost

    public enum MovementDirections { Up, Down, Left, Right }; // the possible directions for the ghost to move in
    public MovementDirections movementDirection = MovementDirections.Right; // the default movement direction
    List<MovementDirections> lastCanMoveDirs = new List<MovementDirections>(); // a list of possible directions that the ghost can move in
    float timeOfDirCheck = 0f; // time since the direction check has run

    public enum PathfindingTypes { Random, Clockwise, Run, Follow, Scatter, StartReset }; // the possible types of pathfinding AI for the ghosts
    public PathfindingTypes pathfindingType = PathfindingTypes.Follow; // the starting pathfinding type
    public PathfindingTypes defaultPathfindingType = PathfindingTypes.Follow; // the default pathfinding type

    public Transform playerPosition; // player to pathfind to
    public Transform[] targetPositions; // positions to pathfind to
    public Transform[] scatterPositions; // positions to pathfind to
    public int currentTarget = 0; // the current target array position
    public int currentTargetScatter = 0; // the current scatter target array position

    public float movementSpeed = 5.5f; // the current movement speed
    public Transform startingPosition; // the position to move to for restarting the ghost
    public Vector3 spawnPosition; // the position where the ghost started in this scene

    private void Start()
    {
        nodeGridReference = GetComponent<NodeGrid>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        spawnPosition = transform.position;
    }

    private void Update()
    {
        HandlePathfinding();
        AnimateSprite();
        if (CheckCanMove(movementDirection))
        {
            Move();
        }
    }

    private void HandlePathfinding()
    {
        switch (pathfindingType)
        {
            case PathfindingTypes.Scatter:
                Scatter();
                break;

            case PathfindingTypes.Random:
                MoveRandomly();
                break;

            case PathfindingTypes.Clockwise:
                MoveClockwise();
                break;

            case PathfindingTypes.Run:
                RunFromPlayer();
                break;

            case PathfindingTypes.Follow:
                FollowPlayer();
                break;

            case PathfindingTypes.StartReset:
                BackToStart();
                break;

            default: return;
        }
    }

    private void Scatter()
    {
        if (scatterPositions.Length > 1)
        {
            float myTempX = (float)Math.Round(transform.position.x * 2, MidpointRounding.AwayFromZero) / 2;
            float myTempY = (float)Math.Round(transform.position.y * 2, MidpointRounding.AwayFromZero) / 2;

            float targetTempX = (float)Math.Round(scatterPositions[currentTargetScatter].position.x * 2, MidpointRounding.AwayFromZero) / 2;
            float targetTempY = (float)Math.Round(scatterPositions[currentTargetScatter].position.y * 2, MidpointRounding.AwayFromZero) / 2;

            if (myTempX == targetTempX && myTempY == targetTempY)
            {
                currentTargetScatter++;
                if (currentTargetScatter == scatterPositions.Length)
                {
                    currentTargetScatter = 0;
                }
            }
            else
            {
                FollowTarget(scatterPositions[currentTargetScatter]);
            }
        }
    }

    private void MoveRandomly()
    {
        if (timeOfDirCheck + 0.4f < Time.time)
        {
            List<MovementDirections> canMoveDirs = new List<MovementDirections>();

            if (CheckCanMove(MovementDirections.Up))
            {
                canMoveDirs.Add(MovementDirections.Up);
            }

            if (CheckCanMove(MovementDirections.Down))
            {
                canMoveDirs.Add(MovementDirections.Down);
            }

            if (CheckCanMove(MovementDirections.Left))
            {
                canMoveDirs.Add(MovementDirections.Left);
            }

            if (CheckCanMove(MovementDirections.Right))
            {
                canMoveDirs.Add(MovementDirections.Right);
            }

            if (!lastCanMoveDirs.SequenceEqual(canMoveDirs))
            {
                if (canMoveDirs.Count > 1)
                {
                    int maxNum = canMoveDirs.Count;
                    int randNum = UnityEngine.Random.Range(0, maxNum);
                    movementDirection = canMoveDirs[randNum];
                    lastCanMoveDirs = canMoveDirs;
                    timeOfDirCheck = Time.time;
                }
            }
        }
    }

    private void MoveClockwise()
    {
        if (targetPositions.Length > 0)
        {
            float myTempX = (float)Math.Round(transform.position.x * 2, MidpointRounding.AwayFromZero) / 2;
            float myTempY = (float)Math.Round(transform.position.y * 2, MidpointRounding.AwayFromZero) / 2;

            float targetTempX = (float)Math.Round(targetPositions[currentTarget].position.x * 2, MidpointRounding.AwayFromZero) / 2;
            float targetTempY = (float)Math.Round(targetPositions[currentTarget].position.y * 2, MidpointRounding.AwayFromZero) / 2;

            if (myTempX == targetTempX && myTempY == targetTempY)
            {
                currentTarget++;
                if (currentTarget == targetPositions.Length)
                {
                    currentTarget = 0;
                }
            }
            else
            {
                FollowTarget(targetPositions[currentTarget]);
            }
        }
    }

    private void RunFromPlayer()
    {
        if (Vector3.Distance(playerPosition.transform.position, transform.position) <= 10.0f)
        {
            if (timeOfDirCheck + 0.4f < Time.time)
            {
                MoveClockwise();
                timeOfDirCheck = Time.time;
            }
        }
        else
        {
            Scatter();
        }
    }

    private void FollowPlayer()
    {
        FollowTarget(playerPosition);
    }

    private void BackToStart()
    {
        FollowTarget(startingPosition);

        float myTempX = (float)Math.Round(transform.position.x * 2, MidpointRounding.AwayFromZero) / 2;
        float myTempY = (float)Math.Round(transform.position.y * 2, MidpointRounding.AwayFromZero) / 2;

        float startTempX = (float)Math.Round(startingPosition.position.x * 2, MidpointRounding.AwayFromZero) / 2;
        float startTempY = (float)Math.Round(startingPosition.position.y * 2, MidpointRounding.AwayFromZero) / 2;

        if (myTempX == startTempX && myTempY == startTempY)
        {
            ResetPathfindingType();
        }
    }

    private void FollowTarget(Transform target)
    {
        float myTempX = (float)Math.Round(transform.position.x * 2, MidpointRounding.AwayFromZero) / 2;
        float myTempY = (float)Math.Round(transform.position.y * 2, MidpointRounding.AwayFromZero) / 2;
        Vector3 myTempPos = new Vector3(myTempX, myTempY, 0);

        float playerTempX = (float)Math.Round(target.position.x * 2, MidpointRounding.AwayFromZero) / 2;
        float playerTempY = (float)Math.Round(target.position.y * 2, MidpointRounding.AwayFromZero) / 2;
        Vector3 playerTempPos = new Vector3(playerTempX, playerTempY, 0);

        FindPath(myTempPos, playerTempPos);

        if (nodeGridReference.FinalPath != null && nodeGridReference.FinalPath.Count > 0)
        {
            Vector3 nextNode = nodeGridReference.FinalPath[0].worldPos;
            nodeGridReference.FinalPath.RemoveAt(0);

            if (nextNode.x != myTempX)
            {
                if (nextNode.x > myTempX)
                {
                    if (CheckCanMove(MovementDirections.Right))
                    {
                        movementDirection = MovementDirections.Right;
                    }
                }
                else if (nextNode.x < myTempX)
                {
                    if (CheckCanMove(MovementDirections.Left))
                    {
                        movementDirection = MovementDirections.Left;
                    }
                }
            }
            else if (nextNode.y != myTempY)
            {
                if (nextNode.y > myTempY)
                {
                    if (CheckCanMove(MovementDirections.Up))
                    {
                        movementDirection = MovementDirections.Up;
                    }
                }
                else if (nextNode.y < myTempY)
                {
                    if (CheckCanMove(MovementDirections.Down))
                    {
                        movementDirection = MovementDirections.Down;
                    }
                }
            }
        }
    }

    private void ResetPathfindingType()
    {
        boxCollider2D.enabled = true;
        pathfindingType = defaultPathfindingType;
        animator.SetBool("Scattering", false);
        animator.SetBool("Dead", false);
    }

    public void Die()
    {
        animator.SetBool("Dead", true);
        pathfindingType = PathfindingTypes.StartReset;
        boxCollider2D.enabled = false;
    }

    public void InvokeScatter()
    {
        animator.SetBool("Scattering", true);
        pathfindingType = PathfindingTypes.Scatter;
        Invoke("ResetPathfindingType", 5.0f);
    }

    /*
     * Animate the sprite to look in the correct direction
     */
    private void AnimateSprite()
    {
        switch (movementDirection)
        {
            case MovementDirections.Right:
                spriteRenderer.flipY = false;
                transform.localEulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0.0f);
                break;

            case MovementDirections.Up:
                spriteRenderer.flipY = false;
                transform.localEulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 90.0f);
                break;

            case MovementDirections.Left:
                spriteRenderer.flipY = true;
                transform.localEulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 180.0f);
                break;

            case MovementDirections.Down:
                spriteRenderer.flipY = false;
                transform.localEulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 270.0f);
                break;

            default:
                return;
        }
    }

    /*
     * Raycast check if the player can move in one of the 4 directions
     */
    private bool CheckCanMove(MovementDirections direction)
    {
        float rayOffsetX = 0.0f;
        float rayOffsetY = 0.0f;
        float rayOffset = 0.9f;
        Vector3 rayDir = Vector3.zero;
        float checkDistance = 1.1f;

        switch (direction)
        {
            case MovementDirections.Right:
                rayOffsetX = 0.0f;
                rayOffsetY = rayOffset;
                rayDir = Vector3.right;
                break;

            case MovementDirections.Left:
                rayOffsetX = 0.0f;
                rayOffsetY = rayOffset;
                rayDir = Vector3.left;
                break;

            case MovementDirections.Up:
                rayOffsetX = rayOffset;
                rayOffsetY = 0.0f;
                rayDir = Vector3.up;
                break;

            case MovementDirections.Down:
                rayOffsetX = rayOffset;
                rayOffsetY = 0.0f;
                rayDir = Vector3.down;
                break;

            default: // this should never happen as direction should always be set
                return false;
        }

        Vector2 vectorOffsetLeft = new Vector2(transform.position.x - rayOffsetX, transform.position.y - rayOffsetY);
        RaycastHit2D hitLeft = Physics2D.Raycast(vectorOffsetLeft, rayDir, checkDistance);
        //Debug.DrawRay(vectorOffsetLeft, rayDir * checkDistance, Color.red);

        if (hitLeft.collider != null && hitLeft.collider.tag != null && hitLeft.collider.gameObject.CompareTag("Walls"))
        {
            return false;
        }

        Vector2 vectorOffsetMiddle = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D hitMiddle = Physics2D.Raycast(vectorOffsetMiddle, rayDir, checkDistance);
        //Debug.DrawRay(vectorOffsetMiddle, rayDir * checkDistance, Color.green);

        if (hitMiddle.collider != null && hitMiddle.collider.tag != null && hitMiddle.collider.gameObject.CompareTag("Walls"))
        {
            return false;
        }

        Vector2 vectorOffsetRight = new Vector2(transform.position.x + rayOffsetX, transform.position.y + rayOffsetY);
        RaycastHit2D hitRight = Physics2D.Raycast(vectorOffsetRight, rayDir, checkDistance);
        //Debug.DrawRay(vectorOffsetRight, rayDir * checkDistance, Color.cyan);

        if (hitRight.collider != null && hitRight.collider.tag != null && hitRight.collider.gameObject.CompareTag("Walls"))
        {
            return false;
        }

        return true;
    }

    /*
     * Move the player with frame-rate independent motion
     */
    private void Move()
    {
        transform.Translate(new Vector3(movementSpeed, 0f, 0f) * Time.deltaTime, Space.Self);
    }

    /*
     * Reset the ghost's position
     */
    public void ResetPosition()
    {
        ResetPathfindingType();
        transform.position = spawnPosition;
    }

    /*
     * Find the path to a target through the array of nodes
     * 
     * Resources used:
     *   https://en.wikipedia.org/wiki/A*_search_algorithm
     *   https://www.youtube.com/watch?v=ySN5Wnu88nE
     *   https://www.geeksforgeeks.org/a-search-algorithm/
     */
    private void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        PathNode startNode = nodeGridReference.NodeFromWorldPoint(startPos); // find the node closest to the starting position
        PathNode targetNode = nodeGridReference.NodeFromWorldPoint(targetPos); // find the node closest to the target position

        List<PathNode> openList = new List<PathNode>(); // a list of nodes still to search
        HashSet<PathNode> closedList = new HashSet<PathNode>(); // hashset of nodes of searched nodes

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            PathNode currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].totalCost < currentNode.totalCost || openList[i].totalCost == currentNode.totalCost && openList[i].distanceCost < currentNode.distanceCost)
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == targetNode)
            {
                GetFinalPath(startNode, targetNode);
                break;
            }

            foreach (PathNode neighbourNode in nodeGridReference.GetNeighbouringNodes(currentNode))
            {
                if (!neighbourNode.isWall || closedList.Contains(neighbourNode))
                {
                    continue;
                }

                int moveCost = currentNode.movementCost + GetDistance(currentNode, neighbourNode);

                if (!openList.Contains(neighbourNode) || moveCost < neighbourNode.totalCost)
                {
                    neighbourNode.movementCost = moveCost;
                    neighbourNode.distanceCost = GetDistance(neighbourNode, targetNode);
                    neighbourNode.parentNode = currentNode;

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }
    }

    /*
     * Calculate the final path of nodes between the start and end nodes
     */
    private void GetFinalPath(PathNode startingNode, PathNode endNode)
    {
        List<PathNode> finalPath = new List<PathNode>();
        PathNode currentNode = endNode;

        while (currentNode != startingNode)
        {
            finalPath.Add(currentNode);
            currentNode = currentNode.parentNode;
        }

        finalPath.Reverse();
        nodeGridReference.FinalPath = finalPath;
    }

    /*
     * Return the distance (in world space) between two nodes
     */
    private int GetDistance(PathNode nodeA, PathNode nodeB)
    {
        int distX = Mathf.Abs(nodeA.arrayPosX - nodeB.arrayPosX);
        int distY = Mathf.Abs(nodeA.arrayPosY - nodeB.arrayPosY);

        return distX + distY;
    }
}
