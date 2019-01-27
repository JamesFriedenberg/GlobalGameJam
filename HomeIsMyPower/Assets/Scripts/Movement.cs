using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;


public class Movement : MonoBehaviour {
    public int moveSpeed;
    private Rigidbody rb;
    private bool moving;
    [SerializeField]
    private int comboCounterSword = 0;
    private int comboTimer = 180;
    private bool shouldCooldown = true;
    private int dashAnim = 1;
    private int dashAnimCounter = 40;
    [SerializeField]
    private ParticleSystem DashEmmitter;

    [SerializeField]
    private ParticleSystem HealEmmitter;
    private int pSystemTimer = 0;
    private ParticleSystem dashClone;
    public int powerSwitch;

    [SerializeField]
    private UIStat healthBar;
    [SerializeField]
    private UIStat manaBar;

    [SerializeField]
    private Image powerIcon;

    [SerializeField]
    private Sprite energyIcon;

    [SerializeField]
    private Sprite healIcon;

    [SerializeField]
    private Sprite dashIcon;

    private float health;
    private float mana;

    [SerializeField]
    public Animator playerAnimation;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        playerAnimation = GetComponent<Animator>();

        health = 100;
        mana = 100;

        healthBar.Init(100, 100);
        manaBar.Init(100,100);
    }

    // Update is called once per frame
    void Update () {
        // A switch for controllering power switches
        if (Input.GetMouseButtonDown(1))
        {
            switch (powerSwitch)
            {
                case 0:
                    Dash();
                    break;

                case 1:
                    Dash();
                    break;

                case 2:
                    Instantiate(HealEmmitter, transform.position, Quaternion.identity);
                    break;
            }
        }

        // Going up the optional powers, also used to changed UI and other things based of power
        if (Input.GetKeyDown(KeyCode.R))
        {
            powerSwitch++;

            if (powerSwitch > 2)
            {
                powerSwitch = 0;
            }

            switch (powerSwitch)
            {
                case 0:
                    Debug.Log("Enery Blast!!!");
                    powerIcon.sprite = energyIcon;
                    break;

                case 1:
                    Debug.Log("Dash!!!");
                    powerIcon.sprite = dashIcon;
                    break;

                case 2:
                    Debug.Log("Heal!!!");
                    powerIcon.sprite = healIcon;
                    break;
            }
        }
        // Going down the optional powers, also used to changed UI and other things based of power
        else if (Input.GetKeyDown(KeyCode.E))
        {
            powerSwitch--;

            if (powerSwitch < 0)
            {
                powerSwitch = 2;
            }

            switch (powerSwitch)
            {
                case 0:
                    Debug.Log("Enery Blast!!!");
                    powerIcon.sprite = energyIcon;
                    break;

                case 1:
                    Debug.Log("Dash!!!");
                    powerIcon.sprite = dashIcon;
                    break;

                case 2:
                    Debug.Log("Heal!!!");
                    powerIcon.sprite = healIcon;
                    break;
            }
        }

        MoveInput();

        Attack();

        comboTimer--;

        if (comboTimer <= 0)
        {
            comboCounterSword = 1;
        }

        dashAnimCounter--;

        if (dashAnimCounter <= 0)
        {
            dashAnim = 0;
            Destroy(dashClone, 1.0f);
            playerAnimation.SetInteger("Jumping", dashAnim);
        }
    }

    private void Dash() {
        dashAnim = 1;
        dashAnimCounter = 40;
        pSystemTimer = 20;
        playerAnimation.SetInteger("Jumping", dashAnim);
        playerAnimation.SetTrigger("JumpTrigger");

        transform.Translate(Vector3.forward * 10);

        dashClone = Instantiate(DashEmmitter, transform.position, Quaternion.identity) ;
    }

    private void MoveInput(){
        // move left
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.right * (Time.deltaTime * (-moveSpeed / 3.75f)));
            moving = true;
            playerAnimation.SetFloat("Velocity X", (moveSpeed / 2));
            playerAnimation.SetFloat("Velocity Z", (moveSpeed / 2));
        }
        // move right
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * (Time.deltaTime * (moveSpeed / 3.75f)));
            moving = true;
            playerAnimation.SetFloat("Velocity X", (moveSpeed / 2));
            playerAnimation.SetFloat("Velocity Z", (moveSpeed / 2));
        }
        // move forward
        else if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * (Time.deltaTime * (moveSpeed / 1.75f)));
            moving = true;
            playerAnimation.SetFloat("Velocity X", moveSpeed);
            playerAnimation.SetFloat("Velocity Z", moveSpeed);
        }
        // move backward
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.forward * (Time.deltaTime * (-moveSpeed / 3.75f)));
            moving = true;
            playerAnimation.SetFloat("Velocity X", (moveSpeed / 2));
            playerAnimation.SetFloat("Velocity Z", (moveSpeed / 2));
        }
        else
        {
            moving = false;
        }

        if (moving)
        {
            playerAnimation.SetBool("Moving", true);
        }
        else
        {
            playerAnimation.SetBool("Moving", false);
            playerAnimation.SetFloat("Velocity X", 0);
            playerAnimation.SetFloat("Velocity Z", 0);
        }
    }

    private void Jump(){
        rb.AddForce(new Vector3(0, 10, 0), ForceMode.Impulse);
    }

    private void Attack() {
            if (Input.GetMouseButtonDown(0))
            {
                comboTimer = 180;
                if (shouldCooldown) // put ! here if needed
                {
                    comboCounterSword++;
                    if (comboCounterSword >= 6) { 
                        comboCounterSword = 1;
                    }
                    playerAnimation.SetInteger("Action", comboCounterSword);
                    playerAnimation.SetTrigger("AttackTrigger");
                }
             }
        else if (Input.GetKeyDown(KeyCode.F) && moving == true)
        {
            comboCounterSword = 1;
            playerAnimation.SetInteger("Action", comboCounterSword);
            playerAnimation.SetTrigger("RollTrigger");
        }
    }
    }
