using UnityEngine;
using TMPro;
using System.Collections;

public class Player1Movement : MonoBehaviour
{
    public float speed = 10.0f;
    public Rigidbody rb;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText; 
    public int winScore = 50; // End game when player reaches this

    private int score = 0;
    private bool poweredUp10 = false;
    private bool poweredUp25 = false;
    private bool isFrozen = false; 

    void Start()
    {
        if (gameOverText != null)
            gameOverText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isFrozen) return; 

        float horizontal = 0f, vertical = 0f;

        if (Input.GetKey(KeyCode.W)) vertical = 1;
        if (Input.GetKey(KeyCode.S)) vertical = -1;
        if (Input.GetKey(KeyCode.A)) horizontal = -1;
        if (Input.GetKey(KeyCode.D)) horizontal = 1;

        rb.MovePosition(rb.position + new Vector3(horizontal, 0f, vertical) * speed * Time.deltaTime);

        rb.AddForce(0, -5f, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //  Normal scoring sphere
        if (collision.gameObject.CompareTag("Normal"))
        {
            Destroy(collision.gameObject);
            score++;
            scoreText.text = "Player 1 Score: " + score;

            if (score >= 15 && !poweredUp10)
            {
                poweredUp10 = true;
                ApplyBuff(1.2f);
            }

            if (score >= 35 && !poweredUp25)
            {
                poweredUp25 = true;
                ApplyBuff(1.5f);
            }

            if (score >= winScore)
            {
                EndGame("Player 1 Wins!", new Color(0f, 1f, 1f));
            }
        }

        
        if (collision.gameObject.CompareTag("Trap"))
        {
            Destroy(collision.gameObject);
            StartCoroutine(FreezeMovement(2f)); // freeze for 2 seconds
        }
    }

    private void ApplyBuff(float multiplier)
    {
        transform.localScale *= multiplier;
        speed *= multiplier;

        Debug.Log("Player 1 powered up! Multiplier: " + multiplier);
    }

    private void EndGame(string message, Color color)
    {
        Debug.Log(message);

        if (gameOverText != null)
        {
            gameOverText.text = message;
            gameOverText.color = color; 
            gameOverText.gameObject.SetActive(true);
        }

        Time.timeScale = 0f; 
    }

    private IEnumerator FreezeMovement(float duration)
    {
        isFrozen = true;
        Debug.Log("Player 1 frozen!");
        yield return new WaitForSeconds(duration);
        isFrozen = false;
        Debug.Log("Player 1 unfrozen!");
    }
}
