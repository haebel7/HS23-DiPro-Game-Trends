using System.Collections;
using System.Linq;
using UnityEngine;
using Friedforfun.ContextSteering.Utilities;

namespace Friedforfun.ContextSteering.Demo
{
    /// <summary>
    /// Common functionality between demo agents, registering to tag cache, and handling collision indicators.
    /// </summary>
    [SelectionBase]
    public class AgentCommon : MonoBehaviour
    {
        [SerializeField] public string DemoID = null;

        private bool blockCollision = true;

        private void Start()
        {
            StartCoroutine(collisionDelay());
        }

        private void Awake()
        {
            TagCache.Register(gameObject);
        }

        private void OnDisable()
        {
            TagCache.DeRegister(gameObject);
        }


        private void OnCollisionEnter(Collision collision)
        {
            if (blockCollision)
                return;

            blockCollision = true;
            StartCoroutine(collisionDelay());

            if (collision.gameObject.tag != "Floor" && collision.gameObject.tag != "TargetPlate")
            {
                //Debug.Log($"Collided with: {collision.gameObject.name}");
            }

        }

        IEnumerator collisionDelay()
        {
            yield return new WaitForSeconds(0.5f);
            blockCollision = false;
        }

    }

}
