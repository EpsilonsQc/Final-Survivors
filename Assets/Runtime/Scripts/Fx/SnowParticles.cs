using UnityEngine;

namespace Final_Survivors.Fx
{
    public class SnowParticles : MonoBehaviour
    {
        Transform camTransform;

        void Awake()
        {
            camTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }

        void Update()
        {
            transform.position = camTransform.position;
        }
    }
}
