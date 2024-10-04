using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SocialPlatforms;
using System;
using UnityEngine.UI;
public class AdsControl : MonoBehaviour
{


    protected AdsControl()
    {
    }

    private static AdsControl _instance;
    public string AdmobID_Android, AdmobID_IOS, BannerID_Android, BannerID_IOS;
    public string UnityID_Android, UnityID_IOS, UnityZoneID;

    public static AdsControl Instance { get { return _instance; } }

    void Awake()
    {
        if (FindObjectsOfType(typeof(AdsControl)).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        MakeNewInterstial();
        RequestBanner();
        /*
        if (PlayerPrefs.GetInt("RemoveAds") == 0)
            ShowBanner();
        else
            HideBanner();
         */

        DontDestroyOnLoad(gameObject); //Already done by CBManager


    }


    public void HandleInterstialAdClosed(object sender, EventArgs args)
    {
        
        MakeNewInterstial();



    }

    void MakeNewInterstial()
    {
        


    }


    public void showAds()
    {
       
    }


    public bool GetRewardAvailable()
    {
        return false;
    }

    public void ShowRewardVideo()
    {
        

    }


    private void RequestBanner()
    {


    }

    public void ShowBanner()
    {
    }

    public void HideBanner()
    {
    }



    public void ShowFB()
    {
        Application.OpenURL("https://www.facebook.com/PonyStudio2507/?ref=settings");
    }

    public void RateMyGame()
    {



    }


   public void PlayDelegateRewardVideo(Action<bool> onVideoPlayed)
    {
        
        onVideoPlayed(false);
    }
}

