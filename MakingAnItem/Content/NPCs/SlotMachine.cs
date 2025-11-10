using Alexandria.BreakableAPI;
using Alexandria.ChestAPI;
using Alexandria.DungeonAPI;
using Alexandria.ItemAPI;
using Dungeonator;
using HarmonyLib;
using SaveAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.Misc;
using static UnityEngine.UI.GridLayoutGroup;
using Brave.BulletScript;

namespace NevernamedsItems
{
    public class SlotMachine : BraveBehaviour
    {
        public static void Init()
        {
            var mainObj = ItemBuilder.SpriteFromBundle("slotmachine_body", Initialisation.NPCCollection.GetSpriteIdByName("slotmachine_body"), Initialisation.NPCCollection, new GameObject("Slot Machine"));
            mainObj.SetActive(false);
            FakePrefab.MarkAsFakePrefab(mainObj);
            tk2dSprite mainObjSprite = mainObj.GetComponent<tk2dSprite>();

            mainObjSprite.HeightOffGround = -1f;
            mainObjSprite.SortingOrder = 0;
            mainObjSprite.renderLayer = 0;
            mainObj.GetComponent<MeshRenderer>().lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            mainObjSprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutout");
            mainObjSprite.usesOverrideMaterial = true;


            mainObj.AddComponent<SlotMachine>();

            var body = mainObj.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(new IntVector2(3, -1), new IntVector2(51, 29));
            body.CollideWithTileMap = false;
            body.CollideWithOthers = true;
            mainObj.GetComponent<tk2dSprite>().renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutout");
            mainObj.GetComponent<tk2dSprite>().usesOverrideMaterial = true;
            body.PrimaryPixelCollider.CollisionLayer = CollisionLayer.HighObstacle;

            #region Face
            var face = ItemBuilder.SpriteFromBundle("slotmachine_face_idle_001", Initialisation.NPCCollection.GetSpriteIdByName("slotmachine_face_idle_001"), Initialisation.NPCCollection, new GameObject("Face"));
            tk2dSprite faceSprite = face.GetComponent<tk2dSprite>();
            faceSprite.HeightOffGround = 5f;

            tk2dSpriteAnimator faceAnimator = face.GetOrAddComponent<tk2dSpriteAnimator>();
            faceAnimator.Library = Initialisation.npcAnimationCollection;
            faceAnimator.defaultClipId = Initialisation.npcAnimationCollection.GetClipIdByName("slotface_idle");
            faceAnimator.DefaultClipId = Initialisation.npcAnimationCollection.GetClipIdByName("slotface_idle");
            faceAnimator.playAutomatically = true;

            face.transform.SetParent(mainObj.transform);
            face.transform.localPosition = new Vector3(9f / 16f, 37f / 16f, 0f);
            #endregion

            var buttonA = ItemBuilder.SpriteFromBundle("slotmachine_button1", Initialisation.NPCCollection.GetSpriteIdByName("slotmachine_button1"), Initialisation.NPCCollection, new GameObject("buttonA"));
            tk2dSprite buttonAsprite = buttonA.GetComponent<tk2dSprite>();
            buttonAsprite.HeightOffGround = 0.5f;
            buttonAsprite.SortingOrder = 0;
            buttonAsprite.renderLayer = 0;
            buttonA.GetComponent<MeshRenderer>().lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            buttonAsprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutout");
            buttonAsprite.usesOverrideMaterial = true;
            buttonA.AddComponent<SlotMachineButton>().betAlteration = 10;
            buttonA.transform.SetParent(mainObj.transform);
            buttonA.transform.localPosition = new Vector3(7f / 16f, 6f / 16f, 0f);


            var buttonB = ItemBuilder.SpriteFromBundle("slotmachine_button2", Initialisation.NPCCollection.GetSpriteIdByName("slotmachine_button2"), Initialisation.NPCCollection, new GameObject("buttonB"));
            tk2dSprite buttonBsprite = buttonB.GetComponent<tk2dSprite>();
            buttonBsprite.HeightOffGround = 0.5f;
            buttonBsprite.SortingOrder = 0;
            buttonBsprite.renderLayer = 0;
            buttonB.GetComponent<MeshRenderer>().lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            buttonBsprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutout");
            buttonBsprite.usesOverrideMaterial = true;
            buttonB.AddComponent<SlotMachineButton>().betAlteration = -10;
            buttonB.transform.SetParent(mainObj.transform);
            buttonB.transform.localPosition = new Vector3(23f / 16f, 6f / 16f, 0f);

            var lever = ItemBuilder.SpriteFromBundle("slotmachine_lever_idle_001", Initialisation.NPCCollection.GetSpriteIdByName("slotmachine_lever_idle_001"), Initialisation.NPCCollection, new GameObject("lever"));
            tk2dSprite leversprite = lever.GetComponent<tk2dSprite>();
            leversprite.HeightOffGround = 2f;
            leversprite.SortingOrder = 0;
            leversprite.renderLayer = 0;
            lever.GetComponent<MeshRenderer>().lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            leversprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutout");
            leversprite.usesOverrideMaterial = true;
            lever.AddComponent<SlotMachineLever>();
            tk2dSpriteAnimator leverAnimator = lever.GetOrAddComponent<tk2dSpriteAnimator>();
            leverAnimator.Library = Initialisation.npcAnimationCollection;
            leverAnimator.defaultClipId = Initialisation.npcAnimationCollection.GetClipIdByName("lever_idle");
            leverAnimator.DefaultClipId = Initialisation.npcAnimationCollection.GetClipIdByName("lever_idle");
            leverAnimator.playAutomatically = true;
            lever.transform.SetParent(mainObj.transform);
            lever.transform.localPosition = new Vector3(54f / 16f, 19f / 16f, 0f);

            var ShadowObj = ItemBuilder.SpriteFromBundle("slotmachine_shadow", Initialisation.NPCCollection.GetSpriteIdByName("slotmachine_shadow"), Initialisation.NPCCollection, new GameObject("Shadow"));
            tk2dSprite shadowSprite = ShadowObj.GetComponent<tk2dSprite>();
            shadowSprite.HeightOffGround = -2f;
            shadowSprite.SortingOrder = 0;
            shadowSprite.IsPerpendicular = false;
            shadowSprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTilted");
            shadowSprite.usesOverrideMaterial = true;
            ShadowObj.transform.SetParent(mainObj.transform);
            ShadowObj.transform.localPosition = new Vector3(0f, -2f / 16f);


            //SLOTS

            var slot1 = ItemBuilder.SpriteFromBundle("slot_default_001", Initialisation.NPCCollection.GetSpriteIdByName("slot_default_001"), Initialisation.NPCCollection, new GameObject("Slot1"));
            tk2dSprite slot1sprite = slot1.GetComponent<tk2dSprite>();
            slot1sprite.HeightOffGround = 3f;
            slot1.transform.SetParent(mainObj.transform);
            slot1.GetOrAddComponent<tk2dSpriteAnimator>().Library = Initialisation.npcAnimationCollection;
            slot1.transform.localPosition = new Vector3(7f / 16f, 17f / 16f, 0f);

            var slot2 = ItemBuilder.SpriteFromBundle("slot_default_002", Initialisation.NPCCollection.GetSpriteIdByName("slot_default_002"), Initialisation.NPCCollection, new GameObject("Slot2"));
            tk2dSprite slot2sprite = slot2.GetComponent<tk2dSprite>();
            slot2sprite.HeightOffGround = 3f;
            slot2.transform.SetParent(mainObj.transform);
            slot2.GetOrAddComponent<tk2dSpriteAnimator>().Library = Initialisation.npcAnimationCollection;
            slot2.transform.localPosition = new Vector3(22f / 16f, 17f / 16f, 0f);

            var slot3 = ItemBuilder.SpriteFromBundle("slot_default_003", Initialisation.NPCCollection.GetSpriteIdByName("slot_default_003"), Initialisation.NPCCollection, new GameObject("Slot3"));
            tk2dSprite slot3sprite = slot3.GetComponent<tk2dSprite>();
            slot3sprite.HeightOffGround = 3f;
            slot3.transform.SetParent(mainObj.transform);
            slot3.GetOrAddComponent<tk2dSpriteAnimator>().Library = Initialisation.npcAnimationCollection;
            slot3.transform.localPosition = new Vector3(38f / 16f, 17f / 16f, 0f);

            Dictionary<GameObject, float> dict = new Dictionary<GameObject, float>() { { mainObj, 1f } };
            DungeonPlaceable placeable = BreakableAPIToolbox.GenerateDungeonPlaceable(dict);
            placeable.isPassable = false;
            placeable.width = 4;
            placeable.height = 2;
            StaticReferences.StoredDungeonPlaceables.Add("slotmachine", placeable);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:slotmachine", placeable);

            mapIcon = ItemBuilder.SpriteFromBundle("slotmachine_map", Initialisation.NPCCollection.GetSpriteIdByName("slotmachine_map"), Initialisation.NPCCollection, new GameObject("slotmachine_map"));
            mapIcon.MakeFakePrefab();


            LobbedProjectile proj = DataCloners.CopyFields<LobbedProjectile>(Instantiate((PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0]));
            proj.gameObject.MakeFakePrefab();
            proj.visualHeight = 2f;
            proj.canHitAnythingEvenWhenNotGrounded = false;
            proj.InstantiateAndFakeprefab();
            proj.baseData.speed *= 0.5f;
            proj.SetProjectileSprite("enemy_grenade", 15, 15, true, tk2dBaseSprite.Anchor.MiddleCenter, 13, 13, overrideCollection: Initialisation.EnemyProjectileCollection);

            ExplosiveModifier explod = proj.gameObject.AddComponent<ExplosiveModifier>();
            explod.IgnoreQueues = true;
            explod.explosionData = StaticExplosionDatas.genericLargeExplosion;

            proj.gameObject.AddComponent<ProjectileSpinner>();

            grenade = proj.gameObject;

            smoke = EnemyDatabase.GetOrLoadByGuid("5e0af7f7d9de4755a68d2fd3bbc15df4").transform.Find("Room Smoke Particles").gameObject;
        }
        public RoomHandler m_room;
        public static GameObject mapIcon;
        public static GameObject smoke;
        public GameObject instancedMapIcon;
        public Chancellot master;

