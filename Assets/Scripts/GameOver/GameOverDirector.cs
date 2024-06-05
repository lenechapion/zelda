using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverDirector : MonoBehaviour
{
    // 移動先のシーン名
    public string targetScene;

    public void OnButtonClicked()
    {
        SceneManager.LoadScene(targetScene);
    }
}