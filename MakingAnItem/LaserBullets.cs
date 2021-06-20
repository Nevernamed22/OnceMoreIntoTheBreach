﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace NevernamedsItems
{
    public class LaserBullets : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Laser Bullets";
            string resourceName = "NevernamedsItems/Resources/laserbullets_icon";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<LaserBullets>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Hybrid";
            string longDesc = "A beam and bullet crossbreed that defies nature, these bullets make your bullets shoot... beams!";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "nn");

            item.CanBeDropped = true;
            item.quality = PickupObject.ItemQuality.B;

            List<string> BeamAnimPaths = new List<string>()
            {
                "NevernamedsItems/Resources/BeamSprites/redbeam_seg_001",
                "NevernamedsItems/Resources/BeamSprites/redbeam_seg_002",
                "NevernamedsItems/Resources/BeamSprites/redbeam_seg_003",
                "NevernamedsItems/Resources/BeamSprites/redbeam_seg_004"
            };
            List<string> ImpactAnimPaths = new List<string>()
            {
                "NevernamedsItems/Resources/BeamSprites/redbeam_impact_001",
                "NevernamedsItems/Resources/BeamSprites/redbeam_impact_002",
                "NevernamedsItems/Resources/BeamSprites/redbeam_impact_003",
                "NevernamedsItems/Resources/BeamSprites/redbeam_impact_004",
            };

            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0]);
            BasicBeamController beamComp = projectile.GenerateBeamPrefab(
                "NevernamedsItems/Resources/BeamSprites/redbeam_seg_001", new Vector2(18,2), new Vector2(0,8), BeamAnimPaths, 8, 
                ImpactAnimPaths, 13, new Vector2(4, 4), new Vector2(7,7));
            projectile.gameObject.SetActive(false);
            projectile.baseData.damage *= 4;
            projectile.baseData.range *= 2;
            projectile.baseData.speed *= 4;
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            beamComp.boneType = BasicBeamController.BeamBoneType.Straight;
            beamComp.interpolateStretchedBones = false;
            beamComp.ContinueBeamArtToWall = true;
            SimpleRedBeam = projectile;
        }
        public static Projectile SimpleRedBeam;
        private void PostProcess(Projectile bullet, float thing)
        {
            float procChance = 0.2f;
            procChance *= thing;
            if (UnityEngine.Random.value <= procChance && bullet.GetComponent<BeamBulletsBehaviour>() == null)
            {
                BeamBulletsBehaviour beambullets = bullet.gameObject.AddComponent<BeamBulletsBehaviour>();
                if (UnityEngine.Random.value <= 0.5f) beambullets.firetype = BeamBulletsBehaviour.FireType.PLUS;
                else beambullets.firetype = BeamBulletsBehaviour.FireType.CROSS;
                if (Owner.PlayerHasActiveSynergy("new Vector2(x, y)")) beambullets.firetype = BeamBulletsBehaviour.FireType.STAR;
                beambullets.beamToFire = SimpleRedBeam;

                List<Projectile> ValidBeams = new List<Projectile>();
                if (Owner.PlayerHasActiveSynergy("Beam Me Up!"))
                {
                    if (UnityEngine.Random.value <= 0.2f)
                    {
                        for (int j = 0; j < Owner.inventory.AllGuns.Count; j++)
                        {
                            if (Owner.inventory.AllGuns[j] && !Owner.inventory.AllGuns[j].InfiniteAmmo)
                            {
                                ProjectileModule defaultModule = Owner.inventory.AllGuns[j].DefaultModule;
                                if (defaultModule.shootStyle == ProjectileModule.ShootStyle.Beam)
                                {
                                    ValidBeams.Add(defaultModule.GetCurrentProjectile());
                                }
                            }
                        }
                        beambullets.beamToFire = BraveUtility.RandomElement(ValidBeams);
                    }
                }

                bullet.baseData.speed *= 0.2f;
                bullet.UpdateSpeed();
            }
        }
        public override void Pickup(PlayerController player)
        {
            player.PostProcessProjectile += this.PostProcess;
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.PostProcessProjectile -= this.PostProcess;

            return base.Drop(player);
        }
        protected override void OnDestroy()
        {
            if (Owner)
            {
                Owner.PostProcessProjectile -= this.PostProcess;
            }
            base.OnDestroy();
        }
    }
    public class BeamBulletsBehaviour : MonoBehaviour
    {
        public BeamBulletsBehaviour()
        {
            beamToFire = LaserBullets.SimpleRedBeam;
            firetype = FireType.PLUS;
        }
        private void Start()
        {
            this.m_projectile = base.GetComponent<Projectile>();
            if (this.m_projectile.Owner && this.m_projectile.Owner is PlayerController)
            {
                this.m_owner = this.m_projectile.Owner as PlayerController;
            }
            Invoke("BeginBeamFire", 0.1f);
        }
        private void BeginBeamFire()
        {
            if (firetype == FireType.CROSS || firetype == FireType.STAR)
            {
                //NorthEast
                BeamController beam = BeamToolbox.FreeFireBeamFromAnywhere(beamToFire, this.m_owner, this.m_projectile.gameObject, Vector2.zero, false,45, 1000, true);
                Projectile beamprojcomponent = beam.GetComponent<Projectile>();
                beamprojcomponent.baseData.damage *= m_owner.stats.GetStatValue(PlayerStats.StatType.Damage);
                //SouthEast
                BeamController beam2 = BeamToolbox.FreeFireBeamFromAnywhere(beamToFire, this.m_owner, this.m_projectile.gameObject, Vector2.zero, false, 135, 1000, true);
                Projectile beamprojcomponent2 = beam2.GetComponent<Projectile>();
                beamprojcomponent2.baseData.damage *= m_owner.stats.GetStatValue(PlayerStats.StatType.Damage);
                //SouthWest
                BeamController beam3 = BeamToolbox.FreeFireBeamFromAnywhere(beamToFire, this.m_owner, this.m_projectile.gameObject, Vector2.zero, false, -45, 1000, true);
                Projectile beamprojcomponent3 = beam3.GetComponent<Projectile>();
                beamprojcomponent3.baseData.damage *= m_owner.stats.GetStatValue(PlayerStats.StatType.Damage);
                //NorthWest
                BeamController beam4 = BeamToolbox.FreeFireBeamFromAnywhere(beamToFire, this.m_owner, this.m_projectile.gameObject, Vector2.zero, false, -135, 1000, true);
                Projectile beamprojcomponent4 = beam4.GetComponent<Projectile>();
                beamprojcomponent4.baseData.damage *= m_owner.stats.GetStatValue(PlayerStats.StatType.Damage);
            }
            if (firetype == FireType.PLUS || firetype == FireType.STAR)
            {
                //Right
                BeamController beam = BeamToolbox.FreeFireBeamFromAnywhere(beamToFire, this.m_owner, this.m_projectile.gameObject, Vector2.zero, false, 0, 1000, true);
                Projectile beamprojcomponent = beam.GetComponent<Projectile>();
                beamprojcomponent.baseData.damage *= m_owner.stats.GetStatValue(PlayerStats.StatType.Damage);
                //Up
                BeamController beam2 = BeamToolbox.FreeFireBeamFromAnywhere(beamToFire, this.m_owner, this.m_projectile.gameObject, Vector2.zero, false, 90, 1000, true);
                Projectile beamprojcomponent2 = beam2.GetComponent<Projectile>();
                beamprojcomponent2.baseData.damage *= m_owner.stats.GetStatValue(PlayerStats.StatType.Damage);
                //Left
                BeamController beam3 = BeamToolbox.FreeFireBeamFromAnywhere(beamToFire, this.m_owner, this.m_projectile.gameObject, Vector2.zero, false, 180, 1000, true);
                Projectile beamprojcomponent3 = beam3.GetComponent<Projectile>();
                beamprojcomponent3.baseData.damage *= m_owner.stats.GetStatValue(PlayerStats.StatType.Damage);
                //Down
                BeamController beam4 = BeamToolbox.FreeFireBeamFromAnywhere(beamToFire, this.m_owner, this.m_projectile.gameObject, Vector2.zero, false, -90, 1000, true);
                Projectile beamprojcomponent4 = beam4.GetComponent<Projectile>();
                beamprojcomponent4.baseData.damage *= m_owner.stats.GetStatValue(PlayerStats.StatType.Damage);
            }
        }
        private void Update()
        {

        }

        private Projectile m_projectile;
        private PlayerController m_owner;
        public Projectile beamToFire;
        public FireType firetype;
        public enum FireType
        {
            PLUS,
            CROSS,
            STAR,
        }
    }
}