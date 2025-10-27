using System.Globalization;
using UnityEngine;

public class BuildButton : MonoBehaviour
{
    [SerializeField] private GameObject buildingPrefab;
    [SerializeField] private BuildingInstaller buildingInstaller;

    public void OnButtonClicked()
    {
        buildingInstaller.BuildingInstance(buildingPrefab);
    }
}
