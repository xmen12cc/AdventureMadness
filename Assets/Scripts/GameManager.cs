using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // UI Elements
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI xpText;
    public Slider healthBar;

    public PlayerController player;

    // Player Stats
    private int coins = 0;
    private float health = 0;
    private int level = 1;
    private int xp = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        UpdatePlayerStats();
    }

    void Start()
    {
        UpdatePlayerStats();
    }

    public void UpdatePlayerStats(){
        health = player.GetHp();
        level = player.GetLevel();
        xp = player.GetXp();
        UpdateUI();
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateUI();
    }

        public void AddXp(int amount)
    {
        xp += amount;
        player.GainXP(amount);
        UpdateUI();
    }

        public void AddLevel(int amount)
    {
        level += amount;
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, 100);
        UpdateUI();

        if (health <= 0)
        {
            PlayerDied();
        }
    }

    public void UpdateUI()
    {
        coinText.text = "Coins: " + coins;
        levelText.text = "Level: " + level;
        xpText.text = "Xp: " + xp;
        healthBar.value = health;
    }

    private void PlayerDied()
    {
        Debug.Log("Game Over!");
    
    }
}
