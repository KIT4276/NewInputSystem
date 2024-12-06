using UnityEngine;

public class Placable : MonoBehaviour
{
    private GridPlace _place;

    public GridPlace GridPlace { get => _place; set { _place = value; } }
}
