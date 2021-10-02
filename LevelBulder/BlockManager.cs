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
        public GameObject[] GetBlocks() => _blocks;
        public static BlockManager GetBlockManager() => _manager == null ? _manager = new BlockManager() : _manager;
        public GameObject ChooseBlockType()
        {
            GameObject block;
            BlockType type = (BlockType)(int)Random.Range(0f, 2f);
            switch (type)
            {
                case BlockType.Block:
                    block = GetBlocks()[0];
                    break;
                case BlockType.BlockObstacles:
                    block = GetBlocks()[1];
                    break;
                case BlockType.BlockShooting:
                    block = GetBlocks()[2];
                    break;
                default:
                    block = null;
                    break;
            }
            return block;
        }
    }
}
