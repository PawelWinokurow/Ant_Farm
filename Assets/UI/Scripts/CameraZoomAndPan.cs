using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntFarm
{
    public class CameraZoomAndPan : MonoBehaviour
    {
        private Vector3 touchStart;
        private Vector3 camPos;
        private Vector3 targetPos;
        public float zoomOutMin=50f;
        public float zoomOutMax=100f;
        private float heightMin;
        private float widthMin;
        private Camera cam;

        private float zoom;
        private float zoomPrev;
        private float zm;
        // Start is called before the first frame update
        void Start()
        {
            cam = Camera.main;

            heightMin = 2f * zoomOutMin;
            widthMin = heightMin * cam.aspect;
 
        }

        Vector3 Clamp(Vector3 pos)
        {
            float z = Mathf.Clamp(pos.z, -heightMin / 2f * (1f - zoom), heightMin / 2f * (1f - zoom));
            float x = Mathf.Clamp(pos.x, -widthMin / 2f * (1f - zoom), widthMin / 2f * (1f - zoom));
            return new Vector3(x, 0, z);
        }
        void Update()
        {


            if (Input.mouseScrollDelta.y != 0)
            {
                zoom -= Input.mouseScrollDelta.y * 0.1f;
                zoom = Mathf.Clamp(zoom, 0f, 1f);
                Camera.main.orthographicSize = ExtensionMethods.Remap(zoom, 0f, 1f, zoomOutMin, zoomOutMax);
                cam.transform.localPosition = Clamp(cam.transform.localPosition);
               
            }
 



            if (Input.GetMouseButtonDown(0))
            {
                touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;

                Zoom(difference * 0.01f);
            }
            else if (Input.GetMouseButton(0))
            {
                Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Camera.main.transform.position += direction;
                float z = Mathf.Clamp(cam.transform.localPosition.z, -heightMin/2f * (1f - zoom), heightMin / 2f * (1f - zoom));
                float x = Mathf.Clamp(cam.transform.localPosition.x, -widthMin/2f * (1f - zoom), widthMin / 2f * (1f - zoom));
                cam.transform.localPosition = new Vector3(x, 0, z);
            }
     
        }
        void Zoom(float increament)
        {
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increament, zoomOutMin, zoomOutMax);
        }
    }
}
