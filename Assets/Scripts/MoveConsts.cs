using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public static class MoveConstNotifier
{
    static private EventHandler<MoveConsts> onLoad;
    static private bool isLoaded = false;
    static private object loadLock = new object();

    public static void RegisterLoad(Action<MoveConsts> action)
    {
        lock(loadLock)
        {
            if (isLoaded)
            {
                action(MoveConsts.instance);
            }
            else
            {
                onLoad += ((s, mc) => action(mc));
            }
        }
    }
    public static void Load(MoveConsts consts)
    {
        lock(loadLock)
        {
            isLoaded = true;
            onLoad.Invoke(consts, consts);
        }
    }
}

public class MoveConsts
{

    public bool useKeyboard = true;
    public float gravity = 4f;
    public float squashedGravity = 10f;
    public float jumpStrength = 15f;
    public float speedIncreaseMultiplier = 1f;
    public bool enableDashing = true;
    public float dashSpeed = 8f;
    public float dashTime = 0.4f;

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
            {
                _instance = loadConsts();
                MoveConstNotifier.Load(_instance);
            }
            return _instance;
        }
    }
    private MoveConsts(){ }

    private static MoveConsts loadConsts()
    {
        var filePath = Path.Combine(Application.dataPath, "MoveConsts.json");
        MoveConsts consts = null;
        if (File.Exists(filePath))
        {
            var contentst = File.ReadAllText(filePath);
            try
            {
                consts = JsonUtility.FromJson<MoveConsts>(contentst);
                //if(consts == null)
                storeConsts(consts);

            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                consts = new MoveConsts();
            }
        }
        else
        {
            consts = new MoveConsts();
            storeConsts(consts);
        }

        return consts;
    }
    private static void storeConsts(MoveConsts consts)
    {
        var filePath = Path.Combine(Application.dataPath, "MoveConsts.json");
        File.WriteAllText(filePath, JsonUtility.ToJson(consts, true));
    }

   /* static public void TriggerOnLoad(Action<MoveConsts> call)
    {
        lock (MoveConstNotifier.loadLock)
        {
            if (MoveConstNotifier.isLoaded)
                call(instance);
            else
                MoveConstNotifier.onLoadAction.Add(call);
        }
    }*/
}
