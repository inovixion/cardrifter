using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private InGameUI igu;
    private GameObject[] runners;

    List<RankingSystem> sortArray = new List<RankingSystem>();

    public int pass;
    public bool finish;
    public bool failed;
    public bool start;

    public string firstPlace, secondPlace, thirdPlace;

    private void Awake()
    {
        instance = this;
        
        igu = FindObjectOfType<InGameUI>();
    }
    void Start()
    {
        StartCoroutine(enumerator());
    }
    IEnumerator enumerator()
    {
        yield return new WaitForSeconds(0.5f);
        runners = GameObject.FindGameObjectsWithTag("Runner");
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < runners.Length; i++)
        {
            sortArray.Add(runners[i].GetComponent<RankingSystem>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculatingRank();
    }

    void CalculatingRank()
    {
        sortArray = sortArray.OrderBy(x => x.counter).ToList();
        switch (sortArray.Count)
        {
            case 3:
                sortArray[0].rank = 3;
                sortArray[1].rank = 2;
                sortArray[2].rank = 1;

                igu.a = sortArray[2].name;
                igu.b = sortArray[1].name;
                igu.c = sortArray[0].name;
                break;
            case 2:
                sortArray[0].rank = 3;
                sortArray[1].rank = 2;
                sortArray[2].rank = 1;

                igu.a = sortArray[2].name;
                igu.b = sortArray[1].name;
                igu.c = sortArray[0].name;
                break;
            case 1:
                sortArray[0].rank = 1;
                sortArray[1].rank = 2;
                sortArray[2].rank = 3;

                igu.a = sortArray[2].name;
                igu.b = sortArray[1].name;
                igu.c = sortArray[0].name;
                break;
        }

    }
    }

