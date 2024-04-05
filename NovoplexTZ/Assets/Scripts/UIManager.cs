using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private TextMeshProUGUI cash;

    private int currentCash = 0;
    private string cashKey = "CurrentCash";

    private void Start()
    {
        winText.gameObject.SetActive(false);
        LoadCash(); 
        UpdateCashText();
    }

    public void UpdateMoney(string prize)
    {
        int prizeAmount = int.Parse(prize);
        currentCash += prizeAmount; 
        UpdateCashText(); 
        SaveCash(); 
        StartCoroutine(ShowWinText(prize));
    }

    private void UpdateCashText()
    {
        cash.text = "Cash: " + currentCash.ToString();
    }

    private IEnumerator ShowWinText(string prize)
    {
        winText.gameObject.SetActive(true);
        winText.text = "YOU WIN: " + prize;

        yield return new WaitForSeconds(3f);

        winText.gameObject.SetActive(false);
    }

    private void SaveCash()
    {
        PlayerPrefs.SetInt(cashKey, currentCash);
        PlayerPrefs.Save();
    }

    private void LoadCash()
    {
        if (PlayerPrefs.HasKey(cashKey))
        {
            currentCash = PlayerPrefs.GetInt(cashKey);
        }
    }
}
