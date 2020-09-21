using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutomaticVerticalSize : MonoBehaviour
{
    public float childHeight = 35f; 

    // Start is called before the first frame update
    void Start()
    {
        AdjustSize(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdjustSize()
    {
        Vector2 size = GetComponent<RectTransform>().sizeDelta; 
        size.y = transform.childCount * (childHeight + GetComponent<VerticalLayoutGroup>().spacing);
        GetComponent<RectTransform>().sizeDelta = size; 


    }
}
