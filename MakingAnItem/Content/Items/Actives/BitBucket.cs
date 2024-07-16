using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Alexandria.ItemAPI;
using Gungeon;
using System.Collections;

namespace NevernamedsItems
{
    class BitBucket : PlayerItem, ILabelItem
    {
        public static void Init()
        {
            BitBucket item = ItemSetup.NewItem<BitBucket>(
              "Bit Bucket",
              "Data Loss",
              "Consumes lost data, regurgitating it forth when agitated." + "\n\nThe cornerstone of modern computing.",
              "bitbucket_icon") as BitBucket;          
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 0.2f);
            item.consumable = false;
            item.quality = ItemQuality.D;
        }
        public override void Update()
        {
            //ETGModConsole.Log("Lastowner is null: " + (LastOwner == null));
            currentLabel = storedProjectiles.Count().ToString();
            base.Update();
        }
        private List<Projectile> storedProjectiles = new List<Projectile>();
        private string currentLabel;
        public string GetLabel()
        {
            return currentLabel;
        }
        public override void Pickup(PlayerController player)
        {
            player.PostProcessProjectile += this.PostProcessProjectile;
            base.Pickup(player);
        }
        public override void OnPreDrop(PlayerController user)
        {
            user.PostProcessProjectile -= this.PostProcessProjectile;
            base.OnPreDrop(user);
        }
        public override void OnDestroy()
        {
            if (LastOwner)
            {
                LastOwner.PostProcessProjectile -= this.PostProcessProjectile;
            }
            base.OnDestroy();
        }
        private void PostProcessProjectile(Projectile bullet, float thing)
        {
            if (storedProjectiles.Count() < 20)
            {
                if (UnityEngine.Random.value <= 0.2f)
                {
                    StartCoroutine(ConsumeProjectile(bullet));
                }
            }
        }
        private IEnumerator ConsumeProjectile(Projectile bullet)
        {
            yield return null;
            GameObject newBulletOBJ = FakePrefab.Clone(bullet.gameObject);
            newBulletOBJ.SetActive(false);
            FakePrefab.MarkAsFakePrefab(newBulletOBJ);
            UnityEngine.Object.DontDestroyOnLoad(newBulletOBJ);

            storedProjectiles.Add(newBulletOBJ.GetComponent<Projectile>());
            yield return null;
            Destroy(bullet.gameObject);
            yield break;
        }
        public override bool CanBeUsed(PlayerController user)
        {
            if (storedProjectiles.Count() > 0) return true;
            else return false;
        }
        public override void DoEffect(PlayerController user)
        {
            int amountOfBullets = storedProjectiles.Count();
            float direction = UnityEngine.Random.insideUnitCircle.ToAngle();
            if (user.CurrentGun != null) direction = user.CurrentGun.CurrentAngle;

            for (int i = amountOfBullets - 1; i >= 0; i--)
            {
                float finaldir = ProjSpawnHelper.GetAccuracyAngled(direction, 25, user);

                GameObject prefab = storedProjectiles[i].gameObject;
                GameObject spawnedBulletOBJ = SpawnManager.SpawnProjectile(prefab, user.sprite.WorldCenter, Quaternion.Euler(0f, 0f, finaldir), true);
                Projectile component = spawnedBulletOBJ.GetComponent<Projectile>();
                if (component != null)
                {
                    component.Owner = user;
                    component.Shooter = user.specRigidbody;
                    component.baseData.speed *= UnityEngine.Random.Range(0.9f, 1.1f);
                }
                storedProjectiles.RemoveAt(i);
                Destroy(prefab);
            }
        }
    }
}
