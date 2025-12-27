using Alexandria.BreakableAPI;
using Alexandria.DungeonAPI;
using Alexandria.ItemAPI;
using Dungeonator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public class MysteriousStranger : BraveBehaviour, IPlayerInteractable
    {
        public static void Init()
        {
            var mysteriousStranger = ItemBuilder.SpriteFromBundle("ms_pretzel_idle_001", Initialisation.MysteriousStrangerCollection.GetSpriteIdByName("ms_pretzel_idle_001"), Initialisation.MysteriousStrangerCollection, new GameObject("Mysterious Stranger"));
            mysteriousStranger.SetActive(false);
            FakePrefab.MarkAsFakePrefab(mysteriousStranger);
            mysteriousStranger.AddComponent<MysteriousStranger>();
            var mysteriousStrangerBody = mysteriousStranger.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(new IntVector2(-6, 7), new IntVector2(9, 12));
            mysteriousStrangerBody.CollideWithTileMap = false;
            mysteriousStrangerBody.CollideWithOthers = true;

            UltraFortunesFavor fortune = mysteriousStranger.AddComponent<UltraFortunesFavor>();
            fortune.sparkOctantVFX = ResourceManager.LoadAssetBundle("shared_auto_002").LoadAsset<GameObject>("npc_blank_jailed").GetComponentInChildren<UltraFortunesFavor>().sparkOctantVFX;

            tk2dSpriteAnimator Animator = mysteriousStranger.GetOrAddComponent<tk2dSpriteAnimator>();
            Animator.Library = Initialisation.mysteriousStrangerAnimationCollection;
            Animator.defaultClipId = Initialisation.mysteriousStrangerAnimationCollection.GetClipIdByName("pretzel_idle");
            Animator.DefaultClipId = Initialisation.mysteriousStrangerAnimationCollection.GetClipIdByName("pretzel_idle");
            Animator.playAutomatically = true;

            mysteriousStranger.GetOrAddComponent<NPCShootReactor>();

            Transform talktransform = new GameObject("talkpoint").transform;
            talktransform.SetParent(mysteriousStranger.transform);
            talktransform.transform.localPosition = new Vector3(0f, 23f / 16f);

            var smallStatueShadow = ItemBuilder.SpriteFromBundle("ms_shadow", Initialisation.MysteriousStrangerCollection.GetSpriteIdByName("ms_shadow"), Initialisation.MysteriousStrangerCollection, new GameObject("ms_shadow"));
            tk2dSprite smallStatueShadowSprite = smallStatueShadow.GetComponent<tk2dSprite>();
            smallStatueShadowSprite.HeightOffGround = -1.7f;
            smallStatueShadowSprite.SortingOrder = 0;
            smallStatueShadowSprite.IsPerpendicular = false;
            smallStatueShadowSprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTilted");
            smallStatueShadowSprite.usesOverrideMaterial = true;
            smallStatueShadow.transform.SetParent(mysteriousStranger.transform);

            Dictionary<GameObject, float> dict = new Dictionary<GameObject, float>() { { mysteriousStranger, 1f } };
            DungeonPlaceable placeable = BreakableAPIToolbox.GenerateDungeonPlaceable(dict);
            StaticReferences.StoredDungeonPlaceables.Add("mysterious_stranger", placeable);
            Alexandria.DungeonAPI.StaticReferences.customPlaceables.Add("nn:mysterious_stranger", placeable);

        }
        private Transform talkpoint;
        public static GameObject mapIcon;
        public RoomHandler m_room;
        public static List<MysteriousStranger> allStrangers = new List<MysteriousStranger>();

        public static List<string> validPeople = new List<string>()
        {
            "pretzel",
            "marcy",
            "notsoai",
            "skilotar",
            "dallan",
            "nickel",
            "qaday",
            "round",
            "accidia",
            "spapi",
            "turtle",
            "littlewasp",
            "bunny",
            "an3s",
            "schro"
        };
        public static Dictionary<string, List<string>> entryDialogue = new Dictionary<string, List<string>>()
        {
            { "pretzel", new List<string>(){ "Welcome to the Club! :D" } },
            { "notsoai", new List<string>(){ "I've never seen Axiom personnel like you before.", "Found any samples?", "Where's your safety gear?" } },
            { "accidia", new List<string>(){ "Hi hello hii!! :D", "{wq}Friend{w}! :D", "New buds are the best buds :D" } },
            { "round", new List<string>(){ "Sup, NICKNAME.", "Alright, let's see... I think I'm supposed to give you something." } },
            { "nickel", new List<string>(){ "Halla.", "Greetings.", "Welcome." } },
            { "spapi", new List<string>(){ "Google Witches" } },
            { "turtle", new List<string>(){ "Hello everybody and welcome to an Enter the Gungeon secret room!" } },
            { "qaday", new List<string>(){ "Oh, hi.", "Oop- you scared me for a second!", "Ah, right on *cue*! {wj}hehe...{w}" } },
            { "skilotar", new List<string>(){ "Hail to you!", "Salutations!", "Approach Knave!" } },
            { "dallan", new List<string>(){ "Heyo! How's it goin'?", "A visitor! Stay awhile." } },
            { "littlewasp", new List<string>(){ "Person! Hello! Don't run away!", "Hi! Hi!", "A carrier of guns! No need to shoot me! Hehe!" } },
            { "bunny", new List<string>(){ "oh, hello.", "hi again.", "welcome back.", "hello." } },
            { "an3s", new List<string>(){ "Howzit.", "Hello!!", "Ho brah, you like come over?" } },
            { "schro", new List<string>(){ "well well well, look who made it! Ive been waiting for ya to arrive!", "Oh hey! Ya caught me at the perfect time!", "Hm? Oh, hey, didnt see ya there!" } },
            { "marcy", new List<string>(){ "Please, free me!" } },
            { "rat", new List<string>(){ "{wj}squeak!{w}" } },
        };
        public static Dictionary<string, List<string>> giveItemDialogue = new Dictionary<string, List<string>>()
        {
            { "pretzel", new List<string>(){"Wow, a visitor! O: It's been a while since someone's come around.","Excuse me, I almost forgot my manners. *ahem* welcome to ...","...the Gungeon Club for Resplendent Appreciation of Firearms and Trinkets! :D","As a token of {wr}my{w} appreciation for you coming by, I have a gift for you!","Hopefully you can appreciate it as much as I have. C:"} },
            { "notsoai", new List<string>(){"At Axiom Munitions, we pride ourselves in the finest products...", "...and the {wj}deadliest weaponry.{w}", "Take this, free of charge."} },
            { "accidia", new List<string>(){ "Hey {wq}friend{w} look at this thingy i found! :D", "Don't know what it does, but it sure tastes bad!" } },
            { "round", new List<string>(){ "Oh, yeah, here it is. ITEMNAME is alright.", "Just not up to my high standards." } },
            { "nickel", new List<string>(){ "This will watch over you in your time of need..." } },
            { "spapi", new List<string>(){ "Google Witches" } },
            { "turtle", new List<string>(){ "Here ya go, have this." } },
            { "qaday", new List<string>(){ "Nice to see ya! I see you're a bit busy, but there's some cool stuff I got!","Why don't I give you a little something? (Don't ask where it came from, and how it materialises out of me)", "{wj}Enjoy your present!{w}" } },
            { "skilotar", new List<string>(){ "By my Crown and the devine right entrusted to me..","I bestow upon thee a gift to aid your quest!" } },
            { "dallan", new List<string>(){ "You look like you need some assistance. I think it would be smart to help each other out down here.", "Take this, and good luck." } },
            { "littlewasp", new List<string>(){ "Wowie! That's (probably) a lot of guns you have on you there!", "I'm sure you really, really, really want another one, huh? Here you go!" } },
            { "bunny", new List<string>(){ "{wj}h a v e   f u n !{w}" } },
            { "an3s", new List<string>(){ "Take this, hopefully you could make some use of it in this god-forsaken Gungeon, god knows I can't.", "Kulia i ka nu'u." } },
            { "schro", new List<string>(){ "Found this earlier, take it. Dont tell the Hegemony anything." } },
            { "marcy", new List<string>(){ "Oh, hello NICKNAME!", "Please help me, some evil +!?+*!!! gave me this artifact, and it twisted my form into this awful monstrosity.", "Please take it off my hands to free me from this terrible, terrible curse!" } },
            { "rat", new List<string>(){ "{wj}squeak.{w}" } },
        };
        public static Dictionary<string, List<string>> randomDialogue = new Dictionary<string, List<string>>()
        {
            { "pretzel", new List<string>(){"Isn't it simply resplendent? O:","How's the appreciating going? O:","Swing by the Club any time! :D"} },
            { "notsoai", new List<string>(){"That was handcrafted by machines.","Don't ask about the costs.","We're hiring unpaid cannon fodder if you're interested."} },
            { "accidia", new List<string>(){ "Oh buddy you don't have to give me anything in return!, (Not that it would fit in this box anyways)" } },
            { "round", new List<string>(){ "What, don't want it? Too bad.", "If you don't want it, just toss it.", "Hey, if you ever see a 'Round King', tell him he's a paradox." } },
            { "nickel", new List<string>(){ "I bid you farewell..." } },
            { "spapi", new List<string>(){ "Google Witches" } },
            { "turtle", new List<string>(){ "There's no more items in this shell.", "Please leave before I break the game horrendously in some completely unrecreatable way." } },
            { "qaday", new List<string>(){ "Hey uh, are you gonna pay for that wall you just broke? Stuff's kinda annoying to repair, you know.", "You know, I love a good bit of TF2 - Thing Furnished 2 You!   ...No? Well, I tried. On the nose and not very good. That's how I roll!", "Yeah, I'm a floating letter. What's it to you?", "I really like ice cream :3" , "Oh yeah, and if anyone asks, I'm not a furry." } },
            { "skilotar", new List<string>(){ "Keep it in your closest care!", "Safe Travels!", "I bid you fairwell!" } },
            { "dallan", new List<string>(){ "Always happy to help!", "Endure and survive.", "A gun wields no strength unless the hands that holds it has courage.", "The only thing that can defeat power, is more power.", "What is bravery, without a dash of recklessness?", "Did I ever tell you the definition of insanity?", "The right gun in the right place can make all the difference in the world.", "There’s two ways of reasoning with the Gundead, and neither of them work.", "The best solution to a problem is usually the easiest one." } },
            { "littlewasp", new List<string>(){ "My name's Little Wasp!", "I'm sure you don't want to hear this, but yes, there ARE much bigger wasps out there!", "The gun's quality? It's rated W, for 'wasp', of course!", "I thought this was a bug-geon, but I was horribly mistaken! Hehe! Help." } },
            { "bunny", new List<string>(){ "ever make a mistake that you wish you could undo? turns out not having opposable thumbs kinda sucks.", "understanding the unknown, knowledge expanding further and further out, all for naught.", "its incredible how much exhaustion this tiny little body can hold.", "i know what you are.", "if youre good enough, you can occasionally trespass into places we were never meant to be in. thats kinda how i got here.", "sometimes, when i press my head against the walls of the Gungeon, i can hear what sounds like a distress signal. but what do i know, im just a bunny.", "know when to turn back, because once it is too late, the only way youll have left to go, is deeper, deeper, yet deeper.", "maybe ill get it right next time." } },
            { "an3s", new List<string>(){ "Carry on with your gungeoneering.", "E hele!", "I have nothing more to say to you." } },
            { "schro", new List<string>(){ "Ever met the trigger twins? Great pair, baked me a cake before.", "Look dude, I've got things to post online.", "Sorry I didnt have anything better, man.", "Ooh! I met this guy once. Cant remember his name. Just before ya, you missed him. Nice revolver he had. Somethin card related." } },
            { "marcy", new List<string>(){ "Lorem Ipsum Dolor Sit Amet" } },
            { "rat", new List<string>(){ "{wj}squeak.{w}",  } },

        };
        public static Dictionary<string, List<string>> boredDialogue = new Dictionary<string, List<string>>()
        {
            { "pretzel", new List<string>(){ "Isn't it simply resplendent? O:", "How's the appreciating going? O:", "Swing by the Club any time! :D" } },
            { "notsoai", new List<string>(){"Don't you have a quota to meet?","Did you forget Axiom's refund policy?","...add three, divide by zero... WAIT HOLD ON","..."} },
            { "accidia", new List<string>(){ "I appreciate the company :D" } },
            { "round", new List<string>(){ "I've got to stare at this wall for a couple more hours.", "Bit busy, sorry." } },
            { "nickel", new List<string>(){ "Run away, Run away.", "You have nothing more to find.", "You will leave me now!" } },
            { "spapi", new List<string>(){ "Google Witches" } },
            { "turtle", new List<string>(){ "Subscribe!", "I'm surprised your game hasn't crashed with me here.", "This shell aint big enough for the both of us.", "DON'T eat the melon....." } },
            { "qaday", new List<string>(){ "Go on now, I've got things on my to-do list to queue!", "You surely have better things to do than to hang out with the physical embodiment of a letter of the alphabet, don't you?", " Bo'a'wo'a", "[Insert Q Pun Here] (I'm very creative)" } },
            { "skilotar", new List<string>(){ "Away with you!", "I grow weary of you.", "Shoo." } },
            { "dallan", new List<string>(){ "Man, I could really use a coffee right now.", "Phew, I think I need a nap.", "Yawn..." } },
            { "littlewasp", new List<string>(){ "You'll never kill your past at this rate!", "Imagine how many minutes you'll have to rewind past when you get The Gun.", "Did you want a hug or something?" } },
            { "bunny", new List<string>(){ "I think it is time for a rest." } },
            { "an3s", new List<string>(){ "Carry on with your gungeoneering.", "E hele!", "I have nothing more to say to you." } },
            { "schro", new List<string>(){ "Look, I ain't got nothin else.", "What, you lookin to chat?", "I could rant for ya if you'd like that.", "Lookin for a date? No thanks.", "well, get a move on now." } },
            { "marcy", new List<string>(){ "Lorem Ipsum Dolor Sit Amet" } },
            { "rat", new List<string>(){ "{wj}squeak.{w}", "{wj}squeak.{w}", "{wj}squeak.{w}", "{wj}squeak.{w}", "{wj}squeak (thank you for freeing me from my curse){w}", "{wj}squeak (I knew those hot goths in my area were too good to be true){w}", "{wj}squeak (I lost my favorite rock. :( I think it was that damn rat){w}", "{wj}squeak (you know that other rat in here is a real piece of work. Just know I don't have anything to do with him){w}" } },
        };
        public static Dictionary<string, List<string>> shotAtDialogue = new Dictionary<string, List<string>>()
        {
            { "pretzel", new List<string>(){ "Can you appreciate in some other direction please?! D:" } },
            { "notsoai", new List<string>(){"You should be on the front line - The production line, that is.","Axiom Munitions 5-Megawatt Personal Refractive Shields never fail!","You're lucky they don't give me arms - or arms.","That ammunition usage is completely suboptimal.","Really?","That's a safety violation!"} },
            { "accidia", new List<string>(){ "Hehehe! {wq}Friend{w} bullets :D", "Bullets from a {wq}friend{w} :D" } },
            { "round", new List<string>(){ "You're lucky that missed.", "I could totally kick your teeth in if I wanted to.", "This really is brutal.", "I could take two of you in a fight, right?" } },
            { "nickel", new List<string>(){ "Sorrow be upon thee!", "You're a charlatan!", "Enough of this!", "Perish at my hands!" } },
            { "spapi", new List<string>(){ "Google Witches" } },
            { "turtle", new List<string>(){ "Whatcha doing??!" } },
            { "qaday", new List<string>(){ "I didn't choose to have this, but it's kinda necessary when this happens ._.", "It's okay, let it all out.", "There's probably better target practice out there not gonna lie.", "Yeah, that's fair." } },
            { "skilotar", new List<string>(){ "GUARRRDSS!!", "Do you fancy yourself a Jester?!", "dude...   uncool." } },
            { "dallan", new List<string>(){ "Parried!", "Nah. I drink bulletproof coffee.", "Do you {wj}like{w} hurting other people?", "What is {wj}wrong{w} with you?", "And to think I'm here to help...", "You're not getting those bullets back you know." } },
            { "littlewasp", new List<string>(){ "Nooo! I'm a nice wasp! I'M NICE!", "Aah! That was close!", "Eek!" } },
            { "bunny", new List<string>(){ "not yet, i have unfinished business.", "you missed.", "maybe next time." } },
            { "an3s", new List<string>(){ "If you no like stop I'm gonna end up giving you mean lickings brah.", "Just give up, this magical ring protects me from your bullets." } },
            { "schro", new List<string>(){ "Did'ja really think id just stand there and take it?", "Y'know, this doesn't seem like the best use of your time.", "Oddly enough, I feel attacked.", "That's rude.", "Ah, so close!", "HAH! YA MISSED!", "I'd say your aim is poison, but poison actually kills people." } },
            { "marcy", new List<string>(){ "Haven't I suffered enough? Look at me!", "This is a hate crime." } },
            { "rat", new List<string>(){ "{wj}SQUEAK{w}" } },
        };
        public static Dictionary<string, string> voice = new Dictionary<string, string>()
        {
            { "pretzel", "male" },
            { "notsoai", "computer" },
            { "accidia", "teen" },
            { "round", "robot" },
            { "nickel", "manly" },
            { "spapi", "truthknower" },
            { "turtle", "goofy" },
            { "qaday", "witch1" },
            { "skilotar", "dice" },
            { "dallan", "manly" },
            { "littlewasp", "bug" },
            { "bunny", "truthknower" },
            { "an3s", "fool" },
            { "schro", "robot" },
            { "marcy", "convict" },
            { "rat", "witch3" },
        };
        public string currentPerson;

        private void Start()
        {
            talkpoint = base.transform.Find("talkpoint");
            List<string> peps = new List<string>(validPeople);
            foreach (MysteriousStranger ms in allStrangers) { peps.Remove(ms.currentPerson); }
            if (peps.Count <= 0) { peps.AddRange(validPeople); }
            currentPerson = BraveUtility.RandomElement(peps);

            SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
            this.m_room = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round));
            this.m_room.RegisterInteractable(this);

            base.GetComponent<NPCShootReactor>().OnShot += OnShot;

            Minimap.Instance.RegisterRoomIcon(m_room, (GameObject)BraveResources.Load("Global Prefabs/Minimap_NPC_Icon", ".prefab"));
            m_room.Entered += PlayerEnteredRoom;

            base.spriteAnimator.Play($"{currentPerson}_idle");

            allStrangers.Add(this);
        }
        private void PlayerEnteredRoom(PlayerController player)
        {
            if (currentPerson != "spapi") base.StartCoroutine(Conversation(BraveUtility.RandomElement(entryDialogue[currentPerson]), player));
        }
        private void OnShot(Projectile proj)
        {
            if (currentPerson != "spapi") base.StartCoroutine(Conversation(BraveUtility.RandomElement(shotAtDialogue[currentPerson]), GameManager.Instance.PrimaryPlayer));
        }
        private void Update()
        {
            PlayerController closestPlayer = GameManager.Instance.GetActivePlayerClosestToPoint(base.transform.position.XY(), false);
            if (closestPlayer != null)
            {
                base.sprite.FlipX = closestPlayer.CenterPosition.x < base.transform.position.x;
            }
        }

        public IEnumerator Conversation(string dialogue, PlayerController speaker, string itemName = "")
        {
            string dialogue2 = dialogue;
            dialogue2 = dialogue2.Replace("NICKNAME", StringTableManager.GetString(StringTableManager.GetTalkingPlayerNick()));
            base.spriteAnimator.PlayForDuration($"{currentPerson}_talk", 3f, $"{currentPerson}_idle", false);
            TextBoxManager.ShowTextBox(talkpoint.position, talkpoint, 3f, dialogue2, voice[currentPerson], false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
            yield break;
        }
        public IEnumerator LongConversation(List<string> dialogue, PlayerController speaker, bool clearAfter = false, string itemName = "")
        {
            int conversationIndex = 0;
            base.spriteAnimator.Play($"{currentPerson}_talk");
            while (conversationIndex <= dialogue.Count - 1)
            {
                string dialogue2 = dialogue[conversationIndex];
                dialogue2 = dialogue2.Replace("NICKNAME", StringTableManager.GetString(StringTableManager.GetTalkingPlayerNick()));
                dialogue2 = dialogue2.Replace("ITEMNAME", itemName);

                TextBoxManager.ClearTextBox(talkpoint);
                TextBoxManager.ShowTextBox(talkpoint.position, talkpoint, -1f, dialogue2, voice[currentPerson], instant: false, showContinueText: true);
                float timer = 0;
                while (!BraveInput.GetInstanceForPlayer(speaker.PlayerIDX).ActiveActions.GetActionFromType(GungeonActions.GungeonActionType.Interact).WasPressed || timer < 0.4f)
                {
                    timer += BraveTime.DeltaTime;
                    yield return null;
                }
                conversationIndex++;
            }
            if (clearAfter) { TextBoxManager.ClearTextBox(talkpoint); }
            base.spriteAnimator.Play($"{currentPerson}_idle");
            yield break;
        }
        public float GetDistanceToPoint(Vector2 point)
        {
            return Vector2.Distance(point, base.transform.position) / 1.5f;
        }

        public void OnEnteredRange(PlayerController interactor)
        {
            SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
            SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white);
        }

        public void OnExitRange(PlayerController interactor)
        {
            SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
            SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
        }

        public void Interact(PlayerController interactor)
        {
            if (!TextBoxManager.HasTextBox(talkpoint))
            {
                base.StartCoroutine(HandleInteract(interactor));
            }
        }
        public IEnumerator HandleInteract(PlayerController interactor)
        {
            if (!GivenItem)
            {
                interactor.SetInputOverride("npcConversation");
                Pixelator.Instance.LerpToLetterbox(0.35f, 0.25f);
                CameraController mainCameraController = GameManager.Instance.MainCameraController;
                mainCameraController.SetManualControl(true, true);
                mainCameraController.OverridePosition = base.transform.position;

                GenericLootTable lootTable = (UnityEngine.Random.value >= 0.5f) ? GameManager.Instance.RewardManager.GunsLootTable : GameManager.Instance.RewardManager.ItemsLootTable;
                GameObject item = null;
                while (item == null)
                {
                    //item = lootTable.SelectByWeightWithoutDuplicatesFullPrereqs(null, true, false);
                    item = GameManager.Instance.CurrentRewardManager.GetItemForPlayer(interactor, lootTable, GameManager.Instance.CurrentRewardManager.GetDaveStyleItemQuality(), new List<GameObject>() { } );
                }

                string itemname = "";
                 if (item != null && item.GetComponent<EncounterTrackable>() != null) { itemname = item.GetComponent<EncounterTrackable>().journalData.GetPrimaryDisplayName(false); }
                else if(item != null && item.GetComponent<PickupObject>() != null) { itemname = item.GetComponent<PickupObject>().DisplayName; }
                else { itemname = "this thing"; }
 

                if (item == null) Debug.LogError("Mysterious Stranger: Tried to give a NULL item prefab!");
                Debug.Log("Stranger TryGive: " + item.name + itemname);


                yield return LongConversation(giveItemDialogue[currentPerson], interactor, true,itemname);

                if (GameStatsManager.Instance.IsRainbowRun)
                {
                    LootEngine.SpawnBowlerNote(GameManager.Instance.RewardManager.BowlerNoteOtherSource, interactor.transform.position + new Vector3(0f, -0.5f, 0f), interactor.CurrentRoom, true);
                }
                else
                {
                LootEngine.TryGivePrefabToPlayer(item, interactor, false);
                }
                GivenItem = true;

                interactor.ClearInputOverride("npcConversation");
                Pixelator.Instance.LerpToLetterbox(1, 0.25f);
                GameManager.Instance.MainCameraController.SetManualControl(false, true);

                if (currentPerson == "marcy")
                {
                    currentPerson = "rat";
                    base.spriteAnimator.Play($"{currentPerson}_idle");
                    //AkSoundEngine.PostEvent("gastervanish", this.gameObject);
                    LootEngine.DoDefaultPurplePoof(base.transform.position);
                }
                if (currentPerson == "spapi")
                {
                    m_room.Entered -= PlayerEnteredRoom;
                    allStrangers.Remove(this);
                    this.m_room.DeregisterInteractable(this);
                    AkSoundEngine.PostEvent("gastervanish", interactor.gameObject);
                    UnityEngine.GameObject.Destroy(this.gameObject);
                    yield break;
                }
            }
            else if (!Bored)
            {
                yield return Conversation(BraveUtility.RandomElement(randomDialogue[currentPerson]), interactor);
                Bored = true;
            }
            else
            {
                yield return Conversation(BraveUtility.RandomElement(boredDialogue[currentPerson]), interactor);
            }
            yield break;
        }

        public bool GivenItem = false;
        public bool Bored = false;

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
            allStrangers.Remove(this);
            base.OnDestroy();
        }
    }
}
