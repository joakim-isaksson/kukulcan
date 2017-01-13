using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonGroup : MonoBehaviour {

    public PolygonGroup TransformTo;
    public float TransformSpeed;

    Polygon[] polygons;

    bool transforming;

    void Awake()
    {
        polygons = GetComponentsInChildren<Polygon>();
    }

    void Update()
    {
        if (!transforming && TransformTo != null && Input.anyKeyDown)
        {
            StartCoroutine(Transform(TransformTo));
        }
    }

    IEnumerator Transform(PolygonGroup target)
    {
        transforming = true;

        bool ready;
        do
        {
            ready = false;
            for (int i = 0; i < polygons.Length; ++i)
            {
                if (polygons[i].MoveTowards(target.polygons[i], TransformSpeed)) ready = true;
            }
            yield return null;
        }
        while (!ready);

        transforming = false;
    }
}
