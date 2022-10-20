using UnityEngine;

namespace GarbageMiner.Utils
{
    [ExecuteInEditMode]
    public class LookAtCamera : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private bool _isLookAtCamera;

        private void Start()
        {
            if (!_camera)
            {
                _camera = Camera.main;
            }
        }

        private void Update()
        {
            if (_isLookAtCamera)
            {
                transform.rotation = _camera.transform.rotation;
            }
            else
            {
                if (_camera)
                {
                    transform.LookAt(_camera.transform);
                }
            }
        }
    }
}