        public SlotMachineLever lever;
        public SlotMachineButton increaseButton;
        public SlotMachineButton decreaseButton;
        public GameObject face;
        public tk2dSpriteAnimator faceAnimator;

        public tk2dSpriteAnimator wheel1;
        public tk2dSpriteAnimator wheel2;
        public tk2dSpriteAnimator wheel3;

        public static GameObject grenade;

        public int NumCreditsPaidOut = 0;
        public GameObject curSmoke = null;
        public bool busy;
        public int currentBet = 20;

        bool playingOutcomeFace = false;
        float timePlayingOutcomeFace = 0f;

        private void Start()
        {
            this.m_room = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round));

            increaseButton = base.transform.Find("buttonA").gameObject.GetComponent<SlotMachineButton>();
            increaseButton.master = this;
            decreaseButton = base.transform.Find("buttonB").gameObject.GetComponent<SlotMachineButton>();
            decreaseButton.master = this;

            lever = base.transform.Find("lever").gameObject.GetComponent<SlotMachineLever>();
            lever.master = this;

            face = base.transform.Find("Face").gameObject;
            faceAnimator = face.GetComponent<tk2dSpriteAnimator>();

            wheel1 = base.transform.Find("Slot1").gameObject.GetComponent<tk2dSpriteAnimator>();
            wheel2 = base.transform.Find("Slot2").gameObject.GetComponent<tk2dSpriteAnimator>();
            wheel3 = base.transform.Find("Slot3").gameObject.GetComponent<tk2dSpriteAnimator>();

