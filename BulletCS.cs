using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCS : MonoBehaviour
{
    public GameObject Target;
    public GameObject Me;
    private float BulletRemainTime =0;
    // Start is called before the first frame update
    void Start()
    {
        Me = GameObject.Find("Black Man");
    }

    // Update is called once per frame
    void Update()
    {
        if(Target !=null){
            transform.position =Vector3.MoveTowards(transform.position,Target.transform.GetChild(0).GetChild(0).position,0.1f); 
            BulletRemainTime += Time.deltaTime; 

            if(Vector3.Distance(transform.position,Target.transform.position) <1f)
                Destroy(gameObject);
        }
       
    }

    public void SetTarget(GameObject _Target){
        Target = _Target;
    }
}
