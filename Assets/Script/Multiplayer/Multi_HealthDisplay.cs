using UnityEngine;
using UnityEngine.UI;

public class Multi_HealthDisplay : MonoBehaviour
{

    private int health;

    [Header("Sprites")]
    [SerializeField] private Sprite emptyHeart;
    [SerializeField] private Sprite fullHeart;

    [SerializeField] private Image[] hearts;

    [Header("Player")]
    [SerializeField] private Multi_Player player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        health = (int)player.GetHealth();
        //maxHealth = playerHealth.maxHealth;
        int maxHealth = 5;

        for (int i = 0; i < hearts.Length; i++)
        {

            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < maxHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                {
                    hearts[i].enabled = false;

                }
            }
        }
    }
}
