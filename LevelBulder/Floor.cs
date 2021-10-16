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
        public const float size = 4.1f;
        public Vector3 position;
        public Vector3 centerPosition => center.position;
        private Transform center;
        public Transform floorObject;

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
        public Floor GetNextTile(Transform trackPanel, Vector3 direction, float jumpDistance = 0)
        {
            trackPanel.position = center.position + direction * (2 * size + jumpDistance);
            Transform centerNext = trackPanel.Find("Center");
            trackPanel.position -= centerNext == null ? Vector3.zero : centerNext.localPosition;
            return new Floor(trackPanel, trackPanel);
        }
        public void DeleteTrack() => GameObject.Destroy(floorObject.gameObject);
    }
}
