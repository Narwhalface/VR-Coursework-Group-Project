using UnityEngine;

public class AmbienceSounds : MonoBehaviour
{
    public Collider Area;
    public GameObject Player;

    // Update is called once per frame
    void Update()
    {
        Vector3 closestPoint = Area.ClosestPoint(Player.transform.position);

        transform.position = closestPoint;
    }
}
