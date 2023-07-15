using UnityEngine;

[ExecuteAlways]
public class ClipBox : MonoBehaviour
{
    private void Update()
    {
        Shader.SetGlobalMatrix("_WorldToBox", transform.worldToLocalMatrix);
    }
}