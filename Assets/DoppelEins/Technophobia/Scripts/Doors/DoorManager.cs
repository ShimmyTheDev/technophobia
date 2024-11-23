using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DoorManager : MonoBehaviour
{

    [SerializeField] private Transform LeftDoor;
    [SerializeField] private Transform RightDoor;
    [SerializeField] private GameObject Light;
    [SerializeField] private Material RedMat;
    [SerializeField] private Material GreenMat;
    public bool ShouldOpenDoor = false;
    public bool DoorOpened = false;
    [SerializeField] AudioClip OpenSound;
    [SerializeField] AudioClip LightOnSuound;
    AudioSource audioSource;
    int numberOfPlays = 0;
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ShouldOpenDoor = true;
        }
        if (ShouldOpenDoor && !DoorOpened)
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        
        if (!audioSource.isPlaying && numberOfPlays == 0)
        {
            audioSource.PlayOneShot(OpenSound); 
            numberOfPlays++;
        }
        // Define the default and target positions for both doors
        Vector3 leftDefaultPosition = new Vector3(-0.75f, -5.5f, 0f);
        Vector3 rightDefaultPosition = new Vector3(0.75f, -5.5f, 0f);
        Vector3 leftTargetPosition = new Vector3(-2.24f, -5.5f, 0f);
        Vector3 rightTargetPosition = new Vector3(2.24f, -5.5f, 0f);

        // Smoothly move the doors towards their target positions
        LeftDoor.localPosition = Vector3.Lerp(LeftDoor.localPosition, leftTargetPosition, 2f * Time.deltaTime);
        RightDoor.localPosition = Vector3.Lerp(RightDoor.localPosition, rightTargetPosition, 2f * Time.deltaTime);

        // Define a small tolerance for comparison
        float threshold = 0.01f;
        
        // Check if both doors are close enough to their target positions
        if (Mathf.Abs(LeftDoor.localPosition.x - leftTargetPosition.x) < threshold &&
            Mathf.Abs(RightDoor.localPosition.x - rightTargetPosition.x) < threshold)
        {
            // Snap doors to their exact target positions
            LeftDoor.localPosition = leftTargetPosition;
            RightDoor.localPosition = rightTargetPosition;

            // Change light to green material and update door states
            Light.GetComponent<Renderer>().material = GreenMat;
            ShouldOpenDoor = false;
            DoorOpened = true;
            audioSource.PlayOneShot(LightOnSuound);
        }
    }


}
