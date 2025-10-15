using UnityEngine;

public class BuildingStatus : MonoBehaviour
{
    [SerializeField] private int residentCapacity = 0;
    [SerializeField] private int cost = 0;

    public int ResidentCapacity { get { return residentCapacity; } }
    public int Cost { get { return cost; } }
}
