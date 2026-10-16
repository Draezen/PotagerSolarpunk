using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class EventSystemManager : MonoBehaviour
{
    public InputSystemUIInputModule inputModul;

    private void Awake()
    {
        inputModul = GetComponent<InputSystemUIInputModule>();
    }

    private void Start()
    {
        StartCoroutine(ResetInputModule());
    }

    private IEnumerator ResetInputModule()
    {
        inputModul.enabled = false;
        yield return null;
        inputModul.enabled = true;
    }
}
