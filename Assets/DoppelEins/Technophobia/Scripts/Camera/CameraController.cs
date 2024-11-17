using UnityEngine;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class CameraController : MonoBehaviour
{
    public Transform startTransform;     // The transform from which the cone originates
    public float height = 100f;            // Height of the cone
    public float baseRadius = 40f;        // Base radius of the cone
    public float startRadius = 0f;    // Small starting radius at the cone tip
    public int segments = 64;            // Number of segments around the base
    public Material coneMaterial;        // Custom material for the cone

    private Mesh mesh;
    private MeshCollider meshCollider;

    private void Start()
    {
        // Initialize the mesh and assign material
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material = coneMaterial;

        // Initialize the MeshCollider and set it to trigger
        meshCollider = GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh; // Assign the mesh to the collider
        meshCollider.convex = true;     // Enable convex for it to be a trigger
        meshCollider.isTrigger = true;  // Set collider as a trigger

        // Create the cone mesh
        CreateConeMesh();
    }

    private void Update()
    {
        // Optional: Update the cone if parameters change at runtime
        CreateConeMesh();
        meshCollider.sharedMesh = mesh; // Update the mesh collider whenever the mesh changes
    }

    private void CreateConeMesh()
    {
        mesh.Clear();

        // Define vertices and triangles arrays
        Vector3[] vertices = new Vector3[segments + 2];   // +2 for tip and center of base
        int[] triangles = new int[segments * 3 * 2];      // Extra triangles for the base cap

        // 1. Tip vertex (at the startTransform position)
        vertices[0] = startTransform.position; // Directly use the position of startTransform

        // 2. Base center vertex (at height in front of startTransform)
        Vector3 baseCenter = startTransform.position + startTransform.forward * height;
        vertices[segments + 1] = baseCenter;

        // 3. Angle step for each segment
        float angleStep = 360f / segments;

        // 4. Generate the vertices for the base circle
        for (int i = 0; i < segments; i++)
        {
            // Calculate the angle around the base circle
            float angle = i * angleStep * Mathf.Deg2Rad;

            // 5. Base vertices (at the radius of the base)
            float baseX = Mathf.Cos(angle) * baseRadius;
            float baseZ = Mathf.Sin(angle) * baseRadius;

            // Calculate the base vertices in world space
            Vector3 baseVertex = startTransform.position + new Vector3(baseX, 0, baseZ) + startTransform.forward * height;
            vertices[i + 1] = baseVertex;

            // 6. Define triangles for the cone's side
            int nextIndex = (i + 1) % segments + 1; // Next vertex index, wrapping around at the end
            triangles[i * 3] = 0;               // Tip vertex
            triangles[i * 3 + 1] = i + 1;       // Current base vertex
            triangles[i * 3 + 2] = nextIndex;   // Next base vertex

            // 7. Define triangles for the base cap
            int nextBaseIndex = (i + 1) % segments + 1;
            triangles[segments * 3 + i * 3] = segments + 1;      // Center of base
            triangles[segments * 3 + i * 3 + 1] = i + 1;         // Current base vertex
            triangles[segments * 3 + i * 3 + 2] = nextBaseIndex; // Next base vertex
        }

        // 8. Apply vertices and triangles to the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered vision cone!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited vision cone!");
        }
    }
}