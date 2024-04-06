using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        menuUI.SetActive(!menuUI.activeSelf);
    }
}