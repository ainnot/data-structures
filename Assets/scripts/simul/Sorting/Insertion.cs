using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Insertion : Sorting
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

        for (int i = 1; i < list.Count; i++)
        {
            int current = list[i];
            int j = i - 1;

            while (j >= 0 && list[j] > current)
            {
                list[j + 1] = list[j];
                count++;
                j--;

                if (count > changedCount)
                {
                    // 누적 변경 횟수를 추가한다
                    changedCount++;

                    // 마지막으로 정렬이 진행된 리스트를 저장한다
                    lastList = list;

                    // 마지막으로 변경된 인덱스를 저장한다
                    changedIndex = (j + 1, j + 2);
                    PrintList(list);

                    // 정렬이 완료 되었는지 안되었는지 확인하다
                    isSorted = CheckList(list);
                    return;
                }
            }
            list[j + 1] = current;
        }
    }
}
