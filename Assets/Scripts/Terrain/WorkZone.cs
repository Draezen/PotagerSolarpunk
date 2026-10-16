using System.Collections;
using TMPro;
using UnityEngine;

public class WorkZone : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI warningField;
    [SerializeField] private string originalText = "Go back to work!";

    private bool isOnField;
    private Coroutine blinkTextCo;

    private void OnTriggerEnter(Collider other)
    {
        {
            if (other.gameObject.CompareTag("Player"))
            {
                isOnField = true;
                warningField.text = "";

                if (blinkTextCo != null)
                    StopCoroutine(blinkTextCo);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        {
            if (other.gameObject.CompareTag("Player"))
            {
                isOnField = false;
                BlinkingText();
            }
        }
    }

    private void BlinkingText()
    {
        if (blinkTextCo != null)
            StopCoroutine(blinkTextCo);

        StartCoroutine(BlinkTextCo());
    }

    private IEnumerator BlinkTextCo()
    {
        while (!isOnField)
        {
            warningField.text = originalText;
            yield return new WaitForSeconds(.3f);
            warningField.text = "";
            yield return new WaitForSeconds(.3f);
        }
    }
}
