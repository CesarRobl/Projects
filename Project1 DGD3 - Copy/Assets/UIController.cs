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

    private int n = 4, m = 5,l = 1,r = 5,p = 3, checkpoint;
    public bool chargemesh, flashmesh;
    private float delay = 3 ;
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
       if(flashmesh) FlashMesh();
        
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

        Mesh.uv = new Vector2[6]
        {
            new Vector2(0, 0),
            new Vector2(0, 0),
            new Vector2(0, 0),
            new Vector2(0, 0),
            new Vector2(0, 0),
            new Vector2(0, 0),
        };
        filter.mesh = Mesh;
    }

    void FlashMesh()
    {
         
        if(GameManagerController.gm.light) {for (int i = 0; i < 1; i++)
        {
            points[n] = Vector3.MoveTowards(points[n], points[l], .1f * Time.deltaTime);
            points[r] = Vector3.MoveTowards(points[r], points[p], .1f * Time.deltaTime);
            
            if (points[n] == points[l]) checkpoint++;
            

            if (checkpoint == 1)
            {
                points[4] = points[1];
                points[5] = points[3];
                n = 1;
                l = 0;
                r = 3;
                p = 2;
            }
        }}
       
    }

    void ChargeMesh()
    {
        
    }
    
}
