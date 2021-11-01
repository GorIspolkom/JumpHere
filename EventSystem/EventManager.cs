using System;
using UnityEngine;
using System.Collections.Generic;

namespace HairyEngine.EventSystem
{
	public static class EventManager
	{
		private static Dictionary<Type, List<EventListenerBase>> _subscribersList;

		static EventManager()
		{
			_subscribersList = new Dictionary<Type, List<EventListenerBase>>();
		}

		/// <summary>
		/// Adds a new subscriber to a certain event.
		/// </summary>
		/// <param name="listener">listener.</param>
		/// <typeparam name="Event">The event data type.</typeparam>
		public static void AddListener<Event>(EventListener<Event> listener) where Event : struct
		{
			Type eventType = typeof(Event);

			if (!_subscribersList.ContainsKey(eventType))
				_subscribersList[eventType] = new List<EventListenerBase>();

			if (!SubscriptionExists(eventType, listener))
				_subscribersList[eventType].Add(listener);
		}

		/// <summary>
		/// Removes a subscriber from a certain event.
		/// </summary>
		/// <param name="listener">listener.</param>
		/// <typeparam name="Event">The event data type.</typeparam>
		public static void RemoveListener<Event>(EventListener<Event> listener) where Event : struct
		{
			Type eventType = typeof(Event);

			if (!_subscribersList.ContainsKey(eventType))
				return;

			List<EventListenerBase> subscriberList = _subscribersList[eventType];

			for (int i = 0; i < subscriberList.Count; i++)
			{
				if (subscriberList[i] == listener)
				{
					subscriberList.Remove(subscriberList[i]);

					if (subscriberList.Count == 0)
						_subscribersList.Remove(eventType);
					return;
				}
			}
		}

		/// <summary>
		/// Triggers an event. All instances that are subscribed to it will receive it (and will potentially act on it).
		/// </summary>
		/// <param name="newEvent">The event to trigger.</param>
		/// <typeparam name="Event">The 1st type parameter.</typeparam>
		public static void TriggerEvent<Event>(Event newEvent) where Event : struct
		{
			List<EventListenerBase> list;
			if (!_subscribersList.TryGetValue(typeof(Event), out list))
				return;

			for (int i = 0; i < list.Count; i++)
				(list[i] as EventListener<Event>).OnEvent(newEvent);
		}

		/// <summary>
		/// Checks if there are subscribers for a certain type of events
		/// </summary>
		/// <returns><c>true</c>, if exists was subscriptioned, <c>false</c> otherwise.</returns>
		/// <param name="type">Type.</param>
		/// <param name="receiver">Receiver.</param>
		private static bool SubscriptionExists(Type type, EventListenerBase receiver)
		{
			List<EventListenerBase> receivers;

			if (!_subscribersList.TryGetValue(type, out receivers))
				return false;

			bool exists = false;

			for (int i = 0; i < receivers.Count; i++)
				if (receivers[i] == receiver)
				{
					exists = true;
					break;
				}

			return exists;
		}
	}
}