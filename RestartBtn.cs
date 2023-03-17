using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class RestartBtn : MonoBehaviour
{
    [SerializeField]
    Button Restart;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Restart.onClick.AddListener(()=>SceneManager.LoadScene(0));
    }

    public void ReLoadScene(){
        //Debug.Log("dd");
       SceneManager.LoadScene(0);
    }
   
   public void OnClick(){
    SceneManager.LoadScene(0);
   }
}
