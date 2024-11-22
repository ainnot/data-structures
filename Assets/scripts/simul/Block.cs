using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    // ref
    Text text;
    private RectTransform rectTransform;
    private SimulManager simulManager;

    // UI
    Outline outline;

    // 목적지
    private Vector2 destination;
    public Vector2 Destination
    {
        get
        {
            return destination;
        }
        set
        {
            destination = value;
        }
    }

    public class Route
    {
        public Vector2 route;
        public bool isReached;

        public Route(Vector2 route, bool isReached)
        {
            this.route = route;
            this.isReached = isReached;
        }
    }

    // 경로
    private List<Route> routeList = new List<Route>();
    public List<Route> RouteList
    {
        get
        {
            if (routeList.Count <= 0)
            {
                return null;
            }
            return routeList;
        }
    }

    // 이동 속도 
    float moveSpeed = 0.03f;

    // 이동 여부 
    bool isMove = false;

    // 블록이 외미하는 값 
    private int number = -1;


    public RectTransform RectTransform
    {
        get
        {
            return rectTransform;
        }
    }

    public int Number
    {
        set
        {
            number = value;
            if (!text)
            {
                Debug.Log("text is null");
                return;
            }
            text.text = number.ToString();
        }
    }

    private void Awake()
    {
        simulManager = FindObjectOfType<SimulManager>().GetComponent<SimulManager>();

        rectTransform = GetComponent<RectTransform>();
        text = transform.GetChild(0).GetComponent<Text>();
        outline = GetComponent<Outline>();
    }

    // Start is called before the first frame update
    void Start()
    {
        destination = rectTransform.anchoredPosition;
        outline.effectColor = Color.red;
        outline.effectDistance = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            Move();
        }
        OnHighLight(isMove);
    }

    public void Move()
    {
        if (routeList == null || routeList.Count <= 0)
        {
            Debug.Log("routeList is empty");
            return;
        }

        isMove = true;
        simulManager.isInputable = false;

        bool isAllReached = true;

        int idx_notReachedPoint = 0;
        for (idx_notReachedPoint = 0; idx_notReachedPoint < routeList.Count; idx_notReachedPoint++)
        {
            if (!routeList[idx_notReachedPoint].isReached)
            {
                isAllReached = false;
                break;
            }
        }

        // 오브젝트가 지정한 목적지를 모두 도달했다면 이동을 멈춤 
        if (isAllReached)
        {
            isMove = false;
            simulManager.isInputable = true;
            return;
        }

        // 오브젝트 이동 
        Vector2 currentLocation = rectTransform.anchoredPosition;
        Vector2 targetLocation = routeList[idx_notReachedPoint].route;

        Vector2 newLocation = Vector2.Lerp(currentLocation, targetLocation, moveSpeed);

        rectTransform.anchoredPosition = newLocation;

        // 오브젝트가 목적지에 도달했다면 해당 경로는 완료로 표시 
        if (IsEqual(currentLocation, targetLocation))
        {
            routeList[idx_notReachedPoint].isReached = true;
            return;
        }
    }

    // 경로 설정 
    public void SetRoute(bool isUp)
    {
        float y = isUp ? 200 : -200;

        Vector2 currentLocation = rectTransform.anchoredPosition;

        routeList.Add((new Route(new Vector2(currentLocation.x, currentLocation.y + y), false)));
        routeList.Add((new Route(new Vector2(destination.x, currentLocation.y + y), false)));
        routeList.Add(new Route(destination, false));

    }

    public bool IsEqual(Vector2 v1, Vector2 v2, float diff = 0.5f)
    {
        return (v1 - v2).sqrMagnitude < diff;
    }

    public void OnHighLight(bool TurnOn)
    {
        if (TurnOn)
        {
            float Thickness = 10f;
            outline.effectDistance = new Vector2(Thickness, Thickness);
        }
        else
        {
            outline.effectDistance = new Vector2(0, 0);
        }
    }
}
