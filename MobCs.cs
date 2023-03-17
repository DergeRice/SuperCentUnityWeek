using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System;

public class MobCs : MonoBehaviour
{
    Component[] RagDollRid;
    Animator Ani;
    Camera mainCam ;
    [SerializeField]
    
    Slider Hpbar;
    GameObject MobCountCanvas;
    [SerializeField] GameObject Plane;
    [SerializeField] GameObject NavDesti;
    [SerializeField] GameObject HitParticle;
    [SerializeField] GameObject BloodParticle;
    [SerializeField]
    GameObject HpCanvas;
    GameObject Player;
    Vector3 HpPosition;
    //private int Mobhp=20;

    private float MaxHp =20;
    public float MobHp = 20;
    float MobBeforHp;

    Slider ThisHpBar;

    NavMeshAgent _Agent;
    Rigidbody rig;
    
    [SerializeField] AudioClip BloodSound;
    [SerializeField] AudioClip BoomSound;


    // Start is called before the first frame update
    void Awake(){
        InvokeRepeating(nameof(ReNewDestination),0,10);
    }
    void Start()
    {
        rig = gameObject.GetComponent<Rigidbody>();
        _Agent =  gameObject.GetComponent<NavMeshAgent>();
        Player =GameObject.Find("Black Man");
        MobCountCanvas =GameObject.Find("MobCountCanvas");
        HpCanvas = GameObject.Find("MobHpCanvas");
        RagDollRid= GetComponentsInChildren<Rigidbody>();
        Ani = GetComponent<Animator>();
        RagDollOFF();
        mainCam = Camera.main;
        
    }

    public void RagDollOFF(){
        Ani.enabled=true;
        gameObject.GetComponent<Collider>().enabled=true;
        for(int i=0; i<RagDollRid.Length;i++){
            RagDollRid[i].GetComponent<Collider>().isTrigger=true;
            RagDollRid[i].GetComponent<Rigidbody>().isKinematic =true;
        }
    }
    public void RagDollOn(){
        StartCoroutine(GoDown());
        gameObject.GetComponent<Collider>().enabled=false;
        Ani.enabled=false;
        for(int i=0; i<RagDollRid.Length;i++){
            RagDollRid[i].GetComponent<Collider>().isTrigger=false;
            RagDollRid[i].GetComponent<Rigidbody>().isKinematic =false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        DamagedCheck();
        UpdateHPbar();
        HpPosition = mainCam.WorldToScreenPoint(gameObject.transform.position+new Vector3(0,2,0));
        if(rig.velocity.magnitude >=0)
            Ani.SetBool("Walking",true);
        else    Ani.SetBool("Walking",false);
        //Debug.Log(rig.velocity);
    }
    void DamagedCheck(){
    }

    void ReNewDestination(){
       Vector3 RandomPos = new Vector3(UnityEngine.Random.Range(-30,30),0,UnityEngine.Random.Range(-30,30));
       NavMeshHit hit;
       if(NavMesh.SamplePosition(RandomPos,out hit, 300, NavMesh.AllAreas)){
        _Agent.SetDestination(hit.position);
       }else ReNewDestination();
    }
    public void Damaged(){
         //Soundmanager.Inst.PlaySound(BoomSound);
          Soundmanager.Inst.PlaySound(BloodSound);

        MobBeforHp = MobHp;

        GameObject HitPart= Instantiate(HitParticle,HitParticle.transform.position,Quaternion.identity);// 
        HitPart.SetActive(true);
        GameObject BloodPart = Instantiate(BloodParticle,BloodParticle.transform.position,Quaternion.identity);
        BloodPart.SetActive(true);
        
        BloodPart.transform.LookAt(Player.transform.position);// = Quaternion.
        if(MobHp == MaxHp){
            ThisHpBar = Instantiate(Hpbar,HpPosition,Quaternion.identity,HpCanvas.transform);
            }
            
        Ani.SetBool("DamageBool",true);
        Invoke(nameof(SetState),0.1f);
        Invoke(nameof(FadeHPbar),0.3f);
        //Invoke(nameof(Delparticle),0.5f);
 
    }
    public void Death(){
        MobCountCanvas.GetComponent<MobCountCanvas>().DeathEvent();
        _Agent.enabled = false;
        //RagDollOn();
        
    }


    private void SetState(){
        Ani.SetBool("DamageBool",false);
        HitParticle.SetActive(false);
        BloodParticle.SetActive(false);
    }
    private void FadeHPbar(){
        float d =0;
        d += Time.deltaTime*0.1f;
        ThisHpBar.transform.GetChild(0).GetComponent<Slider>().value = Mathf.Lerp((MobHp/MaxHp),(MobBeforHp/MaxHp),d);
    }

    private void UpdateHPbar(){
        if(ThisHpBar !=null){
            ThisHpBar.transform.position = HpPosition;
            ThisHpBar.value = MobHp/MaxHp;
            
            }
    }

    IEnumerator GoDown(){
        yield return new WaitForSeconds(3f);
        GetComponent<Rigidbody>().angularDrag =100;
    
        for(int i=0; i<RagDollRid.Length;i++){
            RagDollRid[i].GetComponent<Collider>().isTrigger=true;
            RagDollRid[i].GetComponent<Rigidbody>().isKinematic =false;
        }
        
        
    }
}
