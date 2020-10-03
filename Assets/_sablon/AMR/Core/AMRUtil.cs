using System;
using UnityEngine;

namespace AMR
{
    public class AMRUtil
    {
        public static void Log(string message)
        {
            if (Debug.isDebugBuild)
            {
                //Debug.Log(message);
            }
        }
    }
}
