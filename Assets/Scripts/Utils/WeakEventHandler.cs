using System;
using System.Reflection;

namespace StereoApp.Utils
{
    public static class WeakEventHandler
    {
        private delegate void UnboundEventHandler<TTarget, TEventArgs>(
            TTarget target,
            object sender,
            TEventArgs eventArgs
        )
            where TTarget : class
            where TEventArgs : EventArgs;

        private class WeakEventHandlerImpl<TEventHandler>
            where TEventHandler : class
        {
            // weakref to the wrapped delegate's target
            private readonly WeakReference _targetWeakRef;

            // a function that can unsubscribe previously subscribed callback from `HandlerDelegate`
            private readonly Action<TEventHandler> _unsubscribeAction;

            // the original delegate that we were passed and that we are wrapping
            private readonly Delegate _wrappedDelegate;

            // this event handler's delegate type that can be appended to the event list
            public TEventHandler HandlerDelegate { get; }

            public WeakEventHandlerImpl(
                Delegate wrappedDelegate,
                Action<TEventHandler> unsubscribeAction
            )
            {
                _targetWeakRef = new WeakReference(wrappedDelegate.Target);
                _unsubscribeAction = unsubscribeAction;

                var targetType = wrappedDelegate.Target.GetType();
                var eventHandlerType = typeof(TEventHandler);
                var eventArgsType = eventHandlerType.IsGenericType
                    ? eventHandlerType.GetGenericArguments()[0]
                    : eventHandlerType.GetMethod(nameof(MethodBase.Invoke)).GetParameters()[
                        1
                    ].ParameterType;
                var wrappedDelegateType = typeof(UnboundEventHandler<,>).MakeGenericType(
                    wrappedDelegate.Target.GetType(),
                    eventArgsType
                );

                var handlerDelegateMethodInfo = typeof(WeakEventHandlerImpl<TEventHandler>)
                    .GetMethod(nameof(HandlerImpl), BindingFlags.Instance | BindingFlags.NonPublic)
                    .MakeGenericMethod(targetType, eventArgsType);
                HandlerDelegate = (TEventHandler)
                    (object)
                        Delegate.CreateDelegate(eventHandlerType, this, handlerDelegateMethodInfo);

                _wrappedDelegate = Delegate.CreateDelegate(
                    wrappedDelegateType,
                    null,
                    wrappedDelegate.Method
                );
            }

            private void HandlerImpl<TTarget, TEventArgs>(object sender, TEventArgs e)
                where TTarget : class
                where TEventArgs : EventArgs
            {
                var target = (TTarget)_targetWeakRef.Target;
                if (target == null)
                {
                    _unsubscribeAction(HandlerDelegate);
                    return;
                }

                ((UnboundEventHandler<TTarget, TEventArgs>)_wrappedDelegate)(target, sender, e);
            }
        }

        public static TEventHandler Create<TEventHandler>(
            TEventHandler handler,
            Action<TEventHandler> unsubscribeAction
        )
            where TEventHandler : class
        {
            var wrappedDelegate = (Delegate)(object)handler;
            if (wrappedDelegate.Target == null)
            {
                return handler;
            }

            return new WeakEventHandlerImpl<TEventHandler>(
                wrappedDelegate,
                unsubscribeAction
            ).HandlerDelegate;
        }
    }
}
