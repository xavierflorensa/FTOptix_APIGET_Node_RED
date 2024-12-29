#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.Retentivity;
using FTOptix.UI;
using FTOptix.NativeUI;
using FTOptix.CoreBase;
using FTOptix.Core;
using FTOptix.NetLogic;
using System.IO;
using System.Net.Http;
#endregion

public class APIGetWeather : BaseNetLogic
{
    public override void Start()
    {
        myPeriodictask = new PeriodicTask(GetWeather, 1000, LogicObject);
        myPeriodictask.Start();
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
        myPeriodictask?.Dispose();
    }

    private void GetWeather()
    {
        URL = Project.Current.GetVariable("Model/datosAPI/URL");

        HttpClient httpClient = new();
        var result = httpClient.GetAsync(URL.Value).Result;

        if (result.IsSuccessStatusCode)
        {
            StreamReader reader = new StreamReader(result.Content.ReadAsStream());
            string json = reader.ReadToEnd();
            Log.Info("Valor json:" + json);
        }

        else
        {
            Log.Info("Failed to retrieve data. Status code: " + result.StatusCode);
        }
    }

    private PeriodicTask myPeriodictask;
    private IUAVariable URL; //datos API 
}
