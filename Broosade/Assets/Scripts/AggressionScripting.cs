using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AggressionScripting : MonoBehaviour {

    public Image meterImage;
    public Sprite[] meterSprites;
    public PlayerController player;
    private int totalMeter = 3;
    public int curMeter;
    
    // Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerController>();
        curMeter = PlayerPrefs.GetInt("Meter", player.meter);
    }
	
	// Update is called once per frame
	void Update () {
        curMeter = player.meter;

        if(curMeter == 1)
        {
            meterImage.sprite = meterSprites[0];
        }
        if(curMeter == 2)
        {
            meterImage.sprite = meterSprites[1];
        }
        if(curMeter == 3)
        {
            meterImage.sprite = meterSprites[2];
        }
	}
}
