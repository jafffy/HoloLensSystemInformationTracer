using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace HoloLensSystemInformationCollector
{
    public enum eRest
    {
        ApiPowerBattery,
        ApiPowerState,
        ApiHolographicThermalStage,
        ApiResourcemanagerSystemperf
    };

    public class SystemInformation
    {
        static public int f()
        {
            return -1;
        }

        static public Dictionary<eRest, string> RestValueMap { get; set; } = new Dictionary<eRest, string>();

        static public async void SendHttpGetRequest(string uri, eRest key)
        {
            var httpClient = new HttpClient();

            var requestUri = new Uri(uri);
            var httpResponse = new HttpResponseMessage();
            string httpResponseBody = "";

            try
            {
                httpResponse = await httpClient.GetAsync(requestUri);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                return;
            }

            RestValueMap[key] = httpResponseBody;
        }
        
        static public string Get(string uri, eRest key)
        {
            var restSubpathMap = new Dictionary<eRest, string>();
            restSubpathMap[eRest.ApiPowerBattery] = "/api/power/battery";
            restSubpathMap[eRest.ApiPowerState] = "/api/power/state";
            restSubpathMap[eRest.ApiHolographicThermalStage] = "/api/holographic/thermal/stage";
            restSubpathMap[eRest.ApiResourcemanagerSystemperf] = "/api/resourcemanager/systemperf";

            SendHttpGetRequest(uri + restSubpathMap[key], key);

            int timer = 0;

            while (!RestValueMap.ContainsKey(key))
            {
                Task.Delay(1).Wait();
                ++timer;

                if (timer > 30)
                {
                    throw new Exception("Timeout");
                }
            }

            return RestValueMap[key];
        }
    }
}
