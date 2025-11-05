using UnityEngine;

public class Selecter : MonoBehaviour
{
    private GameObject selectedObject;
    [SerializeField] private GameObject selectedUI;
    private GameObject destinationSigh;
    private MeshRenderer meshRenderer;
    private BuildingInstaller buildingInstaller;

    private Vector3 xzMousePos;
    private Vector3 gridPos;
    private Vector3 destination;

    private bool isMoving = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        destinationSigh = Instantiate(selectedUI, transform.position, Quaternion.identity);
        destinationSigh.GetComponent<MeshRenderer>().material.color = Color.blue;
        selectedUI.SetActive(false);
        buildingInstaller = GetComponent<BuildingInstaller>();
        meshRenderer = selectedUI.GetComponent<MeshRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (buildingInstaller.IsBuilding || Input.GetMouseButtonDown(1))
        {
            isMoving = false;
            selectedUI.SetActive(false);
            destinationSigh.SetActive(false);
        }
        if (Input.GetMouseButtonDown(0) && !GetComponent<BuildingInstaller>().IsBuilding)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.GetComponent<MoveCommander>() != null)
                {
                    selectedObject = hit.collider.gameObject;
                    isMoving = true;
                }
            }
        }

        if (isMoving && buildingInstaller != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane xzplane = new Plane(Vector3.up, Vector3.zero);
            float distance;
            if (xzplane.Raycast(ray, out distance))
            {
                xzMousePos = ray.GetPoint(distance);
            }
            gridPos = buildingInstaller.GridPosition(xzMousePos);

            selectedUI.SetActive(true);
            selectedUI.transform.position = gridPos;
            if (buildingInstaller.buildingPosition.Contains(gridPos))
            {
                meshRenderer.material.color = Color.red;
            }
            else
            {
                meshRenderer.material.color = Color.green;
                if (Input.GetMouseButtonDown(0))
                {
                    destination = gridPos;
                    destinationSigh.SetActive(true);
                    destinationSigh.transform.position = destination;
                    selectedObject.GetComponent<MoveCommander>().MoveTo(destination, buildingInstaller.buildingPosition);
                    //selectedObject.GetComponent<MoveCommander>().Astar(destination);
                }
            }
        }
    }
}
