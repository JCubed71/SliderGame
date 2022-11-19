using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControllerRC : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 5f;

    [SerializeField]
    private float moveSpeed = 0.1f;

    float distance = 1.1f;
    public GameObject selectedGO;
    public bool checkCollision = false;
    public bool isMouseDown = false;

    private int directionFlag = 1;
    public Camera mc;
    public Rigidbody rb;
    private float lx, ly;
    private Vector3 origin;
    void Start()
    {
        if (mc == null) mc = Camera.main;
        if (rb == null) rb = GetComponent<Rigidbody>();
        origin = transform.position;
        
    }

   
    void Update()
    {

        
        if (Input.GetKey(KeyCode.R))
        {
            Vector3 r = transform.eulerAngles;
            transform.eulerAngles = new Vector3(r.x, r.y+rotationSpeed, r.z);
        }else if (!checkCollision&& Input.GetKey(KeyCode.UpArrow))
        {
            Vector3 t = transform.position;
            transform.position = new Vector3(t.x, t.y, t.z+moveSpeed);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            Vector3 t = transform.position;
            transform.position = new Vector3(t.x, t.y, t.z - moveSpeed);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 t = transform.position;
            transform.position = new Vector3(t.x + moveSpeed, t.y, t.z );
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 t = transform.position;
            transform.position = new Vector3(t.x - moveSpeed, t.y, t.z );
        }


        Vector3 origin = transform.position;
        Vector3 direction = directionFlag* transform.forward;

        Ray ray = new Ray(origin, direction);
        Debug.DrawRay(new Vector3(origin.x,origin.y,origin.z), directionFlag * direction , Color.yellow);

        if(isMouseDown && Physics.Raycast(ray, out RaycastHit h, distance))
        {
            GameObject go = h.collider.gameObject;
            if (go.CompareTag("Wall")) {
                go.GetComponent<Renderer>().material.color= Color.yellow;
                checkCollision = true;
            }
            directionFlag *= -1; //MyCode so brick doesn't stick to wall
        }
        if (!isMouseDown && Physics.Raycast(ray, out RaycastHit hit1, distance)) //This if is my code to continue to move brick if touching another
        {
            GameObject go = hit1.collider.gameObject;
            if (go.CompareTag("Player"))
            {
                if(directionFlag == 1) 
                {
                    Vector3 t = transform.position;
                    transform.position = new Vector3(t.x, t.y, t.z-0.1f);
                }
                else
                {
                    Vector3 t = transform.position;
                    transform.position = new Vector3(t.x, t.y, t.z + 0.1f);
                }
                //go.GetComponent<Renderer>().material.color = Color.yellow;
                //checkCollision = true;
            }
            directionFlag *= -1; //MyCode so brick doesn't stick to wall
        }
        if (isMouseDown && Physics.Raycast(ray, out RaycastHit hit, distance)) //My code to see if collision with another brick
        {
            GameObject go = hit.collider.gameObject;
            if (go.CompareTag("Player"))
            {
                //go.GetComponent<Renderer>().material.color = Color.yellow;
                checkCollision = true;
            }
            directionFlag *= -1; //MyCode so brick doesn't stick to wall
        }

    }

    private void OnMouseDown()
    {
        RaycastHit hit = findingObjectRC();

        if (hit.collider.gameObject.CompareTag("Player"))
        {
            selectedGO = hit.collider.gameObject;
            Cursor.visible = false;
            selectedGO.GetComponent<Renderer>().material.color = Color.white;


        }
        isMouseDown = true;
    }

    private void OnMouseDrag()
    {
        if ((origin.z - transform.position.z) <= 0)
            directionFlag = 1;
        else
            directionFlag = -1;


        if (!checkCollision && selectedGO != null)
        {
            lx = Input.mousePosition.x;
            ly = Input.mousePosition.y;
            Vector3 position = new Vector3(lx, ly,
                mc.WorldToScreenPoint(selectedGO.transform.position).z);
            Vector3 worldPos = mc.ScreenToWorldPoint(position);

            Vector3 subVecPosition = new Vector3(origin.x, 0.3f,
                worldPos.z);

            rb.MovePosition(subVecPosition);

        }
        
    }

    private void OnMouseUp()
    {
        checkCollision = false;
            
        selectedGO.GetComponent<Renderer>().material.color = Color.blue;
            
        rb.MovePosition(new Vector3(rb.transform.position.x,
            origin.y, rb.transform.position.z/1.5f-directionFlag*0.2f));
                
        rb.MovePosition(new Vector3(rb.transform.position.x,
            origin.y,
            Mathf.Round(rb.transform.position.z)));
        selectedGO = null;
        Cursor.visible = true;


        isMouseDown = false;

    }



    private RaycastHit findingObjectRC()
    {
        Vector2 mPosition = Input.mousePosition;
        Ray ray = mc.ScreenPointToRay(mPosition);
        Physics.Raycast(ray, out RaycastHit h);
        return h;

    }
}
