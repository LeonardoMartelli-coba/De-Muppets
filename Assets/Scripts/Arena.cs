using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Arena : MonoBehaviour
{
    public List<GameObject> arenas;
    public int sceneNumber;
    private void Awake()
    {
        arenas[ Random.Range(0, arenas.Count)].SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) {
            SceneManager.LoadScene(sceneNumber);
        }
    }
}
