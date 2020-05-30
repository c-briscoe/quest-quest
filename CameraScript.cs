using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [Tooltip("The positions where the camera will lock for a battle")]
    public float[] fightyPlaces;
    //private bool[] clearedPlaces; //the list of places the players have cleared
    private GameManager theM; //the game manager
    private int thisPlace; //the player's position in the level
    private bool fighting = false; //whether the camera should be locked
    private List<GameObject> spawnedEnemies = new List<GameObject>(); //the enemies spawned in each arena
    private GameObject[] spawns; //where to spawn the enemies
    // Start is called before the first frame update
    void Start()
    {
        /*clearedPlaces = new bool[fightyPlaces.Length];
        for(int i = 0; i < clearedPlaces.Length; i++)
        {
            clearedPlaces[i] = false;
        }*/
        theM = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        thisPlace = 0;
        spawns = GameObject.FindGameObjectsWithTag("NetSpawn");
    }

    // Update is called once per frame
    void Update()
    {
        if (!fighting) { transform.Translate(Vector3.right * Time.deltaTime * 5); } //moves the camera along while not locked
        if (thisPlace >= fightyPlaces.Length) //checks to see if the player has reached the end of the level
        {
            theM.LevelClear(); //moves the player to the next level
        }
        if (transform.position.x >= fightyPlaces[thisPlace] && !fighting) //checks to see if the player should be in an arena
        {
            fighting = true; //locks the camera
            for(int i = 0; i < theM.GetLevel() + 1; i++) //spawns enemies
            {
                foreach(GameObject spawn in spawns)
                {
                    spawnedEnemies.Add(Instantiate(Resources.Load("Combat Enemy"), spawn.transform.position + Vector3.up * 5, Quaternion.identity) as GameObject);
                }
            }
        }
        if(fighting && spawnedEnemies.Count == 0) //to see if the player has defeated all the enemies spawned in an arena
        {
            ClearPlace(); //lets the program know that the enemies in an arena have been cleared
        }
        
    }

    public void ClearPlace() //lets the program know that an arena has been cleared
    {
        //clearedPlaces[thisPlace] = true;
        fighting = false;
        thisPlace++;
    }

    public void RemoveEnemy(GameObject theEnem) //called by arena enemies on death to remove them from the list
    {
        spawnedEnemies.Remove(theEnem);
    }
}
