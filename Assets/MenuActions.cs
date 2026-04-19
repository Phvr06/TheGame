using UnityEngine;

public class MenuActions : MonoBehaviour
{
    public GameObject endGameMenu;
    public void IniciaJogo()
    {
        GameController.Init();
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void Menu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
