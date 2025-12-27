using Alexandria.ItemAPI;
using Alexandria.Misc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    public static class TarotCards
    {
        public static List<TarotCardController.TarotCardData> fortuneTellerTarotCards;
        public static List<TarotCardController.TarotCardData> stinkyModifiers;

        public static void InitCards()
        {
            fortuneTellerTarotCards = new List<TarotCardController.TarotCardData>()
            {
                new TarotCardController.TarotCardData()
                {
                    name = "The Fool",
                    subtitle = "Infinite Ammo",
                    effectDescription = "Applies infinite ammo to held gun.",
                    tarotCard = TarotCardController.TarotCards.FOOL,
                    spriteName = "tarotcard_fool",
                    flipDialogue = new List<string>()
                    {
                        "The Fool...",
                        "That old slinger. Master and maker.",
                        "All his work, all his magic, and what became of it?",
                        "Yet in the wake of his tragedy, and despite all odds...",
                        "Could he yet have a new beginning?"
                    },
                    tellerFaceAnimation = "fool",
                    OnRegisterWithGun = delegate (Gun g, PlayerController player)
                    {
                        g.InfiniteAmmo = true;
                        g.GainAmmo(g.maxAmmo);
                    },
                    CanBeAppliedToGun = delegate (Gun g, PlayerController player)
                    {
                        return !g.InfiniteAmmo;
                    }
                },
                new TarotCardController.TarotCardData()
                {
                    name = "The Magician",
                    subtitle = "Shadow Bullets",
                    effectDescription = "Applies a chance for shadow bullets to held gun.",
                    tarotCard = TarotCardController.TarotCards.MAGICIAN,
                    spriteName = "tarotcard_magician",
                    flipDialogue = new List<string>()
                    {
                        "The Magician...",
                        "A lurking shadow skulks the Gungeon...",
                        "Slave to unseen masters",
                        "Who could be so desperate as to employ such a pale facsimile?",
                        "I think you know."
                    },
                    tellerFaceAnimation = "magician",
                    OnFiredBullet = delegate (Projectile p, PlayerController player, Gun g)
                    {
                        if (UnityEngine.Random.value <= 0.2f) { player.SpawnShadowBullet(p, true); }
                    },
                    CanBeAppliedToGun = delegate (Gun g, PlayerController player)
                    {
                        return g.DefaultModule.shootStyle != ProjectileModule.ShootStyle.Beam;
                    }
                },
                new TarotCardController.TarotCardData()
                {
                    name = "The High Priestess",
                    subtitle = "",
                    effectDescription = "",
                    tarotCard = TarotCardController.TarotCards.HIGH_PRIESTESS,
                    spriteName = "tarotcard_highpriestess",
                    flipDialogue = new List<string>()
                    {
                        "The High Priestess...",
                        "Last of her ancestral trade, Edwinsdottir toils eternal.",
                        "She is the last to know some of the most ancient secrets of the Gungeon",
                        "And she is older still than the very stones.",
                        "Be thankful she shines grace upon you, interloper..."
                    },
                    tellerFaceAnimation = "highpriestess"
                },
                 new TarotCardController.TarotCardData()
                {
                    name = "The Empress",
                    subtitle = "Triple Shot",
                    effectDescription = "Slight Damage Down. Adds triple shot to current gun.",
                    tarotCard = TarotCardController.TarotCards.EMPRESS,
                    spriteName = "tarotcard_empress",
                    flipDialogue = new List<string>()
                    {
                        "The Empress...",
                        "Kaliber of the Seven Sidearms.",
                        "Unknowable patron of this chamber and the next, who parts the curtain and rifles the void itself...",
                        "Though dreadful she may seem, she is nurturing in equal measure.",
                        "A mother goddess of the kin below."
                    },
                    tellerFaceAnimation = "empress",
                    OnRegisterWithGun = delegate(Gun g, PlayerController p)
                    {
                        g.AddExtraPermanentModules(
                            p: p,
                            cooldownModifier: 1,
                            numModules: 3,
                            threesixtyspread: false
                            );
                    }
                },
                 new TarotCardController.TarotCardData()
                {
                    name = "The Emperor",
                    subtitle = "360 Spread",
                    effectDescription = "Hugely increases the firerate and charge speed of the current gun, but gives 360 degree spread.",
                    tarotCard = TarotCardController.TarotCards.EMPEROR,
                    spriteName = "tarotcard_emperor",
                    flipDialogue = new List<string>()
                    {
                        "The Emperor...",
                        "The Lead Lord, chambered in his throne within the Gungeon's Keep",
                        "What crude outline of society the Gundead keep all flows back to him",
                        "And yet, once he was a shell like any other...",
                        "Can you imagine him like that now, interloper?",
                        "I imagine not."
                    },
                    tellerFaceAnimation = "emperor",
                    CanBeAppliedToGun = delegate (Gun g, PlayerController player)
                    {
                        return g.DefaultModule.shootStyle != ProjectileModule.ShootStyle.Beam;
                    },
                    OnRegisterWithGun = delegate(Gun g, PlayerController p)
                    {
                        g.AddCurrentGunStatModifier(PlayerStats.StatType.ChargeAmountMultiplier, 0.1f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                        g.AddCurrentGunStatModifier(PlayerStats.StatType.AdditionalClipCapacityMultiplier, 2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                        g.AddExtraPermanentModules(
                            p: p,
                            cooldownModifier: 0.1f,
                            numModules: 2,
                            threesixtyspread: true
                            );
                    }
                },
                 new TarotCardController.TarotCardData()
                {
                    name = "The Hierophant",
                    subtitle = "",
                    effectDescription = "",
                    tarotCard = TarotCardController.TarotCards.HEIROPHANT,
                    spriteName = "tarotcard_heirophant",
                    flipDialogue = new List<string>()
                    {
                        "The Hierophant...",
                        "First of the Order, bound and balmed and set to stand eternal as Kaliber's holy vigil.","" +
                        "He was a man once...",
                        "...once..."
                    }
                },
                 new TarotCardController.TarotCardData()
                {
                    name = "The Lovers",
                    subtitle = "",
                    effectDescription = "", //Give all synergies to current gun
                    tarotCard = TarotCardController.TarotCards.LOVERS,
                    spriteName = "tarotcard_lovers",
                    flipDialogue = new List<string>()
                    {
                        "The Lovers...",
                        "A curious sorceress. Her arrival forebode many changes in this accursed fortress...",
                        "Though, in her loving magics she cradles the impossible intangibles...",
                        "Love, peace, and harmony...",
                        "And yet...",
                        "It is not wise to anger a sorceress..."
                    },
                    tellerFaceAnimation = "lovers"
                },
                 new TarotCardController.TarotCardData()
                {
                    name = "The Chariot",
                    subtitle = "Remote Control",
                    effectDescription = "Adds powerful remote control to current gun.",
                    tarotCard = TarotCardController.TarotCards.CHARIOT,
                    spriteName = "tarotcard_chariot",
                    flipDialogue = new List<string>()
                    {
                        "The Chariot...",
                        "The Blue Hound, come offering challenge for those with only the steeliest of resolve...",
                        "His determination and willpower carry him at speed...",
                        "Leaving both friends and enemies far behind him.",
                        "The past catches up with us all, one day...",
                        "...if we don't catch it first."
                    },
                    tellerFaceAnimation = "chariot",
                    OnFiredBullet = delegate (Projectile p, PlayerController player, Gun g)
                    {
                        RemoteBulletsBehaviour remote = p.gameObject.AddComponent<RemoteBulletsBehaviour>();
                        remote.trackingSpeed *= 2;
                    },
                    CanBeAppliedToGun = delegate (Gun g, PlayerController player)
                    {
                        return g.DefaultModule.shootStyle != ProjectileModule.ShootStyle.Beam;
                    }
                },
                 new TarotCardController.TarotCardData()
                {
                    name = "Strength",
                    subtitle = "Damage Up",
                    effectDescription = "+50% damage to held gun.",
                    tarotCard = TarotCardController.TarotCards.STRENGTH,
                    spriteName = "tarotcard_strength",
                    flipDialogue = new List<string>()
                    {
                        "Strength...",
                        "Soldiers of fortune. Men of courage, whose resolve has not wavered despite the passing ages...",
                        "I wonder if they even remember why they are here...",
                        "They are formiddable and compassionate in equal measure.",
                        "There is no greater camaraderie than this."
                    },
                    OnRegisterWithGun = delegate (Gun g, PlayerController player)
                    {
                        g.AddCurrentGunStatModifier(PlayerStats.StatType.Damage, 1.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                    },
                },
                 new TarotCardController.TarotCardData()
                {
                    name = "The Hermit",
                    subtitle = "Chance to Loot",
                    effectDescription = "Chance for slain enemies to drop pickups.",
                    tarotCard = TarotCardController.TarotCards.HERMIT,
                    spriteName = "tarotcard_hermit",
                    flipDialogue = new List<string>()
                    {
                        "The Hermit...",
                        "...Just an old man...",
                        "Guided by nothing, not even a name, he stands alone despite the danger.",
                        "Ask yourself, how has he survived this long?",
                        "...Never underestimate an old man in a place where most men die young..."
                    },
                    tellerFaceAnimation = "hermit",
                    OnHitEnemy = delegate (Projectile bullet, SpeculativeRigidbody enemy, bool fatal)
                    {
                        if (enemy)
                        {
                            List<int> ids = new List<int>()
                            {
                                78, //Ammo
                                600, //Spread Ammo
                                565, //Glass Guon Stone
                                73, //Half Heart
                                120, //Armor
                                224, //Blank
                                67, //Key
                            };
                        if (UnityEngine.Random.value <= 0.05f && fatal)
                            {
                                LootEngine.SpawnItem(PickupObjectDatabase.GetById(BraveUtility.RandomElement(ids)).gameObject, enemy.UnitCenter, Vector2.zero, 1f, false, true, false);
                            }
                        }
                    }
                },
                 new TarotCardController.TarotCardData()
                {
                    name = "The Wheel of Fortune",
                    subtitle = "Stats Randomised",
                    effectDescription = "Randomises Gun Stats",
                    tarotCard = TarotCardController.TarotCards.WHEEL_OF_FORTUNE,
                    spriteName = "tarotcard_wheeloffortune",
                    flipDialogue = new List<string>()
                    {
                        "The Wheel of Fortune...",
                        "That young-old disciple of the Chaos God.",
                        "Such a sad-joyous life to be buffeted by the winds of change",
                        "Is he happy?",
                        "Perhaps even how he feels changes on the roll of a die..."
                    },
                    OnRegisterWithGun = delegate (Gun g, PlayerController p)
                    {
                       g.PermanentlyRandomiseStats(p);
                    }
                },
                 new TarotCardController.TarotCardData()
                {
                    name = "Justice",
                    subtitle = "",
                    effectDescription = "",
                    tarotCard = TarotCardController.TarotCards.JUSTICE,
                    spriteName = "tarotcard_justice",
                    flipDialogue = new List<string>()
                    {
                        "Justice...",
                        "An enigmatic figure, his Order an ancient and silly one",
                        "Devoted to preposterous ideas, such as honour, and truth...",
                        "And yet, his own judgement is impaired by complete reliance on his tools...",
                        "You wouldn't lie to a truth knower, would you?"
                    }
                },
                 new TarotCardController.TarotCardData()
                {
                    name = "The Hanged Man",
                    subtitle = "Gun Destroyed",
                    effectDescription = "Destroy current gun. Grant 50% of its damage to all other guns in inventory.",
                    tarotCard = TarotCardController.TarotCards.HANGED_MAN,
                    spriteName = "tarotcard_hangedman",
                    flipDialogue = new List<string>()
                    {
                        "The Hanged Man...",
                        "That crestfallen reptile, sat for unknown ages at the entrance to this place",
                        "He has been here a very... very long time",
                        "Ages come and go, and all that is left is to punish himself...",
                        "...sedate himself with drink.",
                        "...letting go of everything he has left to the bottle..."
                    },
                    CanBeAppliedToGun = delegate (Gun g, PlayerController p)
                    {
                        return !g.InfiniteAmmo && g.CanActuallyBeDropped(p);
                    },
                },
                 new TarotCardController.TarotCardData()
                {
                    name = "Death",
                    subtitle = "Combined Volley",
                    effectDescription = "Merge current gun with a random weapon.",
                    tarotCard = TarotCardController.TarotCards.DEATH,
                    spriteName = "tarotcard_death",
                    flipDialogue = new List<string>()
                    {
                        "Death...",
                        "The greatest of changes. Life to unlife. Transformation",
                        "Life to death... then life again? Such is the path of Lichdom...",
                        "Perhaps he was a fool to cling to life as he did, but after all, is that not the way of this place?",
                        "...fixation on the past?"
                    },
                    CanBeAppliedToGun = delegate (Gun g, PlayerController p)
                    {
                        return !g.InfiniteAmmo;
                    },
                    OnRegisterWithGun = delegate (Gun g, PlayerController p)
                    {
                        Gun final = null;
                        while (final == null)
                        {
                            Gun candidate = PickupObjectDatabase.GetRandomGun();
                            if (!candidate.InfiniteAmmo){final = candidate; }
                        }
                        DuctTapeItem.DuctTapeGuns(final, g);
                    }
                },
                 new TarotCardController.TarotCardData()
                {
                    name = "Temperance",
                    subtitle = "Exchange",
                    effectDescription = "Destroy current gun. Gain a random gun of higher quality.",
                    tarotCard = TarotCardController.TarotCards.TEMPERANCE,
                    spriteName = "tarotcard_temperance",
                    flipDialogue = new List<string>()
                    {
                        "Temperance...",
                        "Moderation and balance. Taking what you have and remaking it anew",
                        "Those munchers know not the tribulations of man...",
                        "Two into one, indeed."
                    },
                    CanBeAppliedToGun = delegate (Gun g, PlayerController p)
                    {
                        return !g.InfiniteAmmo && g.CanActuallyBeDropped(p);
                    },
                },
                 new TarotCardController.TarotCardData()
                {
                    name = "The Devil",
                    subtitle = "Jammed Bullets",
                    effectDescription = "+1 Curse. Fire more powerful bullets that break damage caps.",
                    tarotCard = TarotCardController.TarotCards.DEVIL,
                    spriteName = "tarotcard_devil",
                    flipDialogue = new List<string>()
                    {
                        "The Devil...",
                        "A vicious guardian from beyond the curtain.",
                        "In the temptation and corruption of power, he is manifest",
                        "He is the darker part of ourselves, and we can never leave him behind.",
                        "...wherever you go, there you are..."
                    },
                    OnRegisterWithGun = delegate (Gun g, PlayerController p)
                    {
                        g.AddPassiveStatModifier(PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);
                        g.AddCurrentGunStatModifier(PlayerStats.StatType.Damage, 1.2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                    },
                    OnFiredBullet = delegate (Projectile p, PlayerController player, Gun g)
                    {
                        p.ignoreDamageCaps = true;
                        if (p.sprite != null)
                        {
                            p.sprite.usesOverrideMaterial = true;
                            p.sprite.renderer.material.SetFloat("_BlackBullet", 1f);
                            p.sprite.renderer.material.SetFloat("_EmissivePower", -40f);
                        }
                    },
                    OnFiredBeam = delegate (BeamController p, PlayerController player, Gun g)
                    {
                        p.projectile.ignoreDamageCaps = true;
                        if (p.projectile.sprite != null)
                        {
                            p.projectile.sprite.usesOverrideMaterial = true;
                            p.projectile.sprite.renderer.material.SetFloat("_BlackBullet", 1f);
                            p.projectile.sprite.renderer.material.SetFloat("_EmissivePower", -40f);
                        }
                    },
                },
                 new TarotCardController.TarotCardData()
                {
                    name = "The Tower",
                    subtitle = "Explosive Shot",
                    effectDescription = "Grant a chance for explosive shot to the current weapon.",
                    tarotCard = TarotCardController.TarotCards.TOWER,
                    spriteName = "tarotcard_tower",
                    flipDialogue = new List<string>()
                    {
                        "The Tower...",
                        "The Gungeon itself, this stygian fortress on the edge of space...",
                        "Made and remade through calamity, it calls to us... to all of us...",
                        "Yet no matter how many times you strike it through the heart...",
                        "...the Gungeon remains..."
                    },
                    OnFiredBullet = delegate (Projectile p, PlayerController player, Gun g)
                    {
                        if (!p.gameObject.GetComponent<ExplosiveModifier>() && UnityEngine.Random.value <= 0.085f)
                        {
                            ExplosiveModifier explosiveModifier = p.gameObject.AddComponent<ExplosiveModifier>();
                            explosiveModifier.doExplosion = true;
                            explosiveModifier.explosionData = towerExplosionData;
                        }
                    },
                    OnFiredBeam = delegate(BeamController b, PlayerController player, Gun g)
                    {
                        if (!b.projectile.gameObject.GetComponent<BeamExplosiveModifier>())
                        {
                            BeamExplosiveModifier booms = b.projectile.gameObject.AddComponent<BeamExplosiveModifier>();
                            booms.canHarmOwner = false;
                            booms.chancePerTick = 0.085f;
                            booms.explosionData = towerExplosionData;
                            booms.ignoreQueues = true;
                            booms.tickDelay = 0.5f;
                        }
                    }


                },
                  new TarotCardController.TarotCardData()
                {
                    name = "The Star",
                    subtitle = "Lead Prayer",
                    effectDescription = "Grants leaden skin invulnerability while reloading the current gun.",
                    tarotCard = TarotCardController.TarotCards.STAR,
                    spriteName = "tarotcard_star",
                    flipDialogue = new List<string>()
                    {
                        "The Star...",
                        "The Cult of the Gundead offers purpose to those who become truly lost down here...",
                        "Another shot, not at the past, but at the present",
                        "Purpose, community, belonging...",
                        "On the outside they are madmen, cavorting with animals.",
                        "But their faith is unshakeable."
                    },
                    CanBeAppliedToGun = delegate (Gun g, PlayerController p)
                    {
                        TarotCardGunModifier cont = g.gameObject.GetComponent<TarotCardGunModifier>();
                        return g.DefaultModule != null && g.DefaultModule.numberOfShotsInClip != -1 && g.reloadTime > 0 && cont.NumOfTarotApplied(TarotCardController.TarotCards.STAR) == 0;
                    },
                    OnGunReloaded = delegate(PlayerController p, Gun g)
                    {
                        if (p.healthHaver.IsVulnerable)
                        {
                            GameManager.Instance.StartCoroutine(HandleLeadShield(p, g.AdjustedReloadTime));
                        }
                    }
                },
                new TarotCardController.TarotCardData()
                {
                    name = "The Moon",
                    subtitle = "Chance for Blank",
                    effectDescription = "Adds a chance to fire Blank Bullets to the current gun.",
                    tarotCard = TarotCardController.TarotCards.MOON,
                    spriteName = "tarotcard_moon",
                    flipDialogue = new List<string>()
                    {
                        "The Moon...",
                        "A moaning ghost, fallen at the first hurdle to a vicious betrayal",
                        "Trapped by his own fear, he teaches others...",
                        "Guides them to a treasure he can never possess.",
                    },
                    OnFiredBullet = delegate (Projectile p, PlayerController player, Gun g)
                    {
                        if (!p.gameObject.GetComponent<BlankProjModifier>() && UnityEngine.Random.value <= g.NormaliseProbabilityAcrossFirerate(0.55f, 0.08f, true, 0.08f))
                        {
                            BlankProjModifier blankModifier = p.gameObject.AddComponent<BlankProjModifier>();
                            blankModifier.blankType = EasyBlankType.MINI;
                        }
                    },
                    OnFiredBeam = delegate(BeamController b, PlayerController player, Gun g)
                    {
                        if (!b.projectile.gameObject.GetComponent<BeamBlankModifier>())
                        {
                            BeamBlankModifier blankBeam = b.projectile.gameObject.AddComponent<BeamBlankModifier>();
                            blankBeam.chancePerTick = 0.5f;
                            blankBeam.overrideBossroomChancePerTick = 0.08f;
                        }
                    }
                },
                new TarotCardController.TarotCardData()
                {
                    name = "The Sun",
                    subtitle = "",
                    effectDescription = "",
                    tarotCard = TarotCardController.TarotCards.SUN,
                    spriteName = "tarotcard_sun",
                    flipDialogue = new List<string>()
                    {
                        "The Sun...",
                        "A rainbow beacon of hope in the grim darkness of this timelost tomb",
                        "Giddy and foolish, to entertain his whimsy is to change fate.",
                        "Surely he could not have always been this way?",
                        "...praise the sun..."
                    }
                },
                new TarotCardController.TarotCardData()
                {
                    name = "Judgement",
                    subtitle = "Random Reload Blast",
                    effectDescription = "Reloading the current gun with an empty magazine fires a blast of random bullets.",
                    tarotCard = TarotCardController.TarotCards.JUDGEMENT,
                    spriteName = "tarotcard_judgement",
                    flipDialogue = new List<string>()
                    {
                        "Judgement...",
                        "What more can we say about this one, with which we are all familiar?",
                        "On the heels of immense tragedy, he reached out and grasped the world in his paw.",
                        "The Gungeon is remade for his ambition, and in turn he was reborn as its king.",
                        "He is at peace with the past"
                    },
                    CanBeAppliedToGun = delegate (Gun g, PlayerController p)
                    {
                        TarotCardGunModifier cont = g.gameObject.GetComponent<TarotCardGunModifier>();
                        bool lacksComp = (cont == null || cont.NumOfTarotApplied(TarotCardController.TarotCards.JUDGEMENT) == 0);
                        return g.DefaultModule != null && g.DefaultModule.numberOfShotsInClip != -1 && lacksComp;
                    },
                    OnRegisterWithGun = delegate (Gun g, PlayerController p)
                    {
                        Projectile final = null;
                        while (final == null)
                        {
                            Gun candidate = PickupObjectDatabase.GetRandomGun();
                            if (candidate.DefaultModule != null && candidate.DefaultModule.projectiles[0] != null){final = candidate.DefaultModule.projectiles[0]; }
                        }
                        TarotCardGunModifier cardMod = g.GetComponent<TarotCardGunModifier>();
                        cardMod.reloadVolleyProjectile = final;
                    }
                },
                new TarotCardController.TarotCardData()
                {
                    name = "The World",
                    subtitle = "Homing Bullets",
                    effectDescription = "Grants homing to current gun.",
                    tarotCard = TarotCardController.TarotCards.WORLD,
                    spriteName = "tarotcard_world",
                    flipDialogue = new List<string>()
                    {
                        "The World...",
                        "Lonesome and lost, that poor little elf wanders.",
                        "He is not the first to attempt the impossible feat of completion, nor will he be the last",
                        "In every turn and twist we meet the challenge",
                        "Every labyrinth is first conquered or succumbed in the mind..."
                    },
                    OnFiredBullet = delegate (Projectile p, PlayerController player, Gun g)
                    {
                        HomingModifier homingModifier = p.gameObject.GetComponent<HomingModifier>();
                        if (homingModifier == null)
                        {
                            homingModifier = p.gameObject.AddComponent<HomingModifier>();
                            homingModifier.HomingRadius = 0f;
                            homingModifier.AngularVelocity = 0f;
                        }
                        homingModifier.HomingRadius += 10f;
                        homingModifier.AngularVelocity += 50f;
                    },
                    OnBeamChanceTick = delegate (BeamController p, PlayerController player, Gun g)
                    {
                        p.ChanceBasedHomingRadius += 10f;
                        p.ChanceBasedHomingAngularVelocity += 50f;
                    },
                },
                new TarotCardController.TarotCardData()
                {
                    name = "Nine of Cups",
                    subtitle = "",
                    effectDescription = "", // 50% chance to skip reload time
                    tarotCard = TarotCardController.TarotCards.NINE_CUPS,
                    spriteName = "tarotcard_9_cups",
                    flipDialogue = new List<string>()
                    {
                        "The Nine of Cups...",
                        "In the tinkers work, he finds his meaning.",
                        "He is at peace in his toil, he is a granter of his own wishes",
                        "He has been here longer than most, and yet most do not even know his name...",
                        "...say hello to Tailor for me."
                    }
                },
                 new TarotCardController.TarotCardData()
                {
                    name = "Four of Pentacles",
                    subtitle = "Money is Power",
                    effectDescription = "Increases the damage of the current gun the more currency is held.",
                    tarotCard = TarotCardController.TarotCards.FOUR_PENTACLES,
                    spriteName = "tarotcard_4_pentacles",
                    flipDialogue = new List<string>()
                    {
                        "The Four of Pentacles...",
                        "The shopkeeper is the staple institution of any dungeon, and the Gungeon is no different",
                        "All money flows in and out of the shopkeep's coffers eventually.",
                        "He is the centerpoint, the anchoring force of this economy of desperation.",
                        "Who can blame him for being a miser?"
                    },
                    OnFiredBullet = delegate (Projectile p, PlayerController player, Gun g)
                    {
                       float coins = player.carriedConsumables.Currency;
                        p.baseData.damage *= (1f +( coins/500f * (3f - coins/500f)));
                        p.AdjustPlayerProjectileTint(ExtendedColours.gildedBulletsGold, 1);
                    },
                    OnFiredBeam = delegate(BeamController b, PlayerController player, Gun g)
                    {
                        float coins = player.carriedConsumables.Currency;
                        b.projectile.baseData.damage *= (1f +( coins/500f * (3f - coins/500f)));
                        b.AdjustPlayerBeamTint(ExtendedColours.gildedBulletsGold, 1);
                    }
                },
                  new TarotCardController.TarotCardData()
                {
                    name = "Six of Pentacles",
                    subtitle = "",
                    effectDescription = "",
                    tarotCard = TarotCardController.TarotCards.SIX_PENTACLES,
                    spriteName = "tarotcard_6_pentacles",
                    flipDialogue = new List<string>()
                    {
                        "The Six of Pentacles...",
                        "That old shell relies on the gift of charity to survive.",
                        "There is no shame in it...",
                        "But with a past such as his, now forgotten...",
                        "Is it any wonder that his former glory stains his beard in tears?"
                    }
                },
                  new TarotCardController.TarotCardData()
                {
                    name = "Ten of Pentacles",
                    subtitle = "More Money, Less Problems",
                    effectDescription = "Enemies slain with the current gun have a chance to drop more money.",
                    tarotCard = TarotCardController.TarotCards.TEN_PENTACLES,
                    spriteName = "tarotcard_10_pentacles",
                    flipDialogue = new List<string>()
                    {
                        "The Ten of Pentacles...",
                        "A family business. A departure from the work of her ancestors, but it is where she thrives.",
                        "Her trading has put her among the wealthiest on this far-flung world, but at the end of the day she is as slave to its magic as anyone else.",
                        "Trapped here since the Gungeon's inception, cursed to be a child forever.",
                        "...when will she be able to grow up?"
                    },
                    OnHitEnemy = delegate (Projectile bullet, SpeculativeRigidbody enemy, bool fatal)
                    {
                        if (enemy)
                        {
                            if (UnityEngine.Random.value <= 0.5f && fatal)
                            {
                                LootEngine.SpawnCurrency(enemy.UnitCenter,Math.Max(0, UnityEngine.Random.Range(-2, 4)), false);
                            }
                        }
                    }
                },
               new TarotCardController.TarotCardData()
                {
                    name = "Queen of Guns",
                    subtitle = "",
                    effectDescription = "",
                    tarotCard = TarotCardController.TarotCards.QUEEN_GUNS,
                    spriteName = "tarotcard_Q_guns",
                    flipDialogue = new List<string>()
                    {
                        "The Queen of Guns...",
                        "A magical lady of mysterious background. Her powers are far-reaching and choose no favourites in her wrath.",
                        "For what purpose would a lady of her standing need such a retinue?"
                    }
                },
                new TarotCardController.TarotCardData()
                {
                    name = "King of Guns",
                    subtitle = "",
                    effectDescription = "",
                    tarotCard = TarotCardController.TarotCards.KING_GUNS,
                    spriteName = "tarotcard_K_guns",
                    flipDialogue = new List<string>()
                    {
                        "The King of Guns...",
                        "A great beast at the heart of the Forge",
                        "Last descendant of Gunrax, first of a new line of fire...",
                        "The last barrier between the pilgrim and the truth, is is regrettable that such a majestic being must die."
                    }
                },
                new TarotCardController.TarotCardData()
                {
                    name = "Knight of Guns",
                    subtitle = "",
                    effectDescription = "",
                    tarotCard = TarotCardController.TarotCards.KNIGHT_GUNS,
                    spriteName = "tarotcard_Kn_guns",
                    flipDialogue = new List<string>()
                    {
                        "The Knight of Guns...",
                        "That wicked betrayer, forced to face threats head-on.",
                        "Driven by his ambition, he thoughtlessly slew his only friend",
                        "Poetic, that his large head prevents him from dodge rolling."
                    }
                }
            };

            towerExplosionData = new ExplosionData()
            {
                effect = (PickupObjectDatabase.GetById(543) as Gun).DefaultModule.projectiles[0].hitEffects.overrideMidairDeathVFX,
                ignoreList = StaticExplosionDatas.explosiveRoundsExplosion.ignoreList,
                ss = StaticExplosionDatas.explosiveRoundsExplosion.ss,
                damageRadius = 2f,
                damageToPlayer = 0f,
                doDamage = true,
                damage = 35,
                doDestroyProjectiles = true,
                doForce = true,
                debrisForce = 80f,
                force = 50f,
                preventPlayerForce = true,
                explosionDelay = 0.1f,
                usesComprehensiveDelay = false,
                doScreenShake = true,
                playDefaultSFX = true,
            };
        }
        public static ExplosionData towerExplosionData;

        private static IEnumerator HandleLeadShield(PlayerController user, float duration)
        {
            bool m_usedOverrideMaterial = user.sprite.usesOverrideMaterial;
            user.sprite.usesOverrideMaterial = true;
            user.SetOverrideShader(ShaderCache.Acquire("Brave/ItemSpecific/MetalSkinShader"));
            SpeculativeRigidbody specRigidbody = user.specRigidbody;
            specRigidbody.OnPreRigidbodyCollision += HandleLeadShieldCollision;
            user.healthHaver.IsVulnerable = false;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += BraveTime.DeltaTime;
                user.healthHaver.IsVulnerable = false;
                yield return null;
            }
            if (user)
            {
                user.healthHaver.IsVulnerable = true;
                user.ClearOverrideShader();
                user.sprite.usesOverrideMaterial = m_usedOverrideMaterial;
                SpeculativeRigidbody specRigidbody2 = user.specRigidbody;
                specRigidbody.OnPreRigidbodyCollision -= HandleLeadShieldCollision;
            }
            if (user)
            {
                AkSoundEngine.PostEvent("Play_OBJ_metalskin_end_01", user.gameObject);
            }
            yield break;
        }
        private static void HandleLeadShieldCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherCollider)
        {
            Projectile component = otherRigidbody.GetComponent<Projectile>();
            if (component != null && !(component.Owner is PlayerController))
            {
                PassiveReflectItem.ReflectBullet(component, true, GameManager.Instance.PrimaryPlayer, 10f, 1f, 1f, 0f);
                PhysicsEngine.SkipCollision = true;
            }
        }
    }
}
