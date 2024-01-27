using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatManager : MonoBehaviour
{
    public static HatManager Instance;
    public List<GameObject> hats;
    public Dictionary<int, int> points = new Dictionary<int, int>();
    private void Awake()
    {
        points.Add(1,0);
        points.Add(2,0);
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
