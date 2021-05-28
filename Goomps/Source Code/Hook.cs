using HarmonyLib;
using Photon.Pun;
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
            hObj.AddComponent<SphereCollider>();
            hObj.AddComponent<TransformFollow>().transformToFollow = __instance.bodyCollider.transform;
            hObj.AddComponent<HideCollidingRigs>();
        }
    }

    class HideCollidingRigs : MonoBehaviour
    {
        void Awake()
        {
            SphereCollider hCol = GetComponent<SphereCollider>();
            hCol.isTrigger = true;
            hCol.radius = GOOMPS.m_hCfgRadius.Value;
            gameObject.layer = 20;
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