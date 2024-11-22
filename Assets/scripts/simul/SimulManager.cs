using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // 입력 가능 여부 
    public bool isInputable = true;

    // UI
    //ref
    public GameObject blockPrefab;
    public GameObject canvas;
    public Text sortingText;


    // 정렬 이름 
    List<string> option_titles = new List<string>() { "Bubble", "Insertion", "Selection", "Shell", "Quick" };

    // 블록들을 저장할 리스트
    List<Block> blocks = new List<Block>();

    
    public void Awake()
    {
        // 리스트 생성 
        RegenList(inputLength);
        GenBlocks();
    }

    public void Start()
    {
        SetSorting();
    }

    public void Update()
    {
        if (isInputable)
        {
            // R를 클랙하면 새로운 리스트를 생성한다.
            if (Input.GetKeyDown(KeyCode.R))
            {
                RegenList(inputLength);
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
                sortingSelection = ((sortingSelection > 0) ? (sortingSelection - 1) : (sortingCount - 1));
                ChangeSorting();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Debug.Log("Changed");
                sortingSelection = ((sortingSelection < (sortingCount - 1)) ? (sortingSelection + 1) : 0);
                ChangeSorting();
            }
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

            // 블록 스왑 
            SwapBlock(sorting.ChangedIndex.Item1, sorting.ChangedIndex.Item2);
        }
    }

    // 난수 리스트 생성 
    private void RegenList(int length)
    {
        list.Clear();

        for (int i = 0; i < length; i++)
        {
            int randNum = Random.Range(0, 101);
            list.Add(randNum);
        }

        SetSorting();
        GenBlocks();

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
        sortingText.text = option_titles[sortingSelection];
    }

    private void GenBlocks()
    {
        if (!blockPrefab)
        {
            Debug.LogWarning("blockPrafab is null");
            return;
        }

        
        foreach (Block block in blocks)
        {
            Destroy(block.gameObject);
        }

        // block 리스리 삭제한다
        blocks.Clear();

        float x = -((list.Count-1) * 100);

        for (int i = 0; i < list.Count; i++)
        {
            Block newBlock = Instantiate(blockPrefab, canvas.transform).GetComponent<Block>();
            newBlock.Number = list[i];
            newBlock.RectTransform.anchoredPosition = new Vector2(x, 0);
            blocks.Add(newBlock);

            x += 200;
        }
    }

    private void SwapBlock(int idx1, int idx2)
    {
        if (blocks.Count <= 0)
        {
            Debug.Log("blocks is empty");
            return;
        }
        
        Block block1 = blocks[idx1];
        Block block2 = blocks[idx2];

        // 각 블록의 목적지를 교환한다 
        Vector2 destination_temp = block1.Destination;
        block1.Destination = block2.Destination;
        block2.Destination = destination_temp;

        //// 블록의 레퍼런스를 교환한다 
        Block block_temp = blocks[idx1];
        blocks[idx1] = blocks[idx2];
        blocks[idx2] = block_temp;

        // 경로를 설정한다 (왼쪽에 있던 숫자는 위로 오른쪽에 있던 숫자는 아래로 이동한다)
        block1.SetRoute(true);
        block2.SetRoute(false);

        // 블록 이동을 실해한다
        block1.Move();
        block2.Move();
    }
}
