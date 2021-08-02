using System;
using System.Collections.Generic;
using com.intellify.ARSceneCotroller;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// Listens for touch events and performs an AR raycast from the screen touch point.
/// AR raycasts will only hit detected trackables like feature points and planes.
///
/// If a raycast hits a trackable, the <see cref="placedPrefab"/> is instantiated
/// and moved to the hit position.
/// </summary>
///

[Serializable]
public class PaintingInfo {
    public Texture2D texture;
    public float widht;
    public float height;
}

[RequireComponent(typeof(ARRaycastManager))]
public class PlacementController : MonoBehaviour
{
    enum PlacementState
    {
        ItemNotAvailable,
        NotPlaced,
        PlacementInProgress,
        Placed
    };

    public static PlacementController Instance = null;
    public Transform m_ItemTransform;


    private FocusSquare m_FocusSquare;
    private float m_minimumThreshold = 0.001f;
    private Vector2 touchPosition = default;
    private float maxDisatnceOnSelection = 25.0f;
    private ARRaycastManager aRRaycastManager;
    private ARPointCloudManager pointCloudManager;
    private bool onTouchHold = false;

    GameObject obj;

    public bool isFirstPlaced = false;

    public CanvasGroup ScrollActivate;


    private PlacementState m_objectPlacementState = PlacementState.NotPlaced;

    public static event Action onPlacedObject;


