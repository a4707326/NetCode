using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogCell : MonoBehaviour
{
    public void Initial(string playerName,string content)
    { 
        TMP_Text nameText = transform.Find("NameText").GetComponent<TMP_Text>();
        nameText.text = playerName;
        TMP_Text contentText = transform.Find("ContentText").GetComponent<TMP_Text>();
        contentText.text = content;
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
