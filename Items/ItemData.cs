using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Items
{
    [Serializable]
    public class TagManager
    {
        [SerializeField] ItemTag[] _tags;
        public ItemTag[] Tags => _tags;
        public bool IsTag(ItemTag searchedTag)
        {
            foreach (ItemTag tag in _tags)
                if (tag == searchedTag)
                    return true;
            return false;
        }
    }

    public class ItemData : ScriptableObject
    {
        public int ID;
        public string itemName;
        public string description;
        public string shortDescrition;
        public int maxItemStack;
        public TagManager tags;
        public bool is3D;

        public Sprite ItemImage => Resources.Load<Sprite>("Art/Items/" + itemName);
        public Mesh ItemMesh => Resources.Load<Mesh>("Models/Items/" + itemName);
    }
}
