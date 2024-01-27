using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatManager : MonoBehaviour
{
    public static HatManager Instance;
    public List<GameObject> hats;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public GameObject TakeHat()
    {
        GameObject hatToReturn = hats[Random.Range(0, hats.Count)];

        hats.Remove(hatToReturn);
        return hatToReturn;
    }

}
