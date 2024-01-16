using UnityEngine;

public class BarrierController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var playerController = other.GetComponent<PlayerController>();
            //TODO: Change bounce value later on, use it in the editor
            playerController.DoBounceBack(10);
        }
    }
}
