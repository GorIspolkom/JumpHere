using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HairyEngine.HairyCamera
{
    public interface ICameraComponent
    {
        int PriorityOrder { get; set; }
    }
    public interface IPreMove : ICameraComponent
    {
        void HandleStartMove(Vector3 direction);
    }
    public interface IPositionChanged : ICameraComponent
    {
        Vector3 HandlePositionChange(Vector3 newPosition);
    }
    public interface IDeltaPositionChanger : ICameraComponent
    {
        Vector3 AdjustDelta(Vector3 deltaMove);
    }
    public interface IPostMove : ICameraComponent
    {
        void HandleStopMove(Vector3 position);
    }

    public interface IViewSizeDeltaChange : ICameraComponent
    {
        float AdjustSize(float deltaSize);
    }
    public interface IViewSizeChanged : ICameraComponent
    {
        float HandleSizeChanged(float newSize);
    }
}
