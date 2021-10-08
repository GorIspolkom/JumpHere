using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Level
{
    public class Track 
    {
        public Transform startTrack { get; private set; }
        public Transform endTrack { get; private set; }
        public Transform _track;
        private float _size;

        public Track(Vector3 spawnPosition, GameObject track, float jumpDistance, float size, Vector3 direction)
        {
            
            _size = size;
            _track = Object.Instantiate(track, spawnPosition, Quaternion.identity).transform;
            _track.rotation = Quaternion.Euler(direction.x, direction.y, direction.z);
            endTrack = _track.transform.GetChild(0);
            startTrack = _track.transform.GetChild(1);
            _track.position -= startTrack.localPosition;
        }
        public Track GetNextTrack(GameObject trackPanel, float jumpDistance, float size, Vector3 direction)
        {
            Track track = new Track(endTrack.position, trackPanel, jumpDistance, size, direction);
            track._track.position -= startTrack.localPosition;
            Debug.Log("Pos " + startTrack.localPosition);
            return track;
        }
        public void DeleteTrack() => Object.Destroy(_track.gameObject);
    }
}
