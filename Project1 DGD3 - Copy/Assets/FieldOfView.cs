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
    public Vector3[] flashpoints;

    public bool flash;
    
    private void Start()
    {

         points = new Vector3[5]
         {
             new Vector3(-5,-10,-2.4f),
             new Vector3(6,-10,-2.4f),
             new Vector3(-5,-10,1.8f),
             new Vector3(.5f,1,0.5f),
             new Vector3(6,-10,1.8f)
         };
         
         flashpoints = new Vector3[5]
         {
             new Vector3(-5,-10,-2.4f),
             new Vector3(6,-10,-2.4f),
             new Vector3(-5,-10,1.8f),
             new Vector3(.5f,1,0.5f),
             new Vector3(6,-10,1.8f)
         };

    }

    // Update is called once per frame
    void Update()
    {
        Mesh = new Mesh();


        if (!flash) Mesh.vertices = points;
        else Mesh.vertices = flashpoints;
        
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

    void MakeSmaller()
    {
        if (Physics.Raycast(points[1], Vector3.down, 1, LayerMask.NameToLayer("Wall")))
        {
            
        }
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.DrawLine(points[1], new Vector3(1,1,0));
    // }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController pp = other.gameObject.GetComponent<PlayerController>();
        if (pp != null)
        {
            GameManagerController.gm.chasing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController pp = other.gameObject.GetComponent<PlayerController>();
        if (pp != null)
        {
            GameManagerController.gm.chasing = false;
        }
    }

  
}
