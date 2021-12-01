using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStat : MonoBehaviour
{
    [SerializeField]
    Text scoresText;

    [SerializeField]
    GameObject levelFinishedPanel;

    [SerializeField]
    Text finalScoresText;

    [SerializeField]
    GameObject nextButton;

    void Start()
    {
        levelFinishedPanel.SetActive(false);
        nextButton.SetActive(false);
    }

    int scores = 0;
    int levelId = 1;

    void OnEnable()
    {
        PlayerControl.PickedCoin += AddScores;
        PlayerControl.LevelFinished += ShowLevelFinishedPanel;
    }

    void OnDisable()
    {
        PlayerControl.PickedCoin -= AddScores;
        PlayerControl.LevelFinished -= ShowLevelFinishedPanel;
    }

    void AddScores()
    {
        scores += 1000;
        scoresText.text = "Scores: " + scores.ToString();
    }

    void ShowLevelFinishedPanel()
    {
        StartCoroutine(ScoresIncreaseAnim());
    }

    IEnumerator ScoresIncreaseAnim()
    {
        yield return new WaitForSeconds(2.5f);
        levelFinishedPanel.SetActive(true);
        nextButton.SetActive(true);
        int totalScores = scores + 10000 - (int) (Time.time * Time.time);
        for (int i = 1; i < 1001; i++)
        {
            finalScoresText.text = Mathf.Lerp(0, totalScores, i*1f/100f).ToString();
            yield return new WaitForEndOfFrame();
        }
        nextButton.SetActive(true);
    }
}
