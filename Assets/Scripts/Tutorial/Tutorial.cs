using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public TextMeshProUGUI leftText;
    public TextMeshProUGUI startText;
    Rigidbody2D rigidBody;

    public bool isTop = false;

    private Vector2 touchStartPos;

    private bool isSwipedUp = false;
    private bool isSwipedDown = false;


    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                Vector2 touchEndPos = touch.position;
                Vector2 swipeDirection = touchEndPos - touchStartPos;

                // Check if swipe direction is upwards
                if (swipeDirection.y > 0)
                {
                    rigidBody.gravityScale = -2.2f;
                    isTop = true;
                    isSwipedUp = true;
                }
                else
                {
                    rigidBody.gravityScale = 2.2f; 
                    isTop = false;
                    isSwipedDown = true;
                }
            }
        }
        if (isSwipedDown && isSwipedUp)
        {
            leftText.gameObject.SetActive(false);
            startText.gameObject.SetActive(true);
            StartCoroutine(WaitAndLoadScene());
        }

        if (isTop)
            transform.eulerAngles = new Vector3(0, 180, 180);
        else
            transform.eulerAngles = new Vector3(0, 0, 0);
    }

    IEnumerator WaitAndLoadScene()
    {
        // Wait for 3 seconds
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(2);
    }
}
