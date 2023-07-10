using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fieldOfView : MonoBehaviour
{

    /// <summary>
    /// This is all for the field of view and player detection from enemies (the yellow cone when they move around)
    /// I barely know how this works but it works, my first experience with meshes so its probably a mess
    /// </summary>
    /// 

    private Mesh mesh;
    [SerializeField] private LayerMask layerMask;
    private Vector3 origin;
    private float startingAngle = 225f;
    public float angle;
    private int playerLayer;
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        startingAngle = 225f;
        angle = 225f;
        playerLayer = LayerMask.NameToLayer("Player");
        
    }

    private void LateUpdate()
    {
        
    

        float fov = 70f;
        int rayCount = 50;
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;
        float viewDistance = 10f;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D rcHit = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, layerMask);
   
            if(rcHit.collider == null)
            {
                vertex = origin + GetVectorFromAngle(angle) * viewDistance;
            } 
            else if(rcHit.transform.gameObject.layer == playerLayer)
            {
                vertex = rcHit.point;
                //vertex = origin + GetVectorFromAngle(angle) * viewDistance;
                //Debug.Log("Player detected");
            }
            else
            {
                vertex = rcHit.point;
            }
            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }
            vertexIndex++;
            angle -= angleIncrease;
        }


        triangles[2] = 2;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();

    }

    public static Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    public void SetAimDirection(float newAngle)
    {
        startingAngle = newAngle;
    }
}