    public string currentSelectedModel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        aRRaycastManager = GetComponent<ARRaycastManager>();
        pointCloudManager = GetComponent<ARPointCloudManager>();
        m_FocusSquare = GetComponent<FocusSquare>();

       
    }

    private void Start()
    {
        currentSelectedModel = ARSceneCotroller.Instance.modelName[0];
    }

    public void UpdateItemState(string currentSelected)
    {

        // this.m_Painting = m_Painting;
        currentSelectedModel = currentSelected;
       // m_objectPlacementState = PlacementState.NotPlaced;
    }

    public void ResetPlacement()
    {
      
        if (obj != null && !m_objectPlacementState.Equals(PlacementState.Placed))
            Destroy(obj);

        m_FocusSquare.isObjectInProgress = true;
        m_objectPlacementState = PlacementState.ItemNotAvailable;
        StartPlaneTracking();
        StartPointCloud();
    }


    void Update()
    {

        if (m_FocusSquare.SquareState == FocusState.Found)
        {

            switch (m_objectPlacementState)
            {
                case PlacementState.ItemNotAvailable:

                    break;


                case PlacementState.NotPlaced:
                    obj = Instantiate(Resources.Load(currentSelectedModel, typeof(GameObject))) as GameObject;
                    m_ItemTransform = obj.transform;
                    m_ItemTransform.GetChild(1).transform.gameObject.SetActive(true);
                    m_ItemTransform.SetPositionAndRotation(m_FocusSquare.placementPose.position, m_FocusSquare.placementPose.rotation);
                    m_FocusSquare.isObjectPlaced = true;
                    m_FocusSquare.placementIndicator.SetActive(false);

                    m_objectPlacementState = PlacementState.PlacementInProgress;
                    Debug.Log("PlacementInProgress");
                    break;

                case PlacementState.PlacementInProgress:

                    if (Utils.WasTouchStartDetected() && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                    {
                        m_ItemTransform.GetChild(0).GetComponent<Animator>().SetTrigger("Play");
                        m_ItemTransform.GetChild(1).gameObject.SetActive(false);
                        m_objectPlacementState = PlacementState.Placed;
                        isFirstPlaced = true;
                        ScrollActivate.alpha = 1;
                        ScrollActivate.interactable = true;
                        ScrollActivate.blocksRaycasts = true;
                        m_FocusSquare.PlacementFinished();

                        Debug.Log("I am here before call events");

                        StopPlaneTracking();
                        Debug.Log("1");
                        onPlacedObject?.Invoke();
                        Debug.Log("2");
                        StopPointCloud();
                        Debug.Log("3");
                    }
                    else
                    {
                        m_ItemTransform.SetPositionAndRotation(m_FocusSquare.placementPose.position, m_FocusSquare.placementPose.rotation);
                    }

                    break;


                case PlacementState.Placed:

                    /*
                    if (Input.touchCount > 0 && Input.touchCount < 2)
                    {

                        Touch touch = Input.GetTouch(0);

                        if (touch.phase == TouchPhase.Began)
                        {

                            Ray ray = Camera.main.ScreenPointToRay(touch.position);

                            RaycastHit hitObject;

                            if (Physics.Raycast(ray, out hitObject, maxDisatnceOnSelection))
                            {
                                if (hitObject.transform.tag.Equals("Model"))
                                {
                                    onTouchHold = true;
                                    touchPosition = touch.position;
                                    Debug.Log("Touch on object find");
                                }
                            }
                        }


                        if (touch.phase == TouchPhase.Moved)
                        {
                            touchPosition = touch.position;
                        }

                        if (touch.phase == TouchPhase.Ended)
                        {
                            if (onTouchHold)
                                onTouchHold = false;
                        }
                    }
                    else
                    {
                        onTouchHold = false;
                    }

                    if (aRRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
                    {
                        Pose hitPose = hits[0].pose;
                        if (onTouchHold)
                        {
                            Debug.Log("Item drag On plane : ");
                            m_ItemTransform.position = hitPose.position;

                        }
                    }
                    */
                    break;



                /*
                                case PlacementState.NotPlaced:
                                    if (Utils.WasTouchStartDetected())
                                    {
                                        if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                                        {
                                            // On the first tap, instantiate the furniture at the cursor location
                                            // and move the furniture along the ground (with the cursor) till the second tap is detected.

                                            m_ItemTransform = GameObject.Instantiate(m_ItemPrefab, m_FocusSquare.placementPose.position, Quaternion.identity).transform;

                                            // Once the ground plane is established, move the cursor manager to raycast mode. (The World Hit Test mode is too noisy in low light conditions)
                                            // NOTE: The furniture prefab has a very large shadow plane that has raycast enabled on it. (The furniture doesn't participate in raycasts)
                                            // m_cursorManager.SetMode(false);
                                            Debug.Log(m_ItemTransform + " Checking object !! :" + m_ItemTransform.name);


                                            m_ItemTransform.LookAt(Camera.main.transform.position);

                                            m_ItemTransform.eulerAngles = new Vector3(0, m_ItemTransform.eulerAngles.y, 0);

                                            //m_objectPlacementState = PlacementState.PlacementInProgress;
                                            m_FocusSquare.isObjectPlaced = true;
                                            m_FocusSquare.placementIndicator.SetActive(false);
                                            Debug.Log("<color=green>Object Placed Once </color>");

                                            m_ItemTransform.SetPositionAndRotation(m_FocusSquare.placementPose.position, m_FocusSquare.placementPose.rotation);
                                            m_FocusSquare.PlacementFinished();
                                            m_objectPlacementState = PlacementState.Placed;
                                            StopPlaneTracking();
                                        }
                                    }

                                    break;

                                */
                /*

                                case PlacementState.PlacementInProgress:

                                    if (Utils.WasTouchStartDetected() && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                                    {
                                        Debug.Log("Second touch Detect , Object Placed and cant move");
                                        m_FocusSquare.PlacementFinished();
                                        m_objectPlacementState = PlacementState.Placed;
                                        StopPlaneTracking();
                                    }
                                    else
                                    {
                                            Debug.Log("In Progress , object can move with pointer ");
                                            // Move the furniture along with the cursor without modifying the Y position.
                                            m_ItemTransform.SetPositionAndRotation(m_FocusSquare.placementPose.position, m_FocusSquare.placementPose.rotation);

                                    }
                                    break;

                                case PlacementState.Placed:

                                    if (Input.touchCount == 2)
                                    {
                                        Debug.Log("On Double finger , Placement can be reset !! TRicked.");
                                        // On a 2 finger tap gesture, move back to placement mode.

                                        m_FocusSquare.isObjectInProgress = true;
                                        m_objectPlacementState = PlacementState.PlacementInProgress;
                                    }
                                    else
                                    {
                                        // After furniture is placed, rotate furniture on a single finger swipe.
                                        if (Utils.WasTouchStartDetected() && Input.GetTouch(0).phase == TouchPhase.Moved)
                                        {
                                            Debug.Log("Can Rotate object !!");
                                            var delta = Input.GetTouch(0).deltaPosition;
                                            m_ItemTransform.Rotate(new Vector3(0, 1, 0), -90.0f * (delta.x / Screen.currentResolution.width));
                                        }

                                    }
                                    break;*/

                default:
                    break;
            }

        }



    }



    public void StopPlaneTracking()
    {
        m_FocusSquare.TogglePlaneDetection(false);
    }

    public void StartPlaneTracking()
    {
        m_FocusSquare.TogglePlaneDetection(true);
    }


    public void ResetAll()
    {
        m_FocusSquare.isObjectInProgress = true;
        m_FocusSquare.isObjectPlaced = false;
        m_objectPlacementState = PlacementState.NotPlaced;
        StartPlaneTracking();
    }




    public void StopPointCloud()
    {

        var points = pointCloudManager.trackables;
        foreach (var pts in points)
        {
            pts.gameObject.SetActive(false);
        }
        pointCloudManager.enabled = false;
    }

    public void StartPointCloud()
    {

        var points = pointCloudManager.trackables;
        foreach (var pts in points)
        {
            pts.gameObject.SetActive(true);
        }
        pointCloudManager.enabled = true;
    }

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
}
