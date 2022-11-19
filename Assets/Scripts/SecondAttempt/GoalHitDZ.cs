using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalHitDZ : MonoBehaviour
{
    public GameObject winScreen;
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

        if (other.gameObject.name == ("PlayerBrick"))
        {
            winScreen.SetActive(true);
        }
    }
}
