using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class FieldOfView : MonoBehaviour
{
    private Mesh Mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uv;
    private MeshCollider collide;
    private void Awake()
    {
        Mesh = new Mesh();
        
        
        Mesh.vertices = new Vector3[4]
        {
            new Vector3(0,0,0),
            new Vector3(1,0,0),
            new Vector3(0,0,1),
            new Vector3(-1,1,0),
        };
        Mesh.triangles = new int[12]
        {
            0,1,2,
            0,3,1,
            2,1,3,
            3,0,2
            
        };
        Mesh.uv = new Vector2[4]
        {
new Vector2(0,0),
new Vector2(0,0),
new Vector2(0,0),
new Vector2(0,0),


        };
        GetComponent<MeshFilter>().mesh = Mesh;
        GetComponent<MeshCollider>().sharedMesh = Mesh;

    }

    // Update is called once per frame
    void Update()
    {
        // vertices = new Vector3[3]
        // {
        //     new Vector3(0,0,0),
        //     new Vector3(1,0,0),
        //     new Vector3(0,0,1)
        // };
        // triangles = new int[3]
        // {
        //     0,1,2,
        // };
    }
}
