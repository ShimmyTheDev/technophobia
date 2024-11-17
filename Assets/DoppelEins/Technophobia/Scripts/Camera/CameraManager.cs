using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class CameraManager : MonoBehaviour
{
    public string Passcode;

    [Header("Cone Settings")] [SerializeField]
    private Transform coneTipTransform; // Transform where the tip of the cone is located

    [SerializeField] private float coneHeight = 5f; // Height of the cone
    [SerializeField] private float coneRadius = 3f; // Radius at the base of the cone
    [SerializeField] private Material coneMaterial; // Material for the cone
    [SerializeField] private float coneOffsetZ = 1f; // Offset along the Z-axis
    private Mesh coneMesh;

    private GameObject coneObject;
    private MeshCollider meshCollider;
    private MeshFilter meshFilter;
    public bool isEnabled { get; private set; } = true;

    private void Start()
    {
        CreateConeObject();
        GenerateConeMesh();
    }

    private void Update()
    {
        if (GameManager.Instance.SecrectsReady && Passcode == "") GetPasscode();
        FollowConeTipTransform();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + " entered");
        if (other.CompareTag("Player")) Debug.Log("GOCHA!");
    }

    public event Action OnState;

    private void GetPasscode()
    {
        var cameraPairs = GameManager.Instance.SecretCodes;
        foreach (var pair in cameraPairs)
            if (pair.Value == this)
                Passcode = pair.Key;
    }

    private void CreateConeObject()
    {
        // Create a child GameObject to hold the cone mesh
        coneObject = new GameObject("VisionCone");
        coneObject.transform.parent = coneTipTransform;
        coneObject.transform.localPosition = Vector3.forward * coneOffsetZ;

        coneObject.transform.localRotation = Quaternion.Euler(180, 0, 0);

        meshFilter = coneObject.AddComponent<MeshFilter>();
        var meshRenderer = coneObject.AddComponent<MeshRenderer>();
        meshCollider = coneObject.AddComponent<MeshCollider>();

        meshRenderer.material = coneMaterial;
        meshCollider.convex = true;
        meshCollider.isTrigger = true;
        coneObject.AddComponent<TriggerDetection>();
    }

    private void GenerateConeMesh()
    {
        coneMesh = new Mesh();
        var segments = 20;
        var vertices = new Vector3[segments + 2];
        var triangles = new int[segments * 3];

        vertices[0] = Vector3.forward; // Tip of the cone at the origin

        for (var i = 0; i <= segments; i++)
        {
            var angle = (float)i / segments * Mathf.PI * 2;
            var x = Mathf.Cos(angle) * coneRadius;
            var z = Mathf.Sin(angle) * coneRadius;
            vertices[i + 1] = new Vector3(x, -coneHeight, z);

            if (i < segments)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        coneMesh.vertices = vertices;
        coneMesh.triangles = triangles;
        coneMesh.RecalculateNormals();

        // Assign the mesh to both the MeshFilter and MeshCollider
        meshFilter.mesh = coneMesh;
        meshCollider.sharedMesh = coneMesh;
    }

    private void FollowConeTipTransform()
    {
        coneObject.transform.position = coneTipTransform.position + coneTipTransform.forward * coneOffsetZ;
    }

    public void DisableCamera()
    {
        Debug.Log("Disabling camera");
        coneObject.SetActive(false);
        GetComponent<Animation>().enabled = false;
        isEnabled = false;
    }
}