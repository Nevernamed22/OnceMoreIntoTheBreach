using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;

namespace NevernamedsItems
{
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
        public static GoopDefinition WaterGoop;
        public static GoopDefinition HoneyGoop;
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
    }
}
