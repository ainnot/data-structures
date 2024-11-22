using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : Sorting
{
    public override void SortUntilChange()
    {
        base.SortUntilChange();

        // 교환이 일어난 휫수 
        int count = 0;

        List<int> list = new List<int>(copyList);

        // 리스트가 이미 정렬이 되었다면 이 함수를 실행하지 않는다.
        if (isSorted)
        {
            Debug.Log("Sorted");
            return;
        }

        for (int i = 0; i < list.Count; i++)
        {
            for (int j = 0; j < list.Count-1; j++)
            {
                if (list[j] > list[j+1])
                {
                    Swap(list, j, j+1);
                    count++;
                    if (count > changedCount)
                    {
                        // 누적 변경 횟수를 추가한다
                        changedCount++;

                        // 마지막으로 정렬이 진행된 리스트를 저장한다
                        lastList = list;

                        if (lastList == null)
                        {
                            Debug.Log("lastlist is null");
                        }

                        // 미지막으로 변경된 인덱스를 저장한다
                        changedIndex = (list[j], list[j + 1]);
                        PrintList(list);

                        // 정렬이 완료 되었는지 안되었는지 확인하다
                        isSorted = CheckList(list);
                        return;
                    }
                }
            }
        }


        return;
    }

}
