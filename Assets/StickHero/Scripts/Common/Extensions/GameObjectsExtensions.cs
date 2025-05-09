using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CoffeeShopGame.Scripts.Common.Extensions
{
    public static class GameObjectsExtensions 
    {
        public static void FindAndSetInNotNull<T>(this MonoBehaviour monoBehaviour, ref T component) where T : Behaviour {
            if (component) return;
            component = monoBehaviour.GetComponent<T>();
            if (component) return;
            component = monoBehaviour.GetComponentInChildren<T>();
        }
    
        public static T CreateNewGameObject<T>( string name , bool dontDestroy = false)where T : MonoBehaviour
        {
            var gameObject = new GameObject(name);
            if (dontDestroy)
            {
                GameObject.DontDestroyOnLoad(gameObject);
            }
            
            return gameObject.AddComponent<T>();
        }

        public static Transform CreateGameobject(this Transform monoBehaviour,
            Transform root)
        {
            var tr = Object.Instantiate(monoBehaviour);
            tr.gameObject.SetActive(true);
            tr.SetParent(root);
            tr.localScale = new Vector3(1, 1, 1);
            tr.localPosition = Vector3.zero;
            return tr;
        }
        
        public static T CreateGameobject<T>(this T monoBehaviour ,
            Transform root) where T : MonoBehaviour
        {
            var tr = Object.Instantiate(monoBehaviour);
            var component = tr.GetComponent<T>();
            if (component != null)
            {
                component.gameObject.SetActive(true);
                component.transform.SetParent(root);
                component.transform.localScale = new Vector3(1, 1, 1);
                component.transform.localPosition = Vector3.zero; ;
            }
            return component;
        }

        public static T CreateGameobject<T>(
             Transform prefab, Transform root) where T : MonoBehaviour
        {
            var tr = Object.Instantiate(prefab);

            var component = tr.GetComponent<T>();
            if (component != null)
            {
                component.gameObject.SetActive(true);
                component.transform.SetParent(root);
                component.transform.localScale = new Vector3(1, 1, 1);
                component.transform.localPosition = Vector3.zero; ;
            }
            return component;
        }

        public static void DestroyAllChildren(this Transform root , bool isImmediate = false)
        {
            if (root != null)
            {
                var allChildren = root.Cast<Transform>().ToArray();
                for (int i = 0; i < allChildren.Length; i++)
                {
                    if (isImmediate)
                    {
                        if(allChildren[i].gameObject)
                            Object.DestroyImmediate(allChildren[i].gameObject);
                    }
                    else
                    {
                        if (allChildren[i].gameObject)
                        {
                            allChildren[i].gameObject.SetActive(false);
                            Object.Destroy(allChildren[i].gameObject);
                        }
                    }
                }
            }
        }

        public static void DestroyAllChildrenByType<T>(this Transform root, bool includeInactive = true,  bool isImmediate = false)
            where T:MonoBehaviour
        {
            if (root != null)
            {
                var allChildrens = root.GetComponentsInChildren<T>(includeInactive);
                for (int i = 0; i < allChildrens.Length; i++)
                {
                    if (isImmediate)
                    {
                        allChildrens[i].gameObject.SetActive(false);
                        Object.Destroy(allChildrens[i].gameObject);
                    }
                    else
                    {
                        Object.DestroyImmediate(allChildrens[i].gameObject);
                    }
                }
            }
        }
        public static void Hide(this GameObject root)
        {
            if (root)
            {
                root.gameObject.SetActive(false);
            }
        }

        public static void Show(this GameObject root)
        {
            if (root)
            {
                root.gameObject.SetActive(true);
            }
        }

        public static void Hide(this Component root)
        {
            if (root)
            {
                root.gameObject.SetActive(false);
            }
        }

        public static void Show(this Component root)
        {
            if (root)
            {
                root.gameObject.SetActive(true);
            }
        }
        
        public static void HideAll(this IEnumerable<GameObject> objects)
        {
            if (objects != null)
            {
                foreach (var gameObject in objects)
                {
                    gameObject.Hide();
                }
            }
        }

        public static void ShowAll(this IEnumerable<GameObject> objects)
        {
            if (objects != null)
            {
                foreach (var gameObject in objects)
                {
                    gameObject.Show();
                }
            }
        }
        
        public static bool IsShow(this Component root)
        {
            if (root)
            {
                var gameObject = root.gameObject;
                return gameObject.activeSelf && gameObject.activeInHierarchy;
            }

            return false;
        }

        public static void Reset(this Transform root)
        {
            if (root != null)
            {
                root.transform.localScale = new Vector3(1, 1, 1);
                root.transform.localPosition = new Vector3(0, 0, 0);
                root.transform.localRotation = Quaternion.identity;
            }
        }
        
        
        
        public static Transform CreateGameobject(Transform prefab, Transform root , int? layer = null) 
        {
            var tr = Object.Instantiate(prefab);
            tr.gameObject.SetActive(true);
            tr.transform.SetParent(root);
            tr.transform.localScale = new Vector3(1, 1, 1);
            tr.transform.localPosition = Vector3.zero; ;

            if (layer != null)
            {
                SetLayerRecursively(tr.gameObject , layer.Value);
            }
            return tr;
        }
        public static void SetLayerRecursively(this GameObject obj, int layer)
        {
            obj.layer = layer;

            foreach (Transform child in obj.transform)
            {
                child.gameObject.SetLayerRecursively(layer);
            }
        }
    }
}
