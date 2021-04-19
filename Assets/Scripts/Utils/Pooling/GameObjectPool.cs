using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.Pooling
{
    public class GameObjectPool : MonoBehaviour
    {
        private static GameObjectPool s_Instance;
        private static Dictionary<int, Queue<PooledMonoBehaviour>> s_PooledObjects = new Dictionary<int, Queue<PooledMonoBehaviour>>(); //index + queue
            
        //singleton
        private static GameObjectPool Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindObjectOfType<GameObjectPool>(); //called only once
                    if (s_Instance == null)
                    {
                        GameObject gameObj = new GameObject("GameObjectPool"); //Instantiate нельзя
                        s_Instance = gameObj.AddComponent<GameObjectPool>();
                    }
                    s_Instance.gameObject.SetActive(false);
                }
                return s_Instance;
            }
        }

        public static TMonoBehaviour InstantiatePooled<TMonoBehaviour>(TMonoBehaviour prefab)
            where TMonoBehaviour : PooledMonoBehaviour
        {
            TMonoBehaviour instance = InstantiatePooledImpl(prefab);
            instance.transform.parent = null;
            return instance;
        }
        
        public static TMonoBehaviour InstantiatePooled<TMonoBehaviour>(TMonoBehaviour prefab, Vector3 position, Quaternion rotation)
            where TMonoBehaviour : PooledMonoBehaviour
        {
            TMonoBehaviour instance = InstantiatePooledImpl(prefab);
            Transform instanceTransform = instance.transform;
            instanceTransform.parent = null;
            instanceTransform.position = position;
            instanceTransform.rotation = rotation;
            return instance;
        }
        
        public static TMonoBehaviour InstantiatePooled<TMonoBehaviour>(TMonoBehaviour prefab, Transform parent)
            where TMonoBehaviour : PooledMonoBehaviour
        {
            TMonoBehaviour instance = InstantiatePooledImpl(prefab);
            instance.transform.parent = parent;
            return instance;
        }
        
        public static TMonoBehaviour InstantiatePooled<TMonoBehaviour>(TMonoBehaviour prefab, Vector3 position, Quaternion rotation, Transform parent)
            where TMonoBehaviour : PooledMonoBehaviour
        {
            TMonoBehaviour instance = InstantiatePooledImpl(prefab);
            Transform instanceTransform = instance.transform;
            instanceTransform.parent = parent;
            instanceTransform.position = position;
            instanceTransform.rotation = rotation;
            return instance;
        }

        private static TMonoBehaviour InstantiatePooledImpl<TMonoBehaviour>(TMonoBehaviour prefab)
            where TMonoBehaviour : PooledMonoBehaviour
        {
            int id = prefab.GetInstanceID();
            /*
             ID, уникальный для каждого объекта
             Для копии префаба InstanceID будет другой!
             */
            TMonoBehaviour instance = null;
            if (s_PooledObjects.TryGetValue(id, out Queue<PooledMonoBehaviour> queue))
            {
                if (queue.Count > 0)
                {
                    instance = queue.Peek() as TMonoBehaviour;
                    if (instance == null)
                    {
                        throw new NullReferenceException();
                    }

                    queue.Dequeue();
                }
            }

            if (instance == null)
            {
                instance = Instantiate(prefab);
                instance.SetPrefabId(id);
            }
            
            instance.AwakePooled();
            
            return instance;
        }

        public static void ReturnObjectToPool(PooledMonoBehaviour instance)
        {
            int id = instance.PrefabId;
            if (s_PooledObjects.TryGetValue(id, out Queue<PooledMonoBehaviour> queue))
            {
                queue.Enqueue(instance);
            }
            else
            {
                Queue<PooledMonoBehaviour> newQueue = new Queue<PooledMonoBehaviour>();
                newQueue.Enqueue(instance);
                s_PooledObjects[id] = newQueue;
            }

            instance.transform.parent = Instance.transform;
            //Наш объект всегда выключен, поэтому и все его дети тоже выключены
        }
    }
}