using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text ScoreText;
    [SerializeField]
    private GameObject PausePanel;

    private int ScoreNum;
    public static UIManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void RenewScore(int _num)
    {
        ScoreNum += _num;
        ScoreText.text = ScoreNum.ToString();
    }

    public void SetActivePausePanel()
    {
        if (PausePanel.activeSelf)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
        PausePanel.SetActive(!PausePanel.activeSelf);
    }
}
