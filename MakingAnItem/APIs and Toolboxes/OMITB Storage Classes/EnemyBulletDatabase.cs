using Alexandria.ItemAPI;
using Brave.BulletScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public static class EnemyBulletDatabase
    {
        public static void Init()
        {
            Regular_EightByEight = EnemyDatabase.GetOrLoadByGuid("128db2f0781141bcb505d8f00f9e4d47").bulletBank.GetBullet("default").BulletObject;
            Flashing_EightByEight = EnemyDatabase.GetOrLoadByGuid("128db2f0781141bcb505d8f00f9e4d47").bulletBank.GetBullet("flashybullet").BulletObject;
            Bullat = EnemyDatabase.GetOrLoadByGuid("2feb50a6a40f4f50982e89fd276f6f15").bulletBank.GetBullet("self").BulletObject;
            Short_Pointy = EnemyDatabase.GetOrLoadByGuid("336190e29e8a4f75ab7486595b700d4a").bulletBank.GetBullet("default").BulletObject;
            Long_Pointy = EnemyDatabase.GetOrLoadByGuid("022d7c822bc146b58fe3b0287568aaa2").bulletBank.GetBullet("default").BulletObject;
            Small_Spinning_Fire = EnemyDatabase.GetOrLoadByGuid("5f3abc2d561b4b9c9e72b879c6f10c7e").bulletBank.GetBullet("default").BulletObject;
            Cue_Ball = EnemyDatabase.GetOrLoadByGuid("6f818f482a5c47fd8f38cce101f6566c").bulletBank.GetBullet("default").BulletObject;
            Chainlink = EnemyDatabase.GetOrLoadByGuid("b1770e0f1c744d9d887cc16122882b4f").bulletBank.GetBullet("chain").BulletObject;
            Shotgrub_Gross = EnemyDatabase.GetOrLoadByGuid("044a9f39712f456597b9762893fbc19c").bulletBank.GetBullet("gross").BulletObject;
            Rolling_ThirtyTwoByThirtyTwo = EnemyDatabase.GetOrLoadByGuid("ddf12a4881eb43cfba04f36dd6377abb").bulletBank.GetBullet("bigBullet").BulletObject;
            Sniper = EnemyDatabase.GetOrLoadByGuid("31a3ea0c54a745e182e22ea54844a82d").bulletBank.GetBullet("sniper").BulletObject;
            Sniper_Flashing = EnemyDatabase.GetOrLoadByGuid("c5b11bfc065d417b9c4d03a5e385fe2c").bulletBank.GetBullet("default").BulletObject;
            Fireball_EightByEight = EnemyDatabase.GetOrLoadByGuid("37de0df92697431baa47894a075ba7e9").bulletBank.GetBullet("default").BulletObject;
            Arrow = EnemyDatabase.GetOrLoadByGuid("05891b158cd542b1a5f3df30fb67a7ff").bulletBank.GetBullet("default").BulletObject;
            Number_Six = EnemyDatabase.GetOrLoadByGuid("216fd3dfb9da439d9bd7ba53e1c76462").bulletBank.GetBullet("default").BulletObject;
            Spinning_Triangle = EnemyDatabase.GetOrLoadByGuid("78e0951b097b46d89356f004dda27c42").bulletBank.GetBullet("default").BulletObject;
            Egg = EnemyDatabase.GetOrLoadByGuid("ed37fa13e0fa4fcf8239643957c51293").bulletBank.GetBullet("egg").BulletObject;
            Fireball_TwelveByTwelve = EnemyDatabase.GetOrLoadByGuid("d8a445ea4d944cc1b55a40f22821ae69").bulletBank.GetBullet("default").BulletObject;
            Bubble = EnemyDatabase.GetOrLoadByGuid("6e972cd3b11e4b429b888b488e308551").bulletBank.GetBullet("bubble").BulletObject;
            Long_Ellipse = EnemyDatabase.GetOrLoadByGuid("6e972cd3b11e4b429b888b488e308551").bulletBank.GetBullet("default").BulletObject;
            Atom = EnemyDatabase.GetOrLoadByGuid("534f1159e7cf4f6aa00aeea92459065e").bulletBank.GetBullet("default").BulletObject;
            Rolling_TwentyByTwenty = EnemyDatabase.GetOrLoadByGuid("383175a55879441d90933b5c4e60cf6f").bulletBank.GetBullet("bigBullet").BulletObject;
            Chain_Mace = EnemyDatabase.GetOrLoadByGuid("463d16121f884984abe759de38418e48").bulletBank.GetBullet("ball").BulletObject;
            Spore_Small = EnemyDatabase.GetOrLoadByGuid("f905765488874846b7ff257ff81d6d0c").bulletBank.GetBullet("spore1").BulletObject;
            Spore_Large = EnemyDatabase.GetOrLoadByGuid("f905765488874846b7ff257ff81d6d0c").bulletBank.GetBullet("spore2").BulletObject;
            Disruption = EnemyDatabase.GetOrLoadByGuid("3e98ccecf7334ff2800188c417e67c15").bulletBank.GetBullet("disruption").BulletObject;
            Kunai = EnemyDatabase.GetOrLoadByGuid("2e6223e42e574775b56c6349921f42cb").bulletBank.GetBullet("dagger").BulletObject;
            Fireball_Biting = EnemyDatabase.GetOrLoadByGuid("39de9bd6a863451a97906d949c103538").bulletBank.GetBullet("flame").BulletObject;
            Smooth_EighteenByEighteen = EnemyDatabase.GetOrLoadByGuid("ec6b674e0acd4553b47ee94493d66422").bulletBank.GetBullet("bigBullet").BulletObject;
            Narrow_Ellipse = EnemyDatabase.GetOrLoadByGuid("4b992de5b4274168a8878ef9bf7ea36b").transform.Find("tentacle middle left").gameObject.GetComponent<BeholsterTentacleController>().OverrideProjectile.gameObject;

            //Special Behaviours
            Bouncing = EnemyDatabase.GetOrLoadByGuid("1a4872dafdb34fd29fe8ac90bd2cea67").bulletBank.GetBullet("default").BulletObject;
            Spirat = EnemyDatabase.GetOrLoadByGuid("7ec3e8146f634c559a7d58b19191cd43").bulletBank.GetBullet("self").BulletObject;
            Homing_Skull = EnemyDatabase.GetOrLoadByGuid("af84951206324e349e1f13f9b7b60c1a").bulletBank.GetBullet("homing").BulletObject;
            Fuselier_Cannonball = EnemyDatabase.GetOrLoadByGuid("39de9bd6a863451a97906d949c103538").bulletBank.GetBullet("ball").BulletObject;
            Cannonball = EnemyDatabase.GetOrLoadByGuid("5e0af7f7d9de4755a68d2fd3bbc15df4").bulletBank.GetBullet("cannon").BulletObject;
            Molotov = EnemyDatabase.GetOrLoadByGuid("8b913eea3d174184be1af362d441910d").bulletBank.GetBullet("molotov").BulletObject;

            //Explosive
            Grenade = EnemyDatabase.GetOrLoadByGuid("b4666cb6ef4f4b038ba8924fd8adf38f").bulletBank.GetBullet("self").BulletObject;
            Homing_Missile = EnemyDatabase.GetOrLoadByGuid("4b992de5b4274168a8878ef9bf7ea36b").transform.Find("tentacle upper right").gameObject.GetComponent<BeholsterTentacleController>().OverrideProjectile.gameObject;

            //Weird
            Lore_Bard = EnemyDatabase.GetOrLoadByGuid("56fb939a434140308b8f257f0f447829").bulletBank.GetBullet("bard").BulletObject;
            Lore_Wizard = EnemyDatabase.GetOrLoadByGuid("56fb939a434140308b8f257f0f447829").bulletBank.GetBullet("mage").BulletObject;
            Lore_Rogue = EnemyDatabase.GetOrLoadByGuid("56fb939a434140308b8f257f0f447829").bulletBank.GetBullet("rogue").BulletObject;
            Lore_Warrior = EnemyDatabase.GetOrLoadByGuid("56fb939a434140308b8f257f0f447829").bulletBank.GetBullet("knight").BulletObject;
            Astral_Projection = EnemyDatabase.GetOrLoadByGuid("43426a2e39584871b287ac31df04b544").bulletBank.GetBullet("astral").BulletObject;
            Pizza = EnemyDatabase.GetOrLoadByGuid("9215d1a221904c7386b481a171e52859").bulletBank.GetBullet("default").BulletObject;
            Blobulon = EnemyDatabase.GetOrLoadByGuid("1b5810fafbec445d89921a4efb4e42b7").bulletBank.GetBullet("blobulon").BulletObject;
            Blobuloid = EnemyDatabase.GetOrLoadByGuid("1b5810fafbec445d89921a4efb4e42b7").bulletBank.GetBullet("blobuloid").BulletObject;
            Blobulin = EnemyDatabase.GetOrLoadByGuid("1b5810fafbec445d89921a4efb4e42b7").bulletBank.GetBullet("blobulin").BulletObject;

            //Custom Bullets
            BouncySniper = Bouncing.InstantiateAndFakeprefab();
            BouncySniper.GetComponentInChildren<tk2dSprite>().SetSprite(Sniper.GetComponentInChildren<tk2dSprite>().GetCurrentSpriteDef().materialId);

            BouncyDisruption = Disruption.InstantiateAndFakeprefab();
            BouncyDisruption.gameObject.AddComponent<BounceProjModifier>().numberOfBounces = 5;

        }


        public static GameObject Regular_EightByEight;
        public static GameObject Smooth_EighteenByEighteen;
        public static GameObject Flashing_EightByEight;
        public static GameObject Fireball_EightByEight;
        public static GameObject Fireball_TwelveByTwelve;
        public static GameObject Bullat;
        public static GameObject Short_Pointy;
        public static GameObject Long_Pointy;
        public static GameObject Small_Spinning_Fire;
        public static GameObject Cue_Ball;
        public static GameObject Chainlink;
        public static GameObject Shotgrub_Gross;
        public static GameObject Sniper;
        public static GameObject Sniper_Flashing;
        public static GameObject Arrow;
        public static GameObject Number_Six;
        public static GameObject Spinning_Triangle;
        public static GameObject Egg;
        public static GameObject Bubble;
        public static GameObject Long_Ellipse;
        public static GameObject Narrow_Ellipse;
        public static GameObject Atom;
        public static GameObject Rolling_TwentyByTwenty;
        public static GameObject Rolling_ThirtyTwoByThirtyTwo;
        public static GameObject Chain_Mace;
        public static GameObject Spore_Small;
        public static GameObject Spore_Large;
        public static GameObject Disruption;
        public static GameObject Kunai;
        public static GameObject Fireball_Biting;

        //Special Behaviours
        public static GameObject Bouncing;
        public static GameObject Spirat;
        public static GameObject Homing_Skull;
        public static GameObject Fuselier_Cannonball;
        public static GameObject Cannonball;
        public static GameObject Molotov;

        //Explosive
        public static GameObject Grenade;
        public static GameObject Homing_Missile;

        //Weird
        public static GameObject Lore_Bard;
        public static GameObject Lore_Wizard;
        public static GameObject Lore_Warrior;
        public static GameObject Lore_Rogue;
        public static GameObject Astral_Projection;
        public static GameObject Pizza;
        public static GameObject Blobulon;
        public static GameObject Blobuloid;
        public static GameObject Blobulin;

        //Custom
        public static GameObject BouncySniper;
        public static GameObject BouncyDisruption;


    }
}
