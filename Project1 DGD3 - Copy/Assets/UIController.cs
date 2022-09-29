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
    [HideInInspector]public Vector3[] points,save,charge, save2;
   

    private int n , m ,l  ,p , checkpoint;
    private int u, i, o, r, checkpoint2;
    public bool chargemesh, flashmesh,end;
  
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
        save = new Vector3[6]
        {
            new Vector3(0, 0, 0),
            new Vector3(0, 1.5f, 0),
            new Vector3(.5f, 0, 0),
            new Vector3(.5f, 1.5f, 0),
            new Vector3(-.5f, 2.3f, 0),
            new Vector3(1f, 2.3f, 0),
        };
        charge = new Vector3[6]
        {
            new Vector3(-0.1f,0,0),
            new Vector3(0,0,0),
            new Vector3(0.6f,0,0),
            new Vector3(0,0,0),
            new Vector3(0,0,0),
            new Vector3(0,0,0),
        };
        save2 = new Vector3[6]
        {
            new Vector3(-0.1f, 0, 0),
            new Vector3(-0.1f, 1.5f, 0),
            new Vector3(0.6f, 0, 0),
            new Vector3(0.6f, 1.5f, 0),
            new Vector3(-0.7f, 2.4f, 0),
            new Vector3(1.2f, 2.4f, 0),
        };

    }

    // Update is called once per frame
    void Update()
    {
        DrawMesh();
       if(flashmesh) FlashMesh();
        if(chargemesh) ChargeMesh();
    }

    void DrawMesh()
    {
        Mesh = new Mesh();
        if(!chargemesh)Mesh.vertices = points;
        else Mesh.vertices = charge;
       
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
        
        if(GameManagerController.gm.lights) {
            
            for (int i = 0; i < 1; i++) { 
                
            points[n] = Vector3.MoveTowards(points[n], points[l], .1f * Time.deltaTime);
            points[r] = Vector3.MoveTowards(points[r], points[p], .1f * Time.deltaTime);
            if (points[n] == points[l]) checkpoint ++;
            }

            if (points[1] == points[0])
            {
                checkpoint = 2;
            }
            if (checkpoint == 0)
            {
                n = 4;
                l = 1;
                r = 5;
                p = 3;
            }
            if (checkpoint == 1)
            {
                points[4] = points[1];
                points[5] = points[3];
                n = 1;
                l = 0;
                r = 3;
                p = 2;
            }
        }
        if(!GameManagerController.gm.lights) {
            for (int i = 0; i < 1; i++)
            {
                points[n] = Vector3.MoveTowards(points[n], save[n], .1f * Time.deltaTime);
                points[r] = Vector3.MoveTowards(points[r], save[r], .1f * Time.deltaTime);
        
                if (points[n] == save[n]) checkpoint -=1 ;
            }

            if (points[4] == save[4]) checkpoint = 0;
            if (checkpoint == 0)
            {
                n = 4;
                r = 5;
            }
                
            if (checkpoint == 1 || checkpoint==2)
            {
                n = 1;
                r = 3;
                points[4] = points[1];
                points[5] = points[3];
            }
        }
       
    }

    void ChargeMesh()
    {
        if (GameManagerController.pc.charging)
        {
            for (int i = 0; i < 1; i++)
            {
                charge[u] = Vector3.MoveTowards(charge[u], save2[u],
                    (Input.GetAxis("Mouse ScrollWheel")
                     * GameManagerController.gm.scrollwheel)* Time.deltaTime);
                
                charge[r] = Vector3.MoveTowards(charge[r], save2[r],
                    (Input.GetAxis("Mouse ScrollWheel")
                    
                     * GameManagerController.gm.scrollwheel)* Time.deltaTime);
                if (charge[u] == save2[u]) checkpoint2++;

            }

            if (charge[4] == save[4]) checkpoint = 2;
            
            if (checkpoint2 == 0)
            {
                u = 1;
                r = 3;
            }
        }
        
    }
    
}
