using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Transform viewPoint;
    public float mouseSensitivity = 1f;
    private float verticalRotStore;
    private Vector2 mouseInput;
    public bool invertLook; //option to invert controls
    public float moveSpeed = 5f, runSpeed = 8f;

    private float activeMoveSpeed;
    private Vector3 moveDir,movement;
    public CharacterController charcon; //using CharacterController to add rigid body physics

    private Camera cam; // needed to decouple camera from player, to prevent destruction when player dies 
    
    public float jumpForce = 3f, gravityMod = 1f; //jump height

    public Transform groundCheckPoint; //to keep player grounded when jumping
    private bool isGrounded;
    public LayerMask groundLayers;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //hides the cursor
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity; //getting mouse inputs and adjusting for sensitivity
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y + mouseInput.x,transform.rotation.eulerAngles.z); //can be treated as a Vector3 object, x and z have been left as it is
                                                                                                                                                                  //mouse movement in the x direction(horizontal) is a rotation in the y axis of the game ..

        verticalRotStore += mouseInput.y;
        verticalRotStore = Mathf.Clamp(verticalRotStore, -60f, 60f);

        //viewPoint.rotation = Quaternion.Euler(Mathf.Clamp(viewPoint.rotation.eulerAngles.x - mouseInput.y,-60f,60f),viewPoint.rotation.eulerAngles.y,viewPoint.rotation.eulerAngles.z); //viewPoint is rotated in x axis to simulate looking up and down, + for inverted control - for normal
        //weird snapping effect due to nature of Quaternions as the angles are calculated as 360 - x value, so verticalRotStore is used

        if(invertLook)
            viewPoint.rotation = Quaternion.Euler(verticalRotStore, viewPoint.rotation.eulerAngles.y, viewPoint.rotation.eulerAngles.z);
        else
            viewPoint.rotation = Quaternion.Euler(-verticalRotStore, viewPoint.rotation.eulerAngles.y, viewPoint.rotation.eulerAngles.z);

        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"),0f,Input.GetAxisRaw("Vertical")); //using movement so we can move where we face

        if (Input.GetKey(KeyCode.LeftShift)){ //checking if running or not
            activeMoveSpeed = runSpeed;
        }
        else
        {
            activeMoveSpeed = moveSpeed;
        }

        float yVelocity = movement.y; //saving previous velocity to implement acceleration
        movement = ((transform.forward * moveDir.z) + (transform.right * moveDir.x)).normalized; //normalise to prevent faster diagonal movement
        movement.y = yVelocity;
        if (charcon.isGrounded)
        {
            movement.y = 0f;
        }

        isGrounded = Physics.Raycast(groundCheckPoint.position, Vector3.down, 0.25f,groundLayers); //draws a line and checks if interaction occurs with ground layers
        
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            movement.y = jumpForce;
        }

        movement.y += Physics.gravity.y * Time.deltaTime * gravityMod; // adding gravity
        
        charcon.Move(movement * activeMoveSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Cursor.lockState == CursorLockMode.None)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    private void LateUpdate() //takes place after all scripts Update function has executed
    {
        cam.transform.position = viewPoint.position; //snap camera to viewpoint
        cam.transform.rotation = viewPoint.rotation;
    }
}
