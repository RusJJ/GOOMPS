using BepInEx;
using BepInEx.Configuration;
using System.IO;
using UnityEngine;

namespace GOOMPS
{
    /* That's me! */
    [BepInPlugin("redbrumbler.rusjj.goomps", "GetOutOfMyPersonalSpace", "1.0.0")]

    public class GOOMPS : BaseUnityPlugin
    {
        internal static ConfigEntry<float> m_hCfgRadius;
        private static GOOMPS m_hInstance;
        internal static void Log(string msg) => m_hInstance.Logger.LogMessage(msg);
        void Awake()
        {
            var hCfgFile = new ConfigFile(Path.Combine(Paths.ConfigPath, "GOOMPS.cfg"), true);
            m_hCfgRadius = hCfgFile.Bind("CFG", "Radius", 0.25f, "Radius in.. meters?");
            if (m_hCfgRadius.Value < 0.01f) m_hCfgRadius.Value = 0.01f;
            else if (m_hCfgRadius.Value > 5.0f) m_hCfgRadius.Value = 5.0f;

            m_hInstance = this;
            Patch.Apply();
        }
        public static void SetRadius(float radius)
        {
            if (radius < 0.01f) m_hCfgRadius.Value = 0.01f;
            else if (radius > 5.0f) m_hCfgRadius.Value = 5.0f;
            else m_hCfgRadius.Value = radius;

            HideCollidingRigs hider = Object.FindObjectOfType<HideCollidingRigs>();
            SphereCollider collider = hider.GetComponent<SphereCollider>();
            collider.radius = m_hCfgRadius.Value;
        }
    }
}