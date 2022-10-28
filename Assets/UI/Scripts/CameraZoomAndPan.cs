using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntFarm
{
    public class CameraZoomAndPan : MonoBehaviour
    {

        public float zoomOutMin = 50f;
        public float zoomOutMax = 100f;
        private Vector3 touchStart;
        private float heightMin;
        private float widthMin;
        private Camera cam;
        private float zoomL;//линейный
        private float zoom;//синусоидный

        void Start()
        {
            cam = Camera.main;

            heightMin = 2f * zoomOutMin*(1f-0.09f);
            widthMin = 2f * zoomOutMin * cam.aspect;
            Camera.main.orthographicSize = zoomOutMin;
        }

        Vector3 Clamp(Vector3 pos)//задаем границы чтоб камера не уехала за поле
        {
            float z = Mathf.Clamp(pos.z, -heightMin* (1f - 0.09f) / 2f*(1f - zoom) , (heightMin*(1f + 0.09f) / 2f) *(1f - zoom) );
            float x = Mathf.Clamp(pos.x, -widthMin / 2f * (1f - zoom) , widthMin / 2f * (1f - zoom) );
            return new Vector3(x, 0, z);
        }

        void Update()
        {


            if (Input.mouseScrollDelta.y != 0)//для мышки зум
            {
                zoomL -= Input.mouseScrollDelta.y * 0.06f;
                zoomL = Mathf.Clamp(zoomL, 0f, 1f);
                zoom = -Mathf.Cos(zoomL * 3.14f) * 0.5f + 0.5f;
                cam.orthographicSize = ExtensionMethods.Remap(zoom, 0f, 1f, zoomOutMin, zoomOutMax);
                cam.transform.localPosition = Clamp(cam.transform.localPosition);

            }




            if (Input.GetMouseButtonDown(0))
            {
                touchStart = cam.ScreenToWorldPoint(Input.mousePosition);//позиция первого косания
            }

            if (Input.touchCount == 2)//зум камеры в телефоне
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;//насколько растояние между двумя пальцами изменилось

                zoomL -= difference * 0.003f;
                zoomL = Mathf.Clamp(zoomL, 0f, 1f);
                zoom = -Mathf.Cos(zoomL * 3.14f) * 0.5f + 0.5f;

                cam.orthographicSize = ExtensionMethods.Remap(zoom, 0f, 1f, zoomOutMin, zoomOutMax);
                cam.transform.localPosition = Clamp(cam.transform.localPosition);
            }
            else if (Input.GetMouseButton(0))//перемещение камеры
            {
                Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                cam.transform.position += direction;
                cam.transform.localPosition = Clamp(cam.transform.localPosition);


            }



        }
    }
}
