using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : Sorting
{
    public override void SortUntilChange()
    {
        base.SortUntilChange();

        // 교환이 일어난 횟수
        int count = 0;

        List<int> list = new List<int>(copyList);

        // 리스트가 이미 정렬이 되었다면 이 함수를 실행하지 않는다.
        if (isSorted)
        {
            Debug.Log("Sorted");
            return;
        }

        for (int gap = list.Count / 2; gap > 0; gap /= 2)
        {
            for (int i = gap; i < list.Count; i++)
            {
                int current = list[i];
                int j = i;

                while (j >= gap && list[j - gap] > current)
                {
                    list[j] = list[j - gap];
                    count++;
                    j -= gap;

                    if (count > changedCount)
                    {
                        changedCount++;
                        lastList = list;
                        changedIndex = (j, j + gap);
                        PrintList(list);
                        isSorted = CheckList(list);
                        return;
                    }
                }
                list[j] = current;
            }
        }
    }
}
