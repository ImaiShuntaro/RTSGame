using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private float population = 0;
    private int food = 0;
    private int fund = 0;
    private int steel = 0;

    private int maxPopulation = 0;

    public float Population
    {
        get { return population; }
        set { population = value; }
    }

    public int Food
    {
        get { return food; }
        set { food = value; }
    }
    public int Fund
    {
        get { return fund; }
        set { fund = value; }
    }

    public int Steel
    {
        get { return steel; }
        set { steel = value; }
    }

    public int MaxPopulation
    {
        get { return maxPopulation; }
        set { maxPopulation = value; }
    }
}
