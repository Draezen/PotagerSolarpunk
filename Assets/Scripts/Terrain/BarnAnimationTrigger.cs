using UnityEngine;

public class BarnAnimationTrigger : MonoBehaviour
{
    [Header("SFX details")]
    [SerializeField] private AudioSource creakyDoorShort;
    [SerializeField] private AudioSource creakyDoorLong;
    [SerializeField] private AudioSource slammingDoor;

    public void PlayCreakyDoorShort() => creakyDoorShort.Play();

    public void PlayCreakyDoorLong() => creakyDoorLong.Play();

    public void PlaySlammingDoor() => slammingDoor.Play();
}
