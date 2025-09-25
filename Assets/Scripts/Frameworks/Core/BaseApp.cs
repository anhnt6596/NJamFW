using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public abstract class BaseApp : MonoBehaviour
    {
        #region Singleton
        public static BaseApp Instance;
        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                ConfigApp();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        #endregion
        public bool IsInitialized { get; protected set; }

        // call this function at start
        public void LoadGame()
        {
            SetAllManagers();
            InitAllManagers();
            StartAllManagers();
            StartApp();
            IsInitialized = true;
        }

        Dictionary<Type, IManager> _managers = new Dictionary<Type, IManager>();

        public T Get<T>() where T : IManager
        {
            if (_managers.ContainsKey(typeof(T)))
                return (T)_managers[typeof(T)];
            return default;
        }

        public IManager Get(Type type)
        {
            if (_managers.ContainsKey(type))
                return _managers[type];
            return default;
        }

        protected T AddManager<T>(T manager) where T : class, IManager
        {
            print($"Set Manager {typeof(T)}");
            if (_managers.ContainsKey(typeof(T)))
            {
                RemoveManager<T>();
                _managers[typeof(T)] = manager;
            }
            _managers.Add(typeof(T), manager);
            return manager;
        }

        protected T AddManager<T>() where T : class, IManager
        {
            T instance = (T)Activator.CreateInstance(typeof(T));
            AddManager(instance);
            return instance;
        }

        protected void RemoveManager<T>()
        {
            if (_managers.ContainsKey(typeof(T)))
            {
                Debug.Log($"Cleanup {nameof(T)}");
                _managers[typeof(T)].Cleanup();
                _managers.Remove(typeof(T));
            }
        }

        private void InitAllManagers()
        {
            foreach (var service in _managers)
            {
                service.Value.Init();
            }
        }

        private void StartAllManagers()
        {
            foreach (var service in _managers)
            {
                service.Value.StartUp();
            }
        }

        protected abstract void ConfigApp();
        protected abstract void SetAllManagers();
        protected abstract void StartApp();
    }
}