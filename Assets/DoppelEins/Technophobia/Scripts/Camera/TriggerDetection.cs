using UnityEngine;

public class TriggerDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") Debug.Log("GOTCHA Trigger");
    }
}