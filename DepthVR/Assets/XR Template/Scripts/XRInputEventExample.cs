using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class XRInputEventExample : MonoBehaviour
{
    public GameObject placeObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void createObject(Material mat)
    {
        GameObject obj = Instantiate(placeObject);
        float scale = Random.Range(0.3f, 1.0f);
        obj.transform.position = new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(scale/2, 2.0f), Random.Range(-3.0f, 3.0f));
        obj.transform.localScale = new Vector3(scale, scale, scale);
        List<Renderer> rendererList = GetRenderers(obj);
        foreach (var renderer in rendererList)
        {
            renderer.material = mat;
        }
    }

    List<Renderer> GetRenderers(GameObject gameObject)
    {
        List<Renderer> rendererList = new List<Renderer>();
        foreach (Renderer objectRenderer in gameObject.GetComponentsInChildren<Renderer>())
        {
            rendererList.Add(objectRenderer);
        }
        return rendererList;
    }
}
