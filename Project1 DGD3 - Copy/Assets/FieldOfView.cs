using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class FieldOfView : MonoBehaviour
{
    private Mesh Mesh;
    public Vector3 vertices;
    private int[] triangles;
    private Vector2[] uv;
    private MeshCollider collide;
    public Vector3[] points;
    
    private void Awake()
    {
//         Mesh = new Mesh();
//
         points = new Vector3[5]
         {
             new Vector3(-5,-10,-2.4f),
             new Vector3(6,-10,-2.4f),
             new Vector3(-5,-10,1.8f),
             new Vector3(.5f,1,0.5f),
             new Vector3(6,-10,1.8f)
         };
//         Mesh.vertices = points;
//         
//         Mesh.triangles = new int[21]
//         {
//             0,1,2,
//             0,3,1,
//             2,1,3,
//             3,0,2,
//             1,4,2,
//             4,1,3,
//             2,4,3,
//         };
//         Mesh.uv = new Vector2[5]
//         {
// new Vector2(0,0),
// new Vector2(0,0),
// new Vector2(0,0),
// new Vector2(0,0),
// new Vector2(0,0),
//
//
//         };
//         GetComponent<MeshFilter>().mesh = Mesh;
//         GetComponent<MeshCollider>().sharedMesh = Mesh;

    }

    // Update is called once per frame
    void Update()
    {
        Mesh = new Mesh();

      
        Mesh.vertices = points;
        
        Mesh.triangles = new int[21]
        {
            0,1,2,
            0,3,1,
            2,1,3,
            3,0,2,
            1,4,2,
            4,1,3,
            2,4,3,
        };
        Mesh.uv = new Vector2[5]
        {
            new Vector2(0,0),
            new Vector2(0,0),
            new Vector2(0,0),
            new Vector2(0,0),
            new Vector2(0,0),


        };
        GetComponent<MeshFilter>().mesh = Mesh;
        GetComponent<MeshCollider>().sharedMesh = Mesh;
    }
}
