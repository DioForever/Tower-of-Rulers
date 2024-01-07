using UnityEngine;

public class SpellUser : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            FireballScript otherScriptInstance = gameObject.AddComponent<FireballScript>();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Ice3shooterScript otherScriptInstance2 = gameObject.AddComponent<Ice3shooterScript>();
        }
    }
}