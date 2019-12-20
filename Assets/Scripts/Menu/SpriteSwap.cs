using UnityEngine;

public class SpriteSwap : MonoBehaviour
{
    public GameObject on;
    public GameObject off;
    public void Swap() //Swaps button sprites between on and off states.
    {
        on.SetActive(!on.activeSelf);
        off.SetActive(!off.activeSelf);
    }
}
