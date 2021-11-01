using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Items.Inventory
{
    interface IInventoryObject
    {
        void OnDrop(AbstractInventory inventory);
        void OnPick(AbstractInventory inventory);
    }
    class ContainedItem : IInventoryObject
    {
        ItemContainer itemData;
        public virtual void OnDrop(AbstractInventory inventory)
        {
            itemData.CreateItem(inventory.Position);
        }

        public virtual void OnPick(AbstractInventory inventory)
        {

        }
    }
    class AbstractInventory : MonoBehaviour
    {
        Transform _transform;
        IInventoryObject[] inventoryObjects;
        public Vector3 Position => _transform.position;
        protected virtual void Awake()
        {
            
        }
    }
}
