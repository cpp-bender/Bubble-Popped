using System.Collections;
using DG.Tweening;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("")]
    [SerializeField] Transform player;
    [SerializeField] Vector3 offset;
    [SerializeField] float lerpSpeed;
    [SerializeField] float lerpYDamper;

    private void Awake()
    {
        DOTween.Init(true, true, LogBehaviour.Default);
        DOTween.defaultEaseType = Ease.Linear;
        DOTween.defaultAutoPlay = AutoPlay.All;
    }

    private void LateUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        var camCurrentPos = transform.position;
        var playerPos = new Vector3(player.position.x, player.position.y + lerpYDamper, player.position.z);
        var camNextPos = playerPos + offset;
        transform.position = Vector3.Lerp(camCurrentPos, camNextPos, Time.deltaTime * lerpSpeed);
    }

    public IEnumerator ChangeLerpSpeedTo(float speed, float duration = 1f)
    {
        lerpSpeed = speed;
        yield return new WaitForSeconds(duration);
        lerpSpeed = 2f;
    }
}
