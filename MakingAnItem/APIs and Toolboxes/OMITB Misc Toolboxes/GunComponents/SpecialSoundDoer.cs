using Alexandria.ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class SpecialSoundDoer : MonoBehaviour
    { 
        public void Awake()
        {
            m_gun = base.GetComponent<Gun>();
            m_gun.OnPostFired += HandlePostFired;
            m_gun.OnFinishAttack += HandleFinishAttack;
            m_gun.OnBurstContinued += HandleBurstContinued;
        }

        private void HandleBurstContinued(PlayerController arg1, Gun arg2)
        {
            this.HandleFinishAttack(arg1, arg2);
        }
        private void HandlePostFired(PlayerController arg1, Gun arg2)
        {
            if (!m_hasPlayedAudioThisShot)
            {
                m_hasPlayedAudioThisShot = true;
                if (
                    !string.IsNullOrEmpty(StartOfBurstSound) && 

                    (string.IsNullOrEmpty(StartOfBurstRequiredSynergy) || (arg2.GunPlayerOwner() && arg2.GunPlayerOwner().PlayerHasActiveSynergy(StartOfBurstRequiredSynergy))) &&
                    (string.IsNullOrEmpty(StartOfBurstRequiredNamePrefix) || (arg2.sprite != null && arg2.sprite.CurrentSprite != null && arg2.sprite.CurrentSprite.name.StartsWith(StartOfBurstRequiredNamePrefix)))
                    ) AkSoundEngine.PostEvent(StartOfBurstSound, arg2.gameObject);
            }
        }
        private void HandleFinishAttack(PlayerController sourcePlayer, Gun sourceGun)
        {
            m_hasPlayedAudioThisShot = false;
        }

        public string StartOfBurstRequiredNamePrefix = null;
        public string StartOfBurstRequiredSynergy = null;
        public string StartOfBurstSound = null;

        private Gun m_gun;
        private bool m_hasPlayedAudioThisShot;
    }

}
