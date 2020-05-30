using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class player : NetworkBehaviour
{
    [Tooltip("The camera which follows the player")]
    //public Camera perspective;
    private bool paused; //if the player is prevented from moving
    private float facing; //the direction the player is moving
    [Tooltip("The UI object used to show the start screen")]
    public GameObject startScreen;
    //public GameObject uI;
    private Rigidbody thisBody;
    [Tooltip("The model used to render the player")]
    public GameObject knightModel;
    [Tooltip("The animator used to control the player's animations")]
    public Animator knightAnim;
    private bool onRightWall; //whether or not the player has reached the right side of the playable area
    private bool onLeftWall; //whether or not the player has reached the left side of the playable area
    private bool onBackWall; //whether or not the player has reached the back of the playable area
    private bool onFrontWall; //whether or not the player has reached the front side of the playable area
    private bool blocking; //whether or not the player is blocking
    private float idleTime; //how long the player has been standing still since the last time an idle animation played
    private float nextIdle; //when the next secondary idle animation will play
    [Tooltip("The collider attached to the player's sword")]
    public Collider knightSword;
    [Tooltip("The collider attached to the player's shield")]
    public Collider knightShield;
    [Tooltip("The character sheet used by the player's character")]
    public Knight thisKnight;
    //private float maxSpeed;
    private Transform spawnPoint;
    private int aniState; //0 is attack, 1 is for hurt, 2 is for dead,
    [Tooltip("The lengths of the player's animations")]
    public float[] aniTimes;
    private float aniTime;
    private GameManager theManager;

    public override void OnStartLocalPlayer()
    {
        DontDestroyOnLoad(gameObject);
        base.OnStartLocalPlayer();
        paused = true;
        thisBody = GetComponent<Rigidbody>();
        theManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        theManager.localPlayer = this;
        //maxSpeed = 10;
    }
    // Start is called before the first frame update
    void Start()
    {
        onRightWall = onLeftWall = onFrontWall = onBackWall = blocking = false;
        idleTime = 0;
        nextIdle = Random.Range(5, 15);
        spawnPoint = GameObject.FindWithTag("Respawn").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            //perspective.enabled = false;
            startScreen.SetActive(false);
            return;
        }
        if (!paused)
        {
            if (Mathf.Abs(Input.GetAxis("Horizontal")) >= Mathf.Epsilon)
            {
                if (Input.GetAxis("Horizontal") > 0 && thisBody.velocity.x < 10f && !onRightWall)
                {
                thisBody.AddForce(Vector3.right * 550 * Time.deltaTime);
                }
                if (Input.GetAxis("Horizontal") < 0 && thisBody.velocity.x > -10f && !onLeftWall)
                {
                thisBody.AddForce(Vector3.right * -550 * Time.deltaTime);
                }
                knightAnim.SetTrigger("Go");
            }
            if (Mathf.Abs(Input.GetAxis("Vertical")) >= Mathf.Epsilon)
            {
                if (Input.GetAxis("Vertical") > 0 && thisBody.velocity.z < 10f && !onBackWall)
                {
                    thisBody.AddForce(Vector3.forward * 550 * Time.deltaTime);
                }
                if (Input.GetAxis("Vertical") < 0 && thisBody.velocity.z > -10f && !onFrontWall)
                {
                    thisBody.AddForce(Vector3.forward * -550 * Time.deltaTime);
                }
                knightAnim.SetTrigger("Go");
            }
            if(Mathf.Abs(Input.GetAxis("Vertical")) < Mathf.Epsilon && Mathf.Abs(Input.GetAxis("Horizontal")) < Mathf.Epsilon)
            {
                knightAnim.SetTrigger("Stop");
                idleTime++;
                if(nextIdle <= idleTime)
                {
                    knightAnim.SetTrigger("Idle " + Random.Range(1, 4));
                    nextIdle = Random.Range(5, 15);
                }

            }
            if (onRightWall && thisBody.velocity.x > Time.deltaTime)
            {
                thisBody.velocity = new Vector3(Time.deltaTime, thisBody.velocity.y, thisBody.velocity.z);
            }
            if (onLeftWall && thisBody.velocity.x < Time.deltaTime)
            {
                thisBody.velocity = new Vector3(Time.deltaTime, thisBody.velocity.y, thisBody.velocity.z);
            }
            if (onFrontWall && thisBody.velocity.z < 0)
            {
                thisBody.velocity = new Vector3(thisBody.velocity.x, thisBody.velocity.y, 0);
            }
            if (onBackWall && thisBody.velocity.z > 0)
            {
                thisBody.velocity = new Vector3(thisBody.velocity.x, thisBody.velocity.y, 0);
            }
            /*if (Input.GetAxis("Jump") > 0 && Mathf.Abs(thisBody.velocity.y) < Mathf.Epsilon)
            {
            thisBody.AddForce(transform.up * 250);
            }*/
            if (!blocking)
            {
                if (Mathf.Abs(thisBody.velocity.x) > 0.001f && Mathf.Abs(thisBody.velocity.z) > 0.001f)
                {
                    facing = (int)(Mathf.Atan(thisBody.velocity.x / thisBody.velocity.z) * 180 / Mathf.PI);
                    if (thisBody.velocity.z < 0)
                    {
                        facing += 180;
                    }
                    //knightModel.transform.rotation = Quaternion.AngleAxis(facing, Vector3.up);
                }
                else if (Mathf.Abs(thisBody.velocity.z) <= 0.001f && Mathf.Abs(thisBody.velocity.x) > 0.001f)
                {
                    if (thisBody.velocity.x < 0)
                    {
                        facing = -90;
                    }
                    if (thisBody.velocity.x > 0)
                    {
                        facing = 90;
                    }
                }
                else if(Mathf.Abs(thisBody.velocity.x) <= 0.001f && Mathf.Abs(thisBody.velocity.z) > 0.001f)
                {
                    if(thisBody.velocity.z < 0)
                    {
                        facing = 180;
                    }
                    if(thisBody.velocity.z > 0)
                    {
                        facing = 0;
                    }
                }
                knightModel.transform.rotation = Quaternion.AngleAxis(facing, Vector3.up);
            }
            if(Input.GetAxis("Fire3") > 0)
            {
                paused = true;
                aniState = 0;
                aniTime = 0;
                //thisBody.velocity = new Vector3(0, 0, 0);
                knightAnim.SetTrigger("Attack");
            }
        }
        else
        {
            if (onLeftWall && thisBody.velocity.x < Time.deltaTime)
            {
                thisBody.velocity = new Vector3(Time.deltaTime, thisBody.velocity.y, thisBody.velocity.z);
            }
            aniTime += Time.deltaTime;
            if(aniTime >= aniTimes[aniState])
            {
                paused = false;
            }
        }
    }

    public void Pause(bool pause)
    {
        paused = pause;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Right Wall")) //used to determine whether the player is on the right edge of the screen
        {
            onRightWall = true;
        }
        if(collision.gameObject.CompareTag("Left Wall")) //used to determine whether the player is on the left edge of the screen
        {
            onLeftWall = true;
        }
        if (collision.gameObject.CompareTag("Back Wall")) //used to determine if the player is on the back of the level
        {
            onBackWall = true;
        }
        if (collision.gameObject.CompareTag("Front Wall")) //used to determine if the player is on the front of the level
        {
            onFrontWall = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Right Wall")) //used to determine if the player is no longer colliding with the right edge of the screen
        {
            onRightWall = false;
        }
        if (collision.gameObject.CompareTag("Left Wall")) //used to determine if the player is no longer colliding with the left edge of the screen
        {
            onLeftWall = false;
        }
        if (collision.gameObject.CompareTag("Back Wall")) //used to determine if the player is no longer on the back of the level
        {
            onBackWall = false;
        }
        if (collision.gameObject.CompareTag("Front Wall")) //used to determine if the player is no longer on the front of the level
        {
            onFrontWall = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider myCollider = collision.contacts[0].thisCollider;
        if(collision.gameObject.CompareTag("Enemy Sword") && !(myCollider.Equals(knightSword) || (myCollider.Equals(knightShield) && blocking))) //to check whether or not the player should take damage
        {
            thisKnight.hP -= collision.gameObject.GetComponent<Sword>().wielder.GetComponent<Enemy>().attack;

            if(thisKnight.hP <= 0)
            {
                knightAnim.SetTrigger("Die");
                aniTime = 0;
                aniState = 2;
                paused = true;
                thisBody.velocity = new Vector3(0, 0, 0);
            }
            else
            {
                knightAnim.SetTrigger("Hurt");
                aniTime = 0;
                aniState = 1;
                paused = true;
                thisBody.velocity = new Vector3(0, 0, 0);
            }
        }
    }

    public void BeginGame()
    {
        paused = false;
        theManager.SetName(thisKnight.playerName, thisKnight);
    }
}
