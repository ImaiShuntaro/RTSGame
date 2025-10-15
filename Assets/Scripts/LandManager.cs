using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider))]
public class LandManager : MonoBehaviour
{
    [SerializeField] private int xSize;
    [SerializeField] private int zSize;

    private bool isPlaced = false;
    private bool isAvailable = true;

    public bool IsPlaced { get { return isPlaced; } set { isPlaced = value; } }
    public bool IsAvailable { get { return isAvailable; } set { isAvailable = value; } }
    
    public List<Vector3> OccpiedSpace
    {
        get
        {
            List<Vector3> occupiedSpaces = new List<Vector3> ((2 * zSize + 1) * (2 * xSize + 1));
            float startx = -xSize / 2f;
            float startz = -zSize / 2f;
            for (int i = 0; i < (2 * zSize + 1); i++)
            {
                for (int j = 0; j < (2 * xSize + 1); j++)
                {                   
                    occupiedSpaces.Add(new Vector3(startx + j * 0.5f, 0, startz + i * 0.5f) + transform.position);
                }
            }
            return occupiedSpaces;
        }
    }

    private List<Color> originalColor = new List<Color>(4);
    private MeshRenderer meshRenderer;
    void Start()
    {
        GetComponent<BoxCollider>().enabled = false;
    }
    private void Update()
    {
        if (!isPlaced)
        {
            if (meshRenderer == null)
            {
                meshRenderer = GetComponent<MeshRenderer>();
                for (int i = 0; i < meshRenderer.materials.Length; i++)
                {
                    originalColor.Add(meshRenderer.materials[i].color);
                }
            }
            for (int i = 0; i < meshRenderer.materials.Length; i++)
            {
                if (isAvailable)
                {
                    meshRenderer.materials[i].color = Color.green;
                }
                else
                {
                    meshRenderer.materials[i].color = Color.red;
                }
            }
        }
        else
        {
            if (meshRenderer != null)
            {
                for (int i = 0; i < meshRenderer.materials.Length; i++)
                {
                    meshRenderer.materials[i].color = originalColor[i];
                }
                meshRenderer = null;
            }
        }
    }


}
