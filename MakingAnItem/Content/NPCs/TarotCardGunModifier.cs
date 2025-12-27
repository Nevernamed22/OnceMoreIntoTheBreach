using Alexandria.ItemAPI;
using Alexandria.Misc;
using HarmonyLib;
using HutongGames.PlayMaker.Actions;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

namespace NevernamedsItems
{
    public class TarotCardGunModifier : GunBehaviour
    {
        public List<TarotCardController.TarotCardData> ActiveTarotCards = new List<TarotCardController.TarotCardData>();
        public float flatHangedManDamageIncrease = 0;
        public Projectile reloadVolleyProjectile = null;
        public int NumOfTarotApplied(TarotCardController.TarotCards tarotType)
        {
            int i = 0;
            foreach (TarotCardController.TarotCardData dat in ActiveTarotCards)
            {
                if (dat.tarotCard == tarotType) { i++; }
            }
            return i;
        }
        private void Start()
        {
            attachedGun = base.GetComponent<Gun>();
        }
        public Gun attachedGun;
        public void RegisterNewTarotCard(TarotCardController.TarotCardData card)
        {
            ActiveTarotCards.Add(card);
            if (card.OnRegisterWithGun != null) { card.OnRegisterWithGun(attachedGun, attachedGun.GunPlayerOwner()); }
            if (attachedGun && attachedGun.GunPlayerOwner()) { attachedGun.GunPlayerOwner().stats.RecalculateStats(attachedGun.GunPlayerOwner()); }
        }
        public override void InheritData(Gun sourceGun)
        {
            base.InheritData(sourceGun);
            if (sourceGun.gameObject.GetComponent<TarotCardGunModifier>())
            {
                this.ActiveTarotCards = sourceGun.gameObject.GetComponent<TarotCardGunModifier>().ActiveTarotCards;
            }
        }
        public override void PostProcessProjectile(Projectile projectile)
        {
            projectile.baseData.damage += flatHangedManDamageIncrease;
            projectile.OnHitEnemy += OnHitEnemy;
            foreach (TarotCardController.TarotCardData activeTarot in ActiveTarotCards)
            {
                if (activeTarot.OnFiredBullet != null) { activeTarot.OnFiredBullet(projectile, attachedGun.GunPlayerOwner(), attachedGun); }
            }
            base.PostProcessProjectile(projectile);
        }
        public override void PostProcessBeamChanceTick(BeamController beam)
        {
            foreach (TarotCardController.TarotCardData activeTarot in ActiveTarotCards)
            {
                if (activeTarot.OnBeamChanceTick != null) { activeTarot.OnBeamChanceTick(beam, attachedGun.GunPlayerOwner(), attachedGun); }
            }
            base.PostProcessBeam(beam);
        }
        public override void PostProcessBeam(BeamController beam)
        {
            if (beam && beam.projectile)
            {
                beam.projectile.OnHitEnemy += OnHitEnemy;
                beam.projectile.baseData.damage += flatHangedManDamageIncrease;
            }
            foreach (TarotCardController.TarotCardData activeTarot in ActiveTarotCards)
            {
                if (activeTarot.OnFiredBeam != null) { activeTarot.OnFiredBeam(beam, attachedGun.GunPlayerOwner(), attachedGun); }
            }
            base.PostProcessBeam(beam);
        }
        public void OnHitEnemy(Projectile projectile, SpeculativeRigidbody enemy, bool fatal)
        {
            foreach (TarotCardController.TarotCardData activeTarot in ActiveTarotCards)
            {
                if (activeTarot.OnHitEnemy != null) { activeTarot.OnHitEnemy(projectile, enemy, fatal); }
            }
        }
        public override void OnReloadedPlayer(PlayerController owner, Gun gun)
        {
            foreach (TarotCardController.TarotCardData activeTarot in ActiveTarotCards)
            {
                if (activeTarot.OnGunReloaded != null) { activeTarot.OnGunReloaded(owner, gun); }
            }
            if (gun.ClipShotsRemaining == 0 && reloadVolleyProjectile != null)
            {
                float damageTotal = 30;
                float indivDamage = reloadVolleyProjectile.baseData.damage;
                int numToFire = Mathf.CeilToInt(damageTotal / indivDamage);
                for (int i = 0; i < numToFire; i++)
                {
                    FireIndiv(owner, gun);
                }
            }
            base.OnReloadedPlayer(owner, gun);
        }
        private void FireIndiv(PlayerController owner, Gun gun)
        {
            float angle = ProjSpawnHelper.GetAccuracyAngled(gun.CurrentAngle, 25f, owner);
            GameObject spawnObj = SpawnManager.SpawnProjectile(reloadVolleyProjectile.gameObject, gun.barrelOffset.position, Quaternion.Euler(new Vector3(0f, 0f, angle)));
            Projectile component = spawnObj.GetComponent<Projectile>();
            if (component != null)
            {
                component.Owner = owner;
                component.Shooter = owner.specRigidbody;
                component.baseData.speed *= (1f + UnityEngine.Random.Range(-5f, 5f) / 100f);
                component.UpdateSpeed();
                owner.DoPostProcessProjectile(component);
            }
        }
    }
}
