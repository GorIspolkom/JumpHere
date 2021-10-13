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
            _track = GameObject.Instantiate(track, spawnPosition, Quaternion.identity).transform;
            endTrack = _track.transform.GetChild(0);
            startTrack = _track.transform.GetChild(1);
            //_track.position -= startTrack.localPosition;
        }
        public Track GetNextTrack(GameObject trackPanel, float jumpDistance, float size, Vector3 direction)
        {
            Track track = new Track(endTrack.position, trackPanel, jumpDistance, size, direction);
            if(direction.y > 0) 
               track._track.position += new Vector3(jumpDistance + size, 0, 0);
            else 
               track._track.position += new Vector3(0, 0, jumpDistance + size); 
            track._track.rotation = Quaternion.Euler(direction);
            Debug.Log("Pos " + startTrack.localPosition);
            return track;
        }
        public void DeleteTrack() => Object.Destroy(_track.gameObject);
    }
}
