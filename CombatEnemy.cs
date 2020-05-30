using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class CombatEnemy : MonoBehaviour
{
    private GameObject[] players;
    private GameObject closestPlayer;
    [Tooltip("The enemy's maximum HP")]
    public int maxHP;
    public int hP;
    //[Tooltip("The Nav Mesh Agent used to move the character")]
    private NavMeshAgent thisAgent;
    [Tooltip("The aniator used to control the model")]
    public Animator thisAnim;
    [Tooltip("The model for the enemy knight")]
    public GameObject thisModel;
    [Tooltip("How much damage the enemy's attacks deal")]
    public int attack;
    private bool dead; //whether the enemy has died or not
    private float deathTime; //the length of the enemy's death animation
    private GameObject targetPlayer; //the player being targeted by the enemy
    private int xpValue; //how much xp the player gets for defeating the enemy
    [Tooltip("The hitbox used to calculate damage")]
    public Collider thisBox;
    private CameraScript theCam; //the script used to move the camera
    // Start is called before the first frame update
    void Start() //sets up the enemy's stats properly
    {
        if(transform.position.y < 1)
        {
            transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        }
        hP = maxHP;
        dead = false;
        deathTime = 0;
        thisAgent = GetComponent<NavMeshAgent>();
        thisAgent.enabled = false;
        thisAgent.enabled = true;
        Scene thisScene = SceneManager.GetActiveScene();
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            if (thisScene.Equals(SceneManager.GetSceneByBuildIndex(i)))
            {
                xpValue = i + 1;
            }
        }
        if (GameObject.FindGameObjectsWithTag("Player").Length > 0) //targets the player immediately, rather than waiting for a trigger
        {
            targetPlayer = GameObject.FindGameObjectsWithTag("Player")[Random.Range(0, GameObject.FindGameObjectsWithTag("Player").Length)];
        }
        theCam = GameObject.FindWithTag("MainCamera").GetComponent<CameraScript>();
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
            if (targetPlayer == null && GameObject.FindGameObjectsWithTag("Player").Length > 0) //if the original player was gone, retarget
            {
                targetPlayer = GameObject.FindGameObjectsWithTag("Player")[Random.Range(0, GameObject.FindGameObjectsWithTag("Player").Length)];
            }
            if (targetPlayer != null) //null check in case there is no player whatsoever in the world
            {
                thisAgent.destination = targetPlayer.transform.position; //moves toward the player
                thisAnim.SetTrigger("Go");
            }
        }
    }

    private void OnCollisionEnter(Collision collision) //mostly used to calculate damage
    {
        if (collision.gameObject.CompareTag("Player Sword")) //used exclusively for players' swords
        {
            foreach (ContactPoint contact in collision.contacts)
                if (contact.otherCollider == thisBox)
                {
                    hP -= collision.gameObject.GetComponent<Sword>().wielder.GetComponent<Knight>().GetHit();
                    if (hP <= 0)
                    {
                        dead = true;
                        theCam.RemoveEnemy(gameObject);
                        thisAnim.SetTrigger("Die");
                    }
                }
        }
    }

    public void DetectPlayer(GameObject thePlayer) //method left over from other enemy script
    {
        targetPlayer = thePlayer;
    }

    public void DealDamage(int numDamage)
    {
        hP -= numDamage;
        if (hP <= 0)
        {
            dead = true;
            theCam.RemoveEnemy(gameObject);
            thisAnim.SetTrigger("Die");
        }
    }
}
