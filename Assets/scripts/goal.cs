using TMPro;
using UnityEngine;

public class goal : MonoBehaviour
{
    public TextMeshProUGUI team1text; // 1. takimin skor yazisi
    public TextMeshProUGUI team2text; // 2. takimin skor yazisi 
    private int team1score = 0; // skorlar baslangicta sifir
    private int team2score = 0;
    private void OnTriggerEnter2D(Collider2D colllision)
    {
        if (colllision.CompareTag("ball")) // carpan nesnenin tagi ball ise
        {
            if (gameObject.CompareTag("goal")) // carpan nesne goal tagina sahipse(goal tagi takim 1e ait)
            {
                team2score++; // takim 2 nin skorunu 1 artir
                Debug.Log("Puan takim 2 ye gidiyor");
                team2text.text = team2score.ToString();

                if (team2score >= 2)// takim 2 skoru, 2 veya daha fazla ise
                {
                    Time.timeScale = 0; // oyunu durdur
                }
            }
            if (gameObject.CompareTag("goal2")) // carpan nesne goal2 tagina sahipse(goal2 tagi takim2 ye ait)
            {
                team1score++; // takim 1 in skorunu 1 artir
                Debug.Log("Puan takim 1 e gidiyor");
                team1text.text = team1score.ToString();
                if (team1score >= 2) // takim 1 skoru, 2 veya daha fazla ise
                {
                    Time.timeScale = 0; // oyunu durdur
                }
            }
        }
    }
}
