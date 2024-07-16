using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexandria.Misc;
using UnityEngine;
using Alexandria.ItemAPI;
using System.Collections;

namespace NevernamedsItems
{
    class FriendshipBracelet : PlayerItem
    {
        public static void Init()
        {
            PlayerItem item = ItemSetup.NewItem<FriendshipBracelet>(
            "Friendship Bracelet",
            "SaKeyfice",
            "This key is hungry for sustenance so that it may lay its eggs, and spawn the next generation of keys.",
            "sharpkey_improved") as PlayerItem;
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 5);
            item.AddPassiveStatModifier(PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);
            item.consumable = false;
            item.quality = ItemQuality.EXCLUDED;
        }
        public List<GameObject> Summons = new List<GameObject>();

        public void CloneOrbital(GameObject orb, PlayerController play)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(orb, play.transform.position, Quaternion.identity);
            if (gameObject.GetComponent<PlayerOrbital>())
            {
                PlayerOrbital component = gameObject.GetComponent<PlayerOrbital>();
                component.Initialize(play);
            }
            else if (gameObject.GetComponent<PlayerOrbitalFollower>())
            {
                PlayerOrbitalFollower component2 = gameObject.GetComponent<PlayerOrbitalFollower>();
                component2.Initialize(play);
            }
            Summons.Add(gameObject);

        }
        public override void DoEffect(PlayerController user)
        {
            GameManager.Instance.StartCoroutine(HandleEffect(user));      
        }

        public IEnumerator HandleEffect(PlayerController user)
        {
            base.IsCurrentlyActive = true;
            this.m_activeElapsed = 0f;
            this.m_activeDuration = 60;

            int c1 = user.orbitals.Count;
            int c2 = user.trailOrbitals.Count;
            int c3 = user.companions.Count;

            for (int i = 0; i < c1; i++)
            {
                CloneOrbital(user.orbitals[i].GetTransform().gameObject, user);
            }
            for (int j = 0; j < c2; j++)
            {
                CloneOrbital(user.trailOrbitals[j].gameObject, user);
            }
            user.RecalculateOrbitals();

            for (int i = 0; i < c3; i++)
            {
                string guid = user.companions[i].EnemyGuid;
                AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(guid);
                Vector3 vector = user.transform.position;

                GameObject extantCompanion2 = UnityEngine.Object.Instantiate<GameObject>(orLoadByGuid.gameObject, vector, Quaternion.identity);
                CompanionController orAddComponent = extantCompanion2.GetOrAddComponent<CompanionController>();
                orAddComponent.Initialize(user);
                if (orAddComponent.specRigidbody) { PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(orAddComponent.specRigidbody, null, false); }
                Summons.Add(extantCompanion2);
            }

            float elapsed = 0;
            while (elapsed < 60f)
            {
                elapsed += BraveTime.DeltaTime;
                if (LastOwner == null || user != LastOwner || GameManager.Instance.IsLoadingLevel)
                {
                    yield break;
                }
            }
            ClearCompanions(user);
            yield break;
        }
        public override void Pickup(PlayerController player)
        {
            player.OnNewFloorLoaded += OnNewFloor;
            base.Pickup(player);
        }
        public override void OnPreDrop(PlayerController user)
        {
            if (user != null) { Disable(user); }
            base.OnPreDrop(user);
        }
        public override void OnDestroy()
        {
            if (LastOwner != null) { Disable(LastOwner); }
            base.OnDestroy();
        }
        private void ClearCompanions(PlayerController player = null)
        {
            for (int i = Summons.Count - 1; i >= 0; i--)
            {
                UnityEngine.Object.Destroy(Summons[i]);
            }
            if (player) { player.RecalculateOrbitals(); }
            Summons.Clear();
        }
        private void Disable(PlayerController player)
        {
            ClearCompanions(player);
            player.OnNewFloorLoaded -= OnNewFloor;
        }
        private void OnNewFloor(PlayerController player)
        {
            ClearCompanions(player);
        }
        public override bool CanBeUsed(PlayerController user)
        {
            return base.CanBeUsed(user);
        }
    }
}
