using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class Movement2D : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody2D rigidBody2D;

    [Header("Settings")]
    [SerializeField] float playerSpeed = 10;
    [SerializeField] bool gravityEnabled = false;
    [Range(0, .3f)] [SerializeField] float smoothing = .05f;

    [Header("Variables")]
    [SerializeField] [ReadOnly] Vector2 m_direction = Vector2.zero;
    [SerializeField] [ReadOnly] Vector2 m_velocityTarget = Vector2.zero;
    [SerializeField] [ReadOnly] Vector2 m_velocityCurrent = Vector2.zero;
    [SerializeField] [ReadOnly] Vector2 m_velocityDelta = Vector2.zero;

    public Vector2 Direction => m_direction;
    public Vector2 VelocityTarget => m_velocityTarget;
    public Vector2 VelocityCurrent => m_velocityCurrent;

    public virtual void SetMovementTarget(Vector2 playerDirection)
    {
        m_direction = playerDirection.normalized;
        m_velocityTarget = m_direction * playerSpeed * Time.fixedDeltaTime;
    }

    public virtual void Stop()
    {
        m_velocityTarget = Vector2.zero;
        rigidBody2D.velocity = Vector2.zero;
    }

    public virtual void Move()
    {
        //m_velocityTarget.y = rigidBody2D.velocity.y;
        m_velocityDelta = Vector2.SmoothDamp(rigidBody2D.velocity, m_velocityTarget, ref m_velocityCurrent, smoothing) - rigidBody2D.velocity;

        rigidBody2D.velocity += m_velocityDelta;
    }
}
