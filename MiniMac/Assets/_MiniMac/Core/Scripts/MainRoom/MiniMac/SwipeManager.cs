using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeManager : MonoBehaviour
{
    [Header("Swipe Settings")]
    [SerializeField] private float minDistanceToPat;
    private float currentDistance = 0f;
    private Vector3 lastTouchPosition;

    [Header("Raycast Settings")]
    private RaycastHit hit;
    private Ray ray;

    [Header("References")]
    [SerializeField] private PetController petController;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            lastTouchPosition = Input.mousePosition;
            currentDistance = 0f;
        }
        else if (Input.GetMouseButton(0))
        {
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("MiniMac"))
                {
                    float distanceMoved = Vector3.Distance(Input.mousePosition, lastTouchPosition);

                    currentDistance += distanceMoved;

                    if (currentDistance >= minDistanceToPat)
                    {
                        petController.ReactToPat();
                        currentDistance = 0f;
                    }
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            currentDistance = 0f;
        }
    }
}
