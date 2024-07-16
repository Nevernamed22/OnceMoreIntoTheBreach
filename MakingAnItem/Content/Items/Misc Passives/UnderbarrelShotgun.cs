using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using UnityEngine;
using MonoMod.RuntimeDetour;
using System.Reflection;
using SaveAPI;

namespace NevernamedsItems
{
    public class UnderbarrelShotgun : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<UnderbarrelShotgun>(
            "Underbarrel Shotgun",
            "Triple Barrel",
            "Fires a shotgun blast upon reloading.\n\nCreated by the infamous gunslinger \"Glass-wrists Junders\" to add more power to his sidearm. After the end of his ill-fated shooting career, this ridiculous artefact found its way to the Gungeon.",
            "underbarrelshotgun_icon");
            item.quality = PickupObject.ItemQuality.D;
            ID = item.PickupObjectId;
            item.SetupUnlockOnCustomStat(CustomTrackedStats.BEGGAR_TOTAL_DONATIONS, 154, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN);
        }
        public static int ID;
        public override void Pickup(PlayerController player)
        {
            player.OnReloadedGun += HandleGunReloaded;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player) player.OnReloadedGun -= HandleGunReloaded;
            base.DisableEffect(player);
        }
        private void HandleGunReloaded(PlayerController player, Gun playerGun)
        {
            if (canActivate == true)
            {
                for (int i = 0; i < 5; i++)
                {
                    Projectile fired = (PickupObjectDatabase.GetById(56) as Gun).DefaultModule.projectiles[0].InstantiateAndFireInDirection(playerGun.barrelOffset.position, base.Owner.CurrentGun == null ? 0f : base.Owner.CurrentGun.CurrentAngle, 40f, player).GetComponent<Projectile>();
                    fired.Owner = player;
                    fired.Shooter = player.specRigidbody;
                    fired.baseData.range = 10f;
                    fired.baseData.speed *= UnityEngine.Random.Range(0.8f, 1.2f);
                    fired.ScaleByPlayerStats(player);
                    player.DoPostProcessProjectile(fired);
                }
                    AkSoundEngine.PostEvent("Play_WPN_smileyrevolver_shot_01", player.gameObject);
                (PickupObjectDatabase.GetById(83) as Gun).muzzleFlashEffects.SpawnAtPosition(playerGun.barrelOffset.position, base.Owner.CurrentGun.CurrentAngle, null, null, null, -0.05f);

                canActivate = false;
                Invoke("Reset", 1f);
            }
        }
        public bool canActivate = true;
        void Reset()
        {
            canActivate = true;
        }
    }
}
