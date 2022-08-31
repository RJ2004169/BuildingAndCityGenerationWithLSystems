using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHighlight : MonoBehaviour
{
    private Color startcolor;
    

    void OnMouseEnter()
    {
        startcolor = transform.GetComponent<Renderer>().material.color;
        transform.GetComponent<Renderer>().material.color = Color.red;
    }
    void OnMouseExit()
    {
        transform.GetComponent<Renderer>().material.color = startcolor;
    }

    private void OnMouseDown()
    {
        UIController.cityBlockClickEvent(transform.position, transform.rotation);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

