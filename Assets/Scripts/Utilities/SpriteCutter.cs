using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fruit;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using Variables = Data.Variables;

public class SpriteCutter : MonoBehaviour
{

    public static SpriteCutter Instance;

    private void Start()
    {
        Instance = this;
    }

    public GameObject[] CutTnt(GameObject obj)
    {
        Mesh[] meshes = new Mesh[5];
        for(int i =0; i < 5;i++)
        {
            meshes[i] = new Mesh();
        }
        
        SpriteRenderer rend = obj.GetComponent<SpriteRenderer>();

        rend.enabled = false;

        obj.GetComponent<BoxCollider2D>().enabled = false;
        
        Material mat = rend.material;
        mat.mainTexture = rend.sprite.texture;

        Vector2 min = rend.sprite.bounds.min;
        Vector2 max = rend.sprite.bounds.max;
        
        Vector3[][] vertices = new Vector3[5][];
        for(int i = 0; i<vertices.GetLength(0); i++)
        {
            vertices[i] = new Vector3[3];
        }

        float[] angles = new float[5];
        for (int i = 0; i < 5; i++)
        {
            angles[i] = Mathf.Deg2Rad*i * 360 / 5;
        }

        for (int i = 0; i < 5; i++)
        {
            vertices[i][0] = Vector3.zero;
            vertices[i][1] = new Vector3(max.x * Mathf.Cos(angles[i]), max.y * Mathf.Sin(angles[i]), 0);
            if(i==4)
                vertices[i][2] = new Vector3(max.x * Mathf.Cos(angles[0]), max.y * Mathf.Sin(angles[0]), 0);
            else
                vertices[i][2] = new Vector3(max.x * Mathf.Cos(angles[i+1]), max.y * Mathf.Sin(angles[i+1]), 0);
        }

        GameObject[] objects = new GameObject[5];
        for(int i = 0; i<meshes.Length;i++)
        {
            float angle;
            if(i==meshes.Length-1)
                angle =(angles[i] + Mathf.Deg2Rad*360) / 2;
            else
                angle = (angles[i] + angles[i + 1]) / 2;
            Vector2 velocity = new Vector2(150 * Mathf.Cos(angle), 150 * Mathf.Sin(angle));
            SetupMesh(meshes[i], vertices[i], max);
            objects[i] = CopyGameObject(meshes[i], mat, obj, velocity,false);
            objects[i].transform.localScale *= 1.25f;
        }

        return objects;

    }

    public void CutFruit(List<Vector2> cutPoints, GameObject obj)
    {
        obj.GetComponentInChildren<Rigidbody2D>().WakeUp();

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

        CopyGameObject(firstMesh, mat, obj, Vector2.zero,true);
        CopyGameObject(secondMesh, mat, obj, Vector2.zero,true);

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

    private GameObject CopyGameObject(Mesh mesh, Material mat, GameObject orig, Vector2 force, bool fade)
    {
        
        GameObject gameObject = new GameObject();
        gameObject.name = orig.name;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        Vector3 pos = orig.transform.position;
        pos.z -= 1;
        gameObject.transform.position = pos;
        gameObject.transform.localScale = orig.transform.localScale;

        MeshFilter filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
        
        filter.mesh = mesh;

        MeshRenderer renderer = gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;

        renderer.material = mat;

        Rigidbody2D body = gameObject.AddComponent(typeof(Rigidbody2D)) as Rigidbody2D;

        if (force == Vector2.zero)
        {
            body.AddForce(new Vector2(Random.Range(-350, 350), Random.Range(-400, -250)));
            body.AddTorque(5, ForceMode2D.Impulse);
        }
        else
        {
            body.AddForce(force);
        }

        if(fade)
            StartCoroutine(CuttersFadeRoutine(renderer));

        return gameObject;
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

    private IEnumerator CuttersFadeRoutine(MeshRenderer rend)
    {
        float t = 0;
        Color clear = Color.white;
        clear.a = 0;
        while (t < 1)
        {
            rend.material.color = Color.Lerp(Color.white, clear, t);
            t += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Destroy(rend.gameObject);
    }

}
