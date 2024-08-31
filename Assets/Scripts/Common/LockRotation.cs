using UnityEngine;

namespace FreedLOW.MLAgents.Common
{
    public class LockRotation : MonoBehaviour
    {
        private void Update()
        {
            transform.rotation = Quaternion.identity;
        }
    }
}