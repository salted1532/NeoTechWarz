using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private GameObject mouseIndicator;
    [SerializeField]
    private Grid grid;

    [SerializeField]
    private List<ObjectDatabaseSO> database;
    public int selectedOnbjectIndex = -1;

    [SerializeField]
    private GameObject gridVisualization;

    private GridData floorData, furnitureData;

    private List<GameObject> placedGameObject = new();

    [SerializeField]
    private PreviewSystem preview;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    public int WhatRace;

    public float BuildingTime = 1;

    private void Start()
    {
        StopPlacement();
        floorData = new();
        furnitureData = new();

    }
    public void StartPlacement(int ID)
    {
        StopPlacement();
        selectedOnbjectIndex = database[WhatRace].objectsData.FindIndex(data => data.ID == ID);
        if(selectedOnbjectIndex < 0)
        {
            Debug.LogError($"아이디 못찾음 {ID}");
            return;
        }
        gridVisualization.SetActive(true);
        preview.StartShowingPlacementPreview(database[WhatRace].objectsData[selectedOnbjectIndex].Prefab,
            database[WhatRace].objectsData[selectedOnbjectIndex].Size);
        mouseIndicator.SetActive(true);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }
    private void StopPlacement()
    {
        selectedOnbjectIndex = -1;
        gridVisualization.SetActive(false);
        mouseIndicator.SetActive(false);
        preview.StopShowingPreview();
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
    }

    private void PlaceStructure()
    {
        // Check if inputManager, grid, and UiButtonMapiing are null
        if (inputManager == null || grid == null || database == null)
        {
            Debug.LogError("One of the required components (inputManager, grid, or database) is null");
            return;
        }

        var uiButtonMapping = transform.parent.GetComponent<UiButtonMapiing>();

        if (uiButtonMapping == null)
        {
            Debug.LogError("UiButtonMapiing component is not attached to the transform.");
            return;
        }
        

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        // Check if pointer is over UI
        if (inputManager.IsPointerOverUI())
        {
            return;
        }

        Debug.Log($"Grid Position - X: {gridPosition.x}, Y: {gridPosition.y}, Z: {gridPosition.z}");

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedOnbjectIndex);

        if (!placementValidity)
        {
            return;
        }

        // Price calculation and building time setup based on selected index
        if (selectedOnbjectIndex == 0)
        {
            uiButtonMapping.PriceCalculation(150, 0, 0);
            BuildingTime = 37f;
        }
        else if (selectedOnbjectIndex == 1)
        {
            uiButtonMapping.PriceCalculation(200, 100, 0);
            BuildingTime = 37f;
        }
        else if (selectedOnbjectIndex == 2)
        {
            uiButtonMapping.PriceCalculation(150, 100, 0);
            BuildingTime = 37f;
        }
        else if (selectedOnbjectIndex == 3)
        {
            uiButtonMapping.PriceCalculation(400, 0, 0);
            BuildingTime = 50f;
        }
        else if (selectedOnbjectIndex == 4)
        {
            uiButtonMapping.PriceCalculation(100, 0, 0);
            BuildingTime = 25f;
        }
        else if (selectedOnbjectIndex == 5)
        {
            uiButtonMapping.PriceCalculation(125, 0, 0);
            BuildingTime = 37f;
        }
        else if (selectedOnbjectIndex == 6)
        {
            uiButtonMapping.PriceCalculation(0, 0, 0);
            BuildingTime = 0f;
        }
        else
        {
            Debug.LogError("Unhandled selectedOnbjectIndex value: " + selectedOnbjectIndex);
            return;
        }

        if (selectedOnbjectIndex == 6)
        {

        }
        else
        {
            if(uiButtonMapping.SpawnOK == 1)
            {
                GameObject newobject = Instantiate(database[WhatRace].objectsData[7].Prefab);

                // Position the new object in the world
                Vector3 worldPosition = grid.CellToWorld(gridPosition);
                worldPosition.y = mousePosition.y;

                newobject.transform.position = worldPosition;

                // Add the object to placedGameObject and update grid data
                placedGameObject.Add(newobject);
                GridData selectedData = database[WhatRace].objectsData[selectedOnbjectIndex].ID == 0 ? floorData : furnitureData;
                selectedData.AddObjectAt(gridPosition,
                    database[WhatRace].objectsData[selectedOnbjectIndex].Size,
                    database[WhatRace].objectsData[selectedOnbjectIndex].ID,
                    placedGameObject.Count - 1);

                // Update the preview position
                preview.UpdatePosition(worldPosition, false);

                StartCoroutine(BuildingSpawn(worldPosition, gridPosition, selectedOnbjectIndex));
            }
        }
    }

    public IEnumerator BuildingSpawn(Vector3 worldpos, Vector3Int gridpos, int selectindex)
    {
        // Wait for the building time to elapse
        yield return new WaitForSeconds(BuildingTime);


        // Instantiate the selected prefab
        GameObject prefab = database[WhatRace].objectsData[selectindex].Prefab;

        if (prefab == null)
        {
            Debug.LogError("Prefab is null for selectedOnbjectIndex: " + selectindex);
            yield break;
        }

        GameObject newobject = Instantiate(prefab);

        // Add the new building to the relevant lists
        BuildingController unit = newobject.GetComponent<BuildingController>();

        unit.EyesOn();
        unit.myBuildingType = selectindex;
        unit.myGridpos = gridpos;


        transform.parent.GetComponent<UnitSpawner>().buildingList.Add(unit);

        if (newobject.CompareTag("MainBase"))
        {
            transform.parent.GetComponent<RTSUnitController>().MainBase.Add(newobject);
            unit.MainBaseint = transform.parent.GetComponent<RTSUnitController>().MainBase.Count - 1;
        }
        if (newobject.CompareTag("Supply Depot"))
        {
            GameObject.Find("RTSUnitControlSystem").GetComponent<UiButtonMapiing>().maxPopulationCount += 8;
        }

        newobject.transform.position = worldpos;
    }

    public void RemoveGrid(Vector3Int gridpos, int selectindex)
    {
        GridData selectedData = database[WhatRace].objectsData[selectindex].ID == 0 ? floorData : furnitureData;
        selectedData.RemoveObjectAt(gridpos);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedOnbjectIndex)
    {
        GridData selectedData = database[WhatRace].objectsData[selectedOnbjectIndex].ID == 0 ? floorData : furnitureData;

        return selectedData.CanPlaceObejctAt(gridPosition, database[WhatRace].objectsData[selectedOnbjectIndex].Size);
    }

    public void StartRemoving()
    {
        throw new NotImplementedException();
    }

    private void Update()
    {
        if(transform.parent.GetComponent<RTSUnitController>().BuildMode == 1)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                StartPlacement(1);
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                StartPlacement(2);
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                StartPlacement(3);
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                StartPlacement(4);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                StartPlacement(5);
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                StartPlacement(6);
            }
        }

        if (selectedOnbjectIndex < 0)
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if (lastDetectedPosition != gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition, selectedOnbjectIndex);

            mouseIndicator.transform.position = mousePosition;

            Vector3 worldPosition = grid.CellToWorld(gridPosition);

            worldPosition.y = mousePosition.y - 1;

            preview.UpdatePosition(worldPosition, placementValidity);
        }
    }
}
