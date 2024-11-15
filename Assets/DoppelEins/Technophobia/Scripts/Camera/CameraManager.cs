using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class CameraManager : MonoBehaviour
{
    public string Passcode { get; private set; }
    [SerializeField] private bool isEnabled = false;
    public event Action OnState;

    [Header("Cone Settings")]
    [SerializeField] private Transform coneTipTransform;  // Transform where the tip of the cone is located
    [SerializeField] private float coneHeight = 5f;       // Height of the cone
    [SerializeField] private float coneRadius = 3f;       // Radius at the base of the cone
    [SerializeField] private Material coneMaterial;       // Material for the cone
    [SerializeField] private float coneOffsetZ = 1f;      // Offset along the Z-axis

    private GameObject coneObject;
    private Mesh coneMesh;
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;

    private void Start()
    {
        CreateConeObject();
        GenerateConeMesh();
    }

    private void Update()
    {
        FollowConeTipTransform();
    }

    private void CreateConeObject()
    {
        // Create a child GameObject to hold the cone mesh
        coneObject = new GameObject("VisionCone");
        coneObject.transform.parent = coneTipTransform;
        coneObject.transform.localPosition = Vector3.forward * coneOffsetZ;

        coneObject.transform.localRotation = Quaternion.Euler(180, 0, 0);

        meshFilter = coneObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = coneObject.AddComponent<MeshRenderer>();
        meshCollider = coneObject.AddComponent<MeshCollider>();

        meshRenderer.material = coneMaterial;

        meshCollider.convex = true;
        meshCollider.isTrigger = true;
    }

    private void GenerateConeMesh()
    {
        coneMesh = new Mesh();
        int segments = 20;
        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        vertices[0] = Vector3.forward; // Tip of the cone at the origin

        for (int i = 0; i <= segments; i++)
        {
            float angle = (float)i / segments * Mathf.PI * 2;
            float x = Mathf.Cos(angle) * coneRadius;
            float z = Mathf.Sin(angle) * coneRadius;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("GOCHA!");
        }
    }
}
