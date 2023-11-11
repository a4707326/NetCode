using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyCtrl : MonoBehaviour
{
    public static BodyCtrl Instance;
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SwitchGender(0);
    }
    public void SwitchGender(int gender)
    {
        transform.GetChild(gender).gameObject.SetActive(true);
        transform.GetChild(1-gender).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
