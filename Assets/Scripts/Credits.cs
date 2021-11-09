using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour {
    [SerializeField] GameObject creditsPopUp;
    [SerializeField] Button creditsButton;

    void Start() {
        creditsButton.onClick.AddListener(ShowCredits);
    }

    public void ShowCredits() {
        creditsPopUp.SetActive(true);

        creditsButton.onClick.RemoveListener(ShowCredits);
        creditsButton.onClick.AddListener(HideCredits);
    }

    public void HideCredits() {
        creditsPopUp.SetActive(false);

        creditsButton.onClick.RemoveListener(HideCredits);
        creditsButton.onClick.AddListener(ShowCredits);
    }
}
