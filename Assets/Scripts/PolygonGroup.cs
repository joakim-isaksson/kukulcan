using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonGroup : MonoBehaviour {

    public PolygonGroup TargetGroup;
    public float TransformTime;

    Polygon[] polygons;

    bool transforming;
    int waitingForPolygons;

    void Awake()
    {
        polygons = GetComponentsInChildren<Polygon>();
    }

    void Update()
    {
        if (!transforming && TargetGroup != null && Input.anyKeyDown)
        {
            StartCoroutine(TransformTo(TargetGroup));
        }
    }

    IEnumerator TransformTo(PolygonGroup target)
    {
        transforming = true;
        waitingForPolygons = polygons.Length;

        for (int i = 0; i < polygons.Length; ++i)
        {
            polygons[i].TransformTo(target.polygons[i], TransformTime, OnPolygonReady);
        }

        while(waitingForPolygons > 0) yield return null;
        transforming = false;
    }

    void OnPolygonReady()
    {
        waitingForPolygons--;
    }
}
