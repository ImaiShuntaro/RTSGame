using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Mono.Cecil.Cil;
public class GameManager : MonoBehaviour
{
    private PlayerStatus status;
    [SerializeField] private GameObject dropdown;
    [SerializeField] private GameObject housingPanel;
    [SerializeField] private GameObject agriculturePanel;

    [SerializeField] private GameObject populationText;
    [SerializeField] private GameObject foodText;
    [SerializeField] private GameObject fundText;
    [SerializeField] private GameObject steelText;

    

    private void Start()
    {
        housingPanel.SetActive(false);
        agriculturePanel.SetActive(false);
        dropdown.SetActive(false);
        status = GetComponent<PlayerStatus>();
    }

    private void Update()
    {
        populationText.GetComponent<Text>().text = $"{status.Population}/{status.MaxPopulation}";
        foodText.GetComponent<Text>().text = status.Food.ToString();
        fundText.GetComponent<Text>().text = status.Fund.ToString();
        steelText.GetComponent<Text>().text = status.Steel.ToString();

        if (status.Population >status.MaxPopulation)
        {
            populationText.GetComponent<Text>().color = Color.red;
        }
        else
        {
            populationText.GetComponent<Text>().color = Color.black;
        }
    }

    public void OnBuildButtonClicked()
    {
        dropdown.SetActive(true);
        PanelChange(dropdown.GetComponent<TMP_Dropdown>().value);
    }

    public void OnValueChanged()
    {
        PanelChange(dropdown.GetComponent<TMP_Dropdown>().value);
        dropdown.SetActive(false);
    }

    private void PanelChange(int value)
    {
        if (value == 0)
        {
            housingPanel.SetActive(true);
            agriculturePanel.SetActive(false);
        }
        else if (value == 1)
        {
            housingPanel.SetActive(false);
            agriculturePanel.SetActive(true);
        }
        else
        {
            housingPanel.SetActive(false);
            agriculturePanel.SetActive(false);
        }
    }
}
