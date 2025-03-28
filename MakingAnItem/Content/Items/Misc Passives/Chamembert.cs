using System;
using System.Collections.Generic;
using System.Linq;
using Gungeon;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using Dungeonator;
using System.Collections;
using SaveAPI;

namespace NevernamedsItems
{
    public class Chamembert : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<Chamembert>(
            "Chamembert",
            "Wheel Lock",
            "Creates a fine cheesy eruption on reloading." + "\n\nThis creamy variety of cheese is a Gungeon original, its recipe known only to the most elder and revered 'cheesemasters' of the Order.",
            "chamembert_icon");
            item.quality = PickupObject.ItemQuality.A;

            ID = item.PickupObjectId;
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.BEATEN_RAT_BOSS_TURBO_MODE, true);
        }
        public static int ID;
        private void HandleGunReloaded(PlayerController player, Gun playerGun)
        {
            if (playerGun.ClipShotsRemaining == 0 && !active)
            {
                float chance = 1f;
                if (playerGun.InfiniteAmmo && playerGun.PickupObjectId != 626) chance = 0.4f;
                if (UnityEngine.Random.value <= chance) player.StartCoroutine(Encheese(player));
            }
        }
        public bool active = false;
        public IEnumerator Encheese(PlayerController user)
        {
            active = true;
            GameObject sp = UnityEngine.Object.Instantiate<GameObject>(SharedVFX.HighPriestImplosionRing, user.specRigidbody.UnitCenter, Quaternion.identity);
            tk2dBaseSprite component = sp.GetComponent<tk2dBaseSprite>();
            sp.transform.parent = user.transform;
            component.HeightOffGround = 0.2f;
            user.sprite.AttachRenderer(component);
            AkSoundEngine.PostEvent("Play_ITM_Macho_Brace_Trigger_01", user.gameObject);
            yield return new WaitForSeconds(1f);
            AkSoundEngine.PostEvent("Play_BOSS_Rat_Cheese_Burst_01", user.gameObject);
            UnityEngine.Object.Instantiate<GameObject>(StaticStatusEffects.elimentalerCheeseEffect.vfxExplosion, user.specRigidbody.UnitCenter, Quaternion.identity);
            DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.CheeseDef).TimedAddGoopCircle(user.CenterPosition, 4);
            if (user.GetExtComp()) { user.GetExtComp().TriggerInvulnerableFrames(0.25f); }
            List<AIActor> activeEnemies = user.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            if (activeEnemies != null)
            {
                for (int i = 0; i < activeEnemies.Count; i++)
                {
                    AIActor aiactor = activeEnemies[i];
                    if (aiactor.IsNormalEnemy)
                    {
                        float num = Vector2.Distance(user.CenterPosition, aiactor.CenterPosition);
                        if (num <= 6)
                        {
                            aiactor.ApplyEffect(StaticStatusEffects.elimentalerCheeseEffect);
                        }
                    }
                }
            }
            bool ElimentalerSynergy = false;
            if (user.CurrentGun.PickupObjectId == 626) { ElimentalerSynergy = true; }
            int amt = ElimentalerSynergy ? 1 : 5;
            if (user.PlayerHasActiveSynergy("To Brie or Not To Brie")) { amt *= 2; }
            for (int j = 0; j < amt; j++)
            {
                Projectile toSpawn = UnityEngine.Random.value <= 0.5f ? (PickupObjectDatabase.GetById(626) as Gun).DefaultModule.projectiles[0] : (PickupObjectDatabase.GetById(626) as Gun).DefaultModule.projectiles[1];
                if (ElimentalerSynergy) { toSpawn = (PickupObjectDatabase.GetById(808) as Gun).DefaultModule.projectiles[0]; }
                Projectile spawned = toSpawn.InstantiateAndFireInDirection(user.specRigidbody.UnitCenter, ElimentalerSynergy ? user.CurrentGun.CurrentAngle : BraveUtility.RandomAngle()).GetComponent<Projectile>();
                spawned.Owner = user;
                spawned.Shooter = user.specRigidbody;
                if (!ElimentalerSynergy)
                {
                    spawned.baseData.range = UnityEngine.Random.Range(5f, 9f);
                    spawned.baseData.speed *= UnityEngine.Random.Range(0.8f, 1.1f);
                    GoopModifier gooper = spawned.gameObject.AddComponent<GoopModifier>();
                    gooper.SpawnGoopOnCollision = true;
                    gooper.goopDefinition = EasyGoopDefinitions.CheeseDef;
                    gooper.CollisionSpawnRadius = 1;
                }
                spawned.UpdateSpeed();

                if (user.PlayerHasActiveSynergy("To Brie or Not To Brie") && !ElimentalerSynergy) { spawned.gameObject.AddComponent<BounceProjModifier>().numberOfBounces = 1; }
                if (user.PlayerHasActiveSynergy("Cordon Blue"))
                {
                    spawned.baseData.range = 1000;
                    if (UnityEngine.Random.value <= 0.07f) spawned.gameObject.AddComponent<BlankProjModifier>().blankType = EasyBlankType.MINI;
                }

            }
            yield return new WaitForSeconds(user.PlayerHasActiveSynergy("To Brie or Not To Brie") ? 2 : 3);
            active = false;
            yield break;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            active = false;
            player.OnReloadedGun += this.HandleGunReloaded;
        }
        public override void DisableEffect(PlayerController player)
        {
            if (player) player.OnReloadedGun -= HandleGunReloaded;
            base.DisableEffect(player);
        }
    }

}