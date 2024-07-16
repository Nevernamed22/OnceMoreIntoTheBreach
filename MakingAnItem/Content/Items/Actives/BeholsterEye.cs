using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexandria.ItemAPI;
using Dungeonator;
using UnityEngine;

namespace NevernamedsItems
{
    class BeholsterEye : PlayerItem
    {
        public static void Init()
        {
            PlayerItem item = ItemSetup.NewItem<BeholsterEye>(
               "Beholster Eye",
               "Gungeon wind...",
               "Allows you to harness the great and terrible power of the sphere of many guns!" + "\n\nGungeoneers who have spent too long under the influence of this abyssal relic report feeling phantom sensations of at least four other limbs.",
               "beholstereye_icon") as PlayerItem;

            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 800);
            item.consumable = false;
            item.quality = ItemQuality.S;
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
        }
        public override void DoEffect(PlayerController user)
        {
            if (spawnedGuns.Count > 0) { EndEffect(user); }
            user.StartCoroutine(Dur(user));
        }
        public IEnumerator Dur(PlayerController user)
        {
            base.IsCurrentlyActive = true;
            this.m_activeElapsed = 0f;
            this.m_activeDuration = 10;

            List<int> gunIDS = new List<int>()
            {
                43, //Machine Pistol
                42, //Trank Gun
                90, //Eye of the Behoslter
                30, //M1911
                129, //Com4nd0
                32, //Void Marshall
            };

            if (user.PlayerHasActiveSynergy("Binocular"))
            {
                gunIDS.Add(90);

                Projectile projectile = ((Gun)ETGMod.Databases.Items[90]).DefaultModule.finalProjectile;

                float randomangle = UnityEngine.Random.Range(1, 360);
                for (int i = 0; i < 3; i++)
                {
                GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, user.sprite.WorldCenter, Quaternion.Euler(0f, 0f, randomangle), true);
                    Projectile component = gameObject.GetComponent<Projectile>();
                    if (component != null)
                    {
                        component.Owner = user;
                        component.Shooter = user.specRigidbody;
                        component.ScaleByPlayerStats(user);
                        user.DoPostProcessProjectile(component);
                    }
                    randomangle += 120;
                }
                
            }

            foreach (int id in gunIDS)
            {
                Gun gun = PickupObjectDatabase.GetById(id) as Gun;
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(ResourceCache.Acquire("Global Prefabs/HoveringGun") as GameObject, user.CenterPosition.ToVector3ZisY(0f), Quaternion.identity);
                gameObject.transform.parent = user.transform;
                HoveringGunController hover = gameObject.GetComponent<HoveringGunController>();
                hover.ConsumesTargetGunAmmo = false;
                hover.ChanceToConsumeTargetGunAmmo = 0f;
                hover.Position = HoveringGunController.HoverPosition.CIRCULATE;
                hover.Aim = HoveringGunController.AimType.PLAYER_AIM;
                hover.Trigger = HoveringGunController.FireType.ON_FIRED_GUN;
                hover.CooldownTime = ArtemissileShrine.GetProperShootingSpeed(gun);
                hover.ShootDuration = ArtemissileShrine.GetProperShootDuration(gun);
                hover.OnlyOnEmptyReload = false;
                hover.Initialize(gun, user);

                spawnedGuns.Add(gameObject);
            }
            user.stats.RecalculateStats(user, false, false);

            yield return new WaitForSeconds(10f);

            EndEffect(user);

            yield break;
        }
        public override void OnDestroy()
        {
            if (base.LastOwner)
            {
                if (spawnedGuns.Count > 0) { EndEffect(LastOwner); }
            }
            base.OnDestroy();
        }
        public override void OnPreDrop(PlayerController user)
        {
            if (spawnedGuns.Count > 0) { EndEffect(user); }
            base.OnPreDrop(user);
        }
        public List<GameObject> spawnedGuns = new List<GameObject>();
        public void EndEffect(PlayerController player)
        {
            for (int i = spawnedGuns.Count - 1; i >= 0; i--)
            {
                if (spawnedGuns[i] != null) { UnityEngine.Object.Destroy(spawnedGuns[i].gameObject); }
            }
            spawnedGuns.Clear();
            base.IsCurrentlyActive = false;
        }
    }
}
