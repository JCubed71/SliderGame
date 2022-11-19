using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BrickController : MonoBehaviour
{
    GameObject[] bricks;
    private float moveSpeed = 0.01f;
    //public Rigidbody rb;

    

    public enum State
    {
        selected,
        idle,
    }
    public State state;

    // Start is called before the first frame update
    void Start()
    {
        
        bricks = GameObject.FindGameObjectsWithTag("brick");
        state = State.idle;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(state== State.selected)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None; //Unfreeze all constraints
            GetComponent<Rigidbody>().freezeRotation= true;
            
        }
        else
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        if (state == State.selected && Input.GetKey(KeyCode.UpArrow))
        {
            Vector3 t = transform.position;
            transform.position = new Vector3(t.x, t.y, t.z - moveSpeed);
        }
        else if (state == State.selected && Input.GetKey(KeyCode.DownArrow))
        {
            Vector3 t = transform.position;
            transform.position = new Vector3(t.x, t.y, t.z + moveSpeed);
        }
        else if (state == State.selected && Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 t = transform.position;
            transform.position = new Vector3(t.x + moveSpeed, t.y, t.z);
        }
        else if (state == State.selected && Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 t = transform.position;
            transform.position = new Vector3(t.x - moveSpeed, t.y, t.z);
        }
    }
    private void OnMouseDown()
    {
        //rb = gameObject.GetComponent<Rigidbody>();
        //print("selected");
        if (state == State.selected)
        {
            //print("shouldn't");
            //childTextObject.SetActive(false);
            gameObject.GetComponent<Renderer>().material.color = Color.blue;
            state = State.idle;
        }
        else if (state == State.idle)
        {
            foreach (GameObject go in bricks)
            {
                //print("in");
                go.GetComponent<BrickController>().setEnumIdle(); //Deselect all other menu options before selcting a new one
                go.GetComponent<Renderer>().material.color = Color.blue;
            }
            //childTextObject.SetActive(true);
            gameObject.GetComponent<Renderer>().material.color = Color.white;
            state = State.selected;
            //print("out");
        }
    }
    private void setEnumIdle()
    {
        state = State.idle;
        //childTextObject.SetActive(false);

    }
    private void setEnumSelected()
    {
        
        state = State.selected;
        //childTextObject.SetActive(true);
    }
    
}
