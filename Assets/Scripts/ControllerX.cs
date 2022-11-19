using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerX : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 5f;

    [SerializeField]
    private float moveSpeed = 0.1f;

    float distance = 1.1f; //was 1.1
    public GameObject selectedGO;
    public bool checkCollision = false;
    public bool isMouseDown = false;

    private int directionFlag = 1;
    public Camera mc;
    public Rigidbody rb;
    private float lx, ly, lz;
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
            transform.eulerAngles = new Vector3(r.x, r.y + rotationSpeed, r.z);
        }
        else if (!checkCollision && Input.GetKey(KeyCode.UpArrow))
        {
            Vector3 t = transform.position;
            transform.position = new Vector3(t.x, t.y, t.z + moveSpeed);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            Vector3 t = transform.position;
            transform.position = new Vector3(t.x, t.y, t.z - moveSpeed);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 t = transform.position;
            transform.position = new Vector3(t.x + moveSpeed, t.y, t.z);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 t = transform.position;
            transform.position = new Vector3(t.x - moveSpeed, t.y, t.z);
        }


        Vector3 origin = transform.position;
        Vector3 direction = directionFlag * transform.right;

        Ray ray = new Ray(origin, direction);
        Debug.DrawRay(new Vector3(origin.x, origin.y, origin.z), directionFlag * direction, Color.yellow);

        if (isMouseDown && Physics.Raycast(ray, out RaycastHit h, distance))
        {
            GameObject go = h.collider.gameObject;
            if (go.CompareTag("Wall"))
            {
                go.GetComponent<Renderer>().material.color = Color.yellow;
                checkCollision = true;
            }
            //directionFlag *= -1; //MyCode so brick doesn't stick to wall
        }
        if (!isMouseDown && Physics.Raycast(ray, out RaycastHit hit1, distance)) //This if is my code to continue to move brick if touching another
        {
            GameObject go = hit1.collider.gameObject;
            if (go.CompareTag("Player"))
            {
                if (directionFlag == 1)
                {
                    Vector3 t = transform.position;
                    transform.position = new Vector3(t.x+0.1f, t.y, t.z);
                }
                else
                {
                    Vector3 t = transform.position;
                    transform.position = new Vector3(t.x-0.1f, t.y, t.z);
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
            //directionFlag *= -1; //MyCode so brick doesn't stick to wall
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
        if ((origin.x - transform.position.x) <= 0)
            directionFlag = 1;
        else
            directionFlag = -1;


        if (!checkCollision && selectedGO != null)
        {
            lx = Input.mousePosition.x;
            ly = Input.mousePosition.y;
            lz = Input.mousePosition.z;
            Vector3 position = new Vector3(mc.WorldToScreenPoint(selectedGO.transform.position).x, ly,
                lz);
            Vector3 worldPos = mc.ScreenToWorldPoint(position);

            Vector3 subVecPosition = new Vector3(worldPos.x, 0.3f,
                origin.z);

            rb.MovePosition(subVecPosition);

        }

    }

    private void OnMouseUp()
    {
        checkCollision = false;

        selectedGO.GetComponent<Renderer>().material.color = Color.blue;

        rb.MovePosition(new Vector3(rb.transform.position.x / 1.5f-directionFlag*0.2f,
            origin.y, rb.transform.position.z));
        
        rb.MovePosition(new Vector3(Mathf.Round(rb.transform.position.x),
            origin.y,
            rb.transform.position.z));
       
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
