using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPCamera : MonoBehaviour {

    [SerializeField]
    private float RotationSpeed = 1.5f;
    [SerializeField]
    private Transform Target;
    [SerializeField]
    private Transform Player;
    private float mouseX;
    private float mouseY;

	// Use this for initialization
	void Start () {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        CamControl();
	}

    void CamControl()
    {
        mouseX += Input.GetAxis("Mouse X") * RotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * RotationSpeed;
        mouseY = Mathf.Clamp(mouseY, -25, 25);

        transform.LookAt(Target);

        Target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        Player.rotation = Quaternion.Euler(0, mouseX, 0);
    }
}
