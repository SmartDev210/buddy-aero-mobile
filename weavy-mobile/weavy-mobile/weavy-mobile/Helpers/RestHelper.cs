using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WeavyMobile.Helpers
{
    public class RestHelper
    {
        public static async Task<T> GetAsync<T>(string path)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(path);
                await HandleResponseError(response);
                try
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(content);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public static async Task PostAsync(string path, object postData)
        {
            using (var client = new HttpClient())
            {
                StringContent content;

                if (postData != null)
                {
                    var json = JsonConvert.SerializeObject(postData);
                    content = new StringContent(json, Encoding.UTF8, "application/json");
                }
                else
                {
                    content = null;
                }

                var response = await client.PostAsync(path, content);

                await HandleResponseError(response);
            }
        }
        public static async Task<T> PostAsync<T>(string path, object postData)
        {
            using (var client = new HttpClient())
            {
                StringContent content;

                if (postData != null)
                {
                    var json = JsonConvert.SerializeObject(postData);
                    content = new StringContent(json, Encoding.UTF8, "application/json");
                }
                else
                {
                    content = null;
                }

                var response = await client.PostAsync(path, content);

                await HandleResponseError(response);

                try
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(result);

                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        public static async Task PutAsync(string path, object putData)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(putData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(path, content);

                await HandleResponseError(response);
            }
        }
        public static async Task<T> PutAsync<T>(string path, object putData)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(putData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(path, content);

                await HandleResponseError(response);
                try
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(result);

                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public static async Task<T> DeleteAsync<T>(string path)
        {
            using (var client = new HttpClient())
            {
                var response = await client.DeleteAsync(path);
                await HandleResponseError(response);
                try
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(content);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        private static async Task HandleResponseError(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(errorMessage))
                    errorMessage += response.ReasonPhrase;
                throw new Exception(errorMessage);
            }
        }
    }
}
