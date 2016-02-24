using UnityEngine;
using System.Collections;

public class Singleton <T>
{
    public static T instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = System.Activator.CreateInstance<T>();
            }
            return mInstance;
        }
    }
    private static T mInstance;
	
   	
	
}
