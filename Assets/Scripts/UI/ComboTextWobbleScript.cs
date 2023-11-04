using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ComboTextWobbleScript:MonoBehaviour
    {
        private TMP_Text txt;

        private void Awake()
        {
            txt = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            txt.ForceMeshUpdate();
            var mesh = txt.mesh;
            var vertices = mesh.vertices;

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 offset = Wobble(Time.time + i);
                vertices[i] += offset;
                     
            }

            mesh.vertices = vertices;
            txt.canvasRenderer.SetMesh(mesh);
        }
        
        Vector2 Wobble(float time)
        {
            return new Vector2(Mathf.Sin(time*2f), Mathf.Cos(time*2f));
        }
    }
}