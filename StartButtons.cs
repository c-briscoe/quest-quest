using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButtons : MonoBehaviour
{
    public bool newButton;
    public Text enteredText;
    public Knight thisKnight;
    public GameObject startScreen;
    public GameObject thisPlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WhenPressed()
    {
        if (newButton || PlayerPrefs.GetInt(enteredText.text) < 1)
        {
            PlayerPrefs.SetInt(enteredText.text + " Int", 1);
            PlayerPrefs.SetFloat(enteredText.text + " Float", 0);
            PlayerPrefs.SetString(enteredText.text + " String", "0");
            thisKnight.SetData(enteredText.text, 1, 0, "0");
            Debug.Log((enteredText.text + " int: " + PlayerPrefs.GetInt(enteredText.text + " Int")));
            Debug.Log((enteredText.text + " float: " + PlayerPrefs.GetFloat(enteredText.text + " Float")));
            Debug.Log((enteredText.text + " string: " + PlayerPrefs.GetString(enteredText.text + " String")));
        }
        else
        {
            thisKnight.SetData(enteredText.text, PlayerPrefs.GetInt(enteredText.text + " Int"), PlayerPrefs.GetFloat(enteredText.text + " Float"), PlayerPrefs.GetString(enteredText.text + " String"));
        }
        startScreen.SetActive(false);
        thisPlayer.transform.Translate(Vector3.up * 15);
        thisPlayer.GetComponent<player>().BeginGame();
    }
}
