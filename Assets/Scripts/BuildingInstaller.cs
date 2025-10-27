using System.Collections.Generic;
using UnityEngine;

public class BuildingInstaller : MonoBehaviour
{
    private Vector3 xzMousePos;
    private Vector3 constractPos;

    [SerializeField] private GameObject smallHousePrefab;
    [SerializeField] private GameObject mediumHousePrefab;
    [SerializeField] private GameObject bigHousePrefab;
    [SerializeField] private GameObject fieldPrefab;
    private GameObject building;

    // åöï®ÇÃç¿ïW åöï®ÇÃëÂÇ´Ç≥ÇÕ3x3Ç≈å≈íË
    public HashSet<Vector3> buildingPosition = new HashSet<Vector3>();
    private bool isBuilding = false;
    public bool IsBuilding {  get { return isBuilding; } set { isBuilding = value; } }

    private void Start()
    {
        
    }
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane xzplane = new Plane(Vector3.up, Vector3.zero);
        float distance;
        if (xzplane.Raycast(ray, out distance))
        {
            xzMousePos = ray.GetPoint(distance);
        }
        constractPos = GridPosition(xzMousePos);
        if (building != null && !building.GetComponent<LandManager>().IsPlaced)
        {
            building.transform.position = constractPos;
            LandManager landManager = building.GetComponent<LandManager>();
            bool canPlace = true;
            foreach (Vector3 pos in landManager.OccpiedSpace)
            {
                if (buildingPosition.Contains(pos))
                {
                    canPlace = false;
                    break;
                }
            }
            landManager.IsAvailable = canPlace;
            if (canPlace)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    landManager.IsPlaced = true;
                    building.GetComponent<BoxCollider>().enabled = true;
                    building.GetComponent<ParticleSystem>().Play();
                    PlayerStatus playerStatus = GetComponent<PlayerStatus>();
                    BuildingStatus buildingStatus = building.GetComponent<BuildingStatus>();
                    if (playerStatus != null && buildingStatus != null)
                    {
                        playerStatus.Fund -= buildingStatus.Cost;
                        playerStatus.MaxPopulation += buildingStatus.ResidentCapacity;
                    }
                    foreach (Vector3 pos in landManager.OccpiedSpace)
                    {
                        buildingPosition.Add(pos);
                    }
                    isBuilding = false;
                    building = null;
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    Destroy(building);
                    isBuilding = false;
                    building = null;
                }
            }
        }
    }

    public Vector3 GridPosition(Vector3 position)
    {
        Vector3 constractPos = position;
        float absX = Mathf.Abs(position.x);
        float absZ = Mathf.Abs(position.z);
        if (absX - Mathf.Floor(absX) < 0.5f)
        {
            constractPos.x = Mathf.Floor(absX);
        }
        else if (absX - Mathf.Floor(absX) >= 0.5f)
        {
            constractPos.x = Mathf.Floor(absX) + 0.5f;
        }
        if (position.x < 0)
        {
            constractPos.x *= -1;
        }
        if (absZ -Mathf.Floor(absZ) < 0.5f)
        {
            constractPos.z = Mathf.Floor(absZ);
        }
        else if (absZ - Mathf.Floor(absZ) >= 0.5f)
        {
            constractPos.z = Mathf.Floor(absZ) + 0.5f;
        }
        if (position.z < 0)
        {
            constractPos.z *= -1;
        }
        return constractPos;
    }

    public void BuildingInstance(GameObject buildingPrefab)
    {
        if (buildingPrefab == null) 
        {
            return;
        }
        building = Instantiate(buildingPrefab, constractPos, Quaternion.identity);
        isBuilding = true;
    }
}
