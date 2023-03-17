using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCs : MonoBehaviour
{
    public Vector3 clickPos = Vector3.one;
    [SerializeField]
    float GizmoSphere;
    [SerializeField] GameObject joystick;
    public GameObject MousePos;
    public GameObject Cam;
    public Rigidbody rb;
    public GameObject bulletCnt;
    

    [SerializeField]
    GameObject Gun;



    float minSpeed = 3f;
    [SerializeField]
    float moveSpeed =0f;
    [SerializeField]
    float maxSpeed = 5f;
    float addedSpeed;
    float moveTime;
    float ShootTime= 0f;


    Vector3 Movepoint;
    Vector3 BodyLook;
    Vector2 inputVector;


    bool LookRoundBool;
    bool ReloadBool;
    bool ShootAble;
    //Text d;

    int maxBullet =7;
    int curBullet = 7;
    
    Slider sl ;

    Transform UpperBody;
    Collider nearestMob;
    

    Animator animator;

    Collider[] hit;
    Component[] RagDollRid;

    AudioSource _Audio;

   
   [SerializeField] AudioClip ShootSoundClip;
   [SerializeField] AudioClip ReloadSoundClip;
    // Start is called before the first frame update
    void Start()
    {
        _Audio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        Invoke(nameof(TimeRandom),2f);
        sl = bulletCnt.GetComponent<Slider>();
        UpperBody = animator.GetBoneTransform(HumanBodyBones.Spine);
        //hit[0].transform.position = new Vector3(0,0,0);
        RagDollRid = GetComponentsInChildren<Rigidbody>();
        
        
    }

    void RagDollOn(){
        for(int i=0; i<RagDollRid.Length;i++){
            RagDollRid[i].GetComponent<Collider>().isTrigger=false;
            RagDollRid[i].GetComponent<Rigidbody>().isKinematic =false;
        }
    }

    void RagDollOFF(){
        for(int i=0; i<RagDollRid.Length;i++){
            RagDollRid[i].GetComponent<Collider>().isTrigger=false;
            RagDollRid[i].GetComponent<Rigidbody>().isKinematic =false;
        }

    }
    // Update is called once per frame
    void Update()
    {
        //getMouseSign();
        setAniPara();
        bulletUpdate();
        setMaxSpeed();
        MoveUpdate();

        if (ShootAble)ShootTime += Time.deltaTime;
    }

    void MoveUpdate(){
        inputVector = joystick.GetComponent<Joystick>().inputVector;
        if(inputVector!=Vector2.zero){
            moveSpeed = (moveSpeed+(1f)*(Time.deltaTime));
            Vector3 moveInputVector =new Vector3(inputVector.x,0,inputVector.y);
            transform.position += moveInputVector*Time.deltaTime*(moveSpeed+minSpeed);
            
            transform.rotation =Quaternion.LookRotation(moveInputVector).normalized;
        }
        else{
            moveSpeed=0;
            rb.velocity=Vector3.zero;
            }
            
    }
    void setMaxSpeed(){
        if(moveSpeed>=maxSpeed){
            moveSpeed = maxSpeed;
        }
    }
    void bulletUpdate(){
        sl.value=curBullet;
    }
    void setAniPara(){
        
        animator.SetFloat("WalkingSpeed",(moveSpeed)/(maxSpeed));
        if((moveSpeed)/(maxSpeed)!=0)
            animator.SetBool("WalkingBool",true);
        else
            animator.SetBool("WalkingBool",false);
        //Debug.Log((moveSpeed)/(maxSpeed));
        animator.SetBool("LookRoundBool",LookRoundBool);

    }

    public void Shoot(Collider Mob){
        
        ShootSound();
        Cam.GetComponent<CameraCs>().ShakeTime = 0.09f;
        curBullet -=1;
        animator.SetBool("ShootBool",true);
        Invoke(nameof(SetAniDefault),0.1f);

        if(curBullet <= 0){Reload();}
        MobCs mobb = Mob.GetComponent<MobCs>();
        mobb.Damaged();
        mobb.MobHp -= (int)Random.Range(10f,15f);
        if(mobb.MobHp<=0){
            mobb.RagDollOn();
            Mob.GetComponent<Animator>().enabled=false;
            mobb.Death();
        }
        GameObject BulletEffect = Instantiate(Gun.transform.GetChild(0).gameObject,Gun.transform.position,Quaternion.identity);
        
        BulletEffect.SetActive(true);
        BulletEffect.GetComponent<BulletCS>().SetTarget(Mob.gameObject);
    
        Mob.transform.GetChild(0).GetChild(0).GetComponent<Collider>().attachedRigidbody.AddForce((Mob.transform.position-transform.position)*50,ForceMode.Impulse);
        
        
        
    }

    public void ShootSound(){
        Soundmanager.Inst.PlaySound(ShootSoundClip);

    }

    public void Shoot(){

    }
    public void Reload(){

        Soundmanager.Inst.PlaySound(ReloadSoundClip);
        animator.SetBool("ReloadBool",true);
        Invoke(nameof(SetAniDefault),0.1f);
        curBullet =maxBullet;
    }

    
    private void TimeRandom(){
        int i = Random.Range(0,1);
        if(i==1)
            {LookRoundBool = true;}
        else
            {LookRoundBool = false;}
        
        Invoke(nameof(TimeRandom),2f);
    }

    private void SetAniDefault(){
        animator.SetBool("ShootBool",false);
        animator.SetBool("ReloadBool",false);

    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;  
        
    Gizmos.DrawWireSphere(transform.position, GizmoSphere);
    if(nearestMob!=null)
        Gizmos.DrawRay(transform.position, nearestMob.transform.position-transform.position);
        
    }

    void LateUpdate(){
        UpperRotate();
    }
    private void FixedUpdate() {
        FreezeRotation();
        
    }
    void FreezeRotation(){
        rb.angularVelocity = Vector3.zero;
    }

    void UpperRotate(){
        FindNearMob();
    }

    void FindNearMob(){
        int layerMask = 1  << LayerMask.NameToLayer("Monster");  
        int WallMask = 1  << LayerMask.NameToLayer("Wall");  
        
        hit =Physics.OverlapSphere(transform.position,GizmoSphere,layerMask);
       
       
        for(int i=0; i<hit.Length;i++){

            if(i>0){
                nearestMob = Vector3.Distance(hit[i-1].transform.position,transform.position) <Vector3.Distance(hit[i].transform.position,transform.position)
                ? hit[i-1] : hit[i];
            }else nearestMob = hit[i];

            if(!Physics.Raycast(transform.position,nearestMob.transform.position-transform.position,GizmoSphere,WallMask)){//사이에 벽이 없으면
                BodyLook = nearestMob.transform.GetChild(0).GetChild(0).position+new Vector3(0,0f,0f);
                UpperBody.LookAt(BodyLook); 
                ShootAble = true;
               if(ShootTime>1.2f){
                Shoot(nearestMob);
                ShootTime=0;
               }
                
            }else{//사이에 벽이 있으면

            }  
        }

        if(hit.Length==0){
            ShootTime=0;
        }
       
       
    }

}
