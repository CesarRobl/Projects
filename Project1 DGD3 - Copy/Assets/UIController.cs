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
    [HideInInspector] public Vector3[] points, save, charge, save2;
        public Vector3[] sprint, save3;
   

    private int n , m ,l  ,p , checkpoint;
    private int u, b, o, r, checkpoint2;
    private int g, h, j, k, checkpoint3;
    public bool chargemesh, flashmesh,Sprint;
  
    void Start()
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
        sprint = new Vector3[4]
        {
new Vector3(0,0,0),
new Vector3(0.6f,0,0),
new Vector3(0,2.5f,0),
new Vector3(0.6f,2.5f,0)
        };
        
        save3 = new Vector3[4]
        {
            new Vector3(0,0,0),
            new Vector3(0.6f,0,0),
            new Vector3(0,2.5f,0),
            new Vector3(0.6f,2.5f,0)
        };

    }

    // Update is called once per frame
    void Update()
    {
        if (!Sprint)
        {
            DrawMesh();
            if(flashmesh) FlashMesh();
            if(chargemesh) ChargeMesh();
        }
        else
        {
            DrawSprint();
            SprintMesh();
        }
      
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

    void DrawSprint()
    {
        Mesh = new Mesh();
        Mesh.vertices = sprint;
       
        Mesh.triangles = new int[6]
        {
           0,1,2,
           2,3,1
            
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

    void SprintMesh()
    {
        if (GameManagerController.gm.crouching == GameManagerController.PlayerCrouching.Running)
        {
            for (int i = 0; i < 1; i++)
            {

                sprint[g] = Vector3.MoveTowards(sprint[g], sprint[j], .25f * Time.deltaTime);
                sprint[h] = Vector3.MoveTowards(sprint[h], sprint[k], .25f * Time.deltaTime);

            }
            g = 2;
            h = 3;
            j = 0;
            k = 1;
            if (sprint[g] == sprint[j])
                GameManagerController.gm.exhausted = true;
        }
        else
        {
            for (int i = 0; i < 1; i++)
            {

                sprint[g] = Vector3.MoveTowards(sprint[g], save3[g], .25f * Time.deltaTime);
                sprint[h] = Vector3.MoveTowards(sprint[h], save3[h], .25f * Time.deltaTime);

            }
            g = 2;
            h = 3;
            if (sprint[g] == save3[g] && GameManagerController.gm.exhausted)  GameManagerController.gm.exhausted = false;
        }
    }

    void FlashMesh()
    {
       
        if (checkpoint == 2) GameManagerController.gm.nobattery = true;
        else if (checkpoint == 1 && GameManagerController.gm.nobattery)
        {
            GameManagerController.gm.nobattery = false;
            GameManagerController.gm.lights = true;
        }
        
            if (GameManagerController.gm.lights)
            {

                for (int i = 0; i < 1; i++)
                {

                    points[n] = Vector3.MoveTowards(points[n], points[l], .1f * Time.deltaTime);
                    points[r] = Vector3.MoveTowards(points[r], points[p], .1f * Time.deltaTime);
                    if (points[n] == points[l]) checkpoint++;
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
        

        if (!GameManagerController.gm.lights)
            {
               
                for (int i = 0; i < 1; i++)
                {
                    points[n] = Vector3.MoveTowards(points[n], save[n], .1f * Time.deltaTime);
                    points[r] = Vector3.MoveTowards(points[r], save[r], .1f * Time.deltaTime);

                    if (points[n] == save[n]) checkpoint -= 1;
                }

                if (points[4] == save[4]) checkpoint = 0;
                if (checkpoint == 0)
                {
                    n = 4;
                    r = 5;
                }

                if (checkpoint == 1 || checkpoint == 2)
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
                if (checkpoint2 == 0)
                {
                    u = 1;
                    r = 3;
                    charge[4] = charge[1];
                    charge[5] = charge[3];
                }

                if (checkpoint2 == 1)
                {
                    u = 4;
                    r = 5;
                }

                if (charge[4] == save[4]) checkpoint = 2;

                for (int i = 0; i < 1; i++)
                {
                    charge[u] = Vector3.MoveTowards(charge[u], save2[u],
                        (Input.GetAxis("Mouse ScrollWheel")
                         * GameManagerController.gm.scrollwheel) * Time.deltaTime);

                    charge[r] = Vector3.MoveTowards(charge[r], save2[r],
                        (Input.GetAxis("Mouse ScrollWheel") * GameManagerController.gm.scrollwheel) * Time.deltaTime);
                    if (charge[u] == save2[u] && checkpoint2 != 2) checkpoint2++;

                }




            }

            else
            {
                if (checkpoint2 == 2 || checkpoint2 == 1)
                {
                    u = 4;
                    r = 5;
                    b = 1;
                    o = 3;
                }

                if (checkpoint2 == 0)
                {
                    u = 1;
                    r = 3;
                    b = 0;
                    o = 2;
                    charge[4] = charge[1];
                    charge[5] = charge[3];
                }


                for (int i = 0; i < 1; i++)
                {
                    charge[u] = Vector3.MoveTowards(charge[u], charge[b],
                        3 * Time.deltaTime);

                    charge[r] = Vector3.MoveTowards(charge[r], charge[o],
                        3 * Time.deltaTime);

                    if (charge[u] == charge[b] && checkpoint2 != 0) checkpoint2--;

                }

                if (charge[1] == charge[0]) checkpoint2 = 0;

            }

        }
    }


