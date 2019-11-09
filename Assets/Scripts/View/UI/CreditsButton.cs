using UnityEngine;
using UnityEngine.EventSystems;

public class CreditsButton : MonoBehaviour
{
    public void OnClickCreditsButton()
    {
        Application.OpenURL("https://github.com/next-please/I-was-tasked/blob/master/Credits.md");
    }
}
