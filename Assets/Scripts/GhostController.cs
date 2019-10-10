using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    public float movementSpeed = 5.6f;
    public string movementDirection = "Right";

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        movementDirection = "Right";
    }

    private void Update()
    {
        //HandleMovementInput();
        HandlePathfinding();
        AnimateSprite();

        if (CheckCanMove(movementDirection))
        {
            Move();
        }
    }

    private void HandlePathfinding()
    {

    }

    private void HandleMovementInput()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            if (CheckCanMove("Right"))
            {
                movementDirection = "Right";
            }
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            if (CheckCanMove("Left"))
            {
                movementDirection = "Left";
            }
        }
        else if (Input.GetAxis("Vertical") > 0)
        {
            if (CheckCanMove("Up"))
            {
                movementDirection = "Up";
            }
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            if (CheckCanMove("Down"))
            {
                movementDirection = "Down";
            }
        }
    }

    private bool CheckCanMove(string direction)
    {
        float rayOffsetX = 0.0f;
        float rayOffsetY = 0.0f;
        float rayOffset = 0.9f;
        Vector3 rayDir = Vector3.zero;
        float checkDistance = 1.1f;

        switch (direction)
        {
            case "Right":
                rayOffsetX = 0.0f;
                rayOffsetY = rayOffset;
                rayDir = Vector3.right;
                break;

            case "Left":
                rayOffsetX = 0.0f;
                rayOffsetY = rayOffset;
                rayDir = Vector3.left;
                break;

            case "Up":
                rayOffsetX = rayOffset;
                rayOffsetY = 0.0f;
                rayDir = Vector3.up;
                break;

            case "Down":
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

        if (hitLeft.collider != null && hitLeft.collider.tag != null && hitLeft.collider.tag == "Walls")
        {
            return false;
        }

        Vector2 vectorOffsetMiddle = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D hitMiddle = Physics2D.Raycast(vectorOffsetMiddle, rayDir, checkDistance);
        //Debug.DrawRay(vectorOffsetMiddle, rayDir * checkDistance, Color.green);

        if (hitMiddle.collider != null && hitMiddle.collider.tag != null && hitMiddle.collider.tag == "Walls")
        {
            return false;
        }

        Vector2 vectorOffsetRight = new Vector2(transform.position.x + rayOffsetX, transform.position.y + rayOffsetY);
        RaycastHit2D hitRight = Physics2D.Raycast(vectorOffsetRight, rayDir, checkDistance);
        //Debug.DrawRay(vectorOffsetRight, rayDir * checkDistance, Color.cyan);

        if (hitRight.collider != null && hitRight.collider.tag != null && hitRight.collider.tag == "Walls")
        {
            return false;
        }

        return true;
    }

    private void Move()
    {
        transform.Translate(new Vector3(movementSpeed, 0f, 0f) * Time.deltaTime, Space.Self);
    }

    private void AnimateSprite()
    {
        if (movementDirection.Equals("Right"))
        {
            spriteRenderer.flipY = false;
            transform.localEulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0.0f);
        }
        else if (movementDirection.Equals("Up"))
        {
            spriteRenderer.flipY = false;
            transform.localEulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 90.0f);
        }
        else if (movementDirection.Equals("Left"))
        {
            spriteRenderer.flipY = true;
            transform.localEulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 180.0f);
        }
        else if (movementDirection.Equals("Down"))
        {
            spriteRenderer.flipY = false;
            transform.localEulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 270.0f);
        }
    }
}
