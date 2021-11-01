using HairyEngine.HairyCamera;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Level
{
    public abstract class AbstractFloor
    {
        public Transform floorObject;
        public Vector3 Position { get => _position; protected set => _position = value; }
        public Vector3 Center{ get => _center; protected set => _center = value; }
        public abstract Vector3 Sizes { get; protected set; }
        private Vector3 _position;
        private Vector3 _center;

        public abstract void InitParams();
        public AbstractFloor(Transform floorObject)
        {
            this.floorObject = floorObject;
        }
        public AbstractFloor GetNextTile(Transform trackPanel, Vector3 direction, float jumpDistance)
        {
            return InitFloor(trackPanel, Center + Sizes.NablaMultiply(direction) + (direction - Vector3.up) * jumpDistance, direction);
        }
        private static AbstractFloor InitFloor(Transform floorT)
        {
            if (floorT.GetComponentInChildren<BoxCollider>())
                return new TileFloor(floorT);
            else if (floorT.GetComponentInChildren<SphereCollider>())
                return new SphereFloor(floorT);
            else
                return new SimpleFloor(floorT);
        }
        public static AbstractFloor InitFloor(Transform floorT, Vector3 position)
        {
            AbstractFloor floor = InitFloor(floorT);
            floorT.position = position;
            floor.InitParams();
            return floor;
        }
        private AbstractFloor InitFloor(Transform floorT, Vector3 position, Vector3 direction)
        {
            floorT.position = Vector3.zero;
            AbstractFloor floor = InitFloor(floorT);
            floorT.position = position + floor.Sizes.NablaMultiply(direction) - floor.Center;
            floor.InitParams();
            return floor;
        }
        public void AddObstacle(Transform obstacle)
        {
            AddObstacle(obstacle, Vector3.up * Sizes.y);
        }
        public void AddObstacle(Transform obstacle, Vector3 position)
        {
            obstacle.parent = floorObject;
            obstacle.localPosition = position;
            if (floorObject.rotation.y == 0)
                obstacle.localRotation = Quaternion.Euler(0, 90, 0);
            else
                obstacle.localRotation = Quaternion.Euler(0, -90, 0);
        }
        public void DeleteFloor() => GameObject.Destroy(floorObject.gameObject);
    }
    public class TileFloor : AbstractFloor
    {
        private BoxCollider _boxCollider;
        private Transform _boxTransform;
        private Vector3 _size;
        public TileFloor(Transform floorObject) : base(floorObject)
        {
            _boxCollider = floorObject.GetComponentInChildren<BoxCollider>();
            _boxTransform = _boxCollider.transform;
            Sizes = _boxCollider.size.NablaMultiply(_boxTransform.localScale) / 2f;
            Sizes = floorObject.worldToLocalMatrix.MultiplyPoint3x4(_boxTransform.localToWorldMatrix.MultiplyPoint3x4(Sizes));
            Center = _boxTransform.localToWorldMatrix.MultiplyPoint3x4(_boxCollider.center);
        }
        public override Vector3 Sizes { get => _size; protected set => _size = value; }
        public override void InitParams()
        {
            Center = _boxTransform.localToWorldMatrix.MultiplyPoint3x4(_boxCollider.center);
            Position = Center - Sizes;
        }
    }
    public class SimpleFloor : AbstractFloor
    {
        public SimpleFloor(Transform floorObject) : base(floorObject)
        {
            floorObject.position = Center - Sizes;
        }

        public override Vector3 Sizes { get => Vector3.one * 4.1f; protected set { } }

        public override void InitParams()
        {
            Center = floorObject.position;
            Position = Center - Sizes;
        }
    }
    public class SphereFloor : AbstractFloor
    {
        private SphereCollider _sphereCollider;
        private Transform _sphereTransform;
        public SphereFloor(Transform floorObject) : base(floorObject)
        {
            _sphereCollider = floorObject.GetComponentInChildren<SphereCollider>();
            Center = floorObject.localToWorldMatrix.MultiplyPoint3x4(_sphereCollider.center);
        }

        public override Vector3 Sizes { get => Vector3.one * _sphereCollider.radius; protected set => _sphereCollider.radius = value.x; }
        public override void InitParams()
        {
            Center = floorObject.localToWorldMatrix.MultiplyPoint3x4(_sphereCollider.center);
            Position = _sphereTransform.localToWorldMatrix.MultiplyPoint3x4(_sphereCollider.center - Sizes);
        }
    }
}
