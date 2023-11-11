using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;

public class PlayerListCell : MonoBehaviour
{

    public TMP_Text nameTMP;
    public TMP_Text readyTMP;
    public TMP_Text genderTMP;
    public PlayerInfo PlayerInfo { get; private set; }

    public void Initial(PlayerInfo playerInfo)
    {
        nameTMP.text = playerInfo.name;
        readyTMP.text = playerInfo.isReady ? "非称" : "ゼ非称";
        genderTMP.text = playerInfo.gender == 0 ? "╧" : "";
    }
    public void UpdateInfo(PlayerInfo playerInfo)
    {
        PlayerInfo = playerInfo;
        nameTMP.text = playerInfo.name;
        SetReady(playerInfo.isReady);
        SetGender(playerInfo.gender);
        //readyTMP.text = playerInfo.isReady ? "非称" : "ゼ非称";
        //genderTMP.text = playerInfo.gender == 0 ? "╧" : "";

    }

    internal void SetReady(bool arg0)
    {
        readyTMP.text = arg0 ? "非称" : "ゼ非称";
    }
    internal void SetGender(int gender)
    {
        genderTMP.text = gender == 0 ? "╧" : "";
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
