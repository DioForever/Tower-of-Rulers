using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    

    [SerializeField] private GameObject objShow;
    [SerializeField] private GameObject objHide;
    [SerializeField] private GameObject prefab;
    [SerializeField] private List<GameObject> saveObjects = new List<GameObject>();
    [SerializeField] private int lastCheckedSavedObjSize = 0;
    public GameObject parent;

    public GameObject selectedObject;


    
    private void Awake() {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Spawn(int id, int floorNum, string gameName)
    {
        GameObject spawnedPrefab = Instantiate(prefab, Vector2.zero, transform.rotation) as GameObject;
        spawnedPrefab.transform.SetParent(parent.transform);
        Save saveScript = spawnedPrefab.GetComponent<Save>();
        
        saveScript.floorText = floorNum.ToString();
        saveScript.nameText = gameName;
        saveScript.id = id;

        saveObjects.Add(spawnedPrefab);
    }

    public bool UpdateSpawnedObjects(){
        if(lastCheckedSavedObjSize != saveObjects.Count){
            lastCheckedSavedObjSize = saveObjects.Count;
            ClearSpawnedObjects();
            return true;
        }
        return false;
    }

    public void ClearSpawnedObjects(){
        foreach(GameObject obj in saveObjects){
            GameObject.Destroy(obj, 0);
        }
    }

    public void Select(GameObject selectObj, int id, string gameName) {
        Color myColor;
        ColorUtility.TryParseHtmlString("#fbf28c", out myColor);

        if (selectedObject != null) {
            // Reset color for all children of the previously selected object
            Image[] childImages = selectedObject.GetComponentsInChildren<Image>();
            foreach (Image childImage in childImages) {
                childImage.color = new Color(255, 255, 255);
            }
        }

        // Save reference to the newly selected object
        selectedObject = selectObj;
        // Set color for all children of the newly selected object
        Image[] selectedObjectChildImages = selectedObject.GetComponentsInChildren<Image>();
        foreach (Image childImage in selectedObjectChildImages) {
            childImage.color = myColor;
        }
    }


    public void ShowHideObjects()
    {
        objShow.gameObject.SetActive(true);
        objHide.gameObject.SetActive(false);
    }
}
