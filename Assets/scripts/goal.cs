using TMPro;
using UnityEngine;

public class goal:MonoBehaviour
{
    public TextMeshProUGUI team1text; // 1. takýmýn skor yazýsý
    public TextMeshProUGUI team2text; // 2. takýmýn skor yazýsý 
    private int team1score = 0; // skorlar baþlangýçta sýfýr
    private int team2score = 0;
    private void OnTriggerEnter2D(Collider2D colllision)
    {
       if(colllision.CompareTag("ball")) // çarpan nesnenin tagý ball ise
        {
            if(gameObject.CompareTag("goal")) // çarpan nesne goal tagýna sahipse(goal tagý takým 1e ait)
            {
                team2score++; // takým 2 nin skorunu 1 artýr
                Debug.Log("Puan takým 2 ye gidiyor");
                team2text.text = team2score.ToString();

                if(team2score>=2)// takým 2 skoru, 2 veya daha fazla ise
                {
                    Time.timeScale = 0; // oyunu durdur
                }
            }
            if(gameObject.CompareTag("goal2")) // çarpan nesne goal2 tagýna sahipse(goal2 tagý takým2 ye ait)
            {
                team1score++; // takým 1 in skorunu 1 artýr
                Debug.Log("Puan takým 1 e gidiyor");
                team1text.text = team1score.ToString();
                if (team1score >= 2) // takým 1 skoru, 2 veya daha fazla ise
                {
                    Time.timeScale = 0; // oyunu durdur
                }
            }
        }
    }
}