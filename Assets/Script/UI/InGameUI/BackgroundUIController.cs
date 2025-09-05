using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundUIController : MonoBehaviour
{
    public RotateDaycycle rotateDaycycle;
    public RectTransform backgroundImage;
    
    void Awake()
    {
        // float width = transform.root.GetComponent<RectTransform>().rect.width;
        // float height = transform.root.GetComponent<RectTransform>().rect.height;
        // Debug.Log($"{width},{height},{transform.root.name}");

        // Debug.Log($"{resoultion},{transform.root.name}");
        
        Vector2 resoultion = transform.root.GetComponent<CanvasScaler>().referenceResolution;
        backgroundImage.sizeDelta = new Vector2(resoultion.x, resoultion.y);
    }
}
