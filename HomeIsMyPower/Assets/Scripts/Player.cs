using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour
{
	//Movement variables
	[SerializeField]
	private float crouchSpeed;
	[SerializeField]
	private float walkSpeed;
	[SerializeField]
	private float sprintSpeed;
	[SerializeField]
	private Transform camTransform;
	private bool crouching;
	private bool sprinting;
    private bool moving;

	//Weapon variables
	[SerializeField]
	// private Weapon curWeapon;
	private float cooldownTimer;
	private bool shouldCooldown;
    [SerializeField]
	public Animator playerAnimation;
    private bool paused = false;
    public KeyCode[] controls;
    [SerializeField]
    private int comboCounterFist = 0;
    [SerializeField]
    private int comboCounterKick = 1;
    private int comboTimer = 180;
    [SerializeField]
    private Animator transitionAnim;

    /*! \brief Called on initialization
	 */
    private void Start()
	{
		crouchSpeed = 1.0f;
		walkSpeed = 8.0f;
		sprintSpeed = 20.0f;
		// curWeapon = transform.Find("Fists").GetComponent<Weapon>();
		cooldownTimer = 0.0f;
		shouldCooldown = false;
        playerAnimation = GetComponent<Animator>();
		camTransform = GameObject.Find("Main Camera").GetComponent<Transform>();
		Object.DontDestroyOnLoad(gameObject);
		Object.DontDestroyOnLoad(camTransform.gameObject);
	}

	/*! \brief Called once every frame
	 */
	private void Update()
	{
        if (!paused)
        {
            if (shouldCooldown)
            {
				cooldownTimer += Time.deltaTime;

                /*
				if(cooldownTimer > curWeapon.GetCooldown()) {
					cooldownTimer = 0.0f;
					shouldCooldown = false;
				}
                */
			}

            // Reset
            if (Input.GetKey(KeyCode.F10))
            {
                SceneManager.LoadScene("Grimsby");
            }

            Rotate();
            camTransform.position = new Vector3(transform.position.x, camTransform.position.y, transform.position.z);
            GetInput();
            comboTimer--;

            if (comboTimer <= 0) {
                comboCounterFist = 1;
                comboCounterKick = 1;
            }
        }
	}

	/*! \brief Rotates the player to face the mouse
	 */
	private void Rotate()
	{
		//Unity is dumb so raycasting is apparently the best way to so this
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit floorHit;

		if(Physics.Raycast(ray, out floorHit, 100, LayerMask.GetMask("Floor")))
		{
			Vector3 toMouse = floorHit.point - transform.position;
			toMouse.y = 0.0f;
			transform.rotation = Quaternion.LookRotation(toMouse);
		}
	}

	/*! \brief Takes input and moves the player
	 */
	private void GetInput()
	{
		Vector3 movement = new Vector3();

        //Move the player in a specified direction
        if (Input.GetKey(controls[4]))
        {
            movement += transform.forward;
            moving = true;
        }
        else if (Input.GetKey(controls[5]))
        {
            movement -= transform.forward;
            moving = true;
        }
        else if (Input.GetKey(controls[6]))
        {
            movement -= transform.right;
            moving = true;
        }
        else if (Input.GetKey(controls[7]))
        {
            movement += transform.right;
            moving = true;
        }
        else {
            moving = false;
        }


		if(Input.GetKeyDown(controls[2]) && !sprinting) {
			crouching = true;
			sprinting = false;
            moving = false;
		}

		else if(Input.GetKeyUp(controls[2]) && crouching) {
			crouching = false;
		}

		if(Input.GetKeyDown(controls[1])) {
			crouching = false;
			sprinting = true;
            moving = false;
		}

		else if(Input.GetKeyUp(controls[1]) && sprinting) {
			sprinting = false;
		}

		Move(movement);

        //Attack if the left mouse button is pressed
        if (Input.GetMouseButtonDown(0)) {
            AttackFist();
        }
		
        if (Input.GetMouseButtonDown(1)) {
            AttackKick();
        }

	}

	/*! \brief Moves the player in a direction
	 *
	 * \param (Vector3) dir - The direction to move in
	 */
	private void Move(Vector3 dir)
	{
		float speed = walkSpeed;
        if (crouching)
        {
            speed = crouchSpeed;
        }

        if (sprinting)
        {
            speed = sprintSpeed;
            playerAnimation.SetBool("Moving", true);
        }
        else {
            playerAnimation.SetBool("Moving", false);
            playerAnimation.SetFloat("Velocity X", 0);
            playerAnimation.SetFloat("Velocity Z", 0);
        }

        if (moving)
        {
            playerAnimation.SetBool("Moving", true);
        }
        else {
            playerAnimation.SetBool("Moving", false);
            playerAnimation.SetFloat("Velocity X", 0);
            playerAnimation.SetFloat("Velocity Z", 0);
        }

        if (Input.GetKeyDown(KeyCode.F)) {
            playerAnimation.SetTrigger("RollTrigger");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            playerAnimation.SetTrigger("DodgeTrigger");
        }
        transform.position += dir * speed * Time.deltaTime;
        playerAnimation.SetFloat("Velocity X", speed);
        playerAnimation.SetFloat("Velocity Z", speed);
    }

	/*! \brief Calls the weapons fire method
	 */
	private void AttackFist()
	{
        comboTimer = 180;
		if(!shouldCooldown) {
            /*
                if (curWeapon.gameObject.name == "Fists") {
                    comboCounterFist++;
                    if (comboCounterFist >= 6)
                        comboCounterFist = 1;

                    playerAnimation.SetInteger("Action", comboCounterFist);
                    playerAnimation.SetTrigger("AttackTrigger");
                }
                */
            shouldCooldown = true;
		}
	}

    private void AttackKick()
    {
        comboTimer = 180;
        if (!shouldCooldown)
        {
            /*
            if (curWeapon.gameObject.name == "Fists")
            {
                comboCounterKick += 2;
                if (comboCounterKick >= 4)
                    comboCounterKick = 1;

                playerAnimation.SetInteger("Action", comboCounterKick);
                playerAnimation.SetTrigger("AttackKickTrigger");
            }
            */
            shouldCooldown = true;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        WorldTransitions(col);
    }

    void WorldTransitions(Collision col)
    {
        switch (col.gameObject.name)
        {
            #region Transition states for Grimsby
            case "GrimGrave_Transition":
                //StartCoroutine(SceneTransition());
                SceneManager.LoadScene("Grimsby");
                transform.position = new Vector3(11.0f, 0.5f, 48.0f);
                break;

            case "Grim_TransitionNorth":

                break;

            case "Grim_TransitionSouth":
                //StartCoroutine(SceneTransition());
                SceneManager.LoadScene("ShoreSide");
                transform.position = new Vector3(194.0f,0.5f,370.0f);
                break;

            case "Grim_TransitionWest":
                //StartCoroutine(SceneTransition());
                SceneManager.LoadScene("Grimsby_Graveyard");
                transform.position = new Vector3(188.0f, 0.5f, 50.0f);
                break;
            #endregion

            #region Transition states for Braedon
            case "Brae_TransitionNorth":
                //StartCoroutine(SceneTransition());
                SceneManager.LoadScene("ShoreSide");
                transform.position = new Vector3(72.0f, 0.5f, 30);
                break;

            case "Brae_TransitionSouth":
                //StartCoroutine(SceneTransition());
                SceneManager.LoadScene("Test Zone");
                break;

            case "Brae_TransitionEast":

                break;

            case "Brae_TransitionWest":

                break;
            #endregion

            #region Transition states for Shoreside
            case "Shor_TransitionNorth":
                //StartCoroutine(SceneTransition());
                SceneManager.LoadScene("Grimsby");
                transform.position = new Vector3(91.0f, 0.5f, 13.0f);
                break;

            case "Shor_TransitionSouth":
                //StartCoroutine(SceneTransition());
                SceneManager.LoadScene("Braedon");
                transform.position = new Vector3(139.0f, 0.5f, 213.0f);
                break;
                #endregion
		}
	}

	public bool Paused
	{
		get { return paused; }
		set { paused = value; }
	}

	private void Hit()
	{
		// curWeapon.PerformHit(LayerMask.GetMask("Enemy"));
	}

    IEnumerator SceneTransition() {
        transitionAnim.SetTrigger("end");
        yield return new WaitForSeconds(2.0f);
    }
}
