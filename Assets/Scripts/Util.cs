using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * TODO: �۾� �׸��� ǥ���ϴ� �ּ�
 * FIXME: ������ �ʿ��� �κ��� ǥ���ϴ� �ּ�
 * HACK: �ӽ÷� ���� �ڵ带 ǥ���ϴ� �ּ�
 * BUG: ���׸� �����ϴ� �ּ�
 * NOTE: �߰� ���� �Ǵ� ������ �����ϴ� �ּ�
 * REVIEW: �ڵ� ���並 ��û�ϴ� �ּ�
 * OPTIMIZE: ����ȭ�� �ʿ��� �κ��� ǥ���ϴ� �ּ�
 * IDEA: ���̵�� �Ǵ� ���� ������ ǥ���ϴ� �ּ�
 * OBSOLETE: ������ �ʴ� �ڵ带 ��Ÿ���� �ּ�
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

        //�ֻ��� ������Ʈ , ã�� �̸�, ��������� ��ü�� ã�������� 
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

        public static bool IsEqual(double x, double y) // �� �Լ�.
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