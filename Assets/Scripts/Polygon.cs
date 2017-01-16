using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

[ExecuteInEditMode]
public class Polygon : MonoBehaviour
{
    public Color Color;

    [HideInInspector]
    public PolygonCollider2D PC2D;

    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    void Awake()
    {
        PC2D = GetComponent<PolygonCollider2D>();
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        meshRenderer.sharedMaterial.color = Color;

        UpdateMeshFilter();
    }

#if UNITY_EDITOR
    void Update()
    {
        if (Application.isPlaying || PC2D == null) return;

        meshRenderer.sharedMaterial.color = Color;

        UpdateMeshFilter();
    }
#endif

    public void TransformTo(Polygon target, float time, Action callback)
    {
        StartCoroutine(AnimateTo(target, time, callback));
    }

    IEnumerator AnimateTo(Polygon target, float time, Action callback)
    {
        Vector3 startingPos = transform.position;
        Vector2[] startingPoints = PC2D.points;

        float timePassed = 0;
        float progress = 0;
        while (progress < 0.99999f)
        {
            timePassed += Time.deltaTime;
            progress = Mathf.Min(timePassed / time, 1.0f);

            Vector2[] newPoints = new Vector2[PC2D.points.Length];
            for (int i = 0; i < PC2D.points.Length; ++i)
            {
                transform.position = Vector3.Lerp(startingPos, target.transform.position, progress);
                newPoints[i] = Vector2.Lerp(startingPoints[i], target.PC2D.points[i], progress);
            }
            PC2D.points = newPoints;

            meshRenderer.sharedMaterial.color = Color.Lerp(Color, target.Color, progress);

            UpdateMeshFilter();

            yield return null;
        }

        callback();
    }

    void UpdateMeshFilter()
    {
        int pointCount = PC2D.GetTotalPointCount();

        Mesh mesh = new Mesh();
        Vector2[] points = PC2D.points;
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