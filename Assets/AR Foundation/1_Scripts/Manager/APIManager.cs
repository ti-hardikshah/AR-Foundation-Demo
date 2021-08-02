using System.Text;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public enum Environment
{
   Testing,
   Production
}

public class APIManager
{
    public delegate void action(bool success, object data);
    private static bool IsValid(long code) => (int)code / 100 == 2;

    #region API_CALL

    /// Make a raw API request 
    /// </summary>
    /// <param name="method">HTTP method</param>
    /// <param name="endPoint">i.e. PG/painting/get-related-public</param>
    /// <param name="jsonData">Data to post/patch.</param>
    /// <param name="callback">Returns server response in JSON format (status code, response string).</param>
     public void Request(string method, string endPoint, string jsonData, string accessToken , UnityAction<long, string> callback)
        {
            // URL will get from RN : endPoint
        
            UnityWebRequest request = new UnityWebRequest(endPoint, method);

                if (!string.IsNullOrEmpty(accessToken))
                    request.SetRequestHeader("Authorization", accessToken);

                request.SetRequestHeader("Content-Type", "application/json");

            if (!string.IsNullOrEmpty(jsonData)) {
                   // Debug.Log("*********************************************");
                    Debug.Log(jsonData);
                   // Debug.Log("*********************************************");
                    request.uploadHandler = new UploadHandlerRaw(Encoding.ASCII.GetBytes(jsonData)) { contentType = "application/json" };
            }

            request.downloadHandler = new DownloadHandlerBuffer();

            request.SendWebRequest().completed += _ =>
            {
                callback(request.responseCode, request.downloadHandler.text);
            };
        }
    #endregion

    #region WebAPI call
    public static void CallWebAPI(string method, string endPoint, string jsonData, string accessToken, action onComplete = null)
    {

        new APIManager().CreateAPIResponse(method, endPoint, jsonData, accessToken, onComplete);
    }

    private void CreateAPIResponse(string method, string endPoint, string jsonData, string accessToken, action onComplete)
    {

        Request(method, endPoint, jsonData, accessToken, (status, res) =>
        {
            JSONNode _node = JSON.Parse(res);
            Debug.Log("Full Response : " + res);

            if (!IsValid(status))
            {
                onComplete?.Invoke(false, res);
                return;
            }
            onComplete?.Invoke(true, res);
        });
    }
    #endregion

}
