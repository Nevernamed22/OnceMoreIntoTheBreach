using Dungeonator;
//using GungeonAPI;
using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class ExtendedColours
    {
        public static Color freezeBlue = EasyStatusEffectAccess.freezeModifierEffect.TintColor;
        public static Color poisonGreen = EasyStatusEffectAccess.irradiatedLeadEffect.TintColor;
        public static Color pink = new Color(242f / 255f, 116f / 255f, 225f / 255f);
        public static Color paleYellow = new Color(242f / 255f, 238f / 255f, 148f / 255f);
        public static Color lime = new Color(111f / 255f, 252f / 255f, 3f / 255f);
        public static Color brown = new Color(122f / 255f, 71f / 255f, 16f / 255f);
        public static Color orange = new Color(240f / 255f, 160f / 255f, 22f / 255f);
        public static Color vibrantOrange = new Color(255f / 255f, 144f / 255f, 41f / 255f);
        public static Color purple = new Color(171f / 255f, 22f / 255f, 240f / 255f);
        public static Color skyblue = new Color(130f / 255f, 230f / 255f, 2255f / 255f);
        public static Color honeyYellow = new Color(255f / 255f, 180f / 255f, 18f / 255f);
        public static Color maroon = new Color(105f / 255f, 7f / 255f, 9f / 255f);
        public static Color veryDarkRed = new Color(71f / 255f, 4f / 255f, 3f / 255f);
        public static Color plaguePurple = new Color(242f / 255f, 161f / 255f, 255f / 255f);
    }
    public class EasyStatusEffectAccess
    {
        public static GameActorPlagueEffect commonPlague = new GameActorPlagueEffect
        {
            duration = 10,
            effectIdentifier = "Plague",
            resistanceType = EffectResistanceType.None,
            DamagePerSecondToEnemies = 2f,
            ignitesGoops = false,

            OverheadVFX = EasyVFXDatabase.plagueVFXObject,
            AffectsEnemies = true,
            AffectsPlayers = false,
            AppliesOutlineTint = false,
            PlaysVFXOnActor = false,
        };
        public static GameActorHealthEffect irradiatedLeadEffect = Gungeon.Game.Items["irradiated_lead"].GetComponent<BulletStatusEffectItem>().HealthModifierEffect;
        public static GameActorFreezeEffect freezeModifierEffect = PickupObjectDatabase.GetById(278).GetComponent<BulletStatusEffectItem>().FreezeModifierEffect;
        public static GameActorFireEffect hotLeadEffect = Gungeon.Game.Items["hot_lead"].GetComponent<BulletStatusEffectItem>().FireModifierEffect;
        public static GameActorCharmEffect charmingRoundsEffect = Gungeon.Game.Items["charming_rounds"].GetComponent<BulletStatusEffectItem>().CharmModifierEffect;
        public static GameActorSpeedEffect tripleCrossbowSlowEffect = (ETGMod.Databases.Items["triple_crossbow"] as Gun).DefaultModule.projectiles[0].speedEffect;
    }
    public class EasyExplosionDataStorage
    {
        public static ExplosionData explosiveRoundsExplosion = Gungeon.Game.Items["explosive_rounds"].GetComponent<ComplexProjectileModifier>().ExplosionData;
    }
    public class EasyVFXDatabase
    {
        public static void InitComplexVFX()
        {
            //Spirat Teleport VFX
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

            //PLAGUE OVERHEAD

            plagueVFXObject = SpriteBuilder.SpriteFromResource("NevernamedsItems/Resources/lockdown_effect_icon", new GameObject("PlagueIcon"));
            plagueVFXObject.SetActive(false);
            tk2dBaseSprite plaguevfxSprite = plagueVFXObject.GetComponent<tk2dBaseSprite>();
            plaguevfxSprite.GetCurrentSpriteDef().ConstructOffsetsFromAnchor(tk2dBaseSprite.Anchor.LowerCenter, plaguevfxSprite.GetCurrentSpriteDef().position3);
            FakePrefab.MarkAsFakePrefab(plagueVFXObject);
            UnityEngine.Object.DontDestroyOnLoad(plagueVFXObject);

            tk2dSpriteAnimator plagueanimator = plagueVFXObject.AddComponent<tk2dSpriteAnimator>();
            plagueanimator.Library = plagueVFXObject.AddComponent<tk2dSpriteAnimation>();
            plagueanimator.Library.clips = new tk2dSpriteAnimationClip[0];

            tk2dSpriteAnimationClip clip = new tk2dSpriteAnimationClip { name = "PlagueIconClip", fps = 7, frames = new tk2dSpriteAnimationFrame[0] };
            foreach (string path in PlagueVFXPaths)
            {
                int spriteId = SpriteBuilder.AddSpriteToCollection(path, plagueVFXObject.GetComponent<tk2dBaseSprite>().Collection);               

                tk2dSpriteAnimationFrame frame = new tk2dSpriteAnimationFrame { spriteId = spriteId, spriteCollection = plagueVFXObject.GetComponent<tk2dBaseSprite>().Collection };                
                clip.frames = clip.frames.Concat(new tk2dSpriteAnimationFrame[] { frame }).ToArray();
            }
            plagueanimator.Library.clips = plagueanimator.Library.clips.Concat(new tk2dSpriteAnimationClip[] { clip }).ToArray();
            plagueanimator.playAutomatically = true;
            plagueanimator.DefaultClipId = plagueanimator.GetClipIdByName("PlagueIconClip");
        }
        public static GameObject WeakenedStatusEffectOverheadVFX = ResourceCache.Acquire("Global VFX/VFX_Debuff_Status") as GameObject;
        public static VFXPool SpiratTeleportVFXPool;
        public static GameObject SpiratTeleportVFX;
        public static GameObject TeleporterPrototypeTelefragVFX = PickupObjectDatabase.GetById(449).GetComponent<TeleporterPrototypeItem>().TelefragVFXPrefab.gameObject;
        public static GameObject BloodiedScarfPoofVFX = PickupObjectDatabase.GetById(436).GetComponent<BlinkPassiveItem>().BlinkpoofVfx.gameObject;
        public static GameObject ChestTeleporterTimeWarp = (PickupObjectDatabase.GetById(573) as ChestTeleporterItem).TeleportVFX;

        public static List<string> PlagueVFXPaths = new List<string>()
        {
            "NevernamedsItems/Resources/StatusEffectVFX/plaguevfxframe_001",
            "NevernamedsItems/Resources/StatusEffectVFX/plaguevfxframe_002",
            "NevernamedsItems/Resources/StatusEffectVFX/plaguevfxframe_003",
            "NevernamedsItems/Resources/StatusEffectVFX/plaguevfxframe_004",
            "NevernamedsItems/Resources/StatusEffectVFX/plaguevfxframe_005",
        };
        public static GameObject plagueVFXObject;
    }
    public class EasyPlaceableObjects
    {
        public static GameObject CoffinVert = LoadHelper.LoadAssetFromAnywhere<GameObject>("Coffin_Vertical");
        public static GameObject CoffinHoriz = LoadHelper.LoadAssetFromAnywhere<GameObject>("Coffin_Horizontal");
        public static GameObject Brazier = LoadHelper.LoadAssetFromAnywhere<DungeonPlaceable>("brazier").variantTiers[0].GetOrLoadPlaceableObject;
        public static GameObject CursedPot = LoadHelper.LoadAssetFromAnywhere<DungeonPlaceable>("Curse Pot").variantTiers[0].GetOrLoadPlaceableObject;
        public static GameObject PoisonBarrel = LoadHelper.LoadAssetFromAnywhere<GameObject>("Yellow Drum");
        public static GameObject MetalExplosiveBarrel = LoadHelper.LoadAssetFromAnywhere<GameObject>("Red Drum");
        public static GameObject ExplosiveBarrel = LoadHelper.LoadAssetFromAnywhere<GameObject>("Red Barrel");
        public static GameObject WaterBarrel = LoadHelper.LoadAssetFromAnywhere<GameObject>("Blue Drum");
        public static GameObject OilBarrel = LoadHelper.LoadAssetFromAnywhere<GameObject>("Purple Drum");
        public static GameObject IceBomb = LoadHelper.LoadAssetFromAnywhere<GameObject>("Ice Cube Bomb");
        public static GameObject TableHorizontal = LoadHelper.LoadAssetFromAnywhere<GameObject>("Table_Horizontal");
        public static GameObject TableVertical = LoadHelper.LoadAssetFromAnywhere<GameObject>("Table_Vertical");
        public static GameObject TableHorizontalStone = LoadHelper.LoadAssetFromAnywhere<GameObject>("Table_Horizontal_Stone");
        public static GameObject TableVerticalStone = LoadHelper.LoadAssetFromAnywhere<GameObject>("Table_Vertical_Stone");
        public static GameObject SpikeTrap = LoadHelper.LoadAssetFromAnywhere<GameObject>("trap_spike_gungeon_2x2");
        public static GameObject FlameTrap = LoadHelper.LoadAssetFromAnywhere<GameObject>("trap_flame_poofy_gungeon_1x1");
        public static GameObject HangingPot = LoadHelper.LoadAssetFromAnywhere<GameObject>("Hanging_Pot");
        public static GameObject DeadBlow = LoadHelper.LoadAssetFromAnywhere<GameObject>("Forge_Hammer");
        public static GameObject ChestTruth = LoadHelper.LoadAssetFromAnywhere<GameObject>("TruthChest");
        public static GameObject ChestRat = LoadHelper.LoadAssetFromAnywhere<GameObject>("Chest_Rat");
        public static GameObject Mirror = LoadHelper.LoadAssetFromAnywhere<GameObject>("Shrine_Mirror");
        public static GameObject FoldingTable = PickupObjectDatabase.GetById(644).GetComponent<FoldingTableItem>().TableToSpawn.gameObject;
        public static GameObject BabyDragunNPC = LoadHelper.LoadAssetFromAnywhere<GameObject>("BabyDragunJail");
    }
    class EasyGoopDefinitions
    {
        public static void DefineDefaultGoops()
        {
            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            EasyGoopDefinitions.goopDefs = new List<GoopDefinition>();
            foreach (string text in EasyGoopDefinitions.goops)
            {
                GoopDefinition goopDefinition;
                try
                {
                    GameObject gameObject = assetBundle.LoadAsset(text) as GameObject;
                    goopDefinition = gameObject.GetComponent<GoopDefinition>();
                }
                catch
                {
                    goopDefinition = (assetBundle.LoadAsset(text) as GoopDefinition);
                }
                goopDefinition.name = text.Replace("assets/data/goops/", "").Replace(".asset", "");
                EasyGoopDefinitions.goopDefs.Add(goopDefinition);
            }
            List<GoopDefinition> list = EasyGoopDefinitions.goopDefs;

            FireDef = EasyGoopDefinitions.goopDefs[0];
            OilDef = EasyGoopDefinitions.goopDefs[1];
            PoisonDef = EasyGoopDefinitions.goopDefs[2];
            BlobulonGoopDef = EasyGoopDefinitions.goopDefs[3];
            WebGoop = EasyGoopDefinitions.goopDefs[4];
            WaterGoop = EasyGoopDefinitions.goopDefs[5];

            //HONEY GOOP
            HoneyGoop = new GoopDefinition();
            HoneyGoop.CanBeIgnited = false;
            HoneyGoop.damagesEnemies = false;
            HoneyGoop.damagesPlayers = false;
            HoneyGoop.baseColor32 = ExtendedColours.honeyYellow;
            HoneyGoop.goopTexture = ResourceExtractor.GetTextureFromResource("NevernamedsItems/Resources/honey_standard_base_001.png");
            HoneyGoop.usesLifespan = false;
            HoneyGoop.playerStepsChangeLifetime = true;
            HoneyGoop.playerStepsLifetime = 2.5f;
            HoneyGoop.AppliesSpeedModifier = true;
            HoneyGoop.AppliesSpeedModifierContinuously = true;
            HoneyGoop.SpeedModifierEffect = HoneySpeedMod;

            //PROPULSION GOOP
            PropulsionGoop = new GoopDefinition();
            PropulsionGoop.CanBeIgnited = false;
            PropulsionGoop.damagesEnemies = false;
            PropulsionGoop.damagesPlayers = false;
            PropulsionGoop.baseColor32 = ExtendedColours.vibrantOrange;
            PropulsionGoop.goopTexture = PoisonDef.goopTexture;
            PropulsionGoop.lifespan = 30f;
            PropulsionGoop.usesLifespan = true;
            PropulsionGoop.AppliesSpeedModifier = true;
            PropulsionGoop.AppliesSpeedModifierContinuously = true;
            PropulsionGoop.SpeedModifierEffect = HoneySpeedMod;

            //PLAGUE GOOP
            PlagueGoop = new GoopDefinition();
            PlagueGoop.CanBeIgnited = false;
            PlagueGoop.damagesEnemies = false;
            PlagueGoop.damagesPlayers = false;
            PlagueGoop.baseColor32 = ExtendedColours.plaguePurple;
            PlagueGoop.goopTexture = PoisonDef.goopTexture;
            PlagueGoop.lifespan = 10;
            PlagueGoop.usesLifespan = true;
            PlagueGoop.HealthModifierEffect = EasyStatusEffectAccess.commonPlague;
            PlagueGoop.AppliesDamageOverTime = true;

            //PLAYER FRIENDLY WEB GOOP
            GoopDefinition midInitWeb = UnityEngine.Object.Instantiate<GoopDefinition>(WebGoop);
            midInitWeb.playerStepsChangeLifetime = false;
            midInitWeb.SpeedModifierEffect = FriendlyWebGoopSpeedMod;
            PlayerFriendlyWebGoop = midInitWeb;

            //PIT GOOP
            PitGoop = new GoopDefinition();
            PitGoop.CanBeIgnited = false;
            PitGoop.damagesEnemies = false;
            PitGoop.damagesPlayers = false;
            PitGoop.baseColor32 = Color.black;
            PitGoop.goopTexture = ResourceExtractor.GetTextureFromResource("NevernamedsItems/Resources/pitgooptex.png");
            PitGoop.usesLifespan = true;
            PitGoop.usesOverrideOpaqueness = true;
            PitGoop.overrideOpaqueness = 2f;
            PitGoop.lifespan = 30f;
            //PitGoop.playerStepsChangeLifetime = true;
            //PitGoop.playerStepsLifetime = 2.5f;
        }
        private static string[] goops = new string[]
        {
            "assets/data/goops/napalmgoopthatworks.asset",
            "assets/data/goops/oil goop.asset",
            "assets/data/goops/poison goop.asset",
            "assets/data/goops/blobulongoop.asset",
            "assets/data/goops/phasewebgoop.asset",
            "assets/data/goops/water goop.asset",
        };
        private static List<GoopDefinition> goopDefs;

        public static GoopDefinition FireDef;
        public static GoopDefinition OilDef;
        public static GoopDefinition PoisonDef;
        public static GoopDefinition BlobulonGoopDef;
        public static GoopDefinition WebGoop;
        public static GoopDefinition PropulsionGoop;
        public static GoopDefinition PlayerFriendlyWebGoop;
        public static GoopDefinition WaterGoop;
        public static GoopDefinition PlagueGoop;
        public static GoopDefinition HoneyGoop;
        public static GoopDefinition PitGoop;
        public static GoopDefinition CharmGoopDef = PickupObjectDatabase.GetById(310)?.GetComponent<WingsItem>()?.RollGoop;
        public static GoopDefinition GreenFireDef = (PickupObjectDatabase.GetById(698) as Gun).DefaultModule.projectiles[0].GetComponent<GoopModifier>().goopDefinition;
        public static GoopDefinition CheeseDef = (PickupObjectDatabase.GetById(808) as Gun).DefaultModule.projectiles[0].GetComponent<GoopModifier>().goopDefinition;


        //HONEY SLOW EFFECT
        static Gun TripleCrossbow = ETGMod.Databases.Items["triple_crossbow"] as Gun;
        static GameActorSpeedEffect TripleCrossbowEffect = TripleCrossbow.DefaultModule.projectiles[0].speedEffect;
        public static GameActorSpeedEffect HoneySpeedMod = new GameActorSpeedEffect
        {
            duration = 1,
            TintColor = TripleCrossbowEffect.TintColor,
            DeathTintColor = TripleCrossbowEffect.DeathTintColor,
            effectIdentifier = "HoneySlow",
            AppliesTint = false,
            AppliesDeathTint = false,
            resistanceType = EffectResistanceType.None,
            SpeedMultiplier = 0.60f,

            //Eh
            OverheadVFX = TripleCrossbowEffect.OverheadVFX,
            AffectsEnemies = true,
            AffectsPlayers = true,
            AppliesOutlineTint = false,
            OutlineTintColor = TripleCrossbowEffect.OutlineTintColor,
            PlaysVFXOnActor = false,

        };
        //FRIENDLY WEB GOOP SPEED EFFECT
        public static GameActorSpeedEffect FriendlyWebGoopSpeedMod = new GameActorSpeedEffect
        {
            duration = 1,
            TintColor = TripleCrossbowEffect.TintColor,
            DeathTintColor = TripleCrossbowEffect.DeathTintColor,
            effectIdentifier = "FriendlyWebSlow",
            AppliesTint = false,
            AppliesDeathTint = false,
            resistanceType = EffectResistanceType.None,
            SpeedMultiplier = 0.40f,

            //Eh
            OverheadVFX = null,
            AffectsEnemies = true,
            AffectsPlayers = false,
            AppliesOutlineTint = false,
            OutlineTintColor = TripleCrossbowEffect.OutlineTintColor,
            PlaysVFXOnActor = false,
        };
        //PROPULSION GOOP
        public static GameActorSpeedEffect PropulsionGoopSpeedMod = new GameActorSpeedEffect
        {
            duration = 1,
            TintColor = TripleCrossbowEffect.TintColor,
            DeathTintColor = TripleCrossbowEffect.DeathTintColor,
            effectIdentifier = "PropulsionGoopSpeed",
            AppliesTint = false,
            AppliesDeathTint = false,
            resistanceType = EffectResistanceType.None,
            SpeedMultiplier = 1.40f,

            //Eh
            OverheadVFX = null,
            AffectsEnemies = true,
            AffectsPlayers = false,
            AppliesOutlineTint = false,
            OutlineTintColor = TripleCrossbowEffect.OutlineTintColor,
            PlaysVFXOnActor = false,
        };
    }
    public class GameActorPlagueEffect : GameActorHealthEffect
    {
        public GameActorPlagueEffect()
        {
            this.DamagePerSecondToEnemies = 1f;
            this.TintColor = ExtendedColours.plaguePurple;
            this.DeathTintColor = ExtendedColours.plaguePurple;
            this.AppliesTint = true;
            this.AppliesDeathTint = true;
        }
        public override void ApplyTint(GameActor actor)
        {

            if (this.AppliesTint)
            {
                ETGModConsole.Log("AppliesTint was called");
                actor.RegisterOverrideColor(this.TintColor, this.effectIdentifier);
            }
            else
            {
                ETGModConsole.Log("AppliesTint was false");

            }
            base.ApplyTint(actor);
        }
        public override void EffectTick(GameActor actor, RuntimeGameActorEffectData effectData)
        {
            if (this.AffectsEnemies && actor is AIActor)
            {
                actor.healthHaver.ApplyDamage(this.DamagePerSecondToEnemies * BraveTime.DeltaTime, Vector2.zero, this.effectIdentifier, CoreDamageTypes.None, DamageCategory.DamageOverTime, false, null, false);

                if (EasyGoopDefinitions.PlagueGoop != null)
                {
                    DeadlyDeadlyGoopManager goop = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.PlagueGoop);
                    goop.TimedAddGoopCircle(actor.specRigidbody.UnitCenter, 1, 0.75f, true);
                    
                }

                else
                {
                    ETGModConsole.Log("Goop was null");
                }
            }

        }
    }
}
