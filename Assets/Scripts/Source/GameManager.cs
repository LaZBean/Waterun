using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager i;

    public bool isPlaying = false;
    public float gameTime;



    void Awake()
    {
        i = this;
    }


    void Start()
    {
        ShowIntro();
        GameStart();
    }


    void Update()
    {
        if (isPlaying)
            gameTime += Time.deltaTime;
    }


    public void Reload()
    {
        SceneManager.LoadScene(0);
    }

    public void Pause(bool v)
    {
        isPlaying = !v;

        UIManager.i.OpenPausePanel(v);

        Time.timeScale = (v) ? 0 : 1f;
    }

    public void ShowIntro()
    {

    }

    public void GameStart()
    {
        gameTime = 0;
        isPlaying = true;
        Time.timeScale = 1f;
    }

    public void GameFinish()
    {
        

        LevelManager.i.DestroyAll();

        UIManager.i.OpenDefeatPanel(true);
    }
}
