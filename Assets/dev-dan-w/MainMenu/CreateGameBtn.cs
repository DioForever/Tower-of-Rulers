using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CreateGameBtn : MonoBehaviour
{
    [SerializeField] private string BtnName;


    public void updateName(string nameArg){
        BtnName = nameArg;
    }
    public void CreateGame(){
        // Provizorní načítání herních savů
        if(BtnName == "" || BtnName.Length>40 || BtnName == null) return;
        string[] gameSaves = File.ReadAllLines("./GameSaves.save");
        int gameSavesCount = 1;
        List<string> gameSavesList = new List<string>(gameSaves);
        if(gameSaves.Length > 0){
            int.TryParse(gameSaves[0], out gameSavesCount);
        }
        if(gameSavesList.Count == 0) gameSavesList.Add(1.ToString());

        gameSavesList[0] = (gameSavesCount+1).ToString();
        gameSavesList.Add("id: "+ (gameSavesCount+1).ToString());
        gameSavesList.Add("floor: "+1.ToString());
        gameSavesList.Add("name: "+BtnName);
        
        
        File.WriteAllLines("./GameSaves.save", gameSavesList);

        LoadGameSaves.onManageGameSaves?.Invoke();
        // Move to the game - TODO: connect with real game saves
        // UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

}
