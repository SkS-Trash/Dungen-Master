using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

internal class SubscribersList<TSubscriber> where TSubscriber : class, IGlobalSubscriber
{
    // Используем WeakReference, чтобы не удерживать объекты подписчиков насмерть.
    private readonly List<WeakReference<TSubscriber>> _subscribers = new();
    private readonly object _lock = new();

    public void Add(TSubscriber subscriber)
    {
        lock (_lock)
        {
            _subscribers.Add(new WeakReference<TSubscriber>(subscriber));
        }
    }

    public void Remove(TSubscriber subscriber)
    {
        lock (_lock)
        {
            // Удаляем мертвые ссылки.
            _subscribers.RemoveAll(weakReference =>
                !weakReference.TryGetTarget(out var target) ||
                ReferenceEquals(target, subscriber)
            );
        }
    }

    public void Execute(Action<TSubscriber> action)
    {
        List<TSubscriber> snapshot;
        lock (_lock)
        {
            // Снимок с живыми подписчиками.
            snapshot = _subscribers
                .Select(wr => wr.TryGetTarget(out var target) ? target : null)
                .Where(s => s != null)
                .ToList();

            // Очистка "мертвых" ссылок.
            _subscribers.RemoveAll(wr => !wr.TryGetTarget(out _));
        }

        foreach (var subscriber in snapshot)
        {
            try
            {
                action.Invoke(subscriber);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[Ошибка при выполнении действия для подписчика] {ex}");
            }
        }
    }
}