using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public static class PrefabStorage
    {
        public static GameObject OrbPrefab;
        public static GameObject RunningBugPrefab;
        public static GameObject FlyingBugPrefab;

        public static void SetPrefabs(GameObject orb, GameObject runningBug, GameObject flyingBug)
        {
            OrbPrefab = orb;
            RunningBugPrefab = runningBug;
            FlyingBugPrefab = flyingBug;
        }
    }
}
