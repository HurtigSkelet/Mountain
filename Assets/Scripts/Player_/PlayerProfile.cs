using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerProfile : MonoBehaviour
{
    [Header("Player information")]
    [SerializeField]
    int playerID = 1;
    [SerializeField]
    string playerName = "Player";

    [Header("Player state")]
    [SerializeField]
    int playerHealth = 100;
    [SerializeField]
    int playerScore = 0;

    void Start()
    {
        PlayerInputManager inputManager = FindFirstObjectByType<PlayerInputManager>();
        playerID= inputManager.playerCount-1;
        playerName = "Player " + playerID; 
    }

    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
        if (playerHealth < 0)
        {
            playerHealth = 0;
        }
    }

    public void Heal(int amount)
    {
        playerHealth += amount;
        if (playerHealth > 100) // Assuming max health is 100
        {
            playerHealth = 100;
        }
    }

    public void AddScore(int score)
    {
        playerScore += score;
    }
}
