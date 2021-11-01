using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairyEngine.EventSystem
{
	public struct GameEvent
	{
		public string EventName;
		public GameEvent(string newName)
		{
			EventName = newName;
		}
		static GameEvent e;
		public static void Trigger(string newName)
		{
			e.EventName = newName;
			EventManager.TriggerEvent(e);
		}
	}
}
