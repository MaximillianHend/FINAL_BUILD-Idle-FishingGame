using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Tilemaps;
using UnityEngine;

public class FishPatrol : MonoBehaviour
{
    //boundaries
    public float minX = -8f;
    public float maxX = 8f;

    public float minY = -30f;
    public float maxY = -5f;

    // movement
    private float maxSpeed = 2f;
    private float acceleration = 1f;
    private float turnSpeed = 1.5f;

    // roaming
    private float directionChangeTime = 2f;

    private Vector2 velocity;
    private Vector2 targetDirection;
    private float directionTimer;

    // escape state / behabiour
    public Transform player;
    public float detectionDistance = 4f;
    public float normalMaxSpeed = 2f;
    public float escapeMaxSpeed = 5f;


    // hurt state
    public float hurtSpeed = 7f;
    public float hurtDuration = 1.5f;
    public float hurtTimerInterval = 0.25f;

    private bool isHurt = false;
    private float hurtTimer = 0f;
    private float hurtTurnTimer = 0f;




    private void Start()
    {
        // Pick a random starting direction
        targetDirection = Random.insideUnitCircle.normalized;

        if (targetDirection == Vector2.zero)
        {
            targetDirection = Vector2.right;
        }

        // Start the direction timer
        directionTimer = directionChangeTime;
    }

    private void Update()
    {
        if (!isHurt && player != null)
        {
            float distanceToPlayer = Vector2.Distance(
                transform.position,
                player.position
            );

            if (distanceToPlayer <= detectionDistance)
            {
                maxSpeed = escapeMaxSpeed;
            }
            else
            {
                maxSpeed = normalMaxSpeed;
            }
        }

        if (isHurt)
        {
            hurtTimer -= Time.deltaTime;
            hurtTurnTimer -= Time.deltaTime;

            if (hurtTimer <= 0f)
            {
                isHurt = false;
                maxSpeed = normalMaxSpeed;
            }
            else
            {
                maxSpeed = hurtSpeed;

                if (hurtTurnTimer <= 0f)
                {
                    targetDirection = Random.insideUnitCircle.normalized;
                    hurtTurnTimer = hurtTimerInterval;
                }
            }

            ChooseDirection();

        }

        Move();


    }



    private void ChooseDirection()
    {
        directionTimer -= Time.deltaTime;

        if (directionTimer <= 0)
        {
            // Pick a new random direction
            targetDirection = Random.insideUnitCircle.normalized;

            if (targetDirection == Vector2.zero)
            {
                targetDirection = Vector2.right;
            }

            // Pick a random amount of time before changing direction again
            directionTimer = Random.Range(
                directionChangeTime * 0.5f,
                directionChangeTime * 1.5f
            );
        }
    }

    private void Move()
    {
        // Gradually turn toward the desired direction
        Vector2 newDirection = Vector2.Lerp(
            velocity.normalized,
            targetDirection,
            turnSpeed * Time.deltaTime
        ).normalized;

        // Gradually accelerate
        float targetSpeed = maxSpeed;

        float currentSpeed = velocity.magnitude;

        currentSpeed = Mathf.MoveTowards(
            currentSpeed,
            targetSpeed,
            acceleration * Time.deltaTime
        );

        velocity = newDirection * currentSpeed;

        // move the fish and boundaries
        Vector3 nextPosition = transform.position
            + (Vector3)(velocity * Time.deltaTime);

        // left boundary
        if (nextPosition.x < minX)
        {
            nextPosition.x = minX;
            targetDirection.x = Mathf.Abs(targetDirection.x);
            velocity.x = Mathf.Abs(velocity.x);
        }

        // right boundary
        if (nextPosition.x > maxX)
        {
            nextPosition.x = maxX;
            targetDirection.x = -Mathf.Abs(targetDirection.x);
            velocity.x = -Mathf.Abs(velocity.x);
        }

        // bottom boundary
        if (nextPosition.y < minY)
        {
            nextPosition.y = minY;
            targetDirection.y = Mathf.Abs(targetDirection.y);
            velocity.y = Mathf.Abs(velocity.y);
        }

        // top boundary
        if (nextPosition.y > maxY)
        {
            nextPosition.y = maxY;
            targetDirection.y = -Mathf.Abs(targetDirection.y);
            velocity.y = -Mathf.Abs(velocity.y);
        }

        transform.position = nextPosition;
    }


    public void TriggerHurt()
    {
        Debug.Log("Fish has entered the Hurt state");

        if (isHurt)
            return;

        isHurt = true;
        hurtTimer = hurtDuration;
        hurtTurnTimer = 0f;

        // choose random direction
        targetDirection = Random.insideUnitCircle.normalized;

        // immediate increase in fish speed
        maxSpeed = hurtSpeed;
    }

}
