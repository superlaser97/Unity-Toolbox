using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SUPERLASER
{
    // *** Extensions *** //
    // Enum
    // Vector3
    // Transform
    // GameObjectExtensions
    // Rigidbody
    // Color
    // RectTransform

    /// <summary>
    /// Extension methods for Enum
    /// </summary>
    #region EnumExtensions class
    public static class EnumExtensions
    {
        /// <summary>
        /// Converts string value to enum
        /// </summary>
        /// <param name="value">String value of the enum.</param>
        /// <returns>Enum.</returns>
        public static T ToEnum<T>(this string value) where T : struct, IConvertible
        {
            if (string.IsNullOrEmpty(value))
            {
                return default(T);
            }

            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
    #endregion

    /// <summary>
    /// Extension methods for Vector3
    /// </summary>
    #region Vector3Extensions class
    public static class Vector3Extensions
    {
        /// <summary>
        /// Finds the position closest to the given one.
        /// </summary>
        /// <param name="position">World position.</param>
        /// <param name="otherPositions">Other world positions.</param>
        /// <returns>Closest position.</returns>
        public static Vector3 GetClosest(this Vector3 position, IEnumerable<Vector3> otherPositions)
        {
            var closest = Vector3.zero;
            var shortestDistance = Mathf.Infinity;

            foreach (var otherPosition in otherPositions)
            {
                var distance = (position - otherPosition).sqrMagnitude;

                if (distance < shortestDistance)
                {
                    closest = otherPosition;
                    shortestDistance = distance;
                }
            }

            return closest;
        }
    }
    #endregion

    /// <summary>
    /// Extension methods for UnityEngine.Transform.
    /// </summary>
    #region TransformExtensions class
    public static class TransformExtensions
    {
        /// <summary>
        /// Returns a list of all gameobjects that are children of this Transform
        /// </summary>
        /// <param name="t">The transform to search</param>
        /// <returns>A List of all gameobjects that are children of this transform</returns>
        public static List<GameObject> GetChildrensAsGameObjects(this Transform t)
        {
            return t.Cast<Transform>().Select(n => n.gameObject).ToList();
        }

        /// <summary>
        /// Makes the given game objects children of the transform.
        /// </summary>
        /// <param name="transform">Parent transform.</param>
        /// <param name="children">Game objects to make children.</param>
        public static void AddChildren(this Transform transform, GameObject[] children)
        {
            Array.ForEach(children, child => child.transform.parent = transform);
        }

        /// <summary>
        /// Makes the game objects of given components children of the transform.
        /// </summary>
        /// <param name="transform">Parent transform.</param>
        /// <param name="children">Components of game objects to make children.</param>
        public static void AddChildren(this Transform transform, Component[] children)
        {
            Array.ForEach(children, child => child.transform.parent = transform);
        }

        /// <summary>
        /// Sets the position of a transform's children to zero.
        /// </summary>
        /// <param name="transform">Parent transform.</param>
        /// <param name="recursive">Also reset ancestor positions?</param>
        public static void ResetChildPositions(this Transform transform, bool recursive = false)
        {
            foreach (Transform child in transform)
            {
                child.position = Vector3.zero;

                if (recursive)
                {
                    child.ResetChildPositions(recursive);
                }
            }
        }

        /// <summary>
        /// Sets the layer of the transform's children.
        /// </summary>
        /// <param name="transform">Parent transform.</param>
        /// <param name="layerName">Name of layer.</param>
        /// <param name="recursive">Also set ancestor layers?</param>
        public static void SetChildLayers(this Transform transform, string layerName, bool recursive = false)
        {
            var layer = LayerMask.NameToLayer(layerName);
            SetChildLayersHelper(transform, layer, recursive);
        }

        static void SetChildLayersHelper(Transform transform, int layer, bool recursive)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.layer = layer;

                if (recursive)
                {
                    SetChildLayersHelper(child, layer, recursive);
                }
            }
        }

        /// <summary>
        /// Sets the x component of the transform's position.
        /// </summary>
        /// <param name="x">Value of x.</param>
        public static void SetX(this Transform transform, float x)
        {
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }

        /// <summary>
        /// Sets the y component of the transform's position.
        /// </summary>
        /// <param name="y">Value of y.</param>
        public static void SetY(this Transform transform, float y)
        {
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }

        /// <summary>
        /// Sets the z component of the transform's position.
        /// </summary>
        /// <param name="z">Value of z.</param>
        public static void SetZ(this Transform transform, float z)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, z);
        }
    }
    #endregion

    /// <summary>
    /// Extension methods for UnityEngine.GameObject.
    /// </summary>
    #region GameObjectExtensions class
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Gets a component attached to the given game object.
        /// If one isn't found, a new one is attached and returned.
        /// </summary>
        /// <param name="gameObject">Game object.</param>
        /// <returns>Previously or newly attached component.</returns>
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();
        }

        /// <summary>
        /// Checks whether a game object has a component of type T attached.
        /// </summary>
        /// <param name="gameObject">Game object.</param>
        /// <returns>True when component is attached.</returns>
        public static bool HasComponent<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.GetComponent<T>() != null;
        }

        /// <summary>
        /// Destroys the passed gameobject as long as it exists
        /// </summary>
        /// <param name="g">The gameObject to be destroyed</param>
        public static void SafeDestroy(this GameObject g)
        {
            if (g != null)
            {
                UnityEngine.Object.Destroy(g);
            }
        }

        // Set the layer of this GameObject and all of its children.
        public static void SetLayerRecursively(this GameObject gameObject, int layer)
        {
            gameObject.layer = layer;
            foreach (Transform t in gameObject.transform)
                t.gameObject.SetLayerRecursively(layer);
        }

        // Set enable / disable all child components in gameobject
        public static void SetAllComponentsEnableRecursively<T>(this GameObject gameObject, bool enabled) where T : MonoBehaviour
        {
            foreach (Transform t in gameObject.transform)
            {
                if (t.gameObject.HasComponent<T>())
                    t.GetComponent<T>().enabled = enabled;
            }
        }
    }
    #endregion

    /// <summary>
    /// Extension methods for UnityEngine.Rigidbody.
    /// </summary>
    #region RigidbodyExtensions class
    public static class RigidbodyExtensions
    {
        /// <summary>
        /// Changes the direction of a rigidbody without changing its speed.
        /// </summary>
        /// <param name="rigidbody">Rigidbody.</param>
        /// <param name="direction">New direction.</param>
        public static void ChangeDirection(this Rigidbody rigidbody, Vector3 direction)
        {
            rigidbody.velocity = direction * rigidbody.velocity.magnitude;
        }
    }
    #endregion

    /// <summary>
    /// System.Collections.Generic.IList extension methods
    /// </summary>
    #region IListExtensions class
    public static class IListExtensions
    {
        /// <summary>
        /// Randomizes the values within a list or array. This is an O(n) operation.
        /// </summary>
        /// <typeparam name="T">The type of values within the list</typeparam>
        public static void Shuffle<T>(this IList<T> list)
        {
            System.Random random = new System.Random();

            /* Fisher-Yates Shuffle algorithm.
			 * https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle#The_modern_algorithm)
			 * Basically, we're iterating through the list in reverse order,
			 * potentially swapping each value with one closer to the front.
			 */
            int index = list.Count;
            while (index > 1)
            {
                int randomIndex = random.Next(0, index);
                index--;

                /* Swap the values at "index" and "randomIndex".
				 * Note that the swap when index == randomIndex results in no change.
				 * This is intended functionality.
				 * We could do an "if" check and skip the swap.
				 * However, we'll actually get better performance without that
				 * because that case occurs far less frequently and we
				 * won't have to waste the comparison cycles at each iteration.
				 */
                T temp = list[randomIndex];
                list[randomIndex] = list[index];
                list[index] = temp;
            }
        }
    }
    #endregion

    /// <summary>
    /// System.Collections.Generic.ICollection extension methods
    /// </summary>
    #region IListExtensions class
    public static class ICollectionExtensions
    {
        public static void AddRange<T, S>(this ICollection<T> list, params S[] values) where S : T
        {
            foreach (S value in values)
                list.Add(value);
        }
    }
    #endregion

    /// <summary>
    /// General extension methods
    /// </summary>
    #region GeneralExtensions class
    public static class GeneralExtensions
    {

        // Checking if source contains all element in the list
        public static bool ContainsAllOf<T>(this T source, params T[] list)
        {
            if (null == source) throw new ArgumentNullException("source");
            return list.Contains(source);
        }

        // Converting one object to another
        public static T To<T>(this IConvertible obj)
        {
            return (T)Convert.ChangeType(obj, typeof(T));
        }

        // Convert string to unitycolor
        public static Color ToUnityColor(this string htmlColor)
        {
            Color tempColor;
            ColorUtility.TryParseHtmlString(htmlColor, out tempColor);
            return tempColor;
        }

        //Log Exception straight to somewhere else
        public static void LogToDebugConsole(this Exception obj)
        {
            // to somewhere else
        }
    }
    #endregion

    /// <summary>
    /// String extension methods
    /// </summary>
    #region StringExtension methods
    public static class StringExtensions
    {
        // Convert string to bytes
        public static byte[] ToByteArray(this string HexString)
        {
            int NumberChars = HexString.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(HexString.Substring(i, 2), 16);
                Debug.Log(bytes[i / 2]);
            }
            return bytes;
        }

        // Convert the string to Pascal case.
        public static string ToPascalCase(this string the_string)
        {
            // If there are 0 or 1 characters, just return the string.
            if (the_string == null) return the_string;
            if (the_string.Length < 2) return the_string.ToUpper();

            // Split the string into words.
            string[] words = the_string.Split(
                new char[] { },
                StringSplitOptions.RemoveEmptyEntries);

            // Combine the words.
            string result = "";
            foreach (string word in words)
            {
                result +=
                    word.Substring(0, 1).ToUpper() +
                    word.Substring(1);
            }

            return result;
        }

        // Convert the string to camel case.
        public static string ToCamelCase(this string the_string)
        {
            // If there are 0 or 1 characters, just return the string.
            if (the_string == null || the_string.Length < 2)
                return the_string;

            // Split the string into words.
            string[] words = the_string.Split(
                new char[] { },
                StringSplitOptions.RemoveEmptyEntries);

            // Combine the words.
            string result = words[0].ToLower();
            for (int i = 1; i < words.Length; i++)
            {
                result +=
                    words[i].Substring(0, 1).ToUpper() +
                    words[i].Substring(1);
            }

            return result;
        }

        // Capitalize the first character and add a space before
        // each capitalized letter (except the first character).
        public static string ToProperCase(this string the_string)
        {
            // If there are 0 or 1 characters, just return the string.
            if (the_string == null) return the_string;
            if (the_string.Length < 2) return the_string.ToUpper();

            // Start with the first character.
            string result = the_string.Substring(0, 1).ToUpper();

            // Add the remaining characters.
            for (int i = 1; i < the_string.Length; i++)
            {
                if (char.IsUpper(the_string[i])) result += " ";
                result += the_string[i];
            }

            return result;
        }
    }
    #endregion

    /// <summary>
    /// Color extension methods
    /// </summary>
    #region ColorExtensions class
    public static class ColorExtensions
    {
        public static Color SetAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }
    }
    #endregion

    /// <summary>
    /// Color extension methods
    /// </summary>
    #region RectTransformExtensions class
    public static class RectTransformExtensions
    {
        public static void AnchorToCorners(this RectTransform transform)
        {
            if (transform == null)
                throw new ArgumentNullException("transform");

            if (transform.parent == null)
                return;

            var parent = transform.parent.GetComponent<RectTransform>();

            Vector2 newAnchorsMin = new Vector2(transform.anchorMin.x + transform.offsetMin.x / parent.rect.width,
                              transform.anchorMin.y + transform.offsetMin.y / parent.rect.height);

            Vector2 newAnchorsMax = new Vector2(transform.anchorMax.x + transform.offsetMax.x / parent.rect.width,
                              transform.anchorMax.y + transform.offsetMax.y / parent.rect.height);

            transform.anchorMin = newAnchorsMin;
            transform.anchorMax = newAnchorsMax;
            transform.offsetMin = transform.offsetMax = new Vector2(0, 0);
        }

        public static void SetPivotAndAnchors(this RectTransform trans, Vector2 aVec)
        {
            trans.pivot = aVec;
            trans.anchorMin = aVec;
            trans.anchorMax = aVec;
        }

        public static Vector2 GetSize(this RectTransform trans)
        {
            string a = "";
            return trans.rect.size;
        }

        public static float GetWidth(this RectTransform trans)
        {
            return trans.rect.width;
        }

        public static float GetHeight(this RectTransform trans)
        {
            return trans.rect.height;
        }

        public static void SetSize(this RectTransform trans, Vector2 newSize)
        {
            Vector2 oldSize = trans.rect.size;
            Vector2 deltaSize = newSize - oldSize;
            trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
            trans.offsetMax = trans.offsetMax + new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
        }

        public static void SetWidth(this RectTransform trans, float newSize)
        {
            SetSize(trans, new Vector2(newSize, trans.rect.size.y));
        }

        public static void SetHeight(this RectTransform trans, float newSize)
        {
            SetSize(trans, new Vector2(trans.rect.size.x, newSize));
        }

        public static void SetBottomLeftPosition(this RectTransform trans, Vector2 newPos)
        {
            trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
        }

        public static void SetTopLeftPosition(this RectTransform trans, Vector2 newPos)
        {
            trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
        }

        public static void SetBottomRightPosition(this RectTransform trans, Vector2 newPos)
        {
            trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
        }

        public static void SetRightTopPosition(this RectTransform trans, Vector2 newPos)
        {
            trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
        }
    }
    #endregion
}


