using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager i;

    public Text text_kills;
    public Vector2 text_kills_bpos;


    public Image[] item_buttons;

    public RectTransform defeatRect;
    public RectTransform pauseRect;

    // Start is called before the first frame update
    void Awake()
    {
        i = this;

        text_kills_bpos = text_kills.rectTransform.anchoredPosition;

        OpenDefeatPanel(false);
        OpenPausePanel(false);
    }

    

    // Update is called once per frame
    void Update()
    {
       
    }

    public void RefreshItem(int id)
    {
        for (int i = 0; i < item_buttons.Length; i++)
        {
            if (i == id)
                item_buttons[i].enabled = true;
            else
                item_buttons[i].enabled = false;
        }
    }


    public void AddKills()
    {
        StopCoroutine(IEAddKills());
        StartCoroutine(IEAddKills());
    }

    public IEnumerator IEAddKills()
    {
        float t = 0;

        text_kills.text = "" + PlayerManager.i.kills;

        while (t < 1)
        {
            t += Time.deltaTime * 4;

            text_kills.rectTransform.anchoredPosition = text_kills_bpos + new Vector2(0, Mathf.Cos((t-0.5f)*Mathf.PI) * 5);

            yield return null;
        }
    }

    public void OpenDefeatPanel(bool v)
    {
        defeatRect.gameObject.SetActive(v);
    }

    public void OpenPausePanel(bool v)
    {
        pauseRect.gameObject.SetActive(v);
    }
}
