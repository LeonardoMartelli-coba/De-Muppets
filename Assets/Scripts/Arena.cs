using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Arena : MonoBehaviour
{
    public List<GameObject> arenas;
    public int sceneNumber;
    public List<GameObject> images;
    public List<Sprite> sprites;

    private void Awake()
    {
        int r = Random.Range(0, arenas.Count);
        arenas[r].SetActive(true);

        foreach (var image in images) {
            image.GetComponent<Image>().sprite = sprites[r];        
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) {
            SceneManager.LoadScene(sceneNumber);
        }
    }
}
