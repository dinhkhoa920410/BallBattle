using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARController : MonoBehaviour
{
    public GameObject objToPlace;
    public GameObject placementIndicator;
    public Camera cameraNonAR;
    public Camera cameraAR;
    public ARPlaneManager planeManager;
    public GameObject planeVisualizerButton;
    public GameplayUIController gameplayUIController;
    private ARSessionOrigin arOrigin;
    private ARRaycastManager arRaycastManager;
    private Pose placementPose;
    private bool placementPoseIsValid;
    private bool isGameAlreadyPlaced;
    private bool isAROn;
    private bool isPlaneVisualizerOn;
    private Pose originalPose;
    LayerMask layerUI;
    Ray ray;

    // Start is called before the first frame update
    void Start()
    {
        //arOrigin = GetComponent<ARSessionOrigin>();
        arRaycastManager = GetComponent<ARRaycastManager>();
        placementPoseIsValid = false;
        isGameAlreadyPlaced = false;
        isAROn = false;
        isPlaneVisualizerOn = true;
        //layerUI = 1<<LayerMask.NameToLayer("UI");
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
        
        //RaycastHit hit;
        if(Input.GetMouseButtonDown(0)){
            //if(!Physics.Raycast(ray, out hit, 1000, layerUI)){
                if(placementPoseIsValid && !isGameAlreadyPlaced){
                    placeObject();
                    isGameAlreadyPlaced = true;
                    placementIndicator.transform.GetChild(0).gameObject.SetActive(false);
                }
            //}
        }
        //if(Input.GetMouseButtonDown(0) && placementPoseIsValid && !isGameAlreadyPlaced){
        //}
    }

    private void placeObject()
    {
        //Instantiate(objToPlace, placementPose.position, placementPose.rotation);
        objToPlace.SetActive(true);
        originalPose = placementPose;
        objToPlace.transform.position = placementPose.position;
        objToPlace.transform.rotation = placementPose.rotation;
        GameMaster.GM.resumeGame();
    }

    private void UpdatePlacementIndicator()
    {
        if(placementPoseIsValid){
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position,placementPose.rotation);
        }
        else
            placementIndicator.SetActive(false);
    }

    private void UpdatePlacementPose()
    {
        if(isAROn){
            var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f,0.5f));
            var hits = new List<ARRaycastHit>();
            arRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);
            
            placementPoseIsValid = hits.Count>0;

            if(placementPoseIsValid)
            {
                placementPose = hits[0].pose;

                Vector3 cameraForward = Camera.current.transform.forward;
                Vector3 cameraBearing = new Vector3(cameraForward.x, 0f, cameraForward.z).normalized;
                placementPose.rotation = Quaternion.LookRotation(cameraBearing);
            }
        }
    }

    public void switchCamera(){
        isAROn = !isAROn;
        cameraNonAR.gameObject.SetActive(!isAROn);
        cameraAR.gameObject.SetActive(isAROn);
        placementIndicator.SetActive(isAROn);
        planeVisualizerButton.SetActive(isAROn);
        planeManager.enabled = isPlaneVisualizerOn;
        //Pause and invisible playground
        if(!isGameAlreadyPlaced){
            GameMaster.GM.pauseGame();
            objToPlace.SetActive(false);
        }else
            gameplayUIController.onClickBtnIngameMenu();
            
        //Reposition playground when switch between 2 cameras
        if(!isAROn){
            objToPlace.transform.position = Vector3.zero;
            objToPlace.transform.rotation = Quaternion.identity;
        }else if(isGameAlreadyPlaced){
            objToPlace.transform.position = originalPose.position;
            objToPlace.transform.rotation = originalPose.rotation;
        }
    }

    public void togglePlaneVisualizer(){
        isPlaneVisualizerOn = !isPlaneVisualizerOn;
        foreach (var plane in planeManager.trackables)
            plane.gameObject.SetActive(isPlaneVisualizerOn);
    }
}
