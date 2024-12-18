using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SkyBox : MonoBehaviour
{
    public Material skyBox1;
    public Material skyBox2;
    //public Material skyBox3;
    //public Material skyBox4;
    private string sceneName;
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
    }
    private void Update()
    {
        if (sceneName == "Menu")
{
            RenderSettings.skybox = skyBox1;
        }
        if (sceneName == "Run")
{
            RenderSettings.skybox = skyBox2;
        }
//        if (sceneName == “Scene3”)
//{
//            RenderSettings.skybox = skyBox3;
//        }
//        if (sceneName == “Scene4”)
//{
//            RenderSettings.skybox = skyBox4;
//        }
    }
}
