using System;
using System.Collections;
using UnityEngine;

namespace Core
{
    public static class MonoBehaviourExtends
    {
        public static void DelayCall(this MonoBehaviour mono, float delayTime, System.Action action)
        {
            mono.StartCoroutine(_DelayCall(delayTime, action));
        }

        public static IEnumerator WaitUntil(this MonoBehaviour mono, Func<bool> condition, Action callback)
        {
            var action = _WaitUntil(condition, callback);
            mono.StartCoroutine(action);
            return action;
        }

        private static IEnumerator _DelayCall(float delayTime, System.Action action)
        {
            yield return new WaitForSeconds(delayTime);
            action?.Invoke();
        }

        private static IEnumerator _WaitUntil(Func<bool> predicate, Action action)
        {
            while (!predicate.Invoke()) yield return null;
            action?.Invoke();
        }
    }
}
