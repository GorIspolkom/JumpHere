using UnityEngine;

public static class Data
{
    [SerializeField]
    public static double _points;
    [SerializeField]
    public static double _path;
    [SerializeField]
    public static float _timer;
   
    public static void InitData(double points, double path, float timer)
    {
        _points = points;
        _path = path;
        _timer = timer;
    }
}
