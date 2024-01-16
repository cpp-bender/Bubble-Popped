using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    private SkinnedMeshRenderer skinnedMesh;

    private Sequence rotateSequence;
    private Animator animator;
    private CharacterMovement characterMovement;

    private void Awake()
    {
        skinnedMesh = GetComponent<SkinnedMeshRenderer>();
        rotateSequence = DOTween.Sequence();
        animator = GetComponent<Animator>();
        characterMovement = GetComponent<CharacterMovement>();
        InitRotateTween();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Balloon"))
        {
            var pos = other.ClosestPoint(transform.position);
            var balloonController = other.GetComponent<BalloonController>();
            balloonController.DoBalloonOnTrigger(pos);
        }
    }

    public void DoBounceBack(float bounce = 10f, float duration = 1f, bool canFail = false)
    {
        var characterMovement = GetComponent<CharacterMovement>();
        transform.DOMoveZ(transform.position.z - bounce, duration)
            .SetEase(Ease.OutQuart)
            .OnStart(delegate
            {
                transform.DOMoveY(0.8f, .5f);
                characterMovement.canMoveForward = false;
                characterMovement.canMoveDownwards = false;
            })
            .OnComplete(delegate
            {
                if (!canFail)
                {
                    characterMovement.canMoveForward = true;
                    characterMovement.canMoveDownwards = true;
                }
            });
    }

    public float GetBlendShapeBump()
    {
        return skinnedMesh.GetBlendShapeWeight(0);
    }

    public void SetBlendShapeBumpTo(float bumpValue)
    {
        skinnedMesh.SetBlendShapeWeight(0, bumpValue);
    }

    public void Do360()
    {
        transform.DOMove(new Vector3(0f, .2f, transform.position.z), .5f)
            .OnStart(delegate
            {
                characterMovement.canMoveDownwards = false;
                characterMovement.canMoveForward = false;
                animator.SetBool("isHoldingDown", true);
            })
            .OnComplete(() => rotateSequence.Play());

    }

    private void InitRotateTween(float rotateDuration = 1f, Ease easeType = Ease.Linear, float rotateAngle = 360f, int loopCount = 2)
    {
        Vector3 rotateVector = new Vector3(0f, rotateAngle, 0f);
        rotateSequence.Append(transform.DORotate(rotateVector, rotateDuration, RotateMode.WorldAxisAdd).SetEase(easeType).SetLoops(loopCount, LoopType.Incremental).SetDelay(.25f));
        rotateSequence.Append(transform.DORotate(-rotateVector, rotateDuration, RotateMode.WorldAxisAdd).SetEase(easeType).SetLoops(loopCount, LoopType.Incremental).SetDelay(.25f))
            .SetLoops(-1, LoopType.Restart)
            .Pause();
    }
}
