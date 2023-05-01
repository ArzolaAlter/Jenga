using System;
using System.Collections.Generic;
using Jenga.Main;
using Jenga.Settings;
using MEC;
using Newtonsoft.Json;
using UnityEngine.Networking;

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
    public class APIResponse<T>
    {
        public T Data { get; }
        public UnityWebRequest.Result ResponseResult { get; }

        public APIResponse(UnityWebRequest.Result responseResult, T data)
        {
            ResponseResult = responseResult;
            Data = data;
        }
    }

    /// <summary>
    /// <para>Container class for the information about a single subject in a student's curriculum, represented by a single jenga piece.</para>
    /// <para>Capitalization here matters so it matches 1:1 the API data.</para>
    /// </summary>
    [Serializable]
    public class SubjectData
    {
        [JsonProperty] public int id { get; set; }
        [JsonProperty] public string subject { get; set; }
        [JsonProperty] public string grade { get; set; }
        [JsonProperty] public int mastery { get; set; }
        [JsonProperty] public string domainid { get; set; }
        [JsonProperty] public string cluster { get; set; }
        [JsonProperty] public string standardid { get; set; }
        [JsonProperty] public string standarddescription { get; set; }

        [JsonIgnore] public EPieceType PieceType => (EPieceType)mastery;
    }

    /// <summary>
    /// Class in charge of fetching the latest information about the APIs used by the game.
    /// </summary>
    public class APIManager : Singleton<APIManager>
    {
        //------------------------------------------------------------------------------------//
        /*--------------------------------- PROPERTIES ---------------------------------------*/
        //------------------------------------------------------------------------------------//

        private APISettingsSO Settings => Instances.GameSettings.API;
        private string CurrentStudentSubjectDataURL => Settings.CurrentStudentSubjectDataURL;

        protected override APIManager InstanceTarget
        {
            get => Instances.APIManager;
            set => Instances.APIManager = value;
        }

        //------------------------------------------------------------------------------------//
        /*---------------------------------- METHODS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        private void GetResponseData<T>(string url, Action<APIResponse<T>> OnResponse) =>
            Timing.RunCoroutine(IEGetResponseData(url, OnResponse));

        private IEnumerator<float> IEGetResponseData<T>(string url, Action<APIResponse<T>> OnResponse)
        {
            var www = new UnityWebRequest(url, "GET") { downloadHandler = new DownloadHandlerBuffer() };

            www.SetRequestHeader("Content-Type", "application/json");

            yield return Timing.WaitUntilDone(www.SendWebRequest());

            APIResponse<T> response = new APIResponse<T>(www.result, JsonConvert.DeserializeObject<T>(www.downloadHandler.text));
            OnResponse(response);
        }

        public void GetStudentData(Action<APIResponse<SubjectData[]>> OnResponse)
        {
            GetResponseData(CurrentStudentSubjectDataURL, OnResponse);
        }
    }
}