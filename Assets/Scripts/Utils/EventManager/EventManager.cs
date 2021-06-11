using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils 
{
    public class EventManager
    {
        private static EventManager _instance;
        
        private readonly Dictionary<string, List<Handler>> _handlers = new Dictionary<string, List<Handler>>();
        
        private readonly Dictionary<string, Func<object, object>> _requestHandler = new Dictionary<string, Func<object, object>>();

        private readonly object _locker = new object();
        
        private EventManager()
        {
            
        }

        public static EventManager GetInstance()
        {
            if (_instance == null)
                _instance = new EventManager();
            
            return _instance;
        }

        public void AddHandler(string eventName, Action<object> callback, int priority = Priority.Low, bool autoDestroy = false)
        {
            lock (_locker)
            {
                if(!_handlers.ContainsKey(eventName))
                    _handlers[eventName] = new List<Handler>();

                var handler = new Handler {Method = callback, Priority = priority, AutoDestroy = autoDestroy};

                _handlers[eventName].Add(handler);
                _handlers[eventName].Sort((a, b) => a.Priority.CompareTo(b.Priority));
            }
        }

        public void RemoveHandler(string eventName, Action<object> callback)
        {
            lock (_locker)
            {
                if (_handlers.ContainsKey(eventName))
                {
                    for (var i = 0; i < _handlers[eventName].Count; i++)
                    {
                        if (_handlers[eventName][i].Method == callback)
                            _handlers[eventName].Remove(_handlers[eventName][i]);
                    }
                }
            }
        }

        private void RemoveHandlerIfAutoDestroyEnabled(string eventName, List<Handler> handlers)
        {
            foreach (var handler in handlers)
            {
                if (handler.AutoDestroy)
                    RemoveHandler(eventName, handler.Method);
            }
        }

        public void Notify(string eventName, object data = null)
        {
            Debug.LogFormat($"Notify Event: {eventName} => {data}");
            var notifiedHandlers = new List<Handler>();
            
            lock (_locker)
            {
                if (!_handlers.ContainsKey(eventName))
                    return;
                
                var copy = new List<Handler>(_handlers[eventName].ToList());
                
                for (var i = 0; i < copy.Count; i++)
                {
                    if (copy[i] == null) 
                        continue;

                    notifiedHandlers.Add(copy[i]);
                    copy[i].Method(data);
                }
            
            }
            
            RemoveHandlerIfAutoDestroyEnabled(eventName, notifiedHandlers);
        }

        public void Destroy()
        {
            lock (_locker)
            {
                _handlers.Clear();
                _requestHandler.Clear();
            }
            
            _instance = null;
        }
    }

    internal class Handler
    {
        public int Priority;
        public Action<object> Method;
        public bool AutoDestroy;
    }

    public static class Priority
    {
        public const int Core = 0;
        public const int Higher = 1;
        public const int Medium = 2;
        public const int Low = 3;
    }
}   