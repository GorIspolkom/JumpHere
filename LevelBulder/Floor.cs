using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Level
{
    class Floor
    {
        public const float size = 4.2f;
        public Vector3 position;
        private Transform center;
        private Transform floorObject;

        public Floor(Vector3 spawnPosition, GameObject floorPrefab)
        {
            floorObject = GameObject.Instantiate(floorPrefab, spawnPosition, Quaternion.identity).transform;
            center = floorObject.Find("Center");
            position = center.position - Vector3.one * size;
        }
        private Floor(Transform floorObject, Transform center)
        {
            this.floorObject = floorObject;
            this.center = center;
            if (center == null)
                center = floorObject;
            position = center.position - Vector3.one * size;
        }
        public Floor GetNextTile(GameObject trackPanel, Vector3 direction, float jumpDistance = 0)
        {
            Transform floor = GameObject.Instantiate(trackPanel, center.position + direction * (2 * size + jumpDistance), Quaternion.identity).transform;
            Transform centerNext = floor.Find("Center");
            floor.position -= centerNext == null ? Vector3.zero : centerNext.localPosition;
            return new Floor(floor, centerNext);
        }
        public void DeleteTrack() => GameObject.Destroy(floorObject.gameObject);
    }
}
