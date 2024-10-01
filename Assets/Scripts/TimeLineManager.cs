using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLineManager : MonoBehaviour
{

    public PlayableDirector[] directors;

    public GameObject Player;
    public Camera MainPlayerCam;
    public GameObject PlayerStats;
    public GameObject CrossHair;

    public EnemySpawnManager EnemySpawnManagerCS;

    

    public IEnumerator PlayTimeLine(int index)
    {
        if (index < 0 || index > directors.Length) yield break;

        directors[index].Play();

        // Things to disable while playing Cut Scene
        Player.SetActive(false);
        MainPlayerCam.gameObject.SetActive(false);
        PlayerStats.SetActive(false);
        CrossHair.SetActive(false);

        yield return new WaitForSeconds((float)directors[index].duration);

        EnemySpawnManagerCS.setHealth(100);

        Player.SetActive(true);
        MainPlayerCam.gameObject.SetActive(true);
        PlayerStats.SetActive(true);
        CrossHair.SetActive(true);


    }
}
