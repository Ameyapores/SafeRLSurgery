using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class greenpixels : MonoBehaviour
{
    private int counter;

    private RenderTexture cameraTexture;

    // Start is called before the first frame update
    void Start()
    {   
        
        cameraTexture = GetComponent<Camera>().targetTexture;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(DetectGreen());        
    }

    public int getGreenPixels()
    {
        return counter;
    }

    public bool hasGreen()
    {
        return counter > 0;
    }
 
    public IEnumerator DetectGreen()

    {
        counter =0;
        yield return new WaitForEndOfFrame();
        RenderTexture.active = cameraTexture;
        Texture2D image = new Texture2D(cameraTexture.width, cameraTexture.height, TextureFormat.RGBA32, false);		
        
        image.ReadPixels(new Rect(0, 0, cameraTexture.width, cameraTexture.height), 0, 0, false);
        image.Apply();
        
        //Color pixel_color = image.GetPixel(21,16);
        //Debug.Log("Pixels " + pixel_color);

        Color[] pix = image.GetPixels(0, 0, cameraTexture.width, cameraTexture.height);
        //Texture2D destTex = new Texture2D(cameraTexture.width, cameraTexture.height);
        //destTex.SetPixels(pix);
        //destTex.Apply();
        
        //Debug.Log("Pixels " + destTex.GetPixel(16, 16));
        //Debug.Log("Pixels " + pix);
        foreach(Color c in pix)
        { 
            //Debug.Log("Pixels2 " + c);
            bool isGreenGood = c.g >= 0.5f && c.g <= 1.0f; 
            bool isBlueGood = c.b >= 0.0f && c.b <= 0.4f;
            bool isRedGood = c.r >= 0.0f && c.r <= 0.4f;
            
            if (isRedGood && isGreenGood && isBlueGood)
                counter++;
        }
        
        //Debug.Log("Pixels " + counter);
    }       
}