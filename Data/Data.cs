using UnityEngine;

public static class Data
{
    public static double points;
    public static double path;
    public static float timer;
   
    public static void Update(float deltaTime)
    {
        timer += deltaTime;
    }
    public static void AddPath(double deltaPath)
    {
        path += deltaPath;
        points += PathToPoint(deltaPath);
    }
    private static double PathToPoint(double path)
    {
        return path;
    }
    public static void InitData(double points, double path, float timer)
    {
        Data.points = points;
        Data.path = path;
        Data.timer = timer;
    }
}
