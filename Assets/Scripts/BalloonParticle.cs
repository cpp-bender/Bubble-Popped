using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonParticle : MonoBehaviour
{
    [SerializeField] ParticleSystem balloonParticle;
    private SkinnedMeshRenderer meshRenderer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(PlayParticle());
        }
    }

    private IEnumerator PlayParticle()
    {

        Instantiate(balloonParticle, transform.position + Vector3.up * .5f, balloonParticle.transform.rotation);
        yield return new WaitForSeconds(.5f);
        gameObject.SetActive(false);
    }
}