            increaseButton.sprite.UpdateZDepthAttached(-1f, base.transform.position.y, true);
            decreaseButton.sprite.UpdateZDepthAttached(-1f, base.transform.position.y, true);
            lever.sprite.UpdateZDepthAttached(-1f, base.transform.position.y, true);

            instancedMapIcon = Minimap.Instance.RegisterRoomIcon(m_room, mapIcon, false);
        }
        private void Update()
        {
            if (playingOutcomeFace)
            {
                if (timePlayingOutcomeFace > 5f)
                {
                    faceAnimator.Play("slotface_idle");
                    playingOutcomeFace = false;
                    timePlayingOutcomeFace = 0;
                }
                else { timePlayingOutcomeFace += BraveTime.DeltaTime; }
            }
        }
        public void DoodadTriggered(int betAlteration = 0, bool lever = false, PlayerController interactor = null)
        {
            if (betAlteration != 0)
            {
                if (currentBet + betAlteration < 10)
                {
                    Talk($"ERR: Minimum bet is 10[sprite \"ui_coin\"]!", interactor);
                }
                else
                {
                    currentBet += betAlteration;
                    Talk($"Current Bet: {currentBet}[sprite \"ui_coin\"]", interactor);
                }
            }
            if (lever)
            {
                if (interactor.carriedConsumables.Currency >= currentBet)
                {
                    base.StartCoroutine(RollTheBones(interactor));
                }
                else { Talk("ERR: Insufficient Funds!", interactor); }

            }
        }
        public void Talk(string text, PlayerController interactor)
        {
            if (!TextBoxManager.HasTextBox(base.transform)) { TextBoxManager.ClearTextBox(base.transform); }
            TextBoxManager.ShowTextBox(base.transform.position + new Vector3(29f / 16f, 58 / 16f), base.transform, 1.5f, text, "computer", true, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
        }

        private IEnumerator RollTheBones(PlayerController interactor)
        {
            if (master == null)
            {
                foreach (Chancellot chance in Chancellot.allChancelots) { if (chance.m_room == this.m_room) { master = chance; } }
            }

            interactor.carriedConsumables.Currency -= currentBet;
            if (master) { master.Inform("betmade", currentBet); }
            SaveAPIManager.RegisterStatChange(CustomTrackedStats.TIMES_GAMBLED, 1);

            playingOutcomeFace = false;
            timePlayingOutcomeFace = 0f;
            busy = true;
            lever.spriteAnimator.Play("lever_pull");


            //Display
            faceAnimator.Play("slotface_spinning");
            wheel1.Play("slot_spin");
            wheel2.Play("slot_spin");
            wheel3.Play("slot_spin");

            AkSoundEngine.PostEvent("Play_OBJ_Chest_Synergy_Slots_01", base.gameObject);

            string outcomeA = GetOutcome("null", interactor);
            yield return new WaitForSeconds(0.8f);
            wheel1.Play($"slot_{outcomeA}");
            AkSoundEngine.PostEvent("Play_Respawn", base.gameObject);

            string outcomeB = GetOutcome(outcomeA, interactor);
            yield return new WaitForSeconds(0.8f);
            wheel2.Play($"slot_{outcomeB}");
            AkSoundEngine.PostEvent("Play_Respawn", base.gameObject);

            string outcomeC = GetOutcome(outcomeB, interactor);
            yield return new WaitForSeconds(1.5f);
            wheel3.Play($"slot_{outcomeC}");
            AkSoundEngine.PostEvent("Play_Respawn", base.gameObject);

            yield return new WaitForSeconds(0.5f);

            bool anyBullet = outcomeA == "fail" || outcomeA == "fail" || outcomeC == "fail";
            bool x2 = outcomeA == "x2" || outcomeA == "x2" || outcomeC == "x2";
            List<string> noPoints = new List<string>() { "fail", "x2" };

            bool toBreak = false;
            if (winValue > 0)
            {
                float chanceToBreak = winValue / 250f;
                if (outcomeA == "fail" && outcomeA == "fail" && outcomeC == "fail") { chanceToBreak *= 2; }
                if (UnityEngine.Random.value <= chanceToBreak) { toBreak = true; }
            }

            if (toBreak)
            {
                if (master) { master.Inform("break"); }
                Break();
            }
            else
            {
                //Determine Ultimate Outcome
                int exp = 0;
                if (outcomeB == outcomeA)
                {
                    if (outcomeB == outcomeC) { Payout(outcomeB, 2, x2, anyBullet); exp = noPoints.Contains(outcomeB) ? 0 : 2; }
                    else { Payout(outcomeB, 1, x2, anyBullet); exp = noPoints.Contains(outcomeB) ? 0 : 1; }
                }
                else if (outcomeB == outcomeC) { Payout(outcomeB, 1, x2, anyBullet); exp = noPoints.Contains(outcomeB) ? 0 : 1; }
                else if (outcomeA == outcomeC) { Payout(outcomeA, 1, x2, anyBullet); exp = noPoints.Contains(outcomeA) ? 0 : 1; }
                else { winValue -= 0.1f; }
                switch (exp)
                {
                    case 0:
                        if (currentBet > 100) { faceAnimator.Play("slotface_bigloss"); }
                        else { faceAnimator.Play("slotface_minorloss"); }
                        if (master) { master.Inform("loss"); }
                        AkSoundEngine.PostEvent("Play_OBJ_Chest_Synergy_Lose_01", base.gameObject);
                        break;
                    case 1:
                        faceAnimator.Play("slotface_minorwin");
                        if (master) { master.Inform("minorwin"); }
                        AkSoundEngine.PostEvent("Play_OBJ_Chest_Synergy_Win_01", base.gameObject);
                        break;
                    case 2:
                        faceAnimator.Play("slotface_bigwin");
                        if (master) { master.Inform("bigwin"); }
                        AkSoundEngine.PostEvent("Play_OBJ_Chest_Synergy_Win_01", base.gameObject);
                        break;
                }
            }
  
            playingOutcomeFace = true;
            timePlayingOutcomeFace = 0f;

            lever.spriteAnimator.Play("lever_reset");
            busy = false;
            yield break;
        }
        float winValue = 0;
        public void Payout(string payout, int level, bool anysliderisx2, bool anySliderIsBullet)
        {

            switch (payout)
            {
                case "fail":
                    Vector2 firePos = base.transform.position + new Vector3(29f / 16f, -5f / 16f);
                    UnityEngine.Object.Instantiate<GameObject>((PickupObjectDatabase.GetById(9) as Gun).DefaultModule.projectiles[0].hitEffects.overrideMidairDeathVFX, firePos, Quaternion.identity);
                    for (int i = 0; i < (3 * level) * (anysliderisx2 ? 1 : 2); i++)
                    {
                        GameObject spawnedProj = ProjSpawnHelper.SpawnProjectileTowardsPoint(grenade, firePos, GameManager.Instance.PrimaryPlayer.CenterPosition, 0, 40, null);
                        LobbedProjectile component2 = spawnedProj.GetComponent<LobbedProjectile>();
                        component2.UpdateCollisionMask();
                        component2.specRigidbody.RegisterSpecificCollisionException(base.specRigidbody);
                        if (anysliderisx2) { component2.BecomeBlackBullet(); }
                        component2.forcedDistance = Vector2.Distance(firePos, GameManager.Instance.PrimaryPlayer.CenterPosition);
                        component2.forcedDistance *= (UnityEngine.Random.Range(50f, 150f) / 100f);
                        component2.baseData.speed *= (UnityEngine.Random.Range(50f, 150f) / 100f);
                        component2.UpdateSpeed();
                    }
                    winValue -= 1f;
                    break;
                case "x2":
                    winValue -= 0.5f;
                    break;
                case "copper":
                    winValue += 1;
                    CashOut(Mathf.CeilToInt((float)currentBet * (level == 1 ? 1.2f : 1.4f)), false, anysliderisx2, anySliderIsBullet);
                    break;
                case "silver":
                    winValue += 4;
                    CashOut(Mathf.CeilToInt((float)currentBet * (level == 1 ? 1.5f : 2f)), false, anysliderisx2, anySliderIsBullet);
                    if (level == 2) { CashOut(Mathf.CeilToInt((float)currentBet * 0.1f), true, anysliderisx2, anySliderIsBullet); }
                    break;
                case "gold":
                    winValue += 8;
                    CashOut(Mathf.CeilToInt((float)currentBet * (level == 1 ? 2f : 3f)), false, anysliderisx2, anySliderIsBullet);
                    CashOut(Mathf.CeilToInt((float)currentBet * (level == 1 ? 0.1f : 0.5f)), true, anysliderisx2, anySliderIsBullet);
                    break;
                case "chest":
                    winValue += 10;
                    Vector2 spawnPoint = base.transform.position + new Vector3(13f / 16f, -31f / 16f);
                    Vector2 spawnPoint2 = base.transform.position + new Vector3(13f / 16f, -60f / 16f);


                    ChestUtility.ChestTier chosen = ChestUtility.ChestTier.BROWN;

                    if (currentBet <= 30) { chosen = ChestUtility.ChestTier.BROWN; }
                    else if (currentBet <= 40) { chosen = ChestUtility.ChestTier.BLUE; }
                    else if (currentBet <= 60) { chosen = ChestUtility.ChestTier.GREEN; }
                    else if (currentBet <= 80) { chosen = ChestUtility.ChestTier.SYNERGY; }
                    else if (currentBet <= 100) { chosen = ChestUtility.ChestTier.RED; }
                    else { chosen = ChestUtility.ChestTier.BLACK; }

                    if (level == 2)
                    {
                        chosen = UpgradeTier(chosen, UnityEngine.Random.value <= 0.4f ? 2 : 1);
                    }

                    if (UnityEngine.Random.value <= ((level == 2) ? 0.000666f : 0.000333f)) { chosen = ChestUtility.ChestTier.RAINBOW; }

                    ChestUtility.SpawnChestEasy(spawnPoint.ToIntVector2(), chosen, anySliderIsBullet, Chest.GeneralChestType.UNSPECIFIED);
                    if (anysliderisx2) { ChestUtility.SpawnChestEasy(spawnPoint2.ToIntVector2(), chosen, anySliderIsBullet, Chest.GeneralChestType.UNSPECIFIED); }
                    break;
            }
        }
        public bool isBroken = false;
        public void Unbreak()
        {

            if (!isBroken) return;

            this.face.SetActive(true);
            this.lever.gameObject.SetActive(true);
            this.increaseButton.gameObject.SetActive(true);
            this.decreaseButton.gameObject.SetActive(true);
            this.wheel1.gameObject.SetActive(true);
            this.wheel2.gameObject.SetActive(true);
            this.wheel3.gameObject.SetActive(true);
            if (curSmoke) { UnityEngine.Object.Destroy(curSmoke); }

            AkSoundEngine.PostEvent("electricdrillbuzz", base.gameObject);
            UnityEngine.Object.Instantiate<GameObject>((PickupObjectDatabase.GetById(9) as Gun).DefaultModule.projectiles[0].hitEffects.overrideMidairDeathVFX, this.specRigidbody.UnitCenter, Quaternion.identity);


            base.sprite.SetSprite(Initialisation.NPCCollection.GetSpriteIdByName("slotmachine_body"));

            isBroken = false;
        }
        public void Break()
        {
            if (isBroken) return;
            winValue = 0;
            this.face.SetActive(false);
            this.lever.gameObject.SetActive(false);
            this.increaseButton.gameObject.SetActive(false);
            this.decreaseButton.gameObject.SetActive(false);
            this.wheel1.gameObject.SetActive(false);
            this.wheel2.gameObject.SetActive(false);
            this.wheel3.gameObject.SetActive(false);

            if (curSmoke) { UnityEngine.Object.Destroy(curSmoke); }
            GameObject newSmoke = GameObject.Instantiate(smoke, base.transform.position + new Vector3(29f / 16f, -5f / 16f), Quaternion.identity);
            curSmoke = newSmoke;

            Exploder.DoDefaultExplosion(base.specRigidbody.UnitCenter, Vector2.zero);

            base.sprite.SetSprite(Initialisation.NPCCollection.GetSpriteIdByName("slotmachine_broken"));

            isBroken = true;
        }
        public ChestUtility.ChestTier UpgradeTier(ChestUtility.ChestTier tier, int AMT = 1)
        {
            ChestUtility.ChestTier final = ChestUtility.ChestTier.BROWN;
            switch (tier)
            {
                case ChestUtility.ChestTier.BLACK:
                    if (AMT == 2)
                    {
                        if (UnityEngine.Random.value <= 0.02f) { final = ChestUtility.ChestTier.RAINBOW; }
                        else { final = ChestUtility.ChestTier.BLACK; }
                    }
                    else { final = ChestUtility.ChestTier.BLACK; }
                    break;
                case ChestUtility.ChestTier.RED:
                    if (AMT == 2) { final = ChestUtility.ChestTier.BLACK; }
                    else { final = ChestUtility.ChestTier.BLACK; }
                    break;
                case ChestUtility.ChestTier.GREEN:
                    if (AMT == 2) { final = ChestUtility.ChestTier.BLACK; }
                    else
                    {
                        if (UnityEngine.Random.value <= 0.5f) { final = ChestUtility.ChestTier.SYNERGY; }
                        else { final = ChestUtility.ChestTier.RED; }
                    }
                    break;
                case ChestUtility.ChestTier.SYNERGY:
                    if (AMT == 2) { final = ChestUtility.ChestTier.BLACK; }
                    else { final = ChestUtility.ChestTier.RED; }
                    break;
                case ChestUtility.ChestTier.BLUE:
                    if (AMT == 2) { final = ChestUtility.ChestTier.RED; }
                    else { final = ChestUtility.ChestTier.GREEN; }
                    break;
                case ChestUtility.ChestTier.BROWN:
                    if (AMT == 2) { final = ChestUtility.ChestTier.GREEN; }
                    else { final = ChestUtility.ChestTier.BLUE; }
                    break;
            }
            return final;
        }
        public void ChestOut(IntVector2 vec, ChestUtility.ChestTier tier)
        {
            ChestUtility.SpawnChestEasy(vec, tier, false, Chest.GeneralChestType.UNSPECIFIED, Alexandria.Misc.ThreeStateValue.UNSPECIFIED, Alexandria.Misc.ThreeStateValue.FORCENO);
        }
        public void CashOut(int amount, bool hc, bool doubled, bool impacted)
        {

            int fin = doubled ? amount * 2 : amount;
            if (impacted)
            {
                fin = Mathf.FloorToInt((float)fin * UnityEngine.Random.Range(0.75f, 0.95f));
            }
            if (NumCreditsPaidOut >= 100) { return; }
            if (hc)
            {
                int tot = NumCreditsPaidOut += fin;
                if (tot > 100) { fin -= (tot - 100); }
                NumCreditsPaidOut += fin;
            }
            SaveAPIManager.RegisterStatChange(CustomTrackedStats.GAMBLING_WINNINGS, fin - currentBet);
            LootEngine.SpawnCurrency(base.transform.position + new Vector3(29f / 16f, -5f / 16f), fin, hc, new Vector2?(Vector2.down * 1.75f), new float?(45f), 4f, 0.05f);
        }

        public string GetOutcome(string prev, PlayerController interactor)
        {
            string chosen = BraveUtility.RandomElement(outcomes);

            float luck = GetPlayerLuck(interactor);

            int rerollsForConsistency = 0;
            float residualLuck = luck;
            while (residualLuck > 0f)
            {
                if (luck > 1f) { rerollsForConsistency++; residualLuck--; }
                else
                {
                    if (UnityEngine.Random.value <= residualLuck) { rerollsForConsistency++; }
                    residualLuck = 0;
                }
            }
            for (int i = 0; i < rerollsForConsistency; i++)
            {
                if (chosen == "fail" || (prev != "fail" && prev != "x2" && prev != "null" && chosen != prev))
                {
                    chosen = BraveUtility.RandomElement(outcomes);
                }
            }

            if (chosen == "silver" && prev != "silver" && UnityEngine.Random.value <= (luck * 0.25f)) { chosen = "gold"; }
            if (chosen == "copper" && prev != "copper" && UnityEngine.Random.value <= (luck * 0.75f)) { chosen = "silver"; }

            return chosen;
        }

        public float GetPlayerLuck(PlayerController interactor)
        {
            float amt = 0.1f;
            amt += PlayerStats.GetTotalCoolness() * 0.1f;

            if (interactor.HasPickupID(407)) { amt += PlayerStats.GetTotalCurse() * 0.1f; }
            else { amt -= PlayerStats.GetTotalCurse() * 0.1f; }

            foreach (PassiveItem passive in interactor.passiveItems)
            {
                if (passive.HasTag("lucky")) amt += 0.9f;
                if (passive.HasTag("unlucky")) amt -= 0.9f;
                if (passive.HasTag("very_lucky")) amt += 2.5f;
            }
            foreach (PlayerItem active in interactor.activeItems)
            {
                if (active.HasTag("lucky")) amt += 0.9f;
                if (active.HasTag("unlucky")) amt -= 0.9f;
                if (active.HasTag("very_lucky")) amt += 2.5f;
            }

            return Mathf.Max(amt, 0f);
        }

        public static List<string> outcomes = new List<string>()
        {
            "fail",
            "fail",
            "fail",
            "fail",
            "copper",
            "copper",
            "copper",
            "copper",
            "silver",
            "silver",
            "silver",
            "gold",
            "gold",
            "chest",
            "chest",
            "x2",
        };
    }
    public class SlotMachineButton : BraveBehaviour, IPlayerInteractable
    {
        public SlotMachine master;
        public RoomHandler m_room;
        public int betAlteration = 0;

        private void Start()
        {
            this.m_room = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round));
            this.m_room.RegisterInteractable(this);
        }
        public float GetDistanceToPoint(Vector2 point)
        {
            if (!base.sprite) return float.MaxValue;
            Bounds bounds = base.sprite.GetBounds();
            bounds.SetMinMax(bounds.min + base.transform.position, bounds.max + base.transform.position);
            float num = Mathf.Max(Mathf.Min(point.x, bounds.max.x), bounds.min.x);
            float num2 = Mathf.Max(Mathf.Min(point.y, bounds.max.y), bounds.min.y);
            return Mathf.Sqrt((point.x - num) * (point.x - num) + (point.y - num2) * (point.y - num2));
        }

        public void OnEnteredRange(PlayerController interactor)
        {
            SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white);
            base.sprite.UpdateZDepth();
        }

        public void OnExitRange(PlayerController interactor)
        {
            SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
            base.sprite.UpdateZDepth();
        }

        public void Interact(PlayerController interactor)
        {
            if (master && !master.busy && !master.isBroken)
            {
                master.DoodadTriggered(betAlteration, false, interactor);
            }
        }
        public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
        {
            shouldBeFlipped = false;
            return string.Empty;
        }

        public float GetOverrideMaxDistance()
        {
            return 1.5f;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
    public class SlotMachineLever : BraveBehaviour, IPlayerInteractable
    {
        public SlotMachine master;
        public RoomHandler m_room;
        private void Start()
        {
            this.m_room = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round));
            this.m_room.RegisterInteractable(this);
        }
        public float GetDistanceToPoint(Vector2 point)
        {
            if (!base.sprite) return float.MaxValue;
            Bounds bounds = base.sprite.GetBounds();
            bounds.SetMinMax(bounds.min + base.transform.position, bounds.max + base.transform.position);
            float num = Mathf.Max(Mathf.Min(point.x, bounds.max.x), bounds.min.x);
            float num2 = Mathf.Max(Mathf.Min(point.y, bounds.max.y), bounds.min.y);
            return Mathf.Sqrt((point.x - num) * (point.x - num) + (point.y - num2) * (point.y - num2));
        }

        public void OnEnteredRange(PlayerController interactor)
        {
            SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white);
            base.sprite.UpdateZDepth();
        }

        public void OnExitRange(PlayerController interactor)
        {
            SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
            base.sprite.UpdateZDepth();
        }

        public void Interact(PlayerController interactor)
        {
            if (master && !master.busy && !master.isBroken)
            {
                base.StartCoroutine(Sequence(interactor));
            }
        }

        public IEnumerator Sequence(PlayerController interactor)
        {
            int selectedResponse = -1;
            interactor.SetInputOverride("slotMachine");
            yield return null;

            GameUIRoot.Instance.DisplayPlayerConversationOptions(interactor, null, $"Roll the Bones <Current Bet: {master.currentBet}[sprite \"ui_coin\"]>", "Resist the temptation");
            while (!GameUIRoot.Instance.GetPlayerConversationResponse(out selectedResponse)) { yield return null; }

            interactor.ClearInputOverride("slotMachine");
            if (selectedResponse == 0)
            {
                AkSoundEngine.PostEvent("Play_OBJ_daggershield_shot_01", base.gameObject);
                master.DoodadTriggered(0, true, interactor);
            }
            yield break;
        }
        public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
        {
            shouldBeFlipped = false;
            return string.Empty;
        }

        public float GetOverrideMaxDistance()
        {
            return 1.5f;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}