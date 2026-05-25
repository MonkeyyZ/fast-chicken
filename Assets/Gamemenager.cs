using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Lives")]
    public int lives = 3;
    public Image[] hearts;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateHearts();
    }

    public void LoseLife()
    {
        lives--;

        UpdateHearts();

        if (lives <= 0)
        {
            RestartGame();
        }
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < lives;
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}