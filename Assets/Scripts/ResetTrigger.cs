using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ResetTrigger : MonoBehaviour
{
    public Transform spawnDuck1;
    public Transform spawnDuck2;
    public Duck duck1;
    public Duck duck2;
    public UiPoints uiPoints;
    public bool scored;
    public float delay;
    public AudioSource audioSource;
    public List<AudioClip> VictoryRoundsSounds;
    public bool haveWin;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {

            int num = other.GetComponent<Duck>().playerNUm;
            Debug.Log(("OGGG " + num));

            HatManager.Instance.points[num]++;
            uiPoints.UpdatePoints();
            if (!scored) {
                StartCoroutine(DelayTime());
            }
            if (num == 1) {
                duck1.StopFalling();
            }
            else {
                duck2.StopFalling();
            }
            if (num == 1) {
                if (HatManager.Instance.points[num] == 3) {
                    duck2.BigVicoryAnim();
                    haveWin = true;
                    duck1.end = true;
                    duck2.haveWin = true;
                    duck2.end = true;
                }
                else {
                    duck2.PlayRandomVicotryAnimation();
                    StartCoroutine(ResetCoroutine(num));
                }
            }
            else {
                if (HatManager.Instance.points[num] == 3) {
                    duck1.BigVicoryAnim();
                    haveWin = true;
                    duck1.end = true;
                    duck1.haveWin = true;
                    duck2.end = true;
                }
                else {
                    duck1.PlayRandomVicotryAnimation();
                    StartCoroutine(ResetCoroutine(num));
                }
            }
        }
    }

    IEnumerator DelayTime()
    {
        scored = true;
        Debug.Log("MARLENA TORNA A CASA ");
        yield return new WaitForSeconds(delay);
        scored = false;
    }

    IEnumerator ResetCoroutine(int n)
    {


        PlaySoundsRoundom(VictoryRoundsSounds);


        yield return new WaitForSeconds(0.5f);
        duck1.transform.position = spawnDuck1.position;
        duck1.transform.rotation = spawnDuck1.rotation;
        duck2.transform.position = spawnDuck2.position;
        duck2.transform.rotation = spawnDuck2.rotation;
        duck1.havePlayerFallingSounds = false;
        duck2.havePlayerFallingSounds = false;


    }


    private void PlaySoundsRoundom(List<AudioClip> sounds)
    {
        audioSource.PlayOneShot(sounds[Random.Range(0, sounds.Count)]);
    }
}
