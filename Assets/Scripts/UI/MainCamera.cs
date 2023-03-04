using UnityEngine;
using UnityEngine.U2D;

public class MainCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private UI ui;
    [SerializeField]
    private int defaultZoom;
    [SerializeField]
    private int maxZoom;

    private PixelPerfectCamera ppc;

    private int zoomLevel;
    private int maxWidth;
    private int maxHeight;
    private Vector3 touchStart;
    private Vector3 invalidTouchStartVector = new Vector3(-1, -1, -1); // should be used as the mark of invalid touchStart

    // Player following
    private Vector3 offset = new Vector3(0, 0, -5);
    private float smoothTime = 0.18f;
    private Vector3 velocity = Vector3.zero;
    private float lastMoveTime = -1;
    // How many second camera continue to follow the player after end of movement
    private float additionalFollowTime = 0.35f;


    void Start()
    {
        ppc = GetComponent<PixelPerfectCamera>();
        maxWidth = ppc.refResolutionX * defaultZoom;
        maxHeight = ppc.refResolutionY * defaultZoom;
        zoomLevel = defaultZoom;
    }


    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerUnit>().GetState() == Unit.State.IsMakingTurn)
        {
            lastMoveTime = Time.time;
            SmoothCenterOnPlayer();
        } else if(Time.time - lastMoveTime < additionalFollowTime)
        {
            SmoothCenterOnPlayer();
        }


        if (Input.GetMouseButtonDown(0))
        {
            if (!ui.IsMouseOverUI())
            {
                touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            else
            {
                touchStart = invalidTouchStartVector;
            }
        }
        if (Input.touchCount != 2 && Input.GetMouseButton(0) && !ui.IsUIBlockingActions() && !touchStart.Equals(invalidTouchStartVector))
        {
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Mathf.Abs(direction.x) < 0.04 && Mathf.Abs(direction.y) < 0.04) return; // weak touch
            Camera.main.transform.position += direction;
        }
        Zoom(Input.GetAxis("Mouse ScrollWheel"));
        Zoom(GetPinchInput());
    }

    public void SmoothCenterOnPlayer()
    {
        Vector3 targetPosition = player.transform.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    public void CenterOnPlayer()
    {
        Vector3 targetPosition = player.transform.position + offset;
        transform.position = targetPosition;
    }

    private float GetPinchInput()
    {
        if (Input.touchCount >= 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;
            return difference * 0.0025f;
        }
        else return 0f;
    }

    private void Zoom(float inputValue)
    {
        if (inputValue != 0)
        {
            zoomLevel += Mathf.RoundToInt(inputValue * 10);
            zoomLevel = Mathf.Clamp(zoomLevel, 1, maxZoom);
            ppc.refResolutionX = Mathf.FloorToInt(maxWidth / zoomLevel);
            ppc.refResolutionY = Mathf.FloorToInt(maxHeight / zoomLevel);
            // Use only even numbers
            if (ppc.refResolutionX % 2 != 0) ppc.refResolutionX++;
            if (ppc.refResolutionY % 2 != 0) ppc.refResolutionY++;
        }
    }
}
