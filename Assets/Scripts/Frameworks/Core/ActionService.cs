using Core;
using System;
using System.Collections.Generic;

namespace Core
{
    public class ListenerPriority
    {
        public const int LOWEST = 0;
        public const int LOW = 250;
        public const int MEDIUM = 500;
        public const int HIGH = 750;
        public const int HIGHEST = 1000;
    }
    public static class ActionService
    {
        public static void Sub<T>(Action<T> listener, int priority = ListenerPriority.MEDIUM) where T : class, IAction
            => RegisterHandler(typeof(T), listener, priority);

        public static void Unsub<T>(Action<T> listener) where T : class, IAction
            => UnregisterHandler(typeof(T), listener);

        public static void Dispatch<T>(params object[] datas) where T : class, IAction
            => DispatchAction<T>(datas);

        #region Black Box
        static readonly Dictionary<Type, SortedList<int, Delegate>> _delegates = new Dictionary<Type, SortedList<int, Delegate>>();
        static readonly Dictionary<Type, IAction> _actions = new Dictionary<Type, IAction>();
        static readonly ListenerPriorityComparer _comparer = new ListenerPriorityComparer();
        protected class ListenerPriorityComparer : IComparer<int>
        {
            public int Compare(int x, int y) => y.CompareTo(x);
        }

        static void RegisterHandler<T>(Type actionType, Action<T> listener, int priority) where T : class, IAction
        {
            if (!_delegates.ContainsKey(actionType))
                _delegates.Add(actionType, new SortedList<int, Delegate>());

            if (!_delegates[actionType].ContainsKey(priority))
                _delegates[actionType].Add(priority, listener);
            else
                _delegates[actionType][priority] = (Action<T>)_delegates[actionType][priority] + listener;
        }
        static void UnregisterHandler<T>(Type type, Action<T> listener)
           where T : class, IAction
        {
            SortedList<int, Delegate> delegates;

            if (_delegates.TryGetValue(type, out delegates))
            {
                int priorityLen = delegates.Count;
                int[] priorityCopies = new int[priorityLen];
                delegates.Keys.CopyTo(priorityCopies, 0);

                for (int i = 0; i < priorityLen; i++)
                {
                    int priority = priorityCopies[i];
                    delegates[priority] = (Action<T>)delegates[priority] - listener;

                    if (delegates[priority] == null)
                    {
                        delegates.Remove(priority);
                    }
                }

                if (delegates.Count == 0) _delegates.Remove(type);
            }
        }
        static void DispatchAction<T>(object[] datas) where T : class, IAction
        {
            IAction actionTrigger;
            if (!_actions.TryGetValue(typeof(T), out actionTrigger))
            {
                actionTrigger = (IAction)Activator.CreateInstance(typeof(T));
                _actions.Add(typeof(T), actionTrigger);
            }
            if (datas.Length > 0) actionTrigger.SetData(datas);
            DispatchListener(actionTrigger as T);
        }
        static void DispatchListener<T>(T actionTrigger) where T : class, IAction
        {
            Type key = typeof(T);
            SortedList<int, Delegate> delegates;

            if (_delegates.TryGetValue(key, out delegates))
            {
                int priorityLen = delegates.Keys.Count;
                int[] keys = new int[priorityLen];
                delegates.Keys.CopyTo(keys, 0);
                for (int j = 0; j < priorityLen; j++)
                {
                    InvokeListener(actionTrigger, delegates[keys[j]] as Action<T>);
                }
            }
        }
        static void InvokeListener<T>(T actionTrigger, Action<T> listener)
        {
            if (listener == null) throw new Exception("listener do not existed");
            listener.Invoke(actionTrigger);
        }
    }
    #endregion Black Box
}
