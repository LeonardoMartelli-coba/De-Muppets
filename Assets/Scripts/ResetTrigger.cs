using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTrigger : MonoBehaviour
{
    public Transform spawnDuck1;
    public Transform spawnDuck2;
    public Duck duck1;
    public Duck duck2;
    public UiPoints uiPoints;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            Debug.Log(("OGGG"));
            int num = other.GetComponent<Duck>().playerNUm;
            HatManager.Instance.points[num]++;
            Debug.Log("Player 1: " + HatManager.Instance.points[1]);
            Debug.Log("Player 2: " + HatManager.Instance.points[2]);
            uiPoints.UpdatePoints();
            duck1.transform.position = spawnDuck1.position;
            duck1.transform.rotation = spawnDuck1.rotation;
            duck2.transform.position = spawnDuck2.position;
            duck2.transform.rotation = spawnDuck2.rotation;

        }
    }
}
