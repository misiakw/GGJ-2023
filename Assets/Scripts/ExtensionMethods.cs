using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public static class ExtensionMethods
    {

        public static Transform[] FindChildren(this Transform transform, string name)
        {
            return transform.GetComponentsInChildren<Transform>().Where(t => t.name == name).ToArray();
        }
    }

}
