using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPassProtector : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision other)
    {
       
        if (other.gameObject.CompareTag("protector"))
        {
            
            other.gameObject.SetActive(false);
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("protector"))
        { 
            other.gameObject.SetActive(true);
        }
    }
    
}
