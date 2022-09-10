using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody rb;

    public float speed;

    private bool isMoving;
    private Vector3 moveDirection;
    private Vector3 nextCollisionPosition;


    public int minSwipeRecognition = 500;
    private Vector2 swipePosLastFrame;
    private Vector2 swipePosCurrentFrame;
    private Vector2 currentSwipe;

    private Color giveColor;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        giveColor = Random.ColorHSV(0.5f, 1.5f);
        GetComponent<MeshRenderer>().material.color = giveColor;
    }
    private void FixedUpdate()
    {
        if (isMoving)
        {
            rb.velocity = speed * moveDirection;
        }

        //changing color
        Collider[] hitColliders = Physics.OverlapSphere(transform.position - ( Vector3.up/2 ), 0.02f);

        int i = 0;
        while (i< hitColliders.Length)
        {
            Ground ground = hitColliders[i].transform.GetComponent<Ground>();
            if(ground && !ground.isColored)
            {
                ground.ChangeColor(giveColor);
            }
            i++;
        }


        //movement
        if(nextCollisionPosition != Vector3.zero)
        {
            if(Vector3.Distance(transform.position , nextCollisionPosition) < 1)
            {
                isMoving = false;
                moveDirection = Vector3.zero;
                nextCollisionPosition = Vector3.zero;
            }
        }

        if (isMoving)
            return;

        if(Input.GetMouseButton(0))
        {
            swipePosCurrentFrame = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            if(swipePosLastFrame != Vector2.zero)
            {
                currentSwipe = swipePosCurrentFrame - swipePosLastFrame;

                if(currentSwipe.sqrMagnitude > minSwipeRecognition)
                {
                    currentSwipe.Normalize();

                    if (currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                    {
                        //go up / down
                        SetDestination(currentSwipe.y > 0 ? Vector3.forward : Vector3.back);
                        //just a advanced if statemenet

                    }
                    if (currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                    {
                        //go left / right
                        SetDestination(currentSwipe.x > 0 ? Vector3.right : Vector3.left);

                    }
                }
                else
                {
                    return;
                }

                
            }
            swipePosLastFrame = swipePosCurrentFrame;
        }

        if(Input.GetMouseButtonUp(0))
        {
            swipePosLastFrame = Vector2.zero;
            currentSwipe = Vector2.zero;
        }

    }

    private void SetDestination(Vector3 direction)
    {
        moveDirection = direction;

        RaycastHit hit;
        if(Physics.Raycast(transform.position, direction, out hit, 100f))
        {
            nextCollisionPosition = hit.point;

        }

        isMoving = true;
    }

}
