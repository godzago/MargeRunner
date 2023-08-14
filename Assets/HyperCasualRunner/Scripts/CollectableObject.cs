using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HyperCasualRunner.ScriptableObjects;
using HyperCasualRunner.CollectableEffects;
using HyperCasualRunner.GenericModifiers;
using HyperCasualRunner.Interfaces;

namespace HyperCasualRunner
{
public class CollectableObject : MonoBehaviour
{
        private bool collected = false;

        void Update()
        {
            if (collected)
            {
                Vector3 targetPos = UIJoystick.Instance.GetIconPostion(transform.position);
                if (Vector2.Distance(transform.position, targetPos) > 0.5f)
                {
                    transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 5f);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }

        public void SetCollected()
        {

            collected = true;
        }
    }
}
