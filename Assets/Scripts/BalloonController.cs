using UnityEngine;
using DG.Tweening;

public enum BalloonMaterial { None, Red, Yellow, Green, Blue }

[SelectionBase]
public class BalloonController : MonoBehaviour
{
    public Material yellowMat;
    public Material greenMat;
    public Material blueMat;
    public Material redMat;

    [SerializeField] ParticleSystem balloonParticle;
    [SerializeField] BalloonMaterial balloonMaterial;
    [SerializeField] bool isSwinging = false;

    private SkinnedMeshRenderer skinnedMeshRenderer;
    private ReferenceContanier referenceContanier;

    private void Awake()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
    }

    private void Start()
    {
        referenceContanier = ReferenceContanier.Instance;
    }

    public void DoBalloonOnTrigger(Vector3 playerPos)
    {
        HandleColor();
        TurnOffCollider();
        DoBlendShape();
        Instantiate(balloonParticle, transform.position + Vector3.up * .5f, balloonParticle.transform.rotation);
        gameObject.SetActive(false);
    }

    public void HandleColor()
    {
        /* 0 - > Yellow
         * 1 - > Green
         * 2 - > Blue
        */
        DoorController door = referenceContanier.GetActiveDoor();
        if (balloonMaterial == BalloonMaterial.Yellow)
        {
            var barrier = door.barrierSerializables[0];
            barrier.HandleDoorText(false);
            barrier.HandleTextActiveness();
            door.HandleGateLightActiveness();
            barrier.HandleDoorScale(-1);
            barrier.HandleDoorBarrierColor(yellowMat.color);
            barrier.HandleDoorTextColor(yellowMat.color);
        }
        else if (balloonMaterial == BalloonMaterial.Green)
        {
            var barrier = door.barrierSerializables[1];
            barrier.HandleDoorText(false);
            barrier.HandleTextActiveness();
            door.HandleGateLightActiveness();
            barrier.HandleDoorScale(-1);
            barrier.HandleDoorBarrierColor(greenMat.color);
            barrier.HandleDoorTextColor(greenMat.color);
        }
        else if (balloonMaterial == BalloonMaterial.Blue)
        {
            var barrier = door.barrierSerializables[2];
            barrier.HandleDoorText(false);
            barrier.HandleTextActiveness();
            door.HandleGateLightActiveness();
            barrier.HandleDoorScale(-1);
            barrier.HandleDoorBarrierColor(blueMat.color);
            barrier.HandleDoorTextColor(blueMat.color);

        }
        else if (balloonMaterial == BalloonMaterial.Red)
        {
            for (int i = 0; i < door.barrierSerializables.Count; i++)
            {
                var currentBarrierSerializable = door.barrierSerializables[i];
                if (!currentBarrierSerializable.isActiveForThisDoor)
                {
                    continue;
                }
                currentBarrierSerializable.HandleDoorText(true);
                currentBarrierSerializable.HandleTextActiveness();
                door.HandleGateLightActiveness();
                currentBarrierSerializable.HandleDoorScale(1);
                currentBarrierSerializable.HandleDoorBarrierColor(currentBarrierSerializable.GetDoorBarrierColor());
                currentBarrierSerializable.HandleDoorTextColor(currentBarrierSerializable.GetDoorTextColor());
            }
        }
    }

    private void TurnOffCollider()
    {
        GetComponent<BoxCollider>().enabled = false;
    }

    private void DoBlendShape()
    {
        DOTween.To(GetBlendShapeBump, SetBlendShapeBump, 100f, .25f);
    }

    private float GetBlendShapeBump()
    {
        return skinnedMeshRenderer.GetBlendShapeWeight(0);
    }

    private void SetBlendShapeBump(float bumpValue)
    {
        skinnedMeshRenderer.SetBlendShapeWeight(0, bumpValue);
    }

    public void DoBend(Vector3 playerPos, float duration = .06f)
    {
        var directionToTarget = (transform.position - (playerPos - Vector3.up)).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.DORotate(lookRotation.eulerAngles, duration)
            .OnStart(OnBendStart)
            .OnComplete(OnBendEnd);
    }

    private void OnBendStart()
    {
        isSwinging = true;
    }

    private void OnBendEnd()
    {
        transform.DORotate(new Vector3(270f, 0f, 0f), .3f);
        isSwinging = false;
    }
}
