using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class MainCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private int defaultZoom;
    [SerializeField]
    private int maxZoom;

    private PixelPerfectCamera ppc;

    private int zoomLevel;
    private int maxWidth;
    private int maxHeight;
    private Vector3 touchStart;


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
            transform.position = player.transform.position + new Vector3(0, 0, -5);
        }
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.touchCount != 2 && Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Mathf.Abs(direction.x) < 0.04 && Mathf.Abs(direction.y) < 0.04) return; // weak touch
            Camera.main.transform.position += direction;
        }

        Zoom(Input.GetAxis("Mouse ScrollWheel"));
        Zoom(GetPinchInput());
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
            return difference * 0.001f;
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
        }
    }
}
