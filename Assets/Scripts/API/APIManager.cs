using System;
using System.Collections;
using System.Collections.Generic;
using Jenga;
using Jenga.Main;
using Newtonsoft.Json;
using UnityEngine;

namespace Jenga.API
{
    [Serializable]
    public enum EAPIResponseType
    {
        Success = 0,
        Failure = 1
    }

    /// <summary>
    /// Custom wrapper to hold information about an API call's response.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class APIResponse <T>
    {
        public T Data { get; }
        public EAPIResponseType ResponseType { get; }

        public APIResponse(EAPIResponseType responseType, T data)
        {
            ResponseType = responseType;
            Data = data;
        }
    }
    
    /// <summary>
    /// <para>Container class for the information about a single subject in a student's curriculum, represented by a single jenga piece.</para>
    /// <para>Capitalization here matters so it matches 1:1 the API data.</para>
    /// </summary>
    public class SubjectData
    {
        [JsonProperty] public int id { get; }
        [JsonProperty] public string subject { get; }
        [JsonProperty] public string grade { get; }
        [JsonProperty] public int mastery { get; }
        [JsonProperty] public string domainid { get; }
        [JsonProperty] public string cluster { get; }
        [JsonProperty] public string standardid { get; }
        [JsonProperty] public string standarddescription { get; }

        [JsonIgnore] public EPieceType PieceType => (EPieceType) mastery;
    }

    /// <summary>
    /// Class in charge of fetching the latest information about the APIs used by the game.
    /// </summary>
    public class APIManager : Singleton<APIManager>
    {
        //------------------------------------------------------------------------------------//
        /*--------------------------------- PROPERTIES ---------------------------------------*/
        //------------------------------------------------------------------------------------//

        protected override APIManager InstanceTarget
        {
            get => Instances.APIManager;
            set => Instances.APIManager = value;
        }

        //------------------------------------------------------------------------------------//
        /*---------------------------------- METHODS -----------------------------------------*/
        //------------------------------------------------------------------------------------//


    }
}