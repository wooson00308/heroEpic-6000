using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * TODO: 작업 항목을 표시하는 주석
 * FIXME: 수정이 필요한 부분을 표시하는 주석
 * HACK: 임시로 사용된 코드를 표시하는 주석
 * BUG: 버그를 설명하는 주석
 * NOTE: 추가 정보 또는 설명을 포함하는 주석
 * REVIEW: 코드 리뷰를 요청하는 주석
 * OPTIMIZE: 최적화가 필요한 부분을 표시하는 주석
 * IDEA: 아이디어 또는 개선 제안을 표시하는 주석
 * OBSOLETE: 사용되지 않는 코드를 나타내는 주석
 */
namespace Scripts.Utils
{
    public class Util
    {
        const double EPSILON = 0.0001;

        public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
        {
            T component = go.GetComponent<T>();
            if (component == null)
                component = go.AddComponent<T>();

            return component;

        }

        public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
        {
            Transform transform = FindChild<Transform>(go, name, recursive);
            if (transform == null)
                return null;

            return transform.gameObject;
        }

        //최상위 오브젝트 , 찾을 이름, 재귀적으로 전체를 찾을것인지 
        public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
        {
            if (go == null)
                return null;
            if (recursive == false)
            {
                for (int i = 0; i < go.transform.childCount; i++)
                {
                    Transform transform = go.transform.GetChild(i);
                    if (string.IsNullOrEmpty(name) || transform.name == name)
                    {
                        T component = transform.GetComponent<T>();
                        if (component != null)
                            return component;
                    }
                }
            }
            else
            {
                foreach (T component in go.GetComponentsInChildren<T>())
                {
                    if (string.IsNullOrEmpty(name) || component.name == name)
                        return component;
                }
            }
            return null;
        }

        public static T[] FindChildren<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
        {
            if (go == null)
                return null;

            List<T> result = new List<T>();
            if (recursive == false)
            {
                for (int i = 0; i < go.transform.childCount; i++)
                {
                    Transform transform = go.transform.GetChild(i);
                    if (string.IsNullOrEmpty(name) || transform.name == name)
                    {
                        T component = transform.GetComponent<T>();
                        if (component != null)
                            result.Add(component);
                    }
                }
            }
            else
            {
                foreach (T component in go.GetComponentsInChildren<T>())
                {
                    if (string.IsNullOrEmpty(name) || component.name == name)
                        result.Add(component);

                }
            }
            return result.ToArray();
        }

        public static bool IsEqual(double x, double y) // 비교 함수.
        {
            return (((x - EPSILON) < y) && (y < (x + EPSILON)));
        }

        public static string GetStringWithinSection(string str, string begin, string end)
        {

            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            string result = null;
            if (str.IndexOf(begin) > -1)
            {
                str = str.Substring(str.IndexOf(begin) + begin.Length);
                if (str.IndexOf(end) > -1) result = str.Substring(0, str.IndexOf(end));
                else result = str;
            }
            return result;
        }

        public static Vector3 WorldToCanvasPoint(Camera camera, Canvas canvas, Vector3 worldPosition)
        {
            // Vector3 result;
            Vector3 viewportPosition = camera.WorldToViewportPoint(worldPosition);

            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            var delta = canvasRect.sizeDelta;
            var result = new Vector2(
                ((viewportPosition.x * delta.x) - (delta.x * 0.5f)),
                ((viewportPosition.y * delta.y) - (delta.y * 0.5f)));


            return result;

        }
    }
}