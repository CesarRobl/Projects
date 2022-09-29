using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public Mesh Mesh;
    public MeshFilter filter;
    public Vector3 vertices;
    private int[] triangles;
    private Vector2[] uv;
    private MeshCollider collide;
    public Vector3[] points;

    public bool chargemesh, flashmesh;
    void Awake()
    {
        points = new Vector3[6]
        {
            new Vector3(0, 0, 0),
            new Vector3(0, 1.5f, 0),
            new Vector3(.5f, 0, 0),
            new Vector3(.5f,1.5f,0),
            new Vector3(-.5f, 2.3f,0),
            new Vector3(1f,2.3f,0),
            
        };
    }

    // Update is called once per frame
    void Update()
    {
        DrawMesh();
    }

    void DrawMesh()
    {
        Mesh = new Mesh();
        Mesh.vertices = points;
       
        Mesh.triangles = new int[12]
        {
            1,2,0,
            1,3,2,
            4,3,1,
            4,5,3,
            
        };

        Mesh.uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(0, 0),
            new Vector2(0, 0),
            new Vector2(0, 0),
        };
        filter.mesh = Mesh;
    }

    void FlashMesh()
    {
        
    }

    void ChargeMesh()
    {
        
    }
    
}
