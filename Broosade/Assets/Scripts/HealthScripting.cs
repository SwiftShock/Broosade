using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScripting : MonoBehaviour {

    public Image[] healthImages;
    public Sprite[] heartConSprites;
    public PlayerController player;
    private int totalHearts = 5;
    public int startingHearts = 2;
    public int curHealth;
    private int maxHealth;
    public int healthIndex;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        startingHearts = PlayerPrefs.GetInt("MaxHealth", player.maxHealth);
        curHealth = PlayerPrefs.GetInt("Health", player.health);
    }

    void Update()
    {
        startingHearts = player.maxHealth;
        curHealth = player.health;
        healthIndex = startingHearts - curHealth;
        heartConsDisplayed();
        updateHearts();
    }

    void heartConsDisplayed()
    {
        for (int i = 0; i < totalHearts; i++)
        {
            if(startingHearts <= i)
            {
                healthImages[i].enabled = false;
            }
            else
            {
                healthImages[i].enabled = true;
            }
        }
    }

    void updateHearts()
    {
        if(curHealth < 0)
        {
            curHealth = 0;
        }
        if(curHealth < startingHearts)
        {
            if (curHealth == 0)
            {
                foreach(Image image in healthImages)
                {
                    image.sprite = heartConSprites[0];
                }
            }
            else {
                for (int i = startingHearts-1; i >= curHealth; i--)
                {
                    healthImages[i].sprite = heartConSprites[0];
                }   
            }
            
        }
        else
        {
            foreach (Image image in healthImages)
            {
                image.sprite = heartConSprites[1];
            }
        }
    }
}
