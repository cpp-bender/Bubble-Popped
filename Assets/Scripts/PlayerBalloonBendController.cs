using UnityEngine;

public class PlayerBalloonBendController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Balloon"))
        {
            Debug.Log("Bending");
            var pos = other.ClosestPoint(transform.parent.position);
            var ballonController = other.GetComponent<BalloonController>();
            ballonController.DoBend(pos);
        }
    }
}
