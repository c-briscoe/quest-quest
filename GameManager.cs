using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    private Knight thisPlayer;
    private string playerName;
    private int level;
    public player localPlayer;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Save()
    {
        //playerName = thisPlayer.playerName;
        PlayerPrefs.SetInt(playerName + " Int", thisPlayer.level);
        PlayerPrefs.SetFloat(playerName + " Float", thisPlayer.maxHP - thisPlayer.hP);
        PlayerPrefs.SetString(playerName + " String", thisPlayer.xP.ToString());
    }

    public void Save(string inName)
    {
        playerName = inName;
        PlayerPrefs.SetInt(playerName + " Int", thisPlayer.level);
        PlayerPrefs.SetFloat(playerName + " Float", thisPlayer.maxHP - thisPlayer.hP);
        PlayerPrefs.SetString(playerName + " String", thisPlayer.xP.ToString());
    }

    public void LevelClear()
    {
        level++;
        Save();
        SceneManager.LoadScene(level);
    }

    private void OnLevelWasLoaded(int level)
    {
        GameObject[] allP = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < allP.Length; i++)
        {
            if (allP[i].GetComponent<player>().isLocalPlayer)
            {
                thisPlayer = allP[i].GetComponent<Knight>();
            }
        }
        thisPlayer.SetData(playerName, PlayerPrefs.GetInt(playerName + " Int"), PlayerPrefs.GetFloat(playerName + " Float"), PlayerPrefs.GetString(playerName + " String"));
    }

    public int GetLevel()
    {
        return level;
    }

    public void SetName(string inNamer, Knight inKnight)
    {
        playerName = inNamer;
        thisPlayer = inKnight;
    }
}
