using UnityEngine;
using System.Collections;
using RTS;
using System.Runtime;
using System.Runtime.InteropServices;

public class Fly_Cam : MonoBehaviour
{
    public Player player;

    /*AM USING MODIFIED MOVEMENT SCRIPT FROM UNITY FORUMS
     * Based on Windex's flycam script found here: http://forum.unity3d.com/threads/fly-cam-simple-cam-script.67042/
     * C# conversion created by Ellandar
     * Improved camera made by LookForward
     * Modifications created by Angryboy & ME
     * 1) Have to hold right-click to rotate
     * 2) Made variables public for testing/designer purposes
     * 3) Y-axis now locked (as if space was always being held)
     * 4) Q/E keys are used to raise/lower the camera
     */

    public float mainSpeed = 100.0f; //regular speed
    public float shiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
    public float maxShift = 1000.0f; //Maximum speed when holdin gshift
    public float camSens = 0.25f; //How sensitive it with mouse
    private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
    private float totalRun = 1.0f;

    private bool isRotating = false; // Angryboy: Can be called by other things (e.g. UI) to see if camera is rotating
    private float speedMultiplier; // Angryboy: Used by Y axis to match the velocity on X/Z axis

    public float mouseSensitivity = 5.0f;        // Mouse rotation sensitivity.
    private float rotationY = 0.0f;

    //imports for checking caps lock
    [DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
    public static extern short GetKeyState(int keyCode);
    public static bool isCapsLockOn; //check state of caps
    //end of caps lock

    void Start()
    {
        player = transform.root.GetComponent<Player>();
    } 
	

    void Update()
    {
        
        if (player.human)// && player.human ||| if (player && player.human)
        {
            //divide default script moving methods in the update into movecamera and rotatecamera
            MoveCamera();
            RotateCamera();
            MouseActivity();
        }

        isCapsLockOn = (((ushort)GetKeyState(0x14)) & 0xffff) != 0; //check if caps is on every update
        
        
    }
    //update end

    public void MoveCamera() 
    {
        //Keyboard commands
        float f = 0.0f;
        Vector3 p = GetBaseInput();
        if (Input.GetKey(KeyCode.LeftShift))
        {
            totalRun += Time.deltaTime;
            p = p * totalRun * shiftAdd;
            p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
            p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
            p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
            // Angryboy: Use these to ensure that Y-plane is affected by the shift key as well
            speedMultiplier = totalRun * shiftAdd * Time.deltaTime;
            speedMultiplier = Mathf.Clamp(speedMultiplier, -maxShift, maxShift);
        }
        else
        {
            totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
            p = p * mainSpeed;
            speedMultiplier = mainSpeed * Time.deltaTime; // Angryboy: More "correct" speed
        }

        p = p * Time.deltaTime;

        // Angryboy: Removed key-press requirement, now perma-locked to the Y plane
        Vector3 newPosition = transform.position;//If player wants to move on X and Z axis only
        transform.Translate(p);
        newPosition.x = transform.position.x;
        newPosition.z = transform.position.z;

        // Angryboy: Manipulate Y plane by using Q/E keys
        if (Input.GetKey(KeyCode.Q)) //in addition to moving the camera, moves the plane for unit movement
        {
            newPosition.y += -speedMultiplier;
            Vector3 v1 = GameObject.FindGameObjectWithTag("MovementPlane").GetComponent<Transform>().position;
            v1 -= new Vector3(0, 1, 0);
            GameObject.FindGameObjectWithTag("MovementPlane").GetComponent<Transform>().position = v1;
        }
        if (Input.GetKey(KeyCode.E))
        {
            newPosition.y += speedMultiplier;
            Vector3 v1 = GameObject.FindGameObjectWithTag("MovementPlane").GetComponent<Transform>().position;
            v1 += new Vector3(0, 1, 0);
            GameObject.FindGameObjectWithTag("MovementPlane").GetComponent<Transform>().position = v1;
        }

        transform.position = newPosition;
    
    }


    public void RotateCamera()
    {
        // Angryboy: Hold right-mouse button to rotate
        //SUCCESS!
        if (Input.GetMouseButtonDown(1) && (isCapsLockOn == false))//add condition to rotate when caps is turned off
        {
            isRotating = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            isRotating = false;
        }
        if (isRotating)
        {
            // Made by LookForward
            // Angryboy: Replaced min/max Y with numbers, not sure why we had variables in the first place
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;
            rotationY += Input.GetAxis("Mouse Y") * mouseSensitivity;
            rotationY = Mathf.Clamp(rotationY, -90, 90);
            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0.0f);
        }
    
    }


    // Angryboy: Can be called by other code to see if camera is rotating
    // Might be useful in UI to stop accidental clicks while turning?
    public bool amIRotating()
    {
        return isRotating;
    }

    private Vector3 GetBaseInput()
    { //returns the basic values, if it's 0 than it's not active.
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            p_Velocity += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            p_Velocity += new Vector3(1, 0, 0);
        }
        return p_Velocity;
    }

    //maybe change for rght mouse click + some button or smth 
    //- add a check if that is just right click or click + button
    //maybe for cancelling the selection to use alt+left button? or double/second click?
    //Debug.Log()
    private void MouseActivity()
    {
        if (Input.GetMouseButtonDown(0)) LeftMouseClick();//only left mouse click is checked here
        else if (Input.GetMouseButtonDown(1) && (isCapsLockOn == true)) RightMouseClick();
        MouseHover();
    }
    
    private void RightMouseClick()
    {
        if (player.hud.MouseInBounds() && player.SelectedObject)
        {
            player.SelectedObject.SetSelection(false, player.hud.GetPlayingArea());
            player.SelectedObject = null;
        }
    }                                
    private void LeftMouseClick()
    {
        if (player.hud.MouseInBounds())
        {
            GameObject hitObject = FindHitObject();
            Vector3 hitPoint = FindHitPoint();
            if (hitObject && hitPoint != ResourceManager.InvalidPosition)//idk if invalid position is applicable in space lol
            {
                if (player.SelectedObject) 
                {
                    player.SelectedObject.MouseClick(hitObject, hitPoint, player); }
                else if (hitObject.tag != "Sun" && hitObject.tag != "Skybox") //changed changed name for tag, just in case
                {
                    //can safely add skybox check - it doesn't ignore its children YAY :D
                    WorldObject worldObject = hitObject.GetComponentInParent<WorldObject>();//changed here, the problem was in transform.root//changed getcomponent to get component in parent (to modify method from point 8)
                    if (worldObject)
                    {
                        //we already know the player has no selected object
                        player.SelectedObject = worldObject;
                        worldObject.SetSelection(true, player.hud.GetPlayingArea());
                        
                    }
                }
            }
        }
    }

    private GameObject FindHitObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.collider.gameObject;
        }
            return null;
        
    }

    private Vector3 FindHitPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
            return ResourceManager.InvalidPosition;
       
    }

    private void MouseHover() 
    {
        if (player.hud.MouseInBounds())
        {
            GameObject hoverObject = FindHitObject();
            if (hoverObject)
            {
                if (player.SelectedObject) player.SelectedObject.SetHoverState(hoverObject);
                else if (hoverObject.name != "Sun" ) 
                {
                    Player owner = hoverObject.GetComponent<Player>();                                                              
                    if (owner)
                    {
                        Unit unit = hoverObject.GetComponentInParent<Unit>();
                        Base building = hoverObject.GetComponentInParent<Base>();
                        if (owner.name == player.name && (unit || building)) player.hud.SetCursorState(CursorState.Select);
                    }
                }
            }
        }
    }


}