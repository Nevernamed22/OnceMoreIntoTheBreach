using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.Misc;
using SaveAPI;
using Dungeonator;

namespace NevernamedsItems
{
    public class RiteOfPassage : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Rite of Passage", "riteofpassage");
            Game.Items.Rename("outdated_gun_mods:rite_of_passage", "nn:rite_of_passage");
            gun.gameObject.AddComponent<RiteOfPassage>();
            gun.SetShortDescription("Blood Ritual");
            gun.SetLongDescription("Attempting to reload a full clip of this heretical weapon will harm all Gundead in the room- at the cost of the wielders own blood!"+"\n\nIn a forgotten age, a Blood Cult within the Gungeon used blades such as these to carve out the hearts of sacrifices to their dark god.");

            gun.SetGunSprites("riteofpassage", 8, false, 2);

            gun.SetAnimationFPS(gun.shootAnimation, 15);
            gun.SetAnimationFPS(gun.reloadAnimation, 15);
            StabAnim = gun.UpdateAnimation("stab", Initialisation.gunCollection2);
            gun.SetAnimationFPS(StabAnim, 8);
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);
            gun.gunSwitchGroup = (PickupObjectDatabase.GetById(377) as Gun).gunSwitchGroup;

            gun.AddPassiveStatModifier(PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0.5f;
            gun.DefaultModule.cooldownTime = 0.4f;
            gun.DefaultModule.numberOfShotsInClip = 15;
            gun.SetBarrel(51, 17);
            gun.SetBaseMaxAmmo(500);
            gun.gunClass = GunClass.FULLAUTO;

            //BULLET STATS
            Projectile projectile = ProjectileSetupUtility.MakeProjectile(56, 10f);
            projectile.SetProjectileSprite("riteofpassage_proj", 11, 13, true, tk2dBaseSprite.Anchor.MiddleCenter, 9, 9);
            projectile.baseData.UsesCustomAccelerationCurve = true;
            projectile.baseData.AccelerationCurve = AnimationCurve.Linear(0, 0.1f, 0.2f, 2f);
            projectile.hitEffects.overrideMidairDeathVFX = SharedVFX.PaleRedImpact;
            VFXPool pool = VFXToolbox.CreateBlankVFXPool(SharedVFX.PaleRedImpact);
            projectile.hitEffects.tileMapVertical = pool;
            projectile.hitEffects.tileMapHorizontal = pool;
            projectile.hitEffects.enemy = VFXToolbox.CreateBlankVFXPool(SharedVFX.BloodImpactVFX);
            projectile.gameObject.AddComponent<PierceProjModifier>();

            gun.DefaultModule.projectiles[0] = projectile;

            gun.muzzleFlashEffects = VFXToolbox.CreateVFXPoolBundle("RiteOfPassageMuzzle", false, 0, VFXAlignment.Fixed, 5, new Color32(255, 117, 117, 255));
            gun.gunHandedness = GunHandedness.AutoDetect;

            gun.AddClipSprites("riteofpassage");


            gun.quality = PickupObject.ItemQuality.B;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            gun.AddToSubShop(ItemBuilder.ShopType.Cursula);
            ID = gun.PickupObjectId;
        }
        public static int ID;
        public static string StabAnim;
        public override void Update()
        {
            if (isBusy && this.gun != null && this.gun.DefaultModule != null && this.gun.RuntimeModuleData != null && this.gun.RuntimeModuleData.ContainsKey(this.gun.DefaultModule))
            {
                if (!this.gun.RuntimeModuleData[this.gun.DefaultModule].onCooldown)
                {
                    this.gun.RuntimeModuleData[this.gun.DefaultModule].onCooldown = true;
                }
            }
            base.Update();
        }
        public override void OnReloadPressed(PlayerController player, Gun gun, bool manual)
        {
            if (!isBusy && gun.ClipShotsRemaining == gun.ClipCapacity && player && player.healthHaver)
            {
                if (ValidForStab(player)) base.StartCoroutine(Stabby(player));
            }
            base.OnReloadPressed(player, gun, manual);
        }
        public bool ValidForStab(PlayerController player)
        {
            float hits = (player.healthHaver.currentHealth / 0.5f) + player.healthHaver.Armor;
            return (hits > 1 && !player.healthHaver.NextShotKills && player.healthHaver.IsVulnerable && !player.IsDodgeRolling);
        }
        public IEnumerator Stabby(PlayerController player)
        {
            isBusy = true;
            base.gun.Play(StabAnim);
            player.inventory.GunLocked.AddOverride("Rite of Passage", null);
            yield return new WaitForSeconds(0.625f);

            if (ValidForStab(player))
            {
                bool ignoreArmour = player.healthHaver.Armor > 0 && player.healthHaver.currentHealth > 0;
                bool flawless = player.CurrentRoom != null && !player.CurrentRoom.PlayerHasTakenDamageInThisRoom;
                player.healthHaver.NextDamageIgnoresArmor = ignoreArmour;
                player.healthHaver.ApplyDamage(0.5f, Vector2.zero, "Rite of Passage", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
                if (!gun.InfiniteAmmo && gun.CanGainAmmo) { gun.GainAmmo(100); }
                SpawnManager.SpawnVFX(SharedVFX.BloodExplosion, player.specRigidbody.UnitCenter, Quaternion.identity);
                Pixelator.Instance.FadeToColor(0.5f, new Color32(255,0,0,100), true, 0.1f);
                StickyFrictionManager.Instance.RegisterCustomStickyFriction(0.15f, 1f, false, false);

                if (player.CurrentRoom != null)
                {
                    List<AIActor> activeEnemies = player.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                    if (activeEnemies != null)
                    {
                        for (int i = 0; i < activeEnemies.Count; i++)
                        {
                            AIActor aiactor = activeEnemies[i];
                            if (aiactor.IsNormalEnemy)
                            {
                                if (aiactor.healthHaver)
                                {
                                    aiactor.healthHaver.ApplyDamage(100f, Vector2.zero, "Rite of Passage", CoreDamageTypes.Magic, DamageCategory.Unstoppable);
                                    SpawnManager.SpawnVFX(SharedVFX.BloodImpactVFX, player.specRigidbody.UnitCenter, Quaternion.identity);

                                    if (UnityEngine.Random.value <= 0.5f) { aiactor.ApplyEffect(new GameActorExsanguinationEffect() { duration = 10f }); }
                                }
                            }
                        }
                    }
                }

                yield return new WaitForSeconds(0.1f);

                if (flawless && player.CurrentRoom != null) { player.CurrentRoom.PlayerHasTakenDamageInThisRoom = false; }

                yield return new WaitForSeconds(0.275f);
            }
            base.gun.PlayIdleAnimation();
            isBusy = false;

            if (this.gun != null && this.gun.DefaultModule != null && this.gun.RuntimeModuleData != null && this.gun.RuntimeModuleData.ContainsKey(this.gun.DefaultModule))
            {
                if (this.gun.RuntimeModuleData[this.gun.DefaultModule].onCooldown)
                {
                    this.gun.RuntimeModuleData[this.gun.DefaultModule].onCooldown = false;
                }
            }
            player.inventory.GunLocked.RemoveOverride("Rite of Passage");
            yield break;
        }
        public bool isBusy = false;
    }
}

