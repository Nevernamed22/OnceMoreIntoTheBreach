using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using Alexandria.ItemAPI;
using SaveAPI;

namespace NevernamedsItems
{
    public class ERRORShells : PassiveItem
    {
        public static void Init()
        {
            PickupObject item = ItemSetup.NewItem<ERRORShells>(
            "ERROR Shells",
            "What do you mean 74 errors!?",
            "Picks a random selection of enemies to become highly efficient against.\n\n" + "These bullets were moulded by the numerous errors that went into making them, thanks to their incompetent smith.",
            "errorshells_icon");
            item.quality = PickupObject.ItemQuality.A;
            item.SetTag("bullet_modifier");
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.PURCHASED_ERRORSHELLS, true);
            item.AddItemToDougMetaShop(30);
            Doug.AddToLootPool(item.PickupObjectId);

            item.GetComponent<tk2dBaseSprite>().renderer.material = new Material(Shader.Find("Brave/Internal/GlitchUnlit"));
            item.GetComponent<tk2dBaseSprite>().usesOverrideMaterial = true;

            ERRORShellsDummyEffect = new GameActorDecorationEffect()
            {
                AffectsEnemies = true,
                OverheadVFX = SharedVFX.ERRORShellsOverhead,
                AffectsPlayers = false,
                AppliesTint = false,
                AppliesDeathTint = false,
                AppliesOutlineTint = false,
                duration = float.MaxValue,
                effectIdentifier = "ERROR Shells Overheader",
                resistanceType = EffectResistanceType.None,
                PlaysVFXOnActor = false,
                stackMode = GameActorEffect.EffectStackingMode.Ignore,
            };

            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("ApacheThunder.etg.ExpandTheGungeon"))
            {
                InstakillTargets.AddRange(new Dictionary<string, string>()
                {
                    { "Hotshot Bullet Kin", GUIDs.Expand.Hotshot_Bullet_Kin },
                    { "Hotshot Shotgun Kin", GUIDs.Expand.Hotshot_Shotgun_Kin },
                    { "Hotshot Cultist", GUIDs.Expand.Hotshot_Cultist },
                    { "Arrowkin", GUIDs.Arrowkin }
                });
            }
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("kp.etg.frostandgunfire"))
            {
                InstakillTargets.AddRange(new Dictionary<string, string>()
                {
                    { "Bubbulon", GUIDs.FrostAndGunfire.Bubbulon },
                    { "Globbulon", GUIDs.FrostAndGunfire.Globbulon },
                    { "Cannon Kin", GUIDs.FrostAndGunfire.Cannon_Kin },
                    { "Suppressor", GUIDs.FrostAndGunfire.Suppressor },
                    { "Skell", GUIDs.FrostAndGunfire.Skell },
                    { "Salamander", GUIDs.FrostAndGunfire.Salamander },
                    { "Ophaim", GUIDs.FrostAndGunfire.Ophaim },
                    { "Firefly", GUIDs.FrostAndGunfire.Firefly },
                    { "Spitfire", GUIDs.FrostAndGunfire.Spitfire },
                    { "Gazer", GUIDs.FrostAndGunfire.Gazer },
                    { "Observer", GUIDs.FrostAndGunfire.Observer },
                    { "Gunzooka", GUIDs.FrostAndGunfire.Gunzooka },
                    { "Suppores", GUIDs.FrostAndGunfire.Suppores },
                });
            }
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("somebunny.etg.planetsideofgunymede"))
            {
                InstakillTargets.AddRange(new Dictionary<string, string>()
                {
                    { "Coallet", GUIDs.Planetside.Coallet },
                    { "Detscavator", GUIDs.Planetside.Detscavator },
                    { "Trapper Cube", GUIDs.Planetside.Trapper_Cube_Horizontal },
                    { "Arch Gunjurer", GUIDs.Planetside.Arch_Gunjurer },
                    { "Barretina", GUIDs.Planetside.Barretina },
                    { "Cursebulon", GUIDs.Planetside.Cursebulon },
                    { "Glockulus", GUIDs.Planetside.Glockulus },
                    { "Skullvenant", GUIDs.Planetside.Skullvenant },
                    { "Wailer", GUIDs.Planetside.Wailer },
                    { "Jammed Guardian", GUIDs.Planetside.Jammed_Guardian },
                    { "Snipeidolon", GUIDs.Planetside.Snipeidolon }
                });
            }
        }
        public static GameActorDecorationEffect ERRORShellsDummyEffect;

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            if (!hasPicked) PickEnemies();
            player.PostProcessProjectile += this.PostProcessProjectile;
            player.PostProcessBeam += this.PostProcessBeam;
            ETGMod.AIActor.OnPreStart += PreSpawn;
            base.StartCoroutine(ShowText());
        }
        public IEnumerator ShowText()
        {
            foreach (string name in ChosenEnemyNames)
            {
                if (!Owner) { yield break; }
                VFXToolbox.DoRisingStringFade(name, Owner.sprite.WorldCenter + new Vector2(0f, 1f), ExtendedColours.plaguePurple);
                yield return new WaitForSeconds(1f);
            }
            yield break;
        }
       
        public override void DisableEffect(PlayerController player)
        {
            ETGMod.AIActor.OnPreStart -= PreSpawn;
            if (player)
            {
                player.PostProcessProjectile -= this.PostProcessProjectile;
                player.PostProcessBeam -= this.PostProcessBeam;
            }
            base.DisableEffect(player);
        }
        private void PreSpawn(AIActor aIActor)
        {
            if (ChosenEnemyGUIDs.Contains(aIActor.EnemyGuid)) aIActor.ApplyEffect(ERRORShellsDummyEffect);
        }

        public bool hasPicked = false;
        public List<string> ChosenEnemyNames = new List<string>();
        public List<string> ChosenEnemyGUIDs = new List<string>();
        private void PickEnemies(bool log = false)
        {
            List<string> viablePicks = new List<string>();
            viablePicks.AddRange(InstakillTargets.Keys);
            for (int i = 0; i < 10; i++)
            {
                string picked = BraveUtility.RandomElement(viablePicks);
                if (log) { Debug.Log($"ERROR Shells Picked -> {picked}"); }
                ChosenEnemyNames.Add(picked);
                ChosenEnemyGUIDs.Add(InstakillTargets[picked]);

                if (EncompassedEnemyTypes.ContainsKey(InstakillTargets[picked]))
                {
                    ChosenEnemyGUIDs.AddRange(EncompassedEnemyTypes[InstakillTargets[picked]]);
                }

                viablePicks.Remove(picked);
            }
            hasPicked = true;
        }
        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
             if (sourceProjectile.sprite != null)
            {
                VFXToolbox.ApplyGlitchShader(sourceProjectile.sprite);

            }

            ProjectileInstakillBehaviour instakill = sourceProjectile.gameObject.GetOrAddComponent<ProjectileInstakillBehaviour>();
            instakill.enemyGUIDsToKill.AddRange(ChosenEnemyGUIDs);
            instakill.onInstaKill += OnInstaKill;
            instakill.soundEvents.Add("Play_ENV_puddle_zap_01");
        }
        private void OnInstaKill(Projectile bullet, AIActor actor)
        {
            base.StartCoroutine(HandleEnemyDeath(actor, bullet.Direction));
        }
        private IEnumerator HandleEnemyDeath(AIActor target, Vector2 motionDirection)
        {
            target.EraseFromExistenceWithRewards(false);
            Transform copyTransform = this.CreateEmptySprite(target);
            tk2dSprite copySprite = copyTransform.GetComponentInChildren<tk2dSprite>();

            float elapsed = 0f;
            float duration = 2.5f;
            while (elapsed < duration)
            {
                elapsed += BraveTime.DeltaTime;
                copyTransform.position += motionDirection.ToVector3ZisY(0f).normalized * BraveTime.DeltaTime * 1f;
                yield return null;
            }
            UnityEngine.Object.Destroy(copyTransform.gameObject);
            yield break;
        }

        private Transform CreateEmptySprite(AIActor target)
        {
            GameObject gameObject = new GameObject("suck image");
            gameObject.layer = target.gameObject.layer;
            tk2dSprite tk2dSprite = gameObject.AddComponent<tk2dSprite>();
            gameObject.transform.parent = SpawnManager.Instance.VFX;
            tk2dSprite.collection = target.sprite.Collection;
            tk2dSprite.renderer.material = target.sprite.renderer.material;
            tk2dSprite.renderer.material.shaderKeywords = target.sprite.renderer.material.shaderKeywords;
            tk2dSprite.SetSprite(target.sprite.Collection, target.sprite.spriteId);
            if (target.sprite.CurrentSprite == null && target.CorpseObject != null && target.CorpseObject.GetComponent<tk2dBaseSprite>() != null)
            {
                tk2dSprite.SetSprite(target.CorpseObject.GetComponent<tk2dBaseSprite>().Collection, target.CorpseObject.GetComponent<tk2dBaseSprite>().spriteId);

            }
            tk2dSprite.transform.position = target.sprite.transform.position;
            GameObject gameObject2 = new GameObject("image parent");
            gameObject2.transform.position = tk2dSprite.WorldCenter;
            tk2dSprite.transform.parent = gameObject2.transform;

            VFXToolbox.ApplyGlitchShader(tk2dSprite);


            return gameObject2.transform;
        }
        private void PostProcessBeam(BeamController sourceBeam)
        {
            if(sourceBeam.gameObject.GetComponent<tk2dTiledSprite>() != null)
            {
                VFXToolbox.ApplyGlitchShader(sourceBeam.gameObject.GetComponent<tk2dTiledSprite>());

            }
            if (sourceBeam.projectile) { this.PostProcessProjectile(sourceBeam.projectile, 1); }
        }

        public static Dictionary<string, string> InstakillTargets = new Dictionary<string, string>()
        {
            { "Bullet Kin", GUIDs.Bullet_Kin },
            { "Bandana Bullet Kin", GUIDs.Bandana_Bullet_Kin },
            { "Veteran Bullet Kin", GUIDs.Veteran_Bullet_Kin },
            { "Tanker", GUIDs.Tanker },
            { "Minelet", GUIDs.Minelet },
            { "Cardinal", GUIDs.Cardinal },
            { "Shroomer", GUIDs.Shroomer },
            { "Ashen Bullet Kin", GUIDs.Ashen_Bullet_Kin },
            { "Mutant Bullet Kin", GUIDs.Mutant_Bullet_Kin },
            { "Fallen Bullet Kin", GUIDs.Fallen_Bullet_Kin },
            { "Chance Kin", GUIDs.Chance_Kin },
            { "Keybullet Kin", GUIDs.Keybullet_Kin },
            { "Confirmed", GUIDs.Confirmed },
            { "Red Shotgun Kin", GUIDs.Red_Shotgun_Kin },
            { "Blue Shotgun Kin", GUIDs.Blue_Shotgun_Kin },
            { "Veteran Shotgun Kin", GUIDs.Veteran_Shotgun_Kin },
            { "Mutant Shotgun Kin", GUIDs.Mutant_Shotgun_Kin },
            { "Executioner", GUIDs.Executioner },
            { "Ashen Shotgun Kin", GUIDs.Ashen_Shotgun_Kin },
            { "Shotgrub", GUIDs.Shotgrub },
            { "Sniper Shell", GUIDs.Sniper_Shell },
            { "Professional", GUIDs.Professional },
            { "Gummy", GUIDs.Gummy },
            { "Skullet", GUIDs.Skullet },
            { "Skullmet", GUIDs.Skullmet },
            { "Spent", GUIDs.Spent },
            { "Creech", GUIDs.Creech },
            { "Hollowpoint", GUIDs.Hollowpoint },
            { "Bombshee", GUIDs.Bombshee },
            { "Rubber Kin", GUIDs.Rubber_Kin },
            { "Tazie", GUIDs.Tazie },
            { "King Bullat", GUIDs.King_Bullat },
            { "Pinhead", GUIDs.Pinhead },
            { "Nitra", GUIDs.Nitra },
            { "Blobulon", GUIDs.Blobulon },
            { "Poisbulon", GUIDs.Poisbulon },
            { "Blizzbulon", GUIDs.Blizzbulon },
            { "Leadbulon", GUIDs.Leadbulon },
            { "Poopulon", GUIDs.Poopulon },
            { "Bloodbulon", GUIDs.Bloodbulon },
            { "Skusket", GUIDs.Skusket },
            { "Bookllet", GUIDs.Bookllet },
            { "Blue Bookllet", GUIDs.Blue_Bookllet },
            { "Green Bookllet", GUIDs.Green_Bookllet },
            { "Gigi", GUIDs.Gigi },
            { "Muzzle Wisp", GUIDs.Muzzle_Wisp },
            { "Muzzle Flare", GUIDs.Muzzle_Flare },
            { "Cubulon", GUIDs.Cubulon },
            { "Cubulead", GUIDs.Cubulead },
            { "Chancebulon", GUIDs.Chancebulon },
            { "Apprentice Gunjurer", GUIDs.Apprentice_Gunjurer },
            { "Gunjurer", GUIDs.Gunjurer },
            { "High Gunjurer", GUIDs.High_Gunjurer },
            { "Lore Gunjurer", GUIDs.Lore_Gunjurer },
            { "Gunsinger", GUIDs.Gunsinger },
            { "Aged Gunsinger", GUIDs.Aged_Gunsinger },
            { "Ammomancer", GUIDs.Ammomancer },
            { "Jammomancer", GUIDs.Jammomancer },
            { "Jamerlengo", GUIDs.Jamerlengo },
            { "Wizbang", GUIDs.Wizbang },
            { "Mimic", GUIDs.Mimic_Brown },
            { "Pedestal Mimic", GUIDs.Pedestal_Mimic },
            { "Wall Mimic", GUIDs.Wall_Mimic },
            { "Gun Fairy", GUIDs.Gun_Fairy },
            { "Coaler", GUIDs.Coaler },
            { "Gat", GUIDs.Gat },
            { "Det", GUIDs.Det },
            { "Gunzookie", GUIDs.Gunzookie },
            { "Gunzockie", GUIDs.Gunzockie },
            { "Bullet Shark", GUIDs.Bullet_Shark },
            { "Great Bullet Shark", GUIDs.Great_Bullet_Shark },
            { "Tombstoner", GUIDs.Tombstoner },
            { "Gun Cultist", GUIDs.Gun_Cultist },
            { "Beadie", GUIDs.Beadie },
            { "Gun Nut", GUIDs.Gun_Nut },
            { "Chain Gunner", GUIDs.Chain_Gunner },
            { "Spectral Gun Nut", GUIDs.Spectral_Gun_Nut },
            { "Fungun", GUIDs.Fungun },
            { "Spogre", GUIDs.Spogre },
            { "Mountain Cube", GUIDs.Mountain_Cube },
            { "Lead Cube", GUIDs.Lead_Cube },
            { "Flesh Cube", GUIDs.Flesh_Cube },
            { "Lead Maiden", GUIDs.Lead_Maiden },
            { "Misfire Beast", GUIDs.Misfire_Beast },
            { "Phaser Spider", GUIDs.Phaser_Spider },
            { "Killithid", GUIDs.Killithid },
            { "Tarnisher", GUIDs.Tarnisher },
            { "Shambing Round", GUIDs.Shambling_Round },
            { "Shelleton", GUIDs.Shelleton },
            { "Agonizer", GUIDs.Agonizer },
            { "Revolvenant", GUIDs.Revolvenant },
            { "Gunreaper", GUIDs.Gunreaper },
            { "Mouser", GUIDs.Mouser },

            { "Bouncer Bullet Kin", GUIDs.OMITB.Bouncer_Bullet_Kin },
            { "Deacon", GUIDs.OMITB.Deacon },
            { "Molotovnik", GUIDs.OMITB.Molotovnik },
            { "Muskin", GUIDs.OMITB.Muskin }
        };

        public static Dictionary<string, List<string>> EncompassedEnemyTypes = new Dictionary<string, List<string>>()
        {
            { GUIDs.Bullet_Kin, new List<string>(){ GUIDs.Bullet_Kin_AK47, GUIDs.Tutorial_Bullet_Kin }},
            { GUIDs.Tanker, new List<string>(){ GUIDs.Tanker_Summoned }},
            { GUIDs.Spent, new List<string>(){ GUIDs.Large_Spent }},
            { GUIDs.Blobulon, new List<string>(){ GUIDs.Blobuloid, GUIDs.Blobulin }},
            { GUIDs.Poisbulon, new List<string>(){ GUIDs.Poisbuloid, GUIDs.Poisbulin }},
            { GUIDs.Skusket, new List<string>(){ GUIDs.Black_Skusket, GUIDs.Skusket_Head }},
            { GUIDs.Mimic_Brown, new List<string>(){ GUIDs.Mimic_Blue, GUIDs.Mimic_Green, GUIDs.Mimic_Red, GUIDs.Mimic_Black, GUIDs.Mimic_Rat }},
            { GUIDs.Det, GUIDs.AllDetGUIDs },

            { GUIDs.Planetside.Trapper_Cube_Horizontal, new List<string>(){ GUIDs.Planetside.Trapper_Cube_Vertical }},
            { GUIDs.Planetside.Snipeidolon, new List<string>(){ GUIDs.Planetside.Revenant }}
        };

    }
}

