using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.AddCoins(coinValue);
            GameManager.Instance.AddXp(20);
            AudioManager.Instance.PlaySound(AudioManager.Instance.coinSound);
            Destroy(gameObject);
        }
    }
}
