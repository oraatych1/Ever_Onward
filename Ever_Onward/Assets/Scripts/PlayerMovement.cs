using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public DialogueSystem dialogueSystem;
    public Transform playerCamera = null;
    public Rigidbody rb;
    public CharacterController cc;
    public Animator springAnimator;
    public float mouseSensitivity = 3.5f;
    public float walkSpeed = 24.0f;
    public float gravity = -13.0f;
    [SerializeField]  [Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField] [Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;

    public bool lockCursor = true;

    float cameraPitch = 0.0f;
    float velocityY = 0.0f;
    CharacterController controller = null;

    private float timeLeftGrounded = 0;
    public float jumpHeight = 8;

    public Vector3 respawnPosition1;
    GameObject respawnPoint;

    // private bool isCrouched = false;
    // private Vector3 crounchScale;

    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;

    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;

    private float delta; 
    CameraHandler cameraHandler;

    public bool isGrounded
    {
        get
        {
            return controller.isGrounded || timeLeftGrounded > 0;
        }
    }




    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraHandler = CameraHandler.singleton;

        //crounchScale = new Vector3(.5f,.5f, .5f);

        //locks cursor to the middle of the screen and remove the icon
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        respawnPoint = GameObject.Find("RespawnPoint1");
        respawnPosition1 = respawnPoint.transform.position;

    }

    void Update()
    {
        delta  = Time.fixedDeltaTime;
        if (cameraHandler != null)
        {
            cameraHandler.FollowTarget(delta);
            //cameraHandler.HandleCameraRotation(delta);
        }
        UpdateMouseLook();
        

        if (transform.position.y <= -50)
        {
            //print("work fcker");
            //print("heres yer position " + transform.position.ToString());
            print("respawn here " + respawnPosition1);
            transform.position = respawnPosition1;

        }
        else
        {
            UpdateMovement();
        }


    }

    //Camera movements
    void UpdateMouseLook()
    {
        if (DialogueSystem.inConversation) return;
        Vector2 targetmouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetmouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);
        
        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -45.0f, 45.0f);
        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
        /*float mouseX = currentMouseDelta.x;
        float mouseY = currentMouseDelta.y;
        if (cameraHandler != null)
        {
            cameraHandler.HandleCameraRotation(delta, mouseX, mouseY);
        }*/
    }
   

    //Character Movement
    void UpdateMovement()
    {
        if (DialogueSystem.inConversation)
        {
            return;
        }
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            springAnimator.SetBool("isMoving", false);
        }
        else
        {
            springAnimator.SetBool("isMoving", true);
        }
        bool isJumpHeld = Input.GetButton("Jump");
        bool onJumpPress = Input.GetButtonDown("Jump");
        

        if (Input.GetKeyDown(KeyCode.LeftShift) && controller.isGrounded)
        {

            walkSpeed = 30.0f;

        }
        if (!Input.GetKey(KeyCode.LeftShift) && controller.isGrounded)
        {

            walkSpeed = 24.0f;
            
        }


        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        //adds falling to the character
        if (controller.isGrounded)
        {
            velocityY = 0.0f;
            timeLeftGrounded = .2f;

        }

        if (isGrounded)
        {
            if (isJumpHeld)
            {
                velocityY = jumpHeight;
                timeLeftGrounded = 0;
            }
        }
        velocityY += gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);


    }

  
}
