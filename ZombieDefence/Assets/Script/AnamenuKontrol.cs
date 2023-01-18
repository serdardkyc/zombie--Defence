using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class AnamenuKontrol : MonoBehaviour
{
    
    public GameObject LoadingPanel;
    public GameObject CikisPanel;
    public Slider loadingslider;

    public void OyunaBasla()
    {
        StartCoroutine(sahneyuklemeloading());
    }

    IEnumerator sahneyuklemeloading()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);

        LoadingPanel.SetActive(true);


        while (!operation.isDone)
        {
            float ilerleme = Mathf.Clamp01(operation.progress / .9f);

            loadingslider.value = ilerleme;

            yield return null;
        }

    }

    public void oyundancik()
    {


        CikisPanel.SetActive(true);


    }

   public void Tercih (string butondegeri)
    {

        if (butondegeri == "evet")
            Application.Quit();
        else
            CikisPanel.SetActive(false);

    }


}
