using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private GameManager gameManager;
    public Button restartButton;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI winText;
    Rigidbody2D rigidBody;
    public int coins = 0;
    public List<GameObject> lifeObjects;

    float[] initialSpeeds = new float[3] { 2.5f, 3.0f, 3.6f };
    public bool isTop = false;
    public bool isGameOver = false;
    private int lives = 3;

    private Vector2 touchStartPos;

    private void Awake()
    {
        // Reset the static variable to its initial value when a new scene is loaded
        Enemy3.speed = initialSpeeds[0];
        Enemy1.speed = initialSpeeds[1];
        Enemy2.speed = initialSpeeds[2];
    }
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        // Check for touch input 
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began && !isGameOver)
            {
                touchStartPos = touch.position;
            }

            if (touch.phase == TouchPhase.Ended && !isGameOver)
            {
                Vector2 touchEndPos = touch.position;
                Vector2 swipeDirection = touchEndPos - touchStartPos;

                // Check if swipe direction is upwards
                if (swipeDirection.y > 0)
                {
                    rigidBody.gravityScale = -2.2f;
                    isTop = true;
                }
                else
                {
                    rigidBody.gravityScale = 2.2f;
                    isTop = false;
                }
            }
        }

        if (isTop)
        {
            transform.eulerAngles = new Vector3(0, 180, 180);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (gameManager.time >= 360)
        {
            winText.gameObject.SetActive(true);
            GameOver();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemy" )
        {           
            lives -= 1;
            lifeObjects[lives].gameObject.SetActive(false);
            Destroy(collision.gameObject);
            if (lives == 0)
            {
                isGameOver = !isGameOver;
                GameOver();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "coin")
        {
            Destroy(collision.gameObject);
            coins += 1;
        }
    }
}
