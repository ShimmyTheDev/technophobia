using TMPro;
using UnityEngine;

public class Secret : MonoBehaviour
{
    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private DoorManager _doorManager;
    [SerializeField] private string _secretCode;
    [SerializeField] private TMP_Text _secretText;


    void Awake()
    {
        _cameraManager.CameraStatusChanged += OnCameraStatusChanged;
    }

    void OnDestroy()
    {
        _cameraManager.CameraStatusChanged -= OnCameraStatusChanged;
    }

    void OnCameraStatusChanged()
    {
        if(!_doorManager) return;
        _doorManager.ShouldOpenDoor = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.SecrectsReady && _secretCode == "")
        {
            var cameraPairs = GameManager.Instance.SecretCodes;
            foreach (var pair in cameraPairs)
                if (pair.Value == _cameraManager)
                {

                    _secretCode = pair.Value.Passcode;
                    _secretText.text = _secretCode;
                }
        }
    }
}
