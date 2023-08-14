using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCasualRunner
{
    public class CameraFollower : MonoBehaviour
    {
        public Transform target;
        public Vector2 offset = new Vector2(-11.0f, 10.0f);
        public float lerpSpeed = 0.1f;
        public float fixedXPosition;   

        private void LateUpdate()
        {
            if (target == null)
                return;

            Vector3 targetPosition = new Vector3(fixedXPosition, target.position.y + offset.y, target.position.z + offset.x);
            transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed);
        }


        private void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
}
