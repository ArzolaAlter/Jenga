using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jenga
{
    /// <summary>
    /// <para>This parent class is intended for manager classes that you want to save on the <see cref="Instances"/> static class.</para>
    /// <para>Ensure <see cref="InstanceTarget"/> points to its corresponding instance in <see cref="Instances"/>.</para>
    /// </summary>
    /// <typeparam name="T">The same class that's inheriting this (i.e. MyClass : Singleton&lt;MyClass>)</typeparam>
    public abstract class Singleton<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        //------------------------------------------------------------------------------------//
        /*--------------------------------- PROPERTIES ---------------------------------------*/
        //------------------------------------------------------------------------------------//

        protected abstract T InstanceTarget { get; set; }
        protected static T Instance { get; set; }

        //------------------------------------------------------------------------------------//
        /*---------------------------------- METHODS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        protected virtual void Awake()
        {
            InstanceTarget ??= this as T;
            if (InstanceTarget != this) { Destroy(this); return; }
            Instance ??= this as T;
        }
    }
}