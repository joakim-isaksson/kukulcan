using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

[ExecuteInEditMode]
public class Polygon : MonoBehaviour
{
    public Color Color;

    [HideInInspector]
    public PolygonCollider2D PolygonCollider;

    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    float speed = 10.0f;

    public bool MoveTowards(Polygon target)
    {
        Vector2[] currentPos = PolygonCollider.points;
        Vector2[] targetPos = target.PolygonCollider.points;

        bool done = true;
        for (int i = 0; i < currentPos.Length; ++i)
        {
            currentPos[i] = Vector2.MoveTowards(currentPos[i], targetPos[i], Time.deltaTime * speed);
            if (Vector2.Distance(currentPos[i], targetPos[i]) > 0.001f) done = false;
        }

        Debug.Log("asd");

        UpdateMeshFilter();

        return done;
    }

    void Awake()
    {
        PolygonCollider = GetComponent<PolygonCollider2D>();
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        meshRenderer.sharedMaterial.color = Color;

        UpdateMeshFilter();
    }

#if UNITY_EDITOR
    void Update()
    {
        if (Application.isPlaying || PolygonCollider == null) return;

        meshRenderer.sharedMaterial.color = Color;

        UpdateMeshFilter();
    }
#endif

    void UpdateMeshFilter()
    {
        int pointCount = PolygonCollider.GetTotalPointCount();

        Mesh mesh = new Mesh();
        Vector2[] points = PolygonCollider.points;
        Vector3[] vertices = new Vector3[pointCount];
        for (int j = 0; j < pointCount; j++)
        {
            Vector2 actual = points[j];
            vertices[j] = new Vector3(actual.x, actual.y, 0);
        }

        Triangulator tr = new Triangulator(points);
        int[] triangles = tr.Triangulate();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        meshFilter.mesh = mesh;
    }
}