using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTerrain : MonoBehaviour
{ 
    private float detailScale = 60.0f;
    private Mesh mesh;
    private Vector3[] vertices;
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        for (int v = 0; v < vertices.Length; v++)
        {
            float noise = (10.0f  * Mathf.PerlinNoise((vertices[v].x + transform.position.x) / detailScale, (vertices[v].z + transform.position.z) / detailScale) + 5.0f * Mathf.PerlinNoise(2 * (vertices[v].x + transform.position.x) / detailScale, 2 * (vertices[v].z + transform.position.z) / detailScale) + 2.5f * Mathf.PerlinNoise(4 * (vertices[v].x + transform.position.x) / detailScale, 4 * (vertices[v].z + transform.position.z) / detailScale)) / 4.0f;
            vertices[v].y = Mathf.Pow(noise, 4.0f);
        }

        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        gameObject.AddComponent<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
