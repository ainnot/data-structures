using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulManager : MonoBehaviour
{
    // 리스트의 길이 
    public int inputLength = 9;

    // 리스트 
    private List<int> list = new List<int>();

    Sorting sorting;

    // 정렬의 개수 
    int sortingCount = 5;
    // 현재 선탣된 정렬 인덱스 
    int sortingSelection = 0;


// UI
    // 정렬 이름 
    List<string> option_titles = new List<string>() { "Bubble", "Insertion", "Selection", "Shell", "Quick" };

    public void Awake()
    {
        // 리스트 생성 
        RegenerateList(inputLength);
    }

    public void Start()
    {
        SetSorting();
    }

    public void Update()
    {
        // R를 클랙하면 새로운 리스트를 생성한다.
        if (Input.GetKeyDown(KeyCode.R))
        {
            RegenerateList(inputLength);
        }

        // S를 클랙하면 정렬을 수행한다
        if (Input.GetKeyDown(KeyCode.S))
        {
            SortOnce();
        }

        // 정렬 변경 
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("Changed");
            sortingSelection = sortingSelection > 0 ? sortingSelection-1 : sortingCount - 1;
            ChangeSorting();
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("Changed");
            sortingSelection = sortingSelection < sortingCount-1 ? sortingSelection + 1 : 0;
            ChangeSorting();
        }
    }

    private void SortOnce()
    {
        if (sorting == null || sorting.CopyList.Count <= 0)
        {
            Debug.LogWarning("sorting is null");
            return;
        }

        // 정렬이 완료되지 않았다면 정렬을 수행한다.
        if (!sorting.IsSorted)
        {
            sorting.SortUntilChange();
            Debug.Log(sorting.ChangedIndex);
            Debug.Log(sorting.ChangeCount);
        }
    }

    // 난수 리스트 생성 
    private void RegenerateList(int length)
    {
        list.Clear();

        for (int i = 0; i < length; i++)
        {
            int randNum = Random.Range(0, 101);
            list.Add(randNum);
        }

        SetSorting();

        Debug.Log("Regnerate");
        PrintList();
    }

    // 리스트 출력 
    private void PrintList()
    {
        if (list.Count < 0)
        {
            Debug.LogWarning("list is empty");
            return;
        }

        string stringList = "";

        foreach(int num in list)
        {
            stringList += num.ToString() + ", ";
        }

        Debug.Log(stringList);
    }

    public void ChangeSorting()
    {
        // 마지막 정렬 상태로 리스트를 갱신한다.
        list = sorting.LastList;
        SetSorting();
    }

    private void SetSorting()
    {
        switch(sortingSelection)
        {
            case 0:
                sorting = new Bubble();
                break;
            case 1:
                sorting = new Insertion();
                break;
            case 2:
                sorting = new Selection();
                break;
            case 3:
                sorting = new Shell();
                break;
            case 4:
                sorting = new Quick();
                break;
            default:
                sorting = null;
                return;
        }
        sorting.CopyList = list;
        Debug.Log(option_titles[sortingSelection]);
    }
}
