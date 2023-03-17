using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointMove : MonoBehaviour
{
    
    Rigidbody rb;
    [SerializeField]
    int dd;
    // Change the mass of the object's Rigidbody.
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int layerMask = 1  << LayerMask.NameToLayer("Ground");  
        if(Physics.Raycast(ray,out hit,Mathf.Infinity,layerMask)){
            rb.transform.position = hit.point+new Vector3(0,0.1f,0);//방지
        }
        
        
    }
}
