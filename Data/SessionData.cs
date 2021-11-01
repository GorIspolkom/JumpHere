using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Data
{
    static class SessionData
    {
        public static long startTime;
        public static float points;
        public static float velocity;
        public static float path;
        public static float timer;

        public static void Update(float deltaTime)
        {
            timer += deltaTime;
        }
        public static void AddPath(float deltaPath)
        {
            path += deltaPath;
            velocity = VelocityFromPath();
            points += PathToPoint(deltaPath);
        }
        private static float VelocityFromPath()
        {
            return SettingsData.StartVelocity * Mathf.Exp(path / 1000f);
        }
        private static float PathToPoint(float path)
        {
            return path;
        }
        public static void Init()
        {
            points = 0;
            path = 0;
            velocity = SettingsData.StartVelocity;
            timer = 0;
            startTime = DateTime.Now.Ticks;
        }
        public static void Init(long points, float velocity, float path, float timer)
        {
            SessionData.points = points;
            SessionData.velocity = velocity;
            SessionData.path = path;
            SessionData.timer = timer;
        }
    }
}
