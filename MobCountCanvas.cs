using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobCountCanvas : MonoBehaviour
{
    [SerializeField]
    GameObject MobParent;
    [SerializeField]
    Text ShowingTxt;
    [SerializeField] GameObject Timeline;

    public int MaxMobCnt;
    public int KilledMobCnt;
    [SerializeField]
    GameObject EndingCanvas;
    // Start is called before the first frame update
    void Start()
    {
        MaxMobCnt = MobParent.transform.childCount;
        KilledMobCnt =0;
    }

    // Update is called once per frame
    void Update()
    {
        ShowingTxt.text = (KilledMobCnt.ToString()+"/"+MaxMobCnt.ToString());
        if(KilledMobCnt>=MaxMobCnt){
            
            EndingCanvas.SetActive(true);
            Timeline.SetActive(true);
        }
    }
    public void DeathEvent(){
        KilledMobCnt ++;

    }
}
