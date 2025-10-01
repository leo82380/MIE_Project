using System;
using UnityEngine;

namespace MIE.Runtime.BoardSystem.Slot
{
    public class MoveSlot : BaseSlot
    {
        [Header("Movement Settings")]
        [SerializeField] private Vector2 moveDirection = Vector2.right;
        [SerializeField] private float moveSpeed = 100f;
        
        [Header("Infinite Scroll Bounds")]
        [SerializeField] private float leftBound = -1000f;
        [SerializeField] private float rightBound = 1000f;
        [SerializeField] private float topBound = 600f;
        [SerializeField] private float bottomBound = -600f;


        private void Update()
        {
            Move();
            CheckBoundsAndWrap();
        }

        private void Move()
        {
            Vector3 newPosition = transform.localPosition + (Vector3)moveDirection * moveSpeed * Time.deltaTime;
            transform.localPosition = newPosition;
        }

        private void CheckBoundsAndWrap()
        {
            Vector3 currentPos = transform.localPosition;
            bool needsWrap = false;

            if (moveDirection.x > 0 && currentPos.x > rightBound)
            {
                currentPos.x = leftBound;
                needsWrap = true;
            }
            else if (moveDirection.x < 0 && currentPos.x < leftBound)
            {
                currentPos.x = rightBound;
                needsWrap = true;
            }

            if (moveDirection.y > 0 && currentPos.y > topBound)
            {
                currentPos.y = bottomBound;
                needsWrap = true;
            }
            else if (moveDirection.y < 0 && currentPos.y < bottomBound)
            {
                currentPos.y = topBound;
                needsWrap = true;
            }

            if (needsWrap)
            {
                transform.localPosition = currentPos;
            }
        }
    }
}