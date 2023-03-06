using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class MoveConsts
{
    public float gravity = 4f;
    public float squashedGravity = 10f;
    public float jumpStrength = 15f;
    public float speedIncreaseMultiplier = 1.3f;

    public DoubleJumpStrategy doubleJumpStrategy = new DoubleJumpStrategy();
    [Serializable]
    public class DoubleJumpStrategy
    {
        public float topJumpSpeedMultiplier = 1.5f;
        public float risingJumpMultiplier = 0.5f;
        public float fallJumpMultiplier = 1f;
        public bool zeroFallVelocity = false;
    }

    private static MoveConsts _instance = null;
    public static MoveConsts instance
    {
        get
        {
            if (_instance == null)
                _instance = loadConsts();
            return _instance;
        }
    }
    private MoveConsts(){ }

    private static MoveConsts loadConsts()
    {
        var filePath = Path.Combine(Application.dataPath, "MoveConsts.json");
        if (File.Exists(filePath))
        {
            var contentst = File.ReadAllText(filePath);
            try
            {
                var consts = JsonUtility.FromJson<MoveConsts>(contentst);
                storeConsts(consts);
                return consts;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                return new MoveConsts();
            }
        }
        else
        {
            var consts = new MoveConsts();
            storeConsts(consts);
            return consts;
        }
    }
    private static void storeConsts(MoveConsts consts)
    {
        var filePath = Path.Combine(Application.dataPath, "MoveConsts.json");
        File.WriteAllText(filePath, JsonUtility.ToJson(consts, true));
    }
}
