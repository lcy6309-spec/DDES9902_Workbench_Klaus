using UnityEngine;

public class MouseGrab : MonoBehaviour
{
    private Transform grabbedObject;
    private Vector3 offset;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Grabbable")) 
                {
                    grabbedObject = hit.transform;
                    offset = grabbedObject.position - hit.point;
                }
            }
        }

        if (grabbedObject)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 target = ray.origin + ray.direction * 2f; 
            grabbedObject.position = target + offset;
        }

        if (Input.GetMouseButtonUp(0))
        {
            grabbedObject = null;
        }
    }
}