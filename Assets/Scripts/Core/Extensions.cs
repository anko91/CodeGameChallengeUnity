﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static Vector2 ToVector2(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.y);
    }
    public static Vector3 ToVector3(this Vector2 vector)
    {
        return new Vector3(vector.x, vector.y, 0);
    }
}
