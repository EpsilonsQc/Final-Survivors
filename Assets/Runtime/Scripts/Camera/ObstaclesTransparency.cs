using System.Collections.Generic;
using UnityEngine;

namespace Final_Survivors
{
    public class ObstaclesTransparency : MonoBehaviour
    {
        [SerializeField] Shader shader;
        IDictionary<string, Material> tempMaterials = new Dictionary<string, Material>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Environment"))
            {
                var mrEnter = other.transform.gameObject.GetComponent<MeshRenderer>();
                var mat = new Material(shader);
                mat.color = new Color(1f, 1f, 1f, 0.05f);

                if (! tempMaterials.ContainsKey(other.transform.name))
                    tempMaterials.Add(other.transform.name, mrEnter.material);

                mrEnter.material = mat;

                //Debug.Log("Hidding " + other.transform.name);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.transform.CompareTag("Environment"))
            {
                var mrEnter = other.transform.gameObject.GetComponent<MeshRenderer>();
                var mat = new Material(shader);
                mat.color = new Color(1f, 1f, 1f, 0.05f);

                if (!tempMaterials.ContainsKey(other.transform.name))
                    tempMaterials.Add(other.transform.name, mrEnter.material);

                mrEnter.material = mat;

                //Debug.Log("Hidding " + other.transform.name);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.CompareTag("Environment"))
            {
                var mrExit = other.transform.gameObject.GetComponent<MeshRenderer>();

                if (tempMaterials.ContainsKey(other.transform.name))
                    mrExit.material = tempMaterials[other.transform.name];

                //Debug.Log("Showing " + other.transform.name);
            }
        }
    }
}
