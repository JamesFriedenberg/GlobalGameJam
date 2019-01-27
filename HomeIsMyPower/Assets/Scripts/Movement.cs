using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Movement : MonoBehaviour {

    private Vector3 worldPos;
    private float mouseX;
    private float mouseY;
    private Vector3 mousePosition;
    public int moveSpeed;
    public Camera cam;
    private Rigidbody rb;

    // look rotation stuff
    public float XSensitivity = 2f;
    public float YSensitivity = 2f;
    public float MinimumX = -90F;
    public float MaximumX = 90F;

    private Quaternion m_CharacterTargetRot;
    private Quaternion m_CameraTargetRot;
    private bool m_cursorIsLocked = true;

    public bool clampVerticalRotation = true;
    public bool smooth;
    public float smoothTime = 5f;
    public bool lockCursor = true;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
	}

    public void Init(Transform character, Transform camera)
    {
        m_CharacterTargetRot = character.localRotation;
        m_CameraTargetRot = camera.localRotation;
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.LeftShift)){
            Dash();
        }

        /*
        if (Input.GetKeyDown(KeyCode.Space)) {
            Jump();
        }
        */

        MoveInput();

        LookAtMouse();
    }

    void Dash() {
        Debug.Log("Test Dash");

        transform.Translate(Vector3.forward * 5);
    }
    
    void LookAtMouse() {
        Cursor.lockState = CursorLockMode.Locked;

        // mouseX = Input.mousePosition.x;
        // mouseY = Input.mousePosition.y;

        // float yRot = CrossPlatformInputManager.GetAxis("Mouse X") * XSensitivity;
        // float xRot = CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity;

        if (Input.GetAxis("Mouse X") < 0) {
            if (Input.GetAxis("Mouse X") < -0.5)
            {
                mouseX = Input.GetAxis("Mouse X");
            }
            else if (Input.GetAxis("Mouse X") > -0.5)
            {
                mouseX = 0;
            }
        }

        if (Input.GetAxis("Mouse X") > 0) {
            if (Input.GetAxis("Mouse X") > 0.5)
            {
                mouseX = Input.GetAxis("Mouse X");
            }
            else if (Input.GetAxis("Mouse X") < 0.5)
            {
                mouseX = 0;
            }
        }
        

        transform.Rotate(new Vector3(0, mouseX, 0));
        // transform.forward = new Vector3(transform.forward.x, transform.rotation.y, transform.forward.z);
    }

    public void LookRotation(Transform character, Transform camera)
    {
        float yRot = CrossPlatformInputManager.GetAxis("Mouse X") * XSensitivity;
        float xRot = CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity;

        m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
        m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

        if (clampVerticalRotation)
            m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);

        if (smooth)
        {
            character.localRotation = Quaternion.Slerp(character.localRotation, m_CharacterTargetRot,
                smoothTime * Time.deltaTime);
            camera.localRotation = Quaternion.Slerp(camera.localRotation, m_CameraTargetRot,
                smoothTime * Time.deltaTime);
        }
        else
        {
            character.localRotation = m_CharacterTargetRot;
            camera.localRotation = m_CameraTargetRot;
        }

        UpdateCursorLock();
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

    public void UpdateCursorLock()
    {
        //if the user set "lockCursor" we check & properly lock the cursos
        if (lockCursor)
            InternalLockUpdate();
    }

    private void InternalLockUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            m_cursorIsLocked = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            m_cursorIsLocked = true;
        }

        if (m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void MoveInput(){
        // move left
        if (Input.GetKey(KeyCode.A)) {
            transform.Translate(Vector3.right * (Time.deltaTime * -moveSpeed));
        }
        // move right
        if (Input.GetKey(KeyCode.D)){
            transform.Translate(Vector3.right * (Time.deltaTime * moveSpeed));
        }
        // move forward
        if (Input.GetKey(KeyCode.W)){
            transform.Translate(Vector3.forward * (Time.deltaTime * moveSpeed));
        }
        // move backward
        if (Input.GetKey(KeyCode.S)){
            transform.Translate(Vector3.forward * (Time.deltaTime * -moveSpeed));
        }
    }

    void Jump(){
        rb.AddForce(new Vector3(0, 10, 0), ForceMode.Impulse);
    }

}
