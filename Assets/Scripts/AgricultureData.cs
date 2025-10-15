using UnityEngine;

public class AgricultureData : MonoBehaviour
{ 
    [SerializeField] private float harvestCycle;
    [SerializeField] private int harvestYield;
    [SerializeField] private int cost;
    [SerializeField] private GameObject fieldCropGrowingPrefab;
    [SerializeField] private GameObject fieldCropPrefab;
    private GameObject fieldCrop;
    PlayerStatus playerStatus;
    private float passedTime;
    private int count = 0;

    private void Start()
    {
        playerStatus = FindAnyObjectByType<PlayerStatus>();
    }
    private void Update()
    {
        passedTime += Time.deltaTime;
        if (passedTime > harvestCycle)
        {
            switch (count)
            {
                case 0:
                    if (fieldCrop != null)
                    {
                        Destroy(fieldCrop);
                    }
                    count++;
                    Debug.Log("zero");
                    break;

                case 1:
                    fieldCrop = Instantiate(fieldCropGrowingPrefab, transform.position, Quaternion.identity);
                    count++;
                    Debug.Log("one");
                    break;

                case 2:
                    if (fieldCrop != null)
                    {
                        Destroy(fieldCrop);
                    }
                    fieldCrop = Instantiate(fieldCropPrefab, transform.position, Quaternion.identity);
                    if (playerStatus != null)
                    {
                        playerStatus.Food += harvestYield;
                    }                    
                    count = 0;
                    Debug.Log("two");
                    break;
            }
            passedTime = 0;
        }
    }
}
