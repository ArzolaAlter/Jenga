using System.Collections.Generic;
using Jenga.API;
using Newtonsoft.Json.Utilities;
using UnityEngine;

namespace Jenga.Persistence
{
    /// <summary>
    /// <para>This class isn't attached to any game object, this just exists to deal with a known JsonSerialization issue on AoT platforms: https://github.com/jilleJr/Newtonsoft.Json-for-Unity/wiki/Fix-AOT-using-AotHelper</para>
    /// <para>Basically, this forces compilation of types needed on builds for json deserialization.</para>
    /// <para>This DOESN'T need to be attached to a game object, just exist as a thing that needs to be compiled.</para>
    /// </summary>
    public class AotTypeEnforcer : MonoBehaviour
    {
        public void Awake()
        {
            AotHelper.EnsureList<SubjectData>();
            AotHelper.EnsureDictionary<string, List<SubjectData>>();
        }
    }
}