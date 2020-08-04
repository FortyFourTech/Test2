using System;
using UnityEngine;
using Photon.Pun;

namespace Dimar.Networking
{
    public class OwnerDependentSwitch : MonoBehaviour
    {
        [Tooltip("If not filled will try to get one from the same GO")]
        [SerializeField] private PhotonView _photonView;

        [Header("Local")]
        [Tooltip("Rules to execute on local client")]
        [SerializeField] private SwitchRule[] _localRules;

        [Header("Remote")]
        [Tooltip("Rules to execute on remote client")]
        [SerializeField] private SwitchRule[] _remoteRules;

        /// <summary>
        /// Do it in OnEnable because photon view can't know "isMine" before its awake.
        /// So we can't know if our awake is earlier or later then of PhotonVie.
        /// </summary>
        private void OnEnable()
        {
            if (!_photonView)
            {
                _photonView = GetComponent<PhotonView>();
            }

            if (!_photonView)
            {
                Debug.Log("photon view is absent");
                return;
            }

            SwitchRule[] rules;
            if (_photonView.IsMine)
            {
                rules = _localRules;
            }
            else
            {
                rules = _remoteRules;
            }

            foreach (var rule in rules)
            {
                var applied = false;
                switch(rule.action)
                {
                    case ESwitcherAction.enable:
                        applied = SetActive(rule.target, true);
                        break;
                    case ESwitcherAction.disable:
                        applied = SetActive(rule.target, false);
                        break;
                    case ESwitcherAction.remove:
                        if (rule.target is Behaviour)
                        {
                            DestroyImmediate(rule.target as Behaviour);
                            applied = true;
                        }
                        else if (rule.target is GameObject)
                        {
                            DestroyImmediate(rule.target as GameObject);
                            applied = true;
                        }
                        break;
                }
            }
        }

        private bool SetActive(UnityEngine.Object obj, bool active)
        {
            var applied = false;
            if (obj is Behaviour)
            {
                (obj as Behaviour).enabled = true;
                applied = true;
            }
            else if (obj is GameObject)
            {
                (obj as GameObject).SetActive(true);
                applied = true;
            }
            return applied;
        }

        [Serializable]
        private struct SwitchRule
        {
            public UnityEngine.Object target;
            public ESwitcherAction action;
        }
    }

    public enum ESwitcherAction
    {
        enable,
        disable,
        remove,
    }
}
