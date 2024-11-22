using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorting
{
    // 리스트를 북제할 배열 
    protected List<int> copyList;
    // 마지막 리스트를 저장하여 정렬을 교환할 때 이 리스트를 상속한다
    protected List<int> lastList;
    // 마지막으로 변경된 인덱스를 저장한다
    protected (int, int) changedIndex;
    // 정렬이 되었는지 여부
    protected bool isSorted = false;
    // 교환이 일어난 횟수 
    protected int changedCount = 0;

    public int ChangeCount
    {
        get
        {
            return changedCount;
        }
    }
    public (int, int) ChangedIndex
    {
        get
        {
            return changedIndex;
        }
    }

    public bool IsSorted
    {
        get
        {
            return isSorted;
        }
    }

    public List<int> LastList
    {
        get
        {
            return lastList;
        }
    }

    public List<int> CopyList
    {
        get
        {
            if (copyList == null)
            {
                return null;
            }
            return copyList;
        }
        set
        {
            copyList = value;
            lastList = copyList;
            isSorted = CheckList(copyList);
            changedIndex = (-1, -1);
        }
    }

    // 교환이 발생할때까지 정렬을 수행
    public virtual void SortUntilChange()
    {
    }

    // 교환
    protected void Swap(List<int> list, int a_idx, int b_idx)
    {
        if (list.Count <= 0)
        {
            Debug.Log("list is empty");
            return;
        }

        int temp = list[a_idx];
        list[a_idx] = list[b_idx];
        list[b_idx] = temp;
    }

    // 정렬이 완료되었는지 확인
    public bool CheckList(List<int> list)
    {
        if (list == null || list.Count <= 0)
        {
            Debug.Log("list is empty");
            return false;
        }

        bool isSorted = true;

        for (int i = 0; i < list.Count-1; i++)
        {
            if (list[i] > list[i + 1])
            {
                isSorted = false;
                break;
            }
        }

        return isSorted;
    }

    // 리스트 출력 
    protected void PrintList(List<int> list)
    {
        if (list.Count < 0)
        {
            Debug.LogWarning("list is empty");
            return;
        }

        string stringList = "";

        foreach (int num in list)
        {
            stringList += num.ToString() + ", ";
        }

        Debug.Log(stringList);
    }
}
