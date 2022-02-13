using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class MathUtility
{
    static Vector2 vec2Cache;
    static Vector2Int vec2IntCache;

    public static Vector2 SwapVec2(Vector2 vec)
    {
        vec2Cache = vec;

        vec.x = vec2Cache.y;
        vec.y = vec2Cache.x;

        return vec;
    }

    public static Vector2Int SwapVec2(Vector2Int vec)
    {
        vec2IntCache = vec;

        vec.x = vec2IntCache.y;
        vec.y = vec2IntCache.x;

        return vec;
    }

}
