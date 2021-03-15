﻿using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    class EasyGoopDefinitions
    {
        //Basegame Goops
        public static GoopDefinition FireDef;
        public static GoopDefinition OilDef;
        public static GoopDefinition PoisonDef;
        public static GoopDefinition BlobulonGoopDef;
        public static GoopDefinition WebGoop;
        public static GoopDefinition WaterGoop;
        public static GoopDefinition CharmGoopDef = PickupObjectDatabase.GetById(310)?.GetComponent<WingsItem>()?.RollGoop;
        public static GoopDefinition GreenFireDef = (PickupObjectDatabase.GetById(698) as Gun).DefaultModule.projectiles[0].GetComponent<GoopModifier>().goopDefinition;
        public static GoopDefinition CheeseDef = (PickupObjectDatabase.GetById(808) as Gun).DefaultModule.projectiles[0].GetComponent<GoopModifier>().goopDefinition;

        //Custom Goops
        public static GoopDefinition PropulsionGoop;
        public static GoopDefinition PlayerFriendlyWebGoop;
        public static GoopDefinition PlagueGoop;
        public static GoopDefinition HoneyGoop;
        public static GoopDefinition PitGoop;
        public static void DefineDefaultGoops()
        {
            //Sets up the goops that have to be extracted from asset bundles
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

            //Define the asset bundle goops
            FireDef = EasyGoopDefinitions.goopDefs[0];
            OilDef = EasyGoopDefinitions.goopDefs[1];
            PoisonDef = EasyGoopDefinitions.goopDefs[2];
            BlobulonGoopDef = EasyGoopDefinitions.goopDefs[3];
            WebGoop = EasyGoopDefinitions.goopDefs[4];
            WaterGoop = EasyGoopDefinitions.goopDefs[5];

            //HONEY GOOP - An Amber Goop that slows enemies and players
            #region HoneyGoop 
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
            HoneyGoop.SpeedModifierEffect = StaticStatusEffects.HoneySpeedMod;
            #endregion

            //PROPULSION GOOP - An orange goop that speeds up enemies and players
            #region PropulsionGoop
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
            PropulsionGoop.SpeedModifierEffect = StaticStatusEffects.PropulsionGoopSpeedMod;
            #endregion 

            //PLAYER FRIENDLY WEB GOOP - A web-textured goop that slows down enemies, but not players.
            #region PlayerFriendlyWebGoop
            GoopDefinition midInitWeb = UnityEngine.Object.Instantiate<GoopDefinition>(WebGoop);
            midInitWeb.playerStepsChangeLifetime = false;
            midInitWeb.SpeedModifierEffect = StaticStatusEffects.FriendlyWebGoopSpeedMod;
            PlayerFriendlyWebGoop = midInitWeb;
            #endregion

            //PIT GOOP - A black goop that sucks enemies down into itself
            #region PitGoop
            PitGoop = new GoopDefinition();
            PitGoop.CanBeIgnited = false;
            PitGoop.damagesEnemies = false;
            PitGoop.damagesPlayers = false;
            PitGoop.baseColor32 = Color.black;
            PitGoop.goopTexture = ResourceExtractor.GetTextureFromResource("NevernamedsItems/Resources/pitgooptex.png");
            PitGoop.usesLifespan = true;
            PitGoop.lifespan = 30f;
            #endregion

            //PLAGUE GOOP - A sickly purple goop that damages enemies over time, and makes them leave a trail that enplagues other enemies.
            #region PlagueGoop
            PlagueGoop = new GoopDefinition();
            PlagueGoop.CanBeIgnited = false;
            PlagueGoop.damagesEnemies = false;
            PlagueGoop.damagesPlayers = false;
            PlagueGoop.baseColor32 = ExtendedColours.plaguePurple;
            PlagueGoop.goopTexture = PoisonDef.goopTexture;
            PlagueGoop.lifespan = 10;
            PlagueGoop.usesLifespan = true;
            PlagueGoop.HealthModifierEffect = StaticStatusEffects.StandardPlagueEffect;
            PlagueGoop.AppliesDamageOverTime = true;
            #endregion

        }
        public static GoopDefinition GenerateBloodGoop(float dps, float lifeSpan = 20)
        {
           GoopDefinition Blood = new GoopDefinition();
            Blood.CanBeIgnited = false;
            Blood.damagesEnemies = true;
            Blood.damagesPlayers = false;
            Blood.baseColor32 = Color.red;
            Blood.goopTexture = PoisonDef.goopTexture;
            Blood.lifespan = lifeSpan;
            Blood.usesLifespan = true;
            Blood.damagePerSecondtoEnemies = dps;
            return Blood;
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
    }
    
}
