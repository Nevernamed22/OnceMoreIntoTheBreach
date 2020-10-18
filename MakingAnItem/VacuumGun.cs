using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using ItemAPI;

namespace NevernamedsItems
{
    public class VacuumGun : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Vacuum Gun", "vacuumgun");
            Game.Items.Rename("outdated_gun_mods:vacuum_gun", "nn:vacuum_gun");
            gun.gameObject.AddComponent<VacuumGun>();
            gun.SetShortDescription("Ranged Weapon");
            gun.SetLongDescription("Pressing reload sucks up nearby blobs, and uses them as ammo. Cannot gain ammo by any other method." + "\n\nDesigned specifically to combat Blobulonian creatures, in the case of a potential re-emergence of the empire." + "\n\nZZZZZZZ");

            gun.SetupSprite(null, "vacuumgun_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 12);

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);

            //GUN STATS
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.ammo = 0;
            gun.CanGainAmmo = false;
            gun.DefaultModule.cooldownTime = 0.3f;
            gun.DefaultModule.numberOfShotsInClip = 10000;
            gun.barrelOffset.transform.localPosition = new Vector3(1.75f, 0.81f, 0f);
            gun.SetBaseMaxAmmo(10000);

            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 7f;
            projectile.baseData.speed *= 1f;
            projectile.baseData.range *= 1f;
            projectile.SetProjectileSpriteRight("vacuumgun_projectile", 16, 14, false, tk2dBaseSprite.Anchor.MiddleCenter, 15, 13);

            projectile.transform.parent = gun.barrelOffset;

            gun.quality = PickupObject.ItemQuality.C;
            gun.encounterTrackable.EncounterGuid = "this is the Vacuum Gun";
            ETGMod.Databases.Items.Add(gun, null, "ANY");
        }
        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            base.OnReloadPressed(player, gun, bSOMETHING);
            if (player.CurrentRoom != null)
            {
                player.CurrentRoom.ApplyActionToNearbyEnemies(player.CenterPosition, 8f, new Action<AIActor, float>(this.ProcessEnemy));
            }

        }
        private void ProcessEnemy(AIActor target, float distance)
        {
            if (EasyEnemyTypeLists.BlobulonEnemiesALLNOBOSS.Contains(target.EnemyGuid))
            {
                GameManager.Instance.Dungeon.StartCoroutine(this.HandleEnemySuck(target));
                target.EraseFromExistence(true);
                int AmmoWorth = 0;
                if (EasyEnemyTypeLists.BlobulonEnemiesSMALL.Contains(target.EnemyGuid)) AmmoWorth = 5;
                else if (EasyEnemyTypeLists.BlobulonEnemiesMEDIUM.Contains(target.EnemyGuid)) AmmoWorth = 14;
                else if (EasyEnemyTypeLists.BlobulonEnemiesLARGE.Contains(target.EnemyGuid)) AmmoWorth = 20;
                else if (EasyEnemyTypeLists.BlobulonEnemiesMEGA.Contains(target.EnemyGuid)) AmmoWorth = 30;
                if (AmmoWorth > 0)
                {
                    gun.ammo += AmmoWorth;
                }
            }
            else if (target.EnemyGuid == EnemyGuidDatabase.Entries["chicken"])
            {
                if (gun.CurrentOwner && (gun.CurrentOwner as PlayerController).PlayerHasActiveSynergy("Chickadee"))
                {
                    GameManager.Instance.Dungeon.StartCoroutine(this.HandleEnemySuck(target));
                    target.EraseFromExistence(true);
                    gun.ammo += 5;
                }
            }
        }
        private IEnumerator HandleEnemySuck(AIActor target)
        {
            Transform copySprite = this.CreateEmptySprite(target);
            Vector3 startPosition = copySprite.transform.position;
            float elapsed = 0f;
            float duration = 0.5f;
            while (elapsed < duration)
            {
                elapsed += BraveTime.DeltaTime;
                if (gun && copySprite)
                {
                    Vector3 position = gun.PrimaryHandAttachPoint.position;
                    float t = elapsed / duration * (elapsed / duration);
                    copySprite.position = Vector3.Lerp(startPosition, position, t);
                    copySprite.rotation = Quaternion.Euler(0f, 0f, 360f * BraveTime.DeltaTime) * copySprite.rotation;
                    copySprite.localScale = Vector3.Lerp(Vector3.one, new Vector3(0.1f, 0.1f, 0.1f), t);
                }
                yield return null;
            }
            if (copySprite)
            {
                UnityEngine.Object.Destroy(copySprite.gameObject);
            }
            yield break;
        }
        private Transform CreateEmptySprite(AIActor target)
        {
            GameObject gameObject = new GameObject("suck image");
            gameObject.layer = target.gameObject.layer;
            tk2dSprite tk2dSprite = gameObject.AddComponent<tk2dSprite>();
            gameObject.transform.parent = SpawnManager.Instance.VFX;
            tk2dSprite.SetSprite(target.sprite.Collection, target.sprite.spriteId);
            tk2dSprite.transform.position = target.sprite.transform.position;
            GameObject gameObject2 = new GameObject("image parent");
            gameObject2.transform.position = tk2dSprite.WorldCenter;
            tk2dSprite.transform.parent = gameObject2.transform;
            if (target.optionalPalette != null)
            {
                tk2dSprite.renderer.material.SetTexture("_PaletteTex", target.optionalPalette);
            }
            return gameObject2.transform;
        }
        public VacuumGun()
        {

        }
        public string[] TargetEnemies;
        public float SuckRadius;
    }
}
