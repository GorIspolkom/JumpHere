using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairyEngine.HairyCamera
{
    enum MovementAxis
    {
        XY,
        XZ,
        YZ
    }
    enum Isometric
    {
        None,
        Isometric
    }
    enum UpdateType
    {
        FixedUpdate,
        LateUpdate,
        EveryUpdate
    }
}
