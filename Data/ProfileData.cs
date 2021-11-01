using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Data
{
    class ProfileData
    {
        private static ProfileData _instance;
        public static ProfileData Instance => _instance == null ? new ProfileData() : _instance;
        public long bestScore;
        public long time;

        public ProfileData()
        {
            bestScore = PlayerPrefs.HasKey("Profile.BestScore") ? long.Parse(PlayerPrefs.GetString("Profile.BestScore")) : 0;
            time = PlayerPrefs.HasKey("Profile.Time") ? long.Parse(PlayerPrefs.GetString("Profile.Time")) : 0;
        }
        public bool Update(long time, long result)
        {
            this.time += (DateTime.Now.Ticks - time);
            if (result > bestScore)
            {
                bestScore = result;
                return true;
            }
            return false;
        }
    }
}
