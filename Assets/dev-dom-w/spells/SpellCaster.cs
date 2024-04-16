using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    public GameObject fireballPrefab; 
    public GameObject fire3ShooterPrefab; 
    public GameObject fireAuraPrefab; 
    public GameObject fireBoomerangPrefab; 
    public GameObject fireLancePrefab; 
    public GameObject iceSpellPrefab; 
    public GameObject ice3ShooterPrefab; 
    public GameObject iceSlowPrefab; 
    public GameObject iceDestroyPrefab; 
    public GameObject iceFreezePrefab; 
    

    public void CastSpell(int spellType)
    {
        GameObject spellPrefab = null;

        
        switch (spellType)
        {
            case 1:
                spellPrefab = fireballPrefab;
                break;
            case 2:
                spellPrefab = fireLancePrefab;
                break;
            case 3:
                spellPrefab = fireBoomerangPrefab;
                break;
            case 4:
                spellPrefab = fire3ShooterPrefab;
                break;
            case 5:
                spellPrefab = fireAuraPrefab;
                break;
            case 6:
                spellPrefab = iceSpellPrefab;
                break;
            case 7:
                spellPrefab = ice3ShooterPrefab;
                break;
            case 8:
                spellPrefab = iceDestroyPrefab;
                break;
            case 9:
                spellPrefab = iceFreezePrefab;
                break;
            case 10:
                spellPrefab = iceSlowPrefab;
                break;
            default:
                Debug.LogError("Unknown spell type: " + spellType);
                return;
        }

        // Instantiate the selected spell prefab
        if (spellPrefab != null)
        {
            Instantiate(spellPrefab, transform.position, transform.rotation);
        }
        else
        {
            Debug.LogError("No prefab assigned for spell type: " + spellType);
        }
    }
}