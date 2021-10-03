using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Level
{
    public class BlockManager
    {
        private GameObject[] _blocks;
        private static BlockManager _manager;
        private enum BlockType { Block, BlockShooting, BlockObstacles };
        private BlockManager() { _blocks = Resources.LoadAll<GameObject>("Prefabs/"); }
        public static BlockManager GetBlockManager() => _manager == null ? _manager = new BlockManager() : _manager;
        public GameObject ChooseBlockType()
        {
            GameObject block;
            BlockType type = (BlockType)(int)Random.Range(0f, 2f);
            Debug.Log("Num of block type "  + type);
            switch (type)
            {
                case BlockType.Block:
                    block = _blocks[0];
                    break;
                case BlockType.BlockObstacles:
                    block = _blocks[1];
                    break;
                case BlockType.BlockShooting:
                    block = _blocks[2];
                    break;
                default:
                    block = null;
                    break;
            }
            return block;
        }
    }
}