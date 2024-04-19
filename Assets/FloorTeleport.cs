using System.Collections;
using UnityEngine;
public class FloorTeleport : MonoBehaviour
{
    public GenerationteInitiator initiatorScript;
    public UnityEngine.Rendering.Universal.Light2D light2D;
    public float intensityIncreaseRate = 1.5f;
    public float outerRadiusIncreaseRate = 3f;
    public bool achieved = true;

    private bool isPlayerInside = false;

    private void Awake()
    {
        if(initiatorScript == null)
        {
            initiatorScript = FindObjectOfType<GenerationteInitiator>();
        }
        ResetLightProperties();
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Something entered the teleporter");
        if (other.gameObject.name == "player" && !achieved)
        {
            isPlayerInside = true;
            StartCoroutine(TriggerGenerateNextFloor());
            StartCoroutine(IncreaseLightProperties());
            Debug.Log("Player entered the teleporter");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Something exited the teleporter");
        if (other.gameObject.name == "player")
        {
            isPlayerInside = false;
            ResetLightProperties();
            Debug.Log("Player exited the teleporter");
        }
    }

    IEnumerator TriggerGenerateNextFloor()
    {
        yield return new WaitForSeconds(3f);
        if (!achieved)
            initiatorScript.GenerateNextFloor();
    }

    IEnumerator IncreaseLightProperties()
    {
        while (isPlayerInside)
        {
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
}
