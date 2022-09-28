using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashController : MonoBehaviour
{
    private Mesh Mesh;
    public Vector3 vertices;
    private int[] triangles;
    private Vector2[] uv;
    private MeshCollider collide;
    public Vector3[] points;
    void Start()
    {
        points = new Vector3[5]
        {
            new Vector3(0,1,0f),
            new Vector3(1,1,0),
            new Vector3(-1,1,0),
            new Vector3(0,1,1),
            new Vector3(0,1,-1)
            
          
        };
    }

    // Update is called once per frame
    void Update()
    {
        Mesh = new Mesh();

      
        Mesh.vertices = points;
        
        Mesh.triangles = new int[12]
        {
            0,1,4,
            0,2,3,
            0,3,1,
            0,4,2
            
          
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
