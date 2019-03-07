using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SUPERLASER
{
    public class Helpers : MonoBehaviour
    {
        /// <summary>
        /// Determines if mouse pointer is over any UI Objects
        /// </summary>
        /// <returns>Results</returns>
        public static bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        /// <summary>
        /// Draws a Circle Gizmos
        /// </summary>
        /// <param name="point">Center point of circle</param>
        /// <param name="radius">Radius of circle</param>
        /// <param name="dottedLine">If circle is drawn as dotted line</param>
        public static void DrawCircleGizmos(Vector3 point, float radius, bool dottedLine = false)
        {
            float yOffset = 0.01f;
            point = new Vector3(point.x, point.y + yOffset, point.z);
            
            float theta = 0;
            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);
            Vector3 pos = point + new Vector3(x, 0, y);
            Vector3 newPos = pos;
            Vector3 lastPos = pos;

            bool drawLine = false;
            for (theta = 0.1f; theta < Mathf.PI * 2; theta += 0.1f)
            {
                x = radius * Mathf.Cos(theta);
                y = radius * Mathf.Sin(theta);
                newPos = point + new Vector3(x, 0, y);
                if (drawLine || !dottedLine)
                {
                    Gizmos.DrawLine(pos, newPos);
                }
                pos = newPos;
                drawLine = !drawLine;
            }
            Gizmos.DrawLine(pos, lastPos);
        }
    }
}

