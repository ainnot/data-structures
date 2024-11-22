using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quick : Sorting
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

        // 정렬 범위를 저장할 스택
        Stack<(int, int)> ranges = new Stack<(int, int)>();
        ranges.Push((0, list.Count - 1)); // 초기 범위 추가

        while (ranges.Count > 0)
        {
            var (low, high) = ranges.Pop();

            if (low < high)
            {
                // 피벗 선택 및 분할
                int pivot = list[high];
                int i = low - 1;

                for (int j = low; j < high; j++)
                {
                    if (list[j] < pivot)
                    {
                        i++;

                        if (i != j)
                        {
                            Swap(list, i, j);
                            count++;

                            // 교환 횟수가 변경 한도 초과 시 상태 저장 후 종료
                            if (count > changedCount)
                            {
                                changedCount++;
                                lastList = list;
                                changedIndex = (i, j);
                                PrintList(list);
                                isSorted = CheckList(list);
                                return;
                            }
                        }
                    }
                }

                if (i + 1 != high)
                {
                    // 피벗을 제자리에 놓음
                    Swap(list, i + 1, high);
                    count++;

                    // 교환 횟수가 변경 한도 초과 시 상태 저장 후 종료
                    if (count > changedCount)
                    {
                        changedCount++;
                        lastList = list;
                        changedIndex = (i + 1, high);
                        PrintList(list);
                        isSorted = CheckList(list);
                        return;
                    }
                }

                // 피벗 인덱스
                int pivotIndex = i + 1;

                // 나뉜 두 부분 리스트를 스택에 추가
                ranges.Push((low, pivotIndex - 1));
                ranges.Push((pivotIndex + 1, high));
            }
        }
    }
}
