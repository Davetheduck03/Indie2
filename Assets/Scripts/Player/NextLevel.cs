using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] private string scene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.SavePlayerStats(Player.Instance.playerHealth, Player.Instance.playerHunger);
            SceneManager.LoadScene(scene);
        }
    }
}