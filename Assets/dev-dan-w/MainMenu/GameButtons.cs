using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameButtons : MonoBehaviour
{
    public GameObject playMenu;

    public void DeleteGameSave(){
        GameObject obj = SaveManager.Instance.selectedObject;
        // we dont want to delete game save if we dont have anything selected
        if(obj == null) return;


        if(File.Exists("./GameSaves.save")){
        string[] gameSaves = File.ReadAllLines("./GameSaves.save");

        // If its less than 3, there isnt even 1 valid game save
        if(gameSaves.Length < 3) return;
        int gameSaveCount = 0;
        int.TryParse(gameSaves[0], out gameSaveCount);

        List<string> gameSavesList = new List<string>(gameSaves);
        
        // Filter the id
        Save saveScript = obj.GetComponent<Save>();
        string idDesired = saveScript.id.ToString();

        int index = gameSavesList.IndexOf($"id: {idDesired}");
        if(gameSavesList.Count > index+2 || index >= 0){
            Debug.Log(index + ", " + idDesired);
            gameSavesList.RemoveRange(index, 3);

            gameSavesList[0] = (gameSaveCount-1).ToString();
        }

        File.WriteAllLines("./GameSaves.save", gameSavesList);

        LoadGameSaves.onManageGameSaves?.Invoke();
        }
    }
}
