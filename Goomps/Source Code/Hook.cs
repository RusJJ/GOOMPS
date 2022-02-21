using HarmonyLib;
using UnityEngine;

namespace GOOMPS
{
    [HarmonyPatch(typeof(GorillaLocomotion.Player))]
    [HarmonyPatch("Awake", MethodType.Normal)]
    internal class OnPlayerAwake
    {
        private static void Postfix(GorillaLocomotion.Player __instance)
        {
            GameObject hObj = new GameObject();
            GOOMPS.m_hCol = hObj.AddComponent<SphereCollider>();
            hObj.AddComponent<TransformFollow>().transformToFollow = __instance.bodyCollider.transform;
            hObj.AddComponent<HideCollidingRigs>();
        }
    }

    class HideCollidingRigs : MonoBehaviour
    {
        void Awake()
        {
            GOOMPS.m_hCol.isTrigger = true;
            GOOMPS.m_hCol.radius = GOOMPS.m_hCfgRadius.Value;
            gameObject.layer = 31;
        }
        void OnTriggerEnter(Collider hCol)
        {
            if (!GOOMPS.m_hCfgEnabled.Value) return;
            if (hCol.gameObject.name.Contains("Body"))
            {
                VRRig rig = hCol.transform.parent.parent.parent.gameObject.GetComponent<VRRig>();
                if (rig && !rig.photonView.IsMine)
                {
                    rig.mainSkin.enabled = false;
                }
            }
        }
        void OnTriggerExit(Collider hCol)
        {
            if (hCol.gameObject.name.Contains("Body"))
            {
                VRRig rig = hCol.transform.parent.parent.parent.gameObject.GetComponent<VRRig>();
                if (rig && !rig.photonView.IsMine)
                {
                    rig.mainSkin.enabled = true;
                }
            }
        }
    }
}