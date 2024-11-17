using UnityEngine;

public class TriggerDetection : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("GOTCHA Trigger");
        }
    }
}
