using Alexandria.ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class SharedVFX
    {
        //Basegame VFX Objects
        public static GameObject WeakenedStatusEffectOverheadVFX = ResourceCache.Acquire("Global VFX/VFX_Debuff_Status") as GameObject;
        public static GameObject SpiratTeleportVFX;
        public static GameObject LaserSight = LoadHelper.LoadAssetFromAnywhere("assets/resourcesbundle/global vfx/vfx_lasersight.prefab") as GameObject;
        public static GameObject TeleporterPrototypeTelefragVFX = PickupObjectDatabase.GetById(449).GetComponent<TeleporterPrototypeItem>().TelefragVFXPrefab.gameObject;
        public static GameObject BloodiedScarfPoofVFX = PickupObjectDatabase.GetById(436).GetComponent<BlinkPassiveItem>().BlinkpoofVfx.gameObject;
        public static GameObject ChestTeleporterTimeWarp = (PickupObjectDatabase.GetById(573) as ChestTeleporterItem).TeleportVFX;
        public static GameObject MachoBraceDustUpVFX = PickupObjectDatabase.GetById(665).GetComponent<MachoBraceItem>().DustUpVFX;
        public static GameObject MachoBraceBurstVFX = PickupObjectDatabase.GetById(665).GetComponent<MachoBraceItem>().BurstVFX;
        public static GameObject MachoBraceOverheadVFX = PickupObjectDatabase.GetById(665).GetComponent<MachoBraceItem>().OverheadVFX;
        public static GameObject LastBulletStandingX;
        public static GameObject HealingSparkles = BraveResources.Load<GameObject>("Global VFX/VFX_Healing_Sparkles_001", ".prefab");
        //Projectile Death Effects
        public static GameObject GreenLaserCircleVFX = (PickupObjectDatabase.GetById(89) as Gun).DefaultModule.projectiles[0].hitEffects.overrideMidairDeathVFX;
        public static GameObject YellowLaserCircleVFX = (PickupObjectDatabase.GetById(651) as Gun).DefaultModule.projectiles[0].hitEffects.overrideMidairDeathVFX;
        public static GameObject RedLaserCircleVFX = (PickupObjectDatabase.GetById(32) as Gun).DefaultModule.projectiles[0].hitEffects.overrideMidairDeathVFX;
        public static GameObject BlueLaserCircleVFX = (PickupObjectDatabase.GetById(59) as Gun).DefaultModule.projectiles[0].hitEffects.enemy.effects[0].effects[0].effect;
        public static GameObject SmoothLightBlueLaserCircleVFX = (PickupObjectDatabase.GetById(576) as Gun).DefaultModule.projectiles[0].hitEffects.overrideMidairDeathVFX;
        public static GameObject SmoothLightGreenLaserCircleVFX = (PickupObjectDatabase.GetById(360) as Gun).DefaultModule.projectiles[0].hitEffects.overrideMidairDeathVFX;
        public static GameObject WhiteCircleVFX = (PickupObjectDatabase.GetById(330) as Gun).DefaultModule.projectiles[0].hitEffects.overrideMidairDeathVFX;
        public static GameObject BlueFrostBlastVFX = (PickupObjectDatabase.GetById(225) as Gun).DefaultModule.projectiles[0].hitEffects.overrideMidairDeathVFX;
        public static GameObject RedFireBlastVFX = (PickupObjectDatabase.GetById(125) as Gun).DefaultModule.projectiles[0].hitEffects.overrideMidairDeathVFX;
        public static GameObject SmallMagicPuffVFX = (PickupObjectDatabase.GetById(338) as Gun).DefaultModule.projectiles[0].hitEffects.overrideMidairDeathVFX;
        public static GameObject BigDustCloud = (PickupObjectDatabase.GetById(37) as Gun).DefaultModule.chargeProjectiles[0].Projectile.hitEffects.overrideMidairDeathVFX;
        public static GameObject BloodImpactVFX = (PickupObjectDatabase.GetById(369) as Gun).DefaultModule.chargeProjectiles[0].Projectile.hitEffects.tileMapVertical.effects[0].effects[0].effect;

        //Basegame VFX Pools
        public static VFXPool SpiratTeleportVFXPool;
        public static GameObject HighPriestImplosionRing;

        //-----------------------------------------------------------  CUSTOM OVERHEAD VFX

        //Status Effect VFX
        public static GameObject ERRORShellsOverhead;
        public static GameObject PlagueOverhead;
        public static GameObject FriendlyOverhead;
        public static GameObject LockdownOverhead;
        public static GameObject JarateDrip;
        public static GameObject CryingOverhead;
        public static GameObject GildedOverhead;

        //Custom MuzzleFlashes
        public static VFXPool DoomBoomMuzzle = VFXToolbox.CreateVFXPoolBundle("DoomBoomMuzzle", false, 0, VFXAlignment.Fixed);

        //Custom Bullet Impact VFX
        public static GameObject PurpleLaserCircleVFX;
        public static GameObject BigWhitePoofVFX;
        public static GameObject SmallHeartImpact;
        public static GameObject W3irdstarImpact;
        public static GameObject SquarePegImpact;
        public static GameObject ArcImpact;
        public static GameObject ArcImpactRed;
        public static GameObject PaleRedImpact;

        //Explosion Shards
        public static GameObject BlueExplosionShard;

        //Explosions
        public static GameObject ArcExplosion;
        public static GameObject ArcExplosionRed;
        public static GameObject JarateExplosion;
        public static GameObject BloodExplosion;
        public static GameObject KillDevilExplosion;

        //Coloured Poofs
        public static GameObject ColouredPoofRed;
        public static GameObject ColouredPoofOrange;
        public static GameObject ColouredPoofYellow;
        public static GameObject ColouredPoofGreen;
        public static GameObject ColouredPoofBlue;
        public static GameObject ColouredPoofIndigo;
        public static GameObject ColouredPoofBrown;
        public static GameObject ColouredPoofWhite;
        public static GameObject ColouredPoofCyan;
        public static GameObject ColouredPoofGrey;
        public static GameObject ColouredPoofGold;
        public static GameObject ColouredPoofPink;

        //Stat Up VFX
        public static GameObject DamageUpVFX;
        public static GameObject ShotSpeedUpVFX;
        public static GameObject SpeedUpVFX;
        public static GameObject FirerateUpVFX;
        public static GameObject AccuracyUpVFX;
        public static GameObject KnockbackUpVFX;
        public static GameObject ReloadSpeedUpVFX;

        //Misc
        public static GameObject GundetaleSpareVFX;
        public static GameObject LoveBurstAOE;
        public static GameObject LaserSlash;
        public static GameObject LaserSlashUndertale;
        public static GameObject TenPointsPopup;
        public static GameObject GoldenSparkle;
        public static GameObject BulletSmokeTrail;
        public static GameObject BulletSparkTrail;
        public static GameObject BloodCandleVFX;
        public static GameObject TinyBluePoofVFX;
        public static GameObject BlueSparkle;
        public static GameObject PinkSparkle;

        public static void Init()
        {
            GatherVanillaEffects();

            //Status Effect VFX
            ERRORShellsOverhead = VFXToolbox.CreateVFXBundle("ErrorShellsOverhead", false, 0f);
            PlagueOverhead = VFXToolbox.CreateVFXBundle("PlagueOverhead", false, 0f);
            FriendlyOverhead = VFXToolbox.CreateVFXBundle("FriendlyOverhead", false, 0f);
            LockdownOverhead = VFXToolbox.CreateVFXBundle("LockdownOverhead", false, 0f);
            JarateDrip = VFXToolbox.CreateVFXBundle("JarateDrip", false, 0f);
            CryingOverhead = VFXToolbox.CreateVFXBundle("CryingOverhead", false, 0f);
            GildedOverhead = VFXToolbox.CreateVFXBundle("GildedOverhead", false, 0f);

            //Custom Bullet Impact VFX
            PurpleLaserCircleVFX = VFXToolbox.CreateVFXBundle("PurpleLaserImpact", false, 0f);
            BigWhitePoofVFX = VFXToolbox.CreateVFXBundle("BigWhitePoof", false, 0f);
            SmallHeartImpact = VFXToolbox.CreateVFXBundle("LovePistolImpact", true, 5f);
            W3irdstarImpact = VFXToolbox.CreateVFXBundle("W3irdstarImpact", false, 0f);
            SquarePegImpact = VFXToolbox.CreateVFXBundle("SquarePegImpact", false, 0f);
            ArcImpact = VFXToolbox.CreateVFXBundle("ArcImpact", true, 0.18f, 20, 5, new Color32(145, 223, 255, 255));
            ArcImpactRed = VFXToolbox.CreateVFXBundle("ArcImpactRed", true, 0.18f, 20, 5, new Color32(255, 90, 90, 255));
            PaleRedImpact = VFXToolbox.CreateVFXBundle("PaleRedImpact", true, 1f, 7, 7, new Color32(255, 117, 117, 255));

            //Explosion Shards
            BlueExplosionShard = Breakables.GenerateDebrisObject(Initialisation.VFXCollection,
                   "blueexplosiondebris",
                   true,
                   1,
                   1,
                   360,
                   0,
                   null,
                   1,
                   null,
                   null,
                   1
                   ).gameObject;
            GameObject clonedSmoker = (PickupObjectDatabase.GetById(304) as ComplexProjectileModifier).ExplosionData.effect.transform.Find("vfx_explosion_debris_001").gameObject.GetComponent<ExplosionDebrisLauncher>().debrisSources[0].transform.Find("vfx_explosion_subsidiary_001").gameObject.InstantiateAndFakeprefab();
            clonedSmoker.transform.SetParent(BlueExplosionShard.transform);

            //Explosions
            ArcExplosion = VFXToolbox.CreateVFXBundle("ArcExplosion", true, 0.18f, 20, 5, new Color32(145, 223, 255, 255));
            ArcExplosionRed = VFXToolbox.CreateVFXBundle("ArcExplosionRed", true, 0.18f, 20, 5, new Color32(255, 90, 90, 255));
            JarateExplosion = VFXToolbox.CreateVFXBundle("JarateExplosion", false, 0f);
            BloodExplosion = VFXToolbox.CreateVFXBundle("BloodExplosion", false, 0f);
            KillDevilExplosion = VFXToolbox.CreateVFXBundle("KillDevilExplosion", true, 0.18f, 5, 10, new Color32(149, 197, 246, 255));
            GameObject KillDevilDebrisLauncher = new GameObject("Debris Launcher");
            KillDevilDebrisLauncher.transform.SetParent(KillDevilExplosion.transform);
            ExplosionDebrisLauncher KillDevilLNCHR = KillDevilDebrisLauncher.AddComponent<ExplosionDebrisLauncher>();
            KillDevilLNCHR.minExpulsionForce = 3;
            KillDevilLNCHR.maxExpulsionForce = 7;
            KillDevilLNCHR.maxShards = 2;
            KillDevilLNCHR.minShards = 1;
            KillDevilLNCHR.debrisSources = new List<DebrisObject>() { BlueExplosionShard.GetComponent<DebrisObject>() }.ToArray();


            //Coloured Poofs
            ColouredPoofRed = VFXToolbox.CreateVFXBundle("ColourPoofRed", false, 0f);
            ColouredPoofOrange = VFXToolbox.CreateVFXBundle("ColourPoofOrange", false, 0f);
            ColouredPoofYellow = VFXToolbox.CreateVFXBundle("ColourPoofYellow", false, 0f);
            ColouredPoofGreen = VFXToolbox.CreateVFXBundle("ColourPoofGreen", false, 0f);
            ColouredPoofBlue = VFXToolbox.CreateVFXBundle("ColourPoofBlue", false, 0f);
            ColouredPoofIndigo = VFXToolbox.CreateVFXBundle("ColourPoofIndigo", false, 0f);
            ColouredPoofBrown = VFXToolbox.CreateVFXBundle("ColourPoofBrown", false, 0f);
            ColouredPoofWhite = VFXToolbox.CreateVFXBundle("ColourPoofWhite", false, 0f);
            ColouredPoofCyan = VFXToolbox.CreateVFXBundle("ColourPoofCyan", false, 0f);
            ColouredPoofGrey = VFXToolbox.CreateVFXBundle("ColourPoofGrey", false, 0f);
            ColouredPoofGold = VFXToolbox.CreateVFXBundle("ColourPoofGold", false, 0f);
            ColouredPoofPink = VFXToolbox.CreateVFXBundle("ColourPoofPink", false, 0f);

            //Stat Up VFX
            SpeedUpVFX = VFXToolbox.CreateVFXBundle("SpeedUpVFX", true, 0.18f);

            //Misc
            GundetaleSpareVFX = VFXToolbox.CreateVFXBundle("GundertaleSpare", true, 0.18f, 5, 5, new Color32(255, 229, 87, 255));
            LoveBurstAOE = VFXToolbox.CreateVFXBundle("LoveBurstAOE", true, 0.18f);
            LaserSlash = VFXToolbox.CreateVFXBundle("LaserSlash", true, 0.18f, 10, 10, new Color32(255, 116, 255, 255));
            LaserSlashUndertale = VFXToolbox.CreateVFXBundle("LaserSlashUndertale", true, 0.18f, 10, 10, new Color32(255, 116, 255, 255));
            TenPointsPopup = VFXToolbox.CreateVFXBundle("TenPointsPopup", true, 0.18f);
            GoldenSparkle = VFXToolbox.CreateVFXBundle("GoldenSparkle", false, 0f);
            BulletSmokeTrail = VFXToolbox.CreateVFXBundle("BulletSmokeTrail", false, 0);
            BulletSparkTrail = VFXToolbox.CreateVFXBundle("BulletSparkTrail", false, 0, 10, 10, new Color32(246, 217, 101, 255));
            BloodCandleVFX = VFXToolbox.CreateVFXBundle("BloodCandleVFX", false, 0, 10, 20, new Color32(255, 0, 0, 255));
            TinyBluePoofVFX = VFXToolbox.CreateVFXBundle("TinyBluePoof", false, 0, 3, 5, new Color32(149, 197, 246, 255));
            BlueSparkle = VFXToolbox.CreateVFXBundle("DiamondSparkle", new IntVector2(7, 7), tk2dBaseSprite.Anchor.MiddleCenter, true, 0.4f);
            PinkSparkle = VFXToolbox.CreateVFXBundle("PinkSparkle", new IntVector2(7, 7), tk2dBaseSprite.Anchor.MiddleCenter, true, 0.4f);
        }
        public static void GatherVanillaEffects()
        {
            //Last Bullet Standing VFX
            GameObject ChallengeManagerReference = LoadHelper.LoadAssetFromAnywhere<GameObject>("_ChallengeManager");
            LastBulletStandingX = (ChallengeManagerReference.GetComponent<ChallengeManager>().PossibleChallenges[0].challenge as BestForLastChallengeModifier).KingVFX;
            //Spirat Teleportation VFX
            #region SpiratTP
            GameObject teleportBullet = EnemyDatabase.GetOrLoadByGuid("7ec3e8146f634c559a7d58b19191cd43").bulletBank.GetBullet("self").BulletObject;
            Projectile proj = teleportBullet.GetComponent<Projectile>();
            if (proj != null)
            {
                TeleportProjModifier tp = proj.GetComponent<TeleportProjModifier>();
                if (tp != null)
                {
                    SpiratTeleportVFXPool = tp.teleportVfx;
                    SpiratTeleportVFX = tp.teleportVfx.effects[0].effects[0].effect;
                }
            }
            #endregion

            //High Priest Slorp
            AIAnimator aiAnimator = EnemyDatabase.GetOrLoadByGuid("6c43fddfd401456c916089fdd1c99b1c").aiAnimator;
            List<AIAnimator.NamedVFXPool> namedVFX = aiAnimator.OtherVFX;
            foreach (AIAnimator.NamedVFXPool pool in namedVFX)
            {
                if (pool.name == "mergo")
                {
                    foreach (VFXComplex vFXComplex in pool.vfxPool.effects)
                    {
                        foreach (VFXObject vFXObject in vFXComplex.effects)
                        {
                            HighPriestImplosionRing = vFXObject.effect;
                        }
                    }
                }
            }
        }
    }
}
