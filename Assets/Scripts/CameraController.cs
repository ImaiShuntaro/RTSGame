using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 getKeyDownPos;
    private Vector3 lastMousePos;
    private float moveSpeed = 1f;    
    private float scrolledSpeed = 3f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            getKeyDownPos = Input.mousePosition;
        }
        if (Input.GetMouseButton(1))
        {
            Vector3 diff = Input.mousePosition - getKeyDownPos;
            Camera.main.transform.position -= new Vector3(diff.x, 0, diff.y) * Time.deltaTime * moveSpeed;
            getKeyDownPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonDown(2))
        {
            lastMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 diff = Input.mousePosition - lastMousePos;
            Camera.main.transform.Rotate(Vector3.up, diff.x, Space.World);
            Camera.main.transform.Rotate(Vector3.right, -diff.y, Space.Self);
            lastMousePos = Input.mousePosition;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane xzplane = new Plane(Vector3.up, Vector3.zero);
            float distance;
            Vector3 xzMousePos;
            if (xzplane.Raycast(ray, out distance))
            {
                xzMousePos = ray.GetPoint(distance);
                Vector3 direction = xzMousePos - Camera.main.transform.position;
                transform.position += direction.normalized * scroll * scrolledSpeed;
            }            
        }
    }
}
