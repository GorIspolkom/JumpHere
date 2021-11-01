using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Items
{
    struct ItemContainer
    {
        public ItemData data;
        public int quantity;
        public ItemContainer(ItemData data, int quantity)
        {
            this.data = data;
            this.quantity = quantity;
        }
        public Transform CreateItem(Vector3 position)
        {
            if (data.is3D)
                return CreateItem3D(position);
            else
                return CreateItem2D(position);
        }
        private Transform CreateItem2D(Vector3 position)
        {
            GameObject item = new GameObject();
            item.AddComponent<SpriteRenderer>().sprite = data.ItemImage;
            if (data.tags.IsTag(ItemTag.Pickable))
                item.AddComponent<PickableItem>().Init(this, position);
            return item.transform;
        }
        private Transform CreateItem3D(Vector3 position)
        {
            GameObject item = new GameObject();
            item.AddComponent<MeshFilter>().mesh = data.ItemMesh;
            if (data.tags.IsTag(ItemTag.Pickable))
                item.AddComponent<PickableItem>().Init(this, position);
            return item.transform;
        }
    }
    class PickableItem : MonoBehaviour
    {
        ItemContainer itemInfo;
        Transform _transform;
        
        public void Init(ItemContainer itemInfo, Vector3 position)
        {
            this.itemInfo = itemInfo;
            _transform.GetComponent<SpriteRenderer>().sprite = itemInfo.data.ItemImage;
            transform.position = position;
        }
        private void OnCollisionEnter(Collision collision)
        {
            //collision.transform.GetComponent < Inventory >
            if (collision.gameObject.CompareTag("Player"))
            {
            }
        }
    }
}
