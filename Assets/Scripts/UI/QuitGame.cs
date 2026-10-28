using UnityEngine;


public class QuitGame : MonoBehaviour
{
    //Callied by Quit Button's Click Event
   public void Quit()
    {
        Debug.Log("QuittingGame");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}