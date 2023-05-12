using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.MapTileFactory
{
    internal static class MapTileFactoryHelper
    {        
        public static GameObject CreateObject(GameObject prefab, GameObject parent)
        {
            GameObject go = GameObject.Instantiate(prefab);
            go.transform.SetParent(parent.transform, false);
            return go;
        }
    }
}
