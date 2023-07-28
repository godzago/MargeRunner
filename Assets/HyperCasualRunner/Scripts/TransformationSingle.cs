using HyperCasualRunner.Modifiables;
using UnityEngine;

namespace HyperCasualRunner
{
    /// <summary>
    /// Transforms TransformationModifiable one by one, just like cup filling coffee machines in Coffee Stack game 
    /// </summary>
    public class TransformationSingle : MonoBehaviour
    {
        [SerializeField] int _levelToSet = 1;

        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out TransformationModifiable modifiable))
            {
                modifiable.SetLevel(_levelToSet);
            }
        }
    }
}