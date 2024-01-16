using UnityEngine;
using DG.Tweening;

public class MexicanWaveController : MonoBehaviour
{
    private new CapsuleCollider collider;

    [Header("Mexican Wave Parameters")]
    public float waveDamper;
    public float cylinderMoveDuration;
    public float waveStartDuration;
    public float waveEndDuration;
    public Ease easeType;

    private void Awake()
    {
        collider = GetComponent<CapsuleCollider>();
    }

    public void DoMexicanWave(Vector3 initialPos, float moveValue = 20f)
    {
        HandleCollider(false);
        SetPosTo(initialPos);
        float moveTo = transform.position.z + moveValue;
        transform.DOMoveZ(moveTo, cylinderMoveDuration)
            .OnStart(delegate
            {
                HandleCollider(true);
            })
            .OnComplete(delegate
            {
                HandleCollider(false);
            });
    }

    private void SetPosTo(Vector3 pos)
    {
        transform.position = pos;
    }

    private void HandleCollider(bool turnOn)
    {
        if (turnOn)
        {
            collider.enabled = true;
        }
        else if (!turnOn)
        {
            collider.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Balloon"))
        {
            var balloon = other.GetComponent<BalloonController>();
            float initialBalloonPosY = balloon.transform.localPosition.y;
            float nextBalloonPosY = initialBalloonPosY + waveDamper;
            balloon.transform.DOLocalMoveY(nextBalloonPosY, waveStartDuration)
                .SetEase(easeType)
                .OnComplete(delegate
                {
                    balloon.transform.DOLocalMoveY(initialBalloonPosY, waveEndDuration);
                });
        }
    }
}
