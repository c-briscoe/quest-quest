using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Knight : MonoBehaviour
{
    [Tooltip("The name being used to save the player's data")]
    public string playerName;
    [Tooltip("The player's level")]
    public int level;
    [Tooltip("The text box displaying the player's name")]
    public Text nameDisplay;
    [Tooltip("The player's current HP")]
    public int hP;
    [Tooltip("The player's maximum HP")]
    public int maxHP;
    private int attackPower;
    [Tooltip("The HP bar above the player")]
    public Slider hpBar;
    [Tooltip("The player's progress to their next level")]
    public int xP;
    [Tooltip("The player controller")]
    public player thePlayer;
    [Tooltip("The XP bar above the player")]
    public Slider xpBar;

    public Knight(string newName, int newLevel, float newDMG, string newXP)
    {
        SetData(newName, newLevel, newDMG, newXP);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetData(string newName, int newLevel, float newDMG, string newXP)
    {
        playerName = newName;
        level = newLevel;
        maxHP = newLevel * 10;
        hP = (int)(maxHP - newDMG);
        hpBar.value = hpBar.maxValue = hP;
        attackPower = level;
        nameDisplay.text = newName;
        xP = int.Parse(newXP);
        xpBar.maxValue = level * 10;
        xpBar.value = xP;
    }

    public int GetHit()
    {
        return attackPower;
    }

    public void GetXP(int newGain) //made this way to accommodate variable XP gain
    {
        xP += newGain; //adds XP gained to total
        xpBar.value = xP; //adds gained XP to XP bar
        if(xP >= level * 10) //scales XP needed to level up by 10 per level
        {
            xP -= level * 10; //subtracts xp equal to the amount needed to level up to allow for overflow xp
            xpBar.value = xP; //makes the xp the player has equal to the amount it should be
            level++; //increases the player's level
            maxHP += 10; //increases the player's max HP
            hP = maxHP; //fully heals the player
            attackPower++; //scales the player's attack
            hpBar.value = hpBar.maxValue = maxHP; //scales the HP bar's value to reflect the change in HP
            xpBar.maxValue = level * 10; //scales the max value of the XP bar
            //xpBar.value = 0; //resets the xp bar's value
            //xP = 0; //sets XP to 0 to ensure that the xp needed to gain the next level is 10 greater than the previous one
        }
    }
}
