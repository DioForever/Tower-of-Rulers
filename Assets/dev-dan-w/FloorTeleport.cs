using System.Collections;
using UnityEngine;
using TMPro;

public class FloorTeleport : MonoBehaviour
{
    public GenerationteInitiator initiatorScript;
    public UnityEngine.Rendering.Universal.Light2D light2D;
    public GameObject floorMenu;
    public TextMeshProUGUI floorText;
    private FloorBtnScript floorContinueScript;
    public float intensityIncreaseRate = 1.5f;
    public float outerRadiusIncreaseRate = 3f;
    public bool achieved = true;

    private bool isPlayerInside = false;
    private bool isPressingShift = false;

    private void Awake()
    {
        if(initiatorScript == null)
        {
            initiatorScript = FindObjectOfType<GenerationteInitiator>();
        }
        if(floorMenu == null){
            // floorMenu = GameObject.Find("FloorMenu");
            floorMenu = FindInActiveObjectByName("FloorMenu");
        }
        if(floorText == null){
            floorText = FindInActiveObjectByName("ContinueFloorText").GetComponent<TextMeshProUGUI>();
            floorText.text = "Continue to floor " + (PlayerPrefs.GetInt("highestFloor", 1) + 1);
        }
        if(floorContinueScript == null){
            floorContinueScript = GameObject.Find("ContinueFloor").GetComponent<FloorBtnScript>();
            floorContinueScript.floorNumber = (PlayerPrefs.GetInt("highestFloor", 1));
        }

        ResetLightProperties();
    }

    private void Update(){
        // Check if the player presses the Shift key to turn of the floor menu if its on
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            if(floorMenu.activeSelf){
                floorMenu.SetActive(false);
            }
        }
    }
    
    GameObject FindInActiveObjectByName(string name)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].name == name)
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "player" && (!achieved) || (achieved && !IsDungeon()))
        {
            isPlayerInside = true;
            StartCoroutine(TriggerGenerateNextFloor());
            StartCoroutine(IncreaseLightProperties());
            Debug.Log("Player entered the teleporter");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "player")
        {
            isPlayerInside = false;
            ResetLightProperties();
        }
    }

    IEnumerator TriggerGenerateNextFloor()
    {
        yield return new WaitForSeconds(3f);
        floorMenu.SetActive(true);
    }

    IEnumerator IncreaseLightProperties()
    {
        while (isPlayerInside)
        {
            // Limit the intensity and outer radius of the light
            if (light2D.intensity >= intensityIncreaseRate*3 || light2D.pointLightOuterRadius >= outerRadiusIncreaseRate*3)
            {
                yield return null;
            }

            light2D.intensity += intensityIncreaseRate * Time.deltaTime;
            light2D.pointLightOuterRadius += outerRadiusIncreaseRate * Time.deltaTime;
            yield return null;
        }
    }

    private void ResetLightProperties()
    {
        light2D.intensity = 0;
        light2D.pointLightOuterRadius = 0;
    }

    public bool IsDungeon(){
        int floorTypeValue = PlayerPrefs.GetInt("floorType", 0);
        bool isDungeonFloor = floorTypeValue == 1 ? true : false;
        return isDungeonFloor;
    }
}
