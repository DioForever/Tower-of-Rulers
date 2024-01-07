using UnityEngine;

public class SkillUser : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DashScript otherScriptInstance = gameObject.AddComponent<DashScript>();
        }
    }
}