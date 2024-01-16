using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class DoorController : MonoBehaviour
{
    [Range(1f, 50f)] [SerializeField] float bounceValue;

    [Tooltip("Color Queue -> Yellow - Green - Blue")]
    public List<BarrierSerializable> barrierSerializables;
    public GameObject gateLight;

    private ReferenceContanier referenceContanier;
    private Camera mainCam;
    private int doorHitCount;

    private Color gateLightOriginalColor;
    private Color gateLightTransparentColor;

    private void Start()
    {
        referenceContanier = ReferenceContanier.Instance;
        mainCam = Camera.main;
        gateLightOriginalColor = gateLight.GetComponent<MeshRenderer>().material.color;
        gateLightTransparentColor =
            new Color(gateLightOriginalColor.r, gateLightOriginalColor.g, gateLightOriginalColor.b, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var mainCam = Camera.main.GetComponent<CameraFollow>();
            var playerController = other.GetComponent<PlayerController>();
            for (int i = 0; i < barrierSerializables.Count; i++)
            {
                if (Convert.ToInt32(barrierSerializables[i].doorText.text) != 0f)
                {
                    if (doorHitCount > 0)
                    {
                        other.GetComponent<PlayerController>().DoBounceBack(5, 1f, true);
                        GameManager.instance.LevelFail();
                        return;
                    }
                    StartCoroutine(mainCam.ChangeLerpSpeedTo(10));
                    playerController.DoBounceBack(bounceValue, 1.25f);
                    doorHitCount++;
                    return;
                }
            }
            DoorController activeDoor = referenceContanier.GetActiveDoor();
            MexicanWaveController mexicanWave = referenceContanier.GetMexicanWaveObject();
            mexicanWave.DoMexicanWave(activeDoor.transform.TransformPoint(new Vector3(0f, -.5f, -5f)), 30f);
            Taptic.Light();
            referenceContanier.SetActiveDoor(++referenceContanier.ActiveDoorIndex);
            transform.DOMoveY(-3, 1f)
                .SetDelay(1f)
                .OnComplete(() => gameObject.SetActive(false));
        }
    }

    public void HandleGateLightActiveness()
    {
        for (int i = 0; i < barrierSerializables.Count; i++)
        {
            if (Convert.ToInt32(barrierSerializables[i].doorText.text) != 0f)
            {
                // gateLight.GetComponent<MeshRenderer>().material.DOFade(.5f, .25f);
                gateLight.GetComponent<MeshRenderer>().material.color = gateLightOriginalColor;
                return;
            }
        }
        // gateLight.GetComponent<MeshRenderer>().material.DOFade(0f, .25f);
        gateLight.GetComponent<MeshRenderer>().material.color = gateLightTransparentColor;
    }
}
