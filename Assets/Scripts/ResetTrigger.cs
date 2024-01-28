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
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {

            int num = other.GetComponent<Duck>().playerNUm;
            Debug.Log(("OGGG " + num));
            HatManager.Instance.points[num]++;
            PlaySoundsRoundom(VictoryRoundsSounds);
            Debug.Log("Player 2: " + HatManager.Instance.points[1]);
            Debug.Log("Player 1: " + HatManager.Instance.points[2]);
            uiPoints.UpdatePoints();
            duck1.transform.position = spawnDuck1.position;
            duck1.transform.rotation = spawnDuck1.rotation;
            duck2.transform.position = spawnDuck2.position;
            duck2.transform.rotation = spawnDuck2.rotation;
            duck1.havePlayerFallingSounds = false;
            duck2.havePlayerFallingSounds = false;
            if (num == 1) {
                if (HatManager.Instance.points[num] == 3) {
                    duck2.BigVicoryAnim();
                }
                else {
                    duck2.PlayRandomVicotryAnimation();
                }

            }
            else {
                if (HatManager.Instance.points[num] == 3) {
                    duck1.BigVicoryAnim();
                }
                else {
                    duck1.PlayRandomVicotryAnimation();
                }

            }
        }
    }

    IEnumerator DelayTime()
    {
        scored = true;
        yield return new WaitForSeconds(delay);
        scored = false;
    }

    private void PlaySoundsRoundom(List<AudioClip> sounds)
    {
        audioSource.PlayOneShot(sounds[Random.Range(0, sounds.Count)]);
    }
}
