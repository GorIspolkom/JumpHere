using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class ItemManager
    {
        private static ItemManager _instance;
        public static ItemManager Instance => _instance;
        public static void Init()
        {
            _instance = new ItemManager();
        }
        private Dictionary<int, ItemData> _items;
        private ItemManager()
        {
             ItemData[] items = Resources.LoadAll<ItemData>("Items");
            _items = new Dictionary<int, ItemData>();
            foreach (ItemData item in items)
                _items.Add(item.ID, item);
        }
        public ItemData GetItem(int ID) => _items[ID];
    }
}
