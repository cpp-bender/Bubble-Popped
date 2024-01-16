using UnityEngine;
using System;
using TMPro;

[Serializable]
public class BarrierSerializable
{
    public TextMeshPro doorText;
    public GameObject barrier;
    public GameObject counter;
    public int startDoorCount;
    public bool isActiveForThisDoor;

    public void HandleTextActiveness()
    {
        if (Convert.ToInt32(doorText.text) == 0)
        {
            barrier.gameObject.SetActive(false);
            doorText.gameObject.SetActive(false);
        }
        else
        {
            barrier.gameObject.SetActive(true);
            doorText.gameObject.SetActive(true);
        }
    }

    public void HandleDoorText(bool increase)
    {
        int doorCount = Convert.ToInt32(doorText.text);
        doorCount = increase ? Mathf.Clamp(++doorCount, 0, 100) : Mathf.Clamp(--doorCount, 0, 100);
        doorText.text = doorCount.ToString();
    }

    public void HandleDoorScale(int scaleFactor)
    {
        float scaleDamper = 3f / startDoorCount;
        var scaleY = Mathf.Clamp(barrier.transform.localScale.y + (scaleDamper * scaleFactor), 0f, 3f);
        barrier.transform.localScale = new Vector3(barrier.transform.localScale.x, scaleY, barrier.transform.localScale.z);
    }

    public void HandleDoorBarrierColor(Color color)
    {
        barrier.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = color;
    }

    public void HandleDoorTextColor(Color color)
    {
        counter.transform.GetComponent<MeshRenderer>().material.color = color;
    }

    public Color GetDoorBarrierColor()
    {
        return barrier.transform.GetChild(0).GetComponent<MeshRenderer>().material.color;
    }

    public Color GetDoorTextColor()
    {
        return counter.transform.GetComponent<MeshRenderer>().material.color;
    }
}
