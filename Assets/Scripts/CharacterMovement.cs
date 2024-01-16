using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterMovement : MonoBehaviour
{
    public float leftLimit = -2f;
    public float rightLimit = 2f;
    public float forwardSpeed = 4f;

    #region EDITED BY AHMET OMAK
    public bool canMoveForward;
    public bool canMoveDownwards;
    public float desiredDownY;
    public float desiredUpY;
    public float rotateSpeed;
    private Animator animator;
    #endregion

    private float swipeSensivity;
    private float maximumSensivity = 100f;

    private Vector3 targetPos;

    private bool lockLeft;
    private bool lockRight;

    private void Start()
    {
        TouchManager.instance.onTouchBegan += TouchBegan;
        TouchManager.instance.onTouchMoved += TouchMoved;
        TouchManager.instance.onTouchEnded += TouchEnded;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (canMoveDownwards)
        {
            if (transform.position.y >= desiredDownY && Input.GetMouseButton(0))
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, desiredDownY, transform.position.z), Time.deltaTime * 10);
            }
            else if (transform.position.y <= desiredUpY && !Input.GetMouseButton(0))
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, desiredUpY, transform.position.z), Time.deltaTime * 10);
            }
        }


        if (canMoveForward)
        {
            Movement();
        }
    }

    private void TouchBegan(TouchInput touch)
    {
        var player = GetComponent<PlayerController>();
        DOTween.To(player.GetBlendShapeBump, player.SetBlendShapeBumpTo, 0f, .3f);
        targetPos = transform.position;
        if (!UIManager.instance.isGameOver)
        {
            animator.SetBool("isHoldingDown", true);
        }
    }

    private void TouchEnded(TouchInput touch)
    {
        var player = GetComponent<PlayerController>();
        DOTween.To(player.GetBlendShapeBump, player.SetBlendShapeBumpTo, 100f, .3f);
        targetPos = transform.position;
        if (!UIManager.instance.isGameOver)
        {
            animator.SetBool("isHoldingDown", false);
        }
        swipeSensivity = 0f;
    }

    private void TouchMoved(TouchInput touch)
    {
        swipeSensivity = Mathf.Abs(touch.DeltaScreenPosition.x);

        if (swipeSensivity > maximumSensivity)
        {
            swipeSensivity = maximumSensivity;
        }

        if (touch.DeltaScreenPosition.x > 0)
        {
            if (rightLimit < transform.position.x - (swipeSensivity / 1000f))
            {
                targetPos = new Vector3(transform.position.x + (swipeSensivity / 1000f), transform.position.y, transform.position.z);
            }
            else
            {
                targetPos = new Vector3(rightLimit, transform.position.y, transform.position.z);
            }

        }
        else if (touch.DeltaScreenPosition.x < 0)
        {
            if (leftLimit > transform.position.x + (swipeSensivity / 1000f))
            {
                targetPos = new Vector3(transform.position.x + (((swipeSensivity) / -1000f)), transform.position.y, transform.position.z);
            }
            else
            {
                targetPos = new Vector3(leftLimit, transform.position.y, transform.position.z);
            }

        }
        else
        {
            targetPos = transform.position;
        }
    }

    private void Movement()
    {
        if ((transform.position.x - targetPos.x < 0 && !lockRight) || (transform.position.x - targetPos.x > 0 && !lockLeft))
        {
            if (Time.timeScale >= 0.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * swipeSensivity / 2f);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.unscaledDeltaTime * swipeSensivity / 2f);
            }
        }

        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1f), Time.deltaTime * forwardSpeed);
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 180f, 20f * swipeDir), Time.deltaTime * rotateSpeed);
    }
}
