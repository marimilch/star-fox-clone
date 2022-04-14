using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Functional
{
    public delegate void Action();
    public delegate void Action<T>(T t);
    public delegate void Action<T, U>(T t, U u);
    public delegate void Each<T>(T elem);

    public class Function
    {
        public static Action Pass = () => { };

        public static void ForEach<T>(T[] ts, Action<T> map)
        {
            for(int i = 0; i < ts.Length; i++)
            {
                map(ts[i]);
            }
        }

        public static void ForEach<T, U>(T[] ts, U[] us, Action<T,U> map)
        {
            for (int i = 0; i < ts.Length; i++)
            {
                for (int j = 0; i < us.Length; j++)
                {
                    map(ts[i], us[i]);
                }
            }
        }
    } 
}
