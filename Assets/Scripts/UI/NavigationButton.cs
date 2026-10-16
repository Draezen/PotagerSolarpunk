using UnityEngine;

public class NavigationButton : MonoBehaviour
{
    [SerializeField] private SceneList scene;

    public void ChangeSceneOnCLick() => SceneLoader.instance.LoadByScene(scene);

    public void QuitOnClick() => Application.Quit();
}
