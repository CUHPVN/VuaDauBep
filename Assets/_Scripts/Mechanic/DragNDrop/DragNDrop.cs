using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.Mechanic.DragNDrop
{
    public class DragNDrop : MonoBehaviour
    {
        private Transform dndTransform;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100f))
                {
                    // Kiểm tra xem GameObject có component implement IDragNDrop không
                    IDragNDrop dragNDrop = hit.collider.GetComponent<IDragNDrop>();
                    if (dragNDrop != null)
                    {
                        // Xử lý logic khi tìm thấy object có IDragNDrop
                    }
                    else
                    {
                        dndTransform = hit.transform;
                        dragNDrop.State = DragNDropState.Drag;
                    }
                }
            }

        }
    }
}