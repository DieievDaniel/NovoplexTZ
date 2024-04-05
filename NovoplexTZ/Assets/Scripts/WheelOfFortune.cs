using System.Collections;
using UnityEngine;

public class WheelOfFortune : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private string[] prizes;
    [SerializeField] private GameObject wheel;
    [SerializeField] private float spinDuration;
    [SerializeField] private GameObject[] wheelSections;
    [SerializeField] private AnimationCurve rotationCurve;

    private bool isSpinning;

    public void SpinWheel()
    {
        if (!isSpinning)
        {
            StartCoroutine(SpinWheelCoroutine());
        }
    }

    private IEnumerator SpinWheelCoroutine()
    {
        isSpinning = true;      
        float elapsedTime = 0f;
        float startAngle = wheel.transform.eulerAngles.z;
        float stopAngle = 360f / wheelSections.Length * Random.Range(0, wheelSections.Length) + 1800f;

        while (elapsedTime < spinDuration)
        {
            float angle = Mathf.Lerp(startAngle, stopAngle, rotationCurve.Evaluate(elapsedTime / spinDuration));
            wheel.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        wheel.transform.rotation = Quaternion.Euler(0f, 0f, stopAngle);
        isSpinning = false;

        int stoppedIndex = Mathf.FloorToInt(stopAngle % 360f / (360f / wheelSections.Length));
        string prize = prizes[stoppedIndex];
        uiManager.UpdateMoney(prize);
    }
}
