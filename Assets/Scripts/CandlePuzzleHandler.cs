using UnityEngine;

public class CandlePuzzleHandler : MonoBehaviour
{
    [SerializeField] private ParticleSystem candleA;
    [SerializeField] private ParticleSystem candleB;
    [SerializeField] private ParticleSystem candleC;
    [SerializeField] private GameObject teleportPad;

    private void Awake()
    {
        if (teleportPad != null)
        {
            teleportPad.SetActive(false);
        }
    }

    private void Update()
    {
        if (teleportPad == null || candleA == null || candleB == null || candleC == null)
        {
            return;
        }

        bool correctCandlesLit = candleA.isEmitting && candleB.isEmitting;
        bool thirdCandleExtinguished = !candleC.isEmitting;
        teleportPad.SetActive(correctCandlesLit && thirdCandleExtinguished);
    }
}
