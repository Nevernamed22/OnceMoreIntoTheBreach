using Alexandria.ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class ExplosiveSlashModifier : ProjectileSlashingBehaviour
    {
        public ExplosionData explosionData;
        public override void SlashHitTarget(GameActor target, bool fatal)
        {
            for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
            {
                PlayerController playerController = GameManager.Instance.AllPlayers[i];
                if (playerController && playerController.specRigidbody)
                {
                    this.explosionData.ignoreList.Add(playerController.specRigidbody);
                }
            }
            Exploder.Explode(target.CenterPosition, explosionData, Vector2.zero);
            base.SlashHitTarget(target, fatal);
        }
    }
}
