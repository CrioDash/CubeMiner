using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fruit;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpriteCutter : MonoBehaviour
{

    public static SpriteCutter Instance;

    private void Start()
    {
        Instance = this;
    }

    public void Cut(List<Vector2> cutPoints, GameObject obj)
    {
        ParticleSystem system = obj.GetComponentInChildren<ParticleSystem>();

        obj.GetComponentInChildren<Rigidbody2D>().WakeUp();

        system.Play();
        system.transform.SetParent(null);
        Vector3 pos = obj.transform.position;
        pos.z -= 1;
        pos.y -= 1;
        system.transform.position = pos;
        system.transform.localScale = Vector3.one;


        SpriteRenderer rend = obj.GetComponent<SpriteRenderer>();

        Mesh firstMesh = new Mesh();
        Mesh secondMesh = new Mesh();

        Material mat = rend.material;
        mat.mainTexture = rend.sprite.texture;

        Vector2 min = rend.sprite.bounds.min;
        Vector2 max = rend.sprite.bounds.max;

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector2(min.x, max.y);
        vertices[1] = new Vector2(min.x, min.y);
        ;
        vertices[2] = new Vector2(max.x, min.y);
        vertices[3] = max;

        List<Vector3> firstVertices = new List<Vector3>();
        List<Vector3> secondVertices = new List<Vector3>();

        for (int i = 0; i < cutPoints.Count; i++)
        {
            cutPoints[i] = ClosestPoint(cutPoints[i], min, max);
        }

        if (cutPoints[0].x == cutPoints[1].y)
            cutPoints[0] = new Vector2(cutPoints[0].x * -1, cutPoints[0].y);
        if (cutPoints[0].y == cutPoints[1].y)
            cutPoints[0] = new Vector2(cutPoints[0].x, cutPoints[0].y*-1);

    List<Vector2> tempPoints = cutPoints;
        for (int i = 0; i < vertices.Length; i++)
        {
            if (tempPoints.Count != 1)
                firstVertices.Add(vertices[i]);
            else
                secondVertices.Add(vertices[i]);
            for (int j = 0; j < tempPoints.Count; j++)
            {
                
                if ( i==vertices.Length-1 || 
                     (Math.Abs(tempPoints[j].x - vertices[i].x) == 0 &&
                      Math.Abs(tempPoints[j].x - vertices[i + 1].x) == 0) ||
                     (Math.Abs(tempPoints[j].y - vertices[i].y) == 0 &&
                      Math.Abs(tempPoints[j].y - vertices[i + 1].y) == 0))
                {
                    Debug.Log(tempPoints[j]);
                    firstVertices.Add(tempPoints[j]);
                    secondVertices.Add(tempPoints[j]);
                    tempPoints.Remove(tempPoints[j]);
                   
                    break;
                }
            }
        }

        SetupMesh(firstMesh, firstVertices.ToArray(), max);
        SetupMesh(secondMesh, secondVertices.ToArray(), max);

        Vector2 velocity = obj.GetComponent<Rigidbody2D>().velocity;
        
        if(firstVertices.Count <=2 || secondVertices.Count <= 2)
            Debug.Log("Cringe");

        CopyGameObject(firstMesh, mat, velocity, obj, cutPoints.ToArray());
        CopyGameObject(secondMesh, mat, velocity, obj, cutPoints.ToArray());

        Destroy(obj.gameObject);

    }

    private int[] GetTriangles(int verticesLength)
    {
        List<int> triangles = new List<int>();
        switch (verticesLength)
        {
            case 3:
            {
                triangles.Add(0);
                triangles.Add(1);
                triangles.Add(2);
                break;
            }
            case 4:
            {
                triangles.Add(0);
                triangles.Add(2);
                triangles.Add(1);
                triangles.Add(0);
                triangles.Add(2);
                triangles.Add(3);
                break;
            }
            case 5:
            {
                triangles.Add(0);
                triangles.Add(2);
                triangles.Add(1);
                triangles.Add(0);
                triangles.Add(2);
                triangles.Add(3);
                triangles.Add(0);
                triangles.Add(3);
                triangles.Add(4);
                break;
            }
        }

        return triangles.ToArray();
    }

    private Vector2[] GetUV(Vector2 max, Vector3[] vertices)
    {
        Vector2[] result = new Vector2[vertices.Length];
        for(int i=0;i<vertices.Length;i++)
        {
            result[i].x = (vertices[i].x + max.x) / (max.x * 2);
            result[i].y = (vertices[i].y + max.y) / (max.y * 2);
        }
        return result;
    }

    private void SetupMesh(Mesh mesh, Vector3[] vertices, Vector2 maxBounds)
    {
        mesh.vertices = vertices.ToArray();
        mesh.triangles = GetTriangles(vertices.Length);
        mesh.uv = GetUV(maxBounds, vertices.ToArray());
        mesh.Optimize();
        mesh.RecalculateNormals();
    }

    private void CopyGameObject(Mesh mesh, Material mat, Vector2 velocity, GameObject orig, Vector2[] cutPoints)
    {
        
        GameObject gameObject = new GameObject();
        Vector3 pos = orig.transform.position;
        pos.z -= 1;
        gameObject.transform.position = pos;
        gameObject.transform.localScale = orig.transform.localScale;

        MeshFilter filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
        
        filter.mesh = mesh;

        MeshRenderer renderer = gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;

        renderer.material = mat;

        Rigidbody2D body = gameObject.AddComponent(typeof(Rigidbody2D)) as Rigidbody2D;
        body.velocity = velocity;
        
        body.AddForce(new Vector2(Random.Range(-750, 750), Random.Range(-500, -250)));
        body.AddTorque(5, ForceMode2D.Impulse);

        Destroy(gameObject.gameObject, 2);
    }

    Vector3 ClosestPoint(Vector3 p, Vector2 min, Vector2 max)
    {

        Vector3[] edgePoints = new Vector3[4];

       
        edgePoints[0].x = min.x;
        edgePoints[0].y = Mathf.Clamp(p.y, min.y, max.y);

        
        edgePoints[1].x = max.x;
        edgePoints[1].y = Mathf.Clamp(p.y, min.y, max.y);
        
        
        edgePoints[2].x = Mathf.Clamp(p.x, min.x, max.x);
        edgePoints[2].y = min.y;

        
        edgePoints[3].x = Mathf.Clamp(p.x, min.x, max.x);
        edgePoints[3].y = max.y;

        float angle = Mathf.Atan2(Vector2.zero.y - p.y, Vector2.zero.x - p.x) * Mathf.Rad2Deg + 180;


        Vector3 closest = Vector3.zero;

        if (angle is < 45 or > 315)
        {
            closest = edgePoints[1];
        }
        else if (angle is > 45 and < 135)
        {
            closest = edgePoints[3];
        }
        else if (angle is > 135 and < 225)
        {
            closest = edgePoints[0];
        }
        else if (angle is > 225 and < 315)
        {
            closest = edgePoints[2];
        }
        

        return closest;
    }
    
}
