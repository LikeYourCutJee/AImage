using System;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace AImage.SideFunctionality.DALLE_AI
{
    public class DalleRequests
    {
        protected string MySecretApiKey { get;}

        public DalleRequests(string ApiKey)
        {
            MySecretApiKey = ApiKey;
        }

        //Makes API call, recieves an image by description and returns image url stored in cloud(Image existing time is limited)
        public string? GenerateImageByDescription(string ImgDescription) 
        {
            try
            {
                var client = new RestClient("https://api.openai.com/v1/images/generations");
                var request = new RestRequest("", Method.Post);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("Authorization", $"Bearer {MySecretApiKey}", ParameterType.HttpHeader);
                request.AddParameter("application/json", "{\"model\": \"image-alpha-001\",\"prompt\": \""+ ImgDescription +"\",\"num_images\": 1,\"size\": \"512x512\",\"response_format\": \"url\"}", ParameterType.RequestBody);

                RestResponse response = client.Execute(request);

                if (response.Content != null)
                {
                    JObject json = JObject.Parse(response.Content);
                    JArray? dataArray = (JArray?)json["data"];
                    if (dataArray != null && dataArray.Count > 0)
                    {
                        JObject? dataObject = dataArray[0] as JObject;
                        if (dataObject != null && dataObject["url"] != null)
                        {
                            return (string?)dataObject["url"];
                        }
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
