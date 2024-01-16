using UnityEngine;
using DG.Tweening;

public class Finish : MonoBehaviour
{
    public PlayerController player;
    public GameObject leftConfetti;
    public GameObject rightConfetti;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var characterMovement = other.GetComponent<CharacterMovement>();
            var playerController = other.GetComponent<PlayerController>();
            characterMovement.canMoveForward = false;
            characterMovement.canMoveDownwards = false;
            UIManager.instance.isGameOver = true;
            playerController.Do360();
            PlayConfettiVFX();
            GameManager.instance.LevelComplete();
        }
    }

    private void PlayConfettiVFX()
    {
        rightConfetti.transform.position = Camera.main.transform.position + new Vector3(.5f, 0f, 2.5f);
        leftConfetti.transform.position = Camera.main.transform.position + new Vector3(-.5f, 0f, 2.5f);
        rightConfetti.gameObject.SetActive(true);
        leftConfetti.gameObject.SetActive(true);
        float value = 0f;
        DOTween.To(() => value, x => x = value, 1f, 1f)
            .OnComplete(delegate
            {
                rightConfetti.gameObject.SetActive(false);
                leftConfetti.gameObject.SetActive(false);
            });
    }
}
