using BepInEx;
using BepInEx.Configuration;
using System;
using System.IO;
using UnityEngine;

namespace GOOMPS
{
    /* That's me! */
    [BepInPlugin("redbrumbler.rusjj.goomps", "GetOutOfMyPersonalSpace", "1.1")]

    public class GOOMPS : BaseUnityPlugin
    {
        internal static ConfigEntry<bool> m_hCfgEnabled;
        internal static ConfigEntry<float> m_hCfgRadius;
        internal static bool m_bCanUpdateConfig = false;
        private static GOOMPS m_hInstance;
        internal static SphereCollider m_hCol;
        internal static void Log(string msg) => m_hInstance.Logger.LogMessage(msg);
        /* Wrist Buttons Part Start */
        // Do not create a variable of type WristButton! It will fail on load!
        // Create it as an object then use it as a WristButton! (!!!)
        internal static object __toggle_me_btn = null;
        internal static object __radius_btn = null;
        //private void OnReadyForButtons() // WristButtons?
        //{
        //    __toggle_me_btn = WristButtons.WristButton.CreateButton("__toggle_goomps", "Toggle GOOMPS", WristButtons.WristButton.ButtonType.Toggleable);
        //    ((WristButtons.WristButton)__toggle_me_btn).actionToggled = OnBtnToggled;
        //    if (enabled) ((WristButtons.WristButton)__toggle_me_btn).ToggleOn();
        //    __radius_btn = WristButtons.WristButton.CreateButton("__radius_btn", "GOOMPS Radius:\n" + m_hCfgRadius.Value, WristButtons.WristButton.ButtonType.Switchable);
        //    ((WristButtons.WristButton)__radius_btn).actionSwitched = OnRadiusChange;
        //    ((WristButtons.WristButton)__radius_btn).SmallerFont();
        //}
        //private void OnBtnToggled(WristButtons.WristButton b, bool enabled) // WristButtons?
        //{
        //    this.enabled = enabled;
        //    m_hCfgEnabled.Value = enabled;
        //}
        //private void OnRadiusChange(WristButtons.WristButton b, bool left) // WristButtons?
        //{
        //    if(left) SetRadius(m_hCfgRadius.Value - 0.05f);
        //    else SetRadius(m_hCfgRadius.Value + 0.05f);
        //    b.SetText("GOOMPS Radius:\n" + m_hCfgRadius.Value);
        //}
        /* Wrist Buttons Part End */
        void Awake()
        {
            var hCfgFile = new ConfigFile(Path.Combine(Paths.ConfigPath, "GOOMPS.cfg"), true);
            m_hCfgEnabled = hCfgFile.Bind("CFG", "Enabled", true, "Is it enabled?");
            m_hCfgRadius = hCfgFile.Bind("CFG", "Radius", 0.25f, "Radius in.. meters?");

            if (m_hCfgRadius.Value < 0.01f) m_hCfgRadius.Value = 0.01f;
            else if (m_hCfgRadius.Value > 5.0f) m_hCfgRadius.Value = 5.0f;

            m_hInstance = this;
            Patch.Apply();
        }
        void OnEnable()
        {
            if (m_bCanUpdateConfig) m_hCfgEnabled.Value = true;
        }
        void Start()
        {
            m_bCanUpdateConfig = true;
            enabled = m_hCfgEnabled.Value;
        }
        void OnDisable()
        {
            if(m_bCanUpdateConfig) m_hCfgEnabled.Value = false;
        }
        void OnApplicationQuit()
        {
            m_bCanUpdateConfig = false;
        }
        public static void SetRadius(float radius)
        {
            if (radius < 0.05f) m_hCfgRadius.Value = 0.05f;
            else if (radius > 5.0f) m_hCfgRadius.Value = 5.0f;
            else m_hCfgRadius.Value = Convert.ToSingle(Math.Round(Convert.ToDouble(radius), 2, MidpointRounding.ToEven));

            m_hCol.radius = m_hCfgRadius.Value;
        }
    }
}