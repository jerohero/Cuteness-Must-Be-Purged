using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour {
    [SerializeField] TextMeshProUGUI killsText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] Image[] hearts;
    [SerializeField] Sprite filledHeart;
    [SerializeField] Sprite emptyHeart;
    [SerializeField] GameObject gameOverScreen;

    public static UI instance { get; private set; }

    void Start() {
        instance = this;
    }

    void Update() {
        UpdateTimeText();
    }

    public void UpdateKillText() {
        killsText.text = GameSession.instance.GetKills().ToString();
    }

    public void UpdateHealthUI(int newHealth) {
        for (int i = 0; i < hearts.Length; i++) {
            if (i < newHealth) {
                hearts[i].sprite = filledHeart;
            } else {
                hearts[i].sprite = emptyHeart;
            }
        }
    }

    public void UpdateTimeText() {
        timeText.text = GameSession.instance.GetElapsedTime().ToString("F2");
    }

    public void ShowGameOverScreen() {
        gameOverScreen.SetActive(true);
    }

}
