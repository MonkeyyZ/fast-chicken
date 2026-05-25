using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // JEŚLI DOTKNĄŁ GRACZ
        if (collision.CompareTag("Player"))
        {
            // DODAJ PUNKTY
            PlayerController player =
                collision.GetComponent<PlayerController>();

            if (player != null)
            {
                player.AddCoins(value);
            }

            // USUŃ COINA
            Destroy(gameObject);
        }
    }
}