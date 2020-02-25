using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text ScoreText;

    private int ScoreNum;
    public static UIManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void PlusScore()
    {
        ++ScoreNum;
        ScoreText.text = ScoreNum.ToString();
    }
}
