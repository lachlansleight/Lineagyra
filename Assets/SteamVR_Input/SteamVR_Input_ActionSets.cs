//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Valve.VR
{
    using System;
    using UnityEngine;
    
    
    public partial class SteamVR_Actions
    {
        
        private static SteamVR_Input_ActionSet_LineaGyra p_LineaGyra;
        
        public static SteamVR_Input_ActionSet_LineaGyra LineaGyra
        {
            get
            {
                return SteamVR_Actions.p_LineaGyra.GetCopy<SteamVR_Input_ActionSet_LineaGyra>();
            }
        }
        
        private static void StartPreInitActionSets()
        {
            SteamVR_Actions.p_LineaGyra = ((SteamVR_Input_ActionSet_LineaGyra)(SteamVR_ActionSet.Create<SteamVR_Input_ActionSet_LineaGyra>("/actions/LineaGyra")));
            Valve.VR.SteamVR_Input.actionSets = new Valve.VR.SteamVR_ActionSet[] {
                    SteamVR_Actions.LineaGyra};
        }
    }
}
