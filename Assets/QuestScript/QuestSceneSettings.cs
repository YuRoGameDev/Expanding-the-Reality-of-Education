using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class QuestSceneSettings : MonoBehaviour
{
    public bool UseSpaceWarp;
    //public bool SpaceWarpMenu;
    public int SetTextureQuality;
    public UniversalRendererData asset;
    public bool UsePPV;
    public PostProcessData data;
    void Start()
    {
        //OVRManager.SetSpaceWarp(SpaceWarpMenu);
        //QualitySettings.SetQualityLevel(SetTextureQuality, false);
        //asset.postProcessData = UsePPV ? data : null;
    }


}
