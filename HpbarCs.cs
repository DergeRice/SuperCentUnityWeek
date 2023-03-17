using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpbarCs : MonoBehaviour
{
    
   // Camera mainCam = Camera.main;
    [SerializeField]
    //GameObject Mob;
    bool CapturedCam = false;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] Monster = GameObject.FindGameObjectsWithTag("Monster");
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CamOn(){
        CapturedCam =true;
    }
}
