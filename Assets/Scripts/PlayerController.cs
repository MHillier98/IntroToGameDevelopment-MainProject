using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer; // the SpriteRenderer componenet on this player
    private AudioSource audioSource; // the AudioSource component to play sounds through

    private GameManager gameManager; // the game manager script in the scene

    public enum PoweredStates { PoweredUp, PoweredDown } // the possible states of whether the player can eat ghosts or not
    private PoweredStates powerState = PoweredStates.PoweredDown; // by default it shouldn't be powered up on start

    private static int poweredUpTimeMax = 6; // how long the powered up time goes for
    private int poweredUpTimeCurrent; // the current powered up time

    public enum MovementDirections { Up, Down, Left, Right }; // the possible directions for the player to move in
    public MovementDirections movementDirection = MovementDirections.Right; // the default movement direction

    public Vector3 startingPosition = new Vector3(0f, -2f, 0f); // the default starting position of a player
    public float movementSpeed = 6.0f; // the current movement speed
    public float defaultSpeed = 6.0f; // the default movement speed

    public AudioClip chompSound1 = null; // the first sound to play when eating dots
    public AudioClip chompSound2 = null; // the second sound to play when eating dots
    private bool playedChomp1 = false; // if the first sound has played yet, so we can play the second

    private static int startingScore = 0; // the score to start the player with
    private int currentScore = 0; // the current player score

    private static int dotScore = 10; // the score for a normal dot
    private static int largeDotScore = 50; // the score for a large dot
    private static int ghostScore = 100; // the score for a ghost

    public int scoreModifier = 1; // the score modifier, used to multiply the score when adding

    public GameObject dotParticleObject = null; // the particle object to spawn when a large dot is eaten

    private int ghostsEatenCounter = 1; // how many ghosts eaten in a row
    public AudioClip eatGhostSound = null; // the sound to play when a ghost is eated
    public AudioClip deathSound = null; // the sound to play when the player dies

    public int maxLives = 3; // the max number of lives the player has
    public int currentLives = 3; // the current number of lives the player has

    public float ghostEatTimer = -10f; // the timer for how often the ghosts can defeat the player, so that the player won't get stuck in spawn in some situations

    public bool frozen = false; // if the player can move

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        gameManager = FindObjectOfType<GameManager>();

        ResetPosition();
        ResetSpeed();
        ResetLives();

        currentScore = startingScore;
        poweredUpTimeCurrent = poweredUpTimeMax;

        powerState = PoweredStates.PoweredDown;
        movementDirection = MovementDirections.Right;

        playedChomp1 = false;
    }

    private void Update()
    {
        HandleMovementInput();
        AnimateSprite();

        if (!frozen && CheckCanMove(movementDirection))
        {
            Move();
        }
    }

    /*
     * Handle the keyboard movement inputs, checking if the player can move in that direction
     */
    private void HandleMovementInput()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            if (CheckCanMove(MovementDirections.Right))
            {
                movementDirection = MovementDirections.Right;
            }
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            if (CheckCanMove(MovementDirections.Left))
            {
                movementDirection = MovementDirections.Left;
            }
        }
        else if (Input.GetAxis("Vertical") > 0)
        {
            if (CheckCanMove(MovementDirections.Up))
            {
                movementDirection = MovementDirections.Up;
            }
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            if (CheckCanMove(MovementDirections.Down))
            {
                movementDirection = MovementDirections.Down;
            }
        }
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

            default: // direction will always be set, so this will never run
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckCollision(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        CheckCollision(collision);
    }

    /*
     * Check and handle the collisions encounted by the collider
     */
    private void CheckCollision(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Dots"))
        {
            PlayEatSound();
            Destroy(collision.gameObject);
            AddScore(dotScore);
        }
        else if (collision.gameObject.CompareTag("Large Dots"))
        {
            if (gameManager != null)
            {
                gameManager.SendMessage("ScatterAllGhosts");
            }

            if (dotParticleObject != null)
            {
                Instantiate(dotParticleObject, transform.position, transform.rotation);
            }

            if (eatGhostSound != null)
            {
                audioSource.PlayOneShot(eatGhostSound);
            }

            Destroy(collision.gameObject);
            AddScore(largeDotScore);
            PowerUp();
            Invoke("PowerDown", poweredUpTimeCurrent);
        }
        else if (collision.gameObject.CompareTag("Ghosts"))
        {
            if (powerState.Equals(PoweredStates.PoweredUp))
            {
                GhostController ghostController = collision.gameObject.GetComponent<GhostController>();
                ghostController.SendMessage("Die");

                AddScore(ghostScore * ghostsEatenCounter);
                ghostsEatenCounter += 1;
                StartCoroutine(PauseGame(1.0f));

                if (eatGhostSound != null)
                {
                    audioSource.PlayOneShot(eatGhostSound);
                }
            }
            else
            {
                if (ghostEatTimer + 2f < Time.time)
                {
                    if (deathSound != null)
                    {
                        audioSource.PlayOneShot(deathSound);
                    }

                    LoseLife();
                    StartCoroutine(PauseGame(3.0f));
                    ghostEatTimer = Time.time;

                    if (currentLives > 0)
                    {
                        transform.position = startingPosition;
                        movementDirection = MovementDirections.Right;
                    }
                    else
                    {
                        transform.position = new Vector3(999f, 999f);
                        StartCoroutine(EndGame());
                    }
                }
            }
        }
        else if (collision.gameObject.CompareTag("Powerup"))
        {
            if (collision.name.Contains("Double Points"))
            {
                SetScoreModifier(2);
            }
            else if (collision.name.Contains("Extra Life"))
            {
                AddLife();
            }
            else if (collision.name.Contains("Freeze"))
            {
                Freeze();
            }
            else if (collision.name.Contains("Invisible Walls"))
            {
                gameManager.SendMessage("HideWalls");
            }
            else if (collision.name.Contains("No Points"))
            {
                SetScoreModifier(0);
            }
            else if (collision.name.Contains("Speed Boost"))
            {
                SpeedUp(1.4f);
            }

            Destroy(collision.gameObject);
        }
    }

    /*
     * Play the two different dot eating sounds, alternating to create the "waka" sound effect
     */
    private void PlayEatSound()
    {
        if (playedChomp1 && chompSound2 != null)
        {
            audioSource.PlayOneShot(chompSound2);
            playedChomp1 = false;
        }
        else if (!playedChomp1 && chompSound1 != null)
        {
            audioSource.PlayOneShot(chompSound1);
            playedChomp1 = true;
        }
    }

    /*
     * Pause the game while something happens
     */
    private IEnumerator PauseGame(float time)
    {
        Time.timeScale = 0f;
        float pauseEndTime = Time.realtimeSinceStartup + time;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }
        Time.timeScale = 1f;
    }

    /*
     * End the game
     */
    private IEnumerator EndGame()
    {
        gameManager.SendMessage("ScatterAllGhosts");
        gameManager.SendMessage("EndGame");
        float pauseEndTime = Time.realtimeSinceStartup + 5.0f;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }

    /*
     * Set the score of the player
     */
    public void SetScore(int newScore)
    {
        currentScore = newScore;
    }

    /*
     * Get the score of the player
     */
    public int GetScore()
    {
        return currentScore;
    }

    /*
     * Increment the score of the player
     */
    public void IncrementScore()
    {
        currentScore += 1;
    }

    /*
     * Add to the score of the player, multiplying by the scoreModifier
     */
    public void AddScore(int addScore)
    {
        if (scoreModifier > 0)
        {
            currentScore += (addScore * scoreModifier);
        }
    }

    /*
     * Reset the number of lives the player has
     */
    public void ResetLives()
    {
        currentLives = maxLives;
    }

    /*
     * Get the number of lives the player has
     */
    public int GetCurrentLives()
    {
        return currentLives;
    }

    /*
     * Get the max number of lives the player has
     */
    public int GetMaxLives()
    {
        return maxLives;
    }

    /*
     * Add to the number of lives the player has
     */
    public void AddLife()
    {
        currentLives++;
    }

    /*
     * Remove one life from the number of lives the player has
     */
    public void LoseLife()
    {
        currentLives--;
    }

    /*
     * Set the player's power state to Powered Up
     */
    public void PowerUp()
    {
        powerState = PoweredStates.PoweredUp;
    }

    /*
     * Set the player's power state to Powered Down
     */
    public void PowerDown()
    {
        powerState = PoweredStates.PoweredDown;
        ghostsEatenCounter = 1;
    }

    /*
     * Reset the player's position
     */
    public void ResetPosition()
    {
        movementDirection = MovementDirections.Right;
        transform.position = startingPosition;
    }

    /*
     * Increase the speed of the player, and reset it after the specified time
     */
    private IEnumerator SpeedIncrease(float time)
    {
        float pauseEndTime = Time.realtimeSinceStartup + time;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }
        ResetSpeed();
    }

    /*
     * Multiply the speed of the player 
     */
    public void SpeedUp(float increaseBy)
    {
        movementSpeed *= increaseBy;
        StartCoroutine(SpeedIncrease(4.0f));
    }

    /*
     * Reset the speed of the player 
     */
    public void ResetSpeed()
    {
        movementSpeed = defaultSpeed;
    }

    /*
     * Reset the score modifier after a specified time
     */
    private IEnumerator UpdateScoreModifier(float time)
    {
        float pauseEndTime = Time.realtimeSinceStartup + time;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }
        ResetScoreModifier();
    }

    /*
     * Set the score modifier
     */
    public void SetScoreModifier(int newModifier)
    {
        scoreModifier = newModifier;
        StartCoroutine(UpdateScoreModifier(4.0f));
    }

    /*
     * Reset the score modifier
     */
    public void ResetScoreModifier()
    {
        scoreModifier = 1;
    }

    /*
     * Unfreeze the player after a specified time
     */
    private IEnumerator Unfreeze(float time)
    {
        float pauseEndTime = Time.realtimeSinceStartup + time;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }
        SetFrozen(false);
    }

    /*
     * Freeze the player and unfreeze after 4 seconds
     */
    public void Freeze()
    {
        frozen = true;
        StartCoroutine(Unfreeze(4.0f));
    }

    /*
     * Set the freeze value manually
     */
    public void SetFrozen(bool isFrozen)
    {
        frozen = isFrozen;
    }

    /*
     * Set the transparency of the SpriteRenderer to half transparent
     */
    public void SetSpriteTransparent()
    {
        spriteRenderer.color = new Color(255f, 255f, 255f, 0.5f);
    }

    /*
     * Set the transparency of the SpriteRenderer to full
     */
    public void SetSpriteSolid()
    {
        spriteRenderer.color = new Color(255f, 255f, 255f, 1f);
    }
}
