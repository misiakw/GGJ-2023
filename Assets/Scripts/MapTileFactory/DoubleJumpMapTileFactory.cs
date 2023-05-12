using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.MapTileFactory
{
    internal static class DoubleJumpMapTileFactory
    {
        public static void GenerateTile(GameObject mapTile, int variation)
        {
            switch (variation)
            {
                case 1:
                    Variation1(mapTile);
                    break;
                case 2:
                    Variation2(mapTile);
                    break;
                case 3:
                    Variation3(mapTile);
                    break;
                case 4:
                    Variation4(mapTile);
                    break;
            }
        }

        static void Variation1(GameObject mapTile)
        {
            GameObject go;
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.OrbPrefab, mapTile);
            go.transform.localPosition += new Vector3(-7.5f, 3, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.OrbPrefab, mapTile);
            go.transform.localPosition += new Vector3(-7.5f, 4, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.OrbPrefab, mapTile);
            go.transform.localPosition += new Vector3(-7.5f, 5, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.OrbPrefab, mapTile);
            go.transform.localPosition += new Vector3(-6.5f, 4, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.OrbPrefab, mapTile);
            go.transform.localPosition += new Vector3(-6.5f, 5, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.OrbPrefab, mapTile);
            go.transform.localPosition += new Vector3(-6.5f, 6, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.OrbPrefab, mapTile);
            go.transform.localPosition += new Vector3(-5.5f, 3, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.OrbPrefab, mapTile);
            go.transform.localPosition += new Vector3(-5.5f, 4, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.OrbPrefab, mapTile);
            go.transform.localPosition += new Vector3(-5.5f, 5, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.RunningBugPrefab, mapTile);
            go.transform.localPosition += new Vector3(-7.5f, 0, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.RunningBugPrefab, mapTile);
            go.transform.localPosition += new Vector3(-5.5f, 0, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.FlyingBugPrefab, mapTile);
            go.transform.localPosition += new Vector3(-6.5f, 0, 0);
        }

        static void Variation2(GameObject mapTile)
        {
            GameObject go;
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.OrbPrefab, mapTile);
            go.transform.localPosition += new Vector3(-7.5f, 2, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.OrbPrefab, mapTile);
            go.transform.localPosition += new Vector3(-6.5f, 2, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.OrbPrefab, mapTile);
            go.transform.localPosition += new Vector3(-6.5f, 3, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.OrbPrefab, mapTile);
            go.transform.localPosition += new Vector3(-5.5f, 4, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.OrbPrefab, mapTile);
            go.transform.localPosition += new Vector3(-4.5f, 2, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.OrbPrefab, mapTile);
            go.transform.localPosition += new Vector3(-4.5f, 3, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.OrbPrefab, mapTile);
            go.transform.localPosition += new Vector3(-3.5f, 2, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.OrbPrefab, mapTile);
            go.transform.localPosition += new Vector3(-5.5f, 0, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.OrbPrefab, mapTile);
            go.transform.localPosition += new Vector3(-5.5f, 1, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.OrbPrefab, mapTile);
            go.transform.localPosition += new Vector3(-5.5f, 2, 0);

            var floorElements = mapTile.transform.FindChildren("FloorElement");
            floorElements[1].gameObject.SetActive(false);
            floorElements[2].gameObject.SetActive(false);
            floorElements[3].gameObject.SetActive(false);
            floorElements[4].gameObject.SetActive(false);
            floorElements[5].gameObject.SetActive(false);
            floorElements[6].gameObject.SetActive(false);
            floorElements[7].gameObject.SetActive(false);
        }

        static void Variation3(GameObject mapTile)
        {
            GameObject go;
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.OrbPrefab, mapTile);
            go.transform.localPosition += new Vector3(-7.5f, 2, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.OrbPrefab, mapTile);
            go.transform.localPosition += new Vector3(-6.5f, 2, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.OrbPrefab, mapTile);
            go.transform.localPosition += new Vector3(-6.5f, 3, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.OrbPrefab, mapTile);
            go.transform.localPosition += new Vector3(-5.5f, 2, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.OrbPrefab, mapTile);
            go.transform.localPosition += new Vector3(-5.5f, 3, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.OrbPrefab, mapTile);
            go.transform.localPosition += new Vector3(-4.5f, 2, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.OrbPrefab, mapTile);
            go.transform.localPosition += new Vector3(-4.5f, 3, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.OrbPrefab, mapTile);
            go.transform.localPosition += new Vector3(-3.5f, 2, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.RunningBugPrefab, mapTile);
            go.transform.localPosition += new Vector3(-4f, 0, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.RunningBugPrefab, mapTile);
            go.transform.localPosition += new Vector3(-5.5f, 0, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.RunningBugPrefab, mapTile);
            go.transform.localPosition += new Vector3(-7f, 0, 0);
        }

        static void Variation4(GameObject mapTile)
        {
            GameObject go;
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.OrbPrefab, mapTile);
            go.transform.localPosition += new Vector3(-7.5f, 2, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.OrbPrefab, mapTile);
            go.transform.localPosition += new Vector3(-6.5f, 2, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.OrbPrefab, mapTile);
            go.transform.localPosition += new Vector3(-6.5f, 3, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.OrbPrefab, mapTile);
            go.transform.localPosition += new Vector3(-5.5f, 2, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.OrbPrefab, mapTile);
            go.transform.localPosition += new Vector3(-5.5f, 3, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.OrbPrefab, mapTile);
            go.transform.localPosition += new Vector3(-4.5f, 2, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.OrbPrefab, mapTile);
            go.transform.localPosition += new Vector3(-4.5f, 3, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.OrbPrefab, mapTile);
            go.transform.localPosition += new Vector3(-3.5f, 2, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.RunningBugPrefab, mapTile);
            go.transform.localPosition += new Vector3(-4f, 0, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.RunningBugPrefab, mapTile);
            go.transform.localPosition += new Vector3(-5.5f, 0, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.RunningBugPrefab, mapTile);
            go.transform.localPosition += new Vector3(-7f, 0, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.FlyingBugPrefab, mapTile);
            go.transform.localPosition += new Vector3(-4f, 4, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.FlyingBugPrefab, mapTile);
            go.transform.localPosition += new Vector3(-5.5f, 4, 0);
            go = MapTileFactoryHelper.CreateObject(PrefabStorage.FlyingBugPrefab, mapTile);
            go.transform.localPosition += new Vector3(-7f, 4, 0);
        }
    }
}
