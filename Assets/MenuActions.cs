using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuActions : MonoBehaviour
{
    public void IniciaJogo() => SceneManager.LoadScene("SampleScene");

    public void Menu() => SceneManager.LoadScene("MainMenu");
}
