using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairyEngine.EventSystem
{
	public static class EventRegister
	{
		public delegate void Delegate<T>(T eventType);

		public static void EventStartListening<EventType>(this EventListener<EventType> caller) where EventType : struct
		{
			EventManager.AddListener(caller);
		}

		public static void EventStopListening<EventType>(this EventListener<EventType> caller) where EventType : struct
		{
			EventManager.RemoveListener(caller);
		}
	}

	public interface EventListenerBase { };

	public interface EventListener<T> : EventListenerBase
	{
		void OnEvent(T eventType);
	}
}
