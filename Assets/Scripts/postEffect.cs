using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class postEffect : MonoBehaviour
{
    // Start is called before the first frame update
    public Material mat;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //Shader greyShader = Shader.Find("shader/greyScale");
        //Debug.LogFormat("shader find", greyShader.name);
        Graphics.Blit(source, destination, mat);
    }
}
