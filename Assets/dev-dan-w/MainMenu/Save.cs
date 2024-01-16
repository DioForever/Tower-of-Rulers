using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Save : MonoBehaviour
{
    public TextMeshProUGUI  floorTextObj;
    public TextMeshProUGUI  nameTextObj;

    public string nameText;
    public string floorText;
    public int id;
    public void Init(){
        floorTextObj.text = floorText;
        nameTextObj.text = nameText;
    }

    void Update()
    {
        Init();
    }

    public void selected(){
        SaveManager.Instance.Select(this.gameObject, id, nameText);
    }

}
