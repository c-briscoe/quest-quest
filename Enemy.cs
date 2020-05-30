using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    private GameObject[] players;
    private GameObject closestPlayer;
    [Tooltip("The enemy's maximum HP")]
    public int maxHP;
    public int hP;
    [Tooltip("The Nav Mesh Agent used to move the character")]
    public NavMeshAgent thisAgent;
    [Tooltip("The aniator used to control the model")]
    public Animator thisAnim;
    [Tooltip("The model for the enemy knight")]
    public GameObject thisModel;
    [Tooltip("How much damage the enemy's attacks deal")]
    public int attack;
    private bool dead;
    private float deathTime;
    private GameObject targetPlayer;
    private int xpValue;
    [Tooltip("The hitbox used to calculate damage")]
    public Collider thisBox;
    private bool targeted;
    [Tooltip("The points around which this enemy will patrol")]
    public Transform[] patrolPoints;
    private int patrolPos = 0;
    // Start is called before the first frame update
    void Start()
    {
        hP = maxHP;
        dead = false;
        deathTime = 0;
        //thisAgent = GetComponent<NavMeshAgent>();
        Scene thisScene = SceneManager.GetActiveScene();
        for(int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            if(thisScene.Equals(SceneManager.GetSceneByBuildIndex(i)))
            {
                xpValue = i + 1;
            }
        }
        /*if (GameObject.FindGameObjectsWithTag("Player").Length > 0)
        {
            targetPlayer = GameObject.FindGameObjectsWithTag("Player")[Random.Range(0, GameObject.FindGameObjectsWithTag("Player").Length)];
        }*/
        targeted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
        {

            deathTime += Time.deltaTime; //waits for the death animation to fully play
            if (deathTime >= 2.3f) //the end of the death animation
            {
                GameObject reward = Instantiate(Resources.Load("XP Blob"), transform.position, Quaternion.identity) as GameObject; //creates an XP blob
                reward.GetComponent<XPBlob>().xpValue = this.xpValue; //gives the XP blob the proper amount of XP
                Destroy(gameObject);
            }
        }
        else
        {
            /*if (targetPlayer == null && GameObject.FindGameObjectsWithTag("Player").Length > 0)
            {
                targetPlayer = GameObject.FindGameObjectsWithTag("Player")[Random.Range(0, GameObject.FindGameObjectsWithTag("Player").Length)];
            }*/
            if(targeted)
            {
                thisAgent.destination = targetPlayer.transform.position;
                thisAnim.SetTrigger("Go");
            }
            else
            {
                thisAgent.destination = patrolPoints[patrolPos].position;
                if(Vector3.Distance(transform.position, patrolPoints[patrolPos].position) <= .5f)
                {
                    patrolPos++;
                    if(patrolPos >= patrolPoints.Length)
                    {
                        patrolPos = 0;
                    }
                }
                thisAnim.SetTrigger("Go");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player Sword"))
        {
            hP -= collision.gameObject.GetComponent<Sword>().wielder.GetComponent<Knight>().GetHit();
            if(hP <= 0)
            {
                dead = true;
                thisAnim.SetTrigger("Die");
            }
        }
    }

    public void DetectPlayer(GameObject thePlayer)
    {
        if (!targeted)
        {
            targetPlayer = thePlayer;
            targeted = true;
        }
    }

    public void DealDamage(int numDamage)
    {
        hP -= numDamage;
        if (hP <= 0)
        {
            dead = true;
            thisAnim.SetTrigger("Die");
        }
    }
}
