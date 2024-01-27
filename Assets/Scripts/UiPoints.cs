using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiPoints : MonoBehaviour
{
    public List<Image> p1;
    public List<Image> p2;
    public GameObject panel;
    public TMP_Text text;
    private bool haveWin;

    public void UpdatePoints()
    {
        if(haveWin){return;}
        if (HatManager.Instance.points[1] != 0)
        {
            p1[HatManager.Instance.points[1]-1].color= Color.white;
        }
        if (HatManager.Instance.points[2] != 0)
        {
            p2[HatManager.Instance.points[2]-1].color= Color.white;
        }
        if (HatManager.Instance.points[1] == 3)
        {
            text.text = "WIN P1";
            haveWin = true;
            StartCoroutine(DisplayWin());
            return;
        }
        if (HatManager.Instance.points[2] == 3)
        {
            text.text = "WIN P2";
            haveWin = true;
            StartCoroutine(DisplayWin());
            return;
        }
    }

    IEnumerator DisplayWin()
    {
        yield return new WaitForSeconds(0.5f);
        panel.SetActive(true);
    }
}
