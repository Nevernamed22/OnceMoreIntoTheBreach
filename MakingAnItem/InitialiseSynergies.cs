using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;

namespace NevernamedsItems
{
    class InitialiseSynergies
    {
        public static void DoInitialisation()
        {
            //HANDLE SYNERGIES
            //Keybullet Effigy / Spare Key
            List<string> mandatorySynergyItems = new List<string>() { "nn:keybullet_effigy", "nn:spare_key" };
            CustomSynergies.Add("Spare Kin", mandatorySynergyItems);
            //GUN GREASE SYNERGIES
            List<string> mandatorySynergyItemsWorldOnFire = new List<string>() { "nn:gun_grease" };
            List<string> optionalSynergyItemsWorldOnFire = new List<string>() { "napalm_strike", "big_boy", "hot_lead", "ring_of_fire_resistance", "gungeon_pepper" };
            CustomSynergies.Add("Set The World On Fire", mandatorySynergyItemsWorldOnFire, optionalSynergyItemsWorldOnFire);
            List<string> mandatorySynergyItemsOilSlick = new List<string>() { "nn:gun_grease" };
            List<string> optionalSynergyItemsOilSlick = new List<string>() { "nn:sanctified_oil", "oiled_cylinder" };
            CustomSynergies.Add("Oil Slick", mandatorySynergyItemsOilSlick, optionalSynergyItemsOilSlick);
            //BOMBER JACKET / LIL BOMBER SYNERGY
            List<string> mandatorySynergyItemsBigBomber = new List<string>() { "nn:bomber_jacket", "lil_bomber" };
            CustomSynergies.Add("Big Bomber", mandatorySynergyItemsBigBomber);
            //DRAGUN SCALE / TABLE TECH HEAT SYNERGY
            List<string> mandatorySynergyItemsHotHotTable = new List<string>() { "nn:dragun_scale", "table_tech_heat" };
            CustomSynergies.Add("Hot Hot Table", mandatorySynergyItemsHotHotTable);
            //ELEVATOR BUTTON / TIME FUDDLER'S ROBE
            List<string> mandatorySynergyItemsTurnBackTime = new List<string>() { "nn:elevator_button", "space_friend" };
            CustomSynergies.Add("If We Could Turn Back Time", mandatorySynergyItemsTurnBackTime);
            //G-SWITCH / BRICK BREAKER
            List<string> mandatorySynergyItemsGIsForGain = new List<string>() { "nn:g-switch", "brick_breaker" };
            CustomSynergies.Add("G is for Gain", mandatorySynergyItemsGIsForGain);
            //RUSTY CASING / ORGAN DONOR CARD
            List<string> mandatorySynergyItemsHeartSoWetAndCold = new List<string>() { "nn:organ_donor_card", "nn:rusty_casing" };
            CustomSynergies.Add("Heart so wet and cold...", mandatorySynergyItemsHeartSoWetAndCold);
            //JUNKLLETS / FULL ARMOUR JACKET
            List<string> mandatorySynergyItemsManOrMachine = new List<string>() { "nn:junkllets", "nn:full_armour_jacket" };
            CustomSynergies.Add("Man... or Machine?", mandatorySynergyItemsManOrMachine);
            //GOLDEN ARMOUR / SILVER BULLETS
            List<string> mandatorySynergyItemsZilveredUp = new List<string>() { "nn:golden_armour", "silver_bullets" };
            CustomSynergies.Add("Zilvered Up", mandatorySynergyItemsZilveredUp);
            //GOLDEN ARMOUR / OLD GOLDIE / GOLD JUNK / GOLD AMMOLET / AU GUN / GILDED HYDRA
            List<string> mandatorySynergyItemsGoldReserves = new List<string>() { "nn:golden_armour" };
            List<string> optionalSynergyItemsGoldReserves = new List<string>() { "old_goldie", "gold_junk", "gold_ammolet", "au_gun", "gilded_hydra" };
            CustomSynergies.Add("Gold Reserves", mandatorySynergyItemsGoldReserves, optionalSynergyItemsGoldReserves);
            //BASHFUL SHOT / CROWDED CLIPS
            List<string> mandatorySynergyItemsHighsAndLows = new List<string>() { "nn:bashful_shot", "nn:crowded_clips" };
            CustomSynergies.Add("Highs and Lows", mandatorySynergyItemsHighsAndLows);
            //BULLET BULLETS / EASY RELOAD BULLETS
            List<string> mandatorySynergyItemsEasyPeasy = new List<string>() { "nn:bullet_bullets", "easy_reload_bullets" };
            CustomSynergies.Add("Easy Peasy", mandatorySynergyItemsEasyPeasy);
            //EYE OF THE BEHOLSTER / THE BEHOLSTER
            List<string> mandatorySynergyItemsTHEEYES = new List<string>() { "nn:the_beholster", "eye_of_the_beholster" };
            CustomSynergies.Add("THE EYES", mandatorySynergyItemsTHEEYES);
            //HIVE HOLSTER / BEE HIVE / JAR OF BEES / HONEYCOMB / BUMBULLETS
            List<string> mandatorySynergyItemsHiveMind = new List<string>() { "nn:hive_holster" };
            List<string> optionalSynergyItemsHiveMind = new List<string>() { "bee_hive", "jar_of_bees", "bumbullets", "honeycomb" };
            CustomSynergies.Add("Hive Mind", mandatorySynergyItemsHiveMind, optionalSynergyItemsHiveMind);
            //BLANK BOOTS / BLANK BULLETS
            List<string> mandatorySynergyItemsMinorConfusion = new List<string>() { "nn:blank_boots", "blank_bullets" };
            CustomSynergies.Add("Minor Confusion", mandatorySynergyItemsMinorConfusion);
            //BLANK BOOTS / FULL METAL JACKET / TRUE BLANK / FALSE BLANK / SPARE BLANK / BLANK STARE
            List<string> mandatorySynergyItemsBlankItStatement = new List<string>() { "nn:blank_boots" };
            List<string> optionalSynergyItemsBlankItStatement = new List<string>() { "full_metal_jacket", "nn:true_blank", "nn:false_blank", "nn:spare_blank", "nn:blank_stare" };
            CustomSynergies.Add("Blank-It Statement", mandatorySynergyItemsBlankItStatement, optionalSynergyItemsBlankItStatement);
            //BLANK STARE / SPARE BLANK
            List<string> mandatorySynergyItemsBuy1Get1Free = new List<string>() { "nn:blank_stare", "nn:spare_blank" };
            CustomSynergies.Add("Buy 1 Get 1 Free", mandatorySynergyItemsBuy1Get1Free);
            //ROCKET MAN / ROCKET LAUNCHERS
            List<string> mandatorySynergyItemsHighAsAKite = new List<string>() { "nn:rocket_man" };
            List<string> optionalSynergyItemsHighAsAKite = new List<string>() { "stinger", "rpg", "the_exotic", "com4nd0", "rc_rocket", "yari_launcher", "void_core_cannon", "bullet_bore", "gungeon_ant" };
            CustomSynergies.Add("High as a Kite", mandatorySynergyItemsHighAsAKite, optionalSynergyItemsHighAsAKite);
            //ROCKET MAN / JETPACK
            List<string> mandatorySynergyItemsTimelessFlight = new List<string>() { "nn:rocket_man", "jetpack" };
            CustomSynergies.Add("Timeless Flight", mandatorySynergyItemsTimelessFlight);
            //HEART OF GOLD / ORGAN DONOR CARD
            List<string> mandatorySynergyItemsDoGooder = new List<string>() { "nn:heart_of_gold", "nn:organ_donor_card" };
            CustomSynergies.Add("Do-Gooder", mandatorySynergyItemsDoGooder);
            //CHANCE KIN EFFIGY / BABY GOOD CHANCE KIN
            List<string> mandatorySynergyItemsWorship = new List<string>() { "nn:baby_good_chance_kin", "nn:chance_effigy" };
            CustomSynergies.Add("Worship", mandatorySynergyItemsWorship);
            //BABY GOOD CHANCE KIN / BABY GOOD MIMIC / BABY GOOD SHELLETON
            List<string> mandatorySynergyItemsGoodLads = new List<string>() { "nn:baby_good_chance_kin" };
            List<string> optionalSynergyItemsGoodLads = new List<string>() { "baby_good_mimic", "baby_good_shelleton" };
            CustomSynergies.Add("Good Lads", mandatorySynergyItemsGoodLads, optionalSynergyItemsGoodLads);
            //GILDED LEAD / GILDED BULLETS
            List<string> mandatorySynergyItemsGuildoftheGilded = new List<string>() { "nn:gilded_lead", "gilded_bullets" };
            CustomSynergies.Add("Guild of the Gilded", mandatorySynergyItemsGuildoftheGilded);
            //GILDED LEAD / MEGA DOUSER
            List<string> mandatorySynergyItemsGoldenShower = new List<string>() { "nn:gilded_lead", "mega_douser" };
            CustomSynergies.Add("Golden Shower", mandatorySynergyItemsGoldenShower);
            //CHANCE KIN EFFIGY / KEYBULLET EFFIGY
            List<string> mandatorySynergyItemsLuckOfTheQuickdraw = new List<string>() { "nn:keybullet_effigy", "nn:chance_effigy" };
            CustomSynergies.Add("Luck of the Quickdraw", mandatorySynergyItemsLuckOfTheQuickdraw);
            //MAGICKE CAULDRON / SIXTH CHAMBER / YELLOW CHAMBER / HIGH KALIBER / KALIBER'S EYE
            List<string> mandatorySynergyItemsAMosteAccursedBrew = new List<string>() { "nn:alchemy_crucible" };
            List<string> optionalSynergyItemsAMosteAccursedBrew = new List<string>() { "sixth_chamber", "yellow_chamber", "high_kaliber", "nn:kaliber's_eye" };
            CustomSynergies.Add("A Moste Accursed Brew", mandatorySynergyItemsAMosteAccursedBrew, optionalSynergyItemsAMosteAccursedBrew);
            //MAGICKE CAULDRON / CHAOS AMMOLET / CHAOS BULLETS
            List<string> mandatorySynergyItemsICANDOANYTHING = new List<string>() { "nn:alchemy_crucible" };
            List<string> optionalSynergyItemsICANDOANYTHING = new List<string>() { "chaos_bullets", "chaos_ammolet" };
            CustomSynergies.Add("I CAN DO ANYTHING", mandatorySynergyItemsICANDOANYTHING, optionalSynergyItemsICANDOANYTHING);
            //GUNPOWDER PHEROMONES
            List<string> mandatorySynergyItemsShotgunPheromones = new List<string>() { "nn:gunpowder_pheromones", "nn:shutdown_shells" };
            CustomSynergies.Add("Shotgun Pheromones", mandatorySynergyItemsShotgunPheromones);
            //CLAY SCULPTURE / GUNDROMEDA STRAIN
            List<string> mandatorySynergyItemsLynks = new List<string>() { "nn:clay_sculpture", "gundromeda_strain" };
            CustomSynergies.Add("Lynks", mandatorySynergyItemsLynks);
            //BOMBINOMICON / ICE BOMB
            List<string> mandatorySynergyItemsBlueBomber = new List<string>() { "nn:bombinomicon" };
            List<string> optionalSynergyItemsBlueBomber = new List<string>() { "ice_bomb", "ice_cube", "heart_of_ice" };
            CustomSynergies.Add("Blue Bomber", mandatorySynergyItemsBlueBomber, optionalSynergyItemsBlueBomber);
            //BOMBINOMICON / GRENADE LAUNCHER / LIL BOMBER
            List<string> mandatorySynergyItemsDemoman = new List<string>() { "nn:bombinomicon" };
            List<string> optionalSynergyItemsDemoman = new List<string>() { "grenade_launcher", "lil_bomber" };
            CustomSynergies.Add("A Bomby Nation", mandatorySynergyItemsDemoman, optionalSynergyItemsDemoman);
            //WALKIN AROUND WITH YER HEADS FULL O' EYEBALLS
            List<string> mandatorySynergyItemsDemoscreech = new List<string>() { "nn:bombinomicon", "eyepatch" };
            CustomSynergies.Add("HEADS FULL O' EYEBALLS", mandatorySynergyItemsDemoscreech);
            //SPRINGLOADED CHAMBER / WIND UP GUN
            List<string> mandatorySynergyItemsTinkerToys = new List<string>() { "nn:springloaded_chamber", "wind_up_gun" };
            CustomSynergies.Add("Tinker Toys", mandatorySynergyItemsTinkerToys);
            //SPRINGLOADED CHAMBER / BULLET TIME
            List<string> mandatorySynergyItemsClockpunk = new List<string>() { "nn:springloaded_chamber", "bullet_time" };
            CustomSynergies.Add("Clockpunk", mandatorySynergyItemsClockpunk);
            //TRACER ROUNDS / ORBITAL BULLETS
            List<string> mandatorySynergyItemsRingOfFire = new List<string>() { "nn:tracer_rounds", "orbital_bullets" };
            CustomSynergies.Add("Ring of Fire", mandatorySynergyItemsRingOfFire);
            //TRACER ROUNDS / CAMERA
            List<string> mandatorySynergyItemsHotPics = new List<string>() { "nn:tracer_rounds", "camera" };
            CustomSynergies.Add("Hot Pics", mandatorySynergyItemsHotPics);
            //TRACER ROUNDS / GRACEFUL GOOP
            List<string> mandatorySynergyItemsHotTempered = new List<string>() { "nn:tracer_rounds", "nn:graceful_goop" };
            CustomSynergies.Add("Hot Tempered", mandatorySynergyItemsHotTempered);
            //TRACER ROUNDS / FLARE GUN
            List<string> mandatorySynergyItemsEvenMoreVisible = new List<string>() { "nn:tracer_rounds", "flare_gun" };
            CustomSynergies.Add("Even More Visible!", mandatorySynergyItemsEvenMoreVisible);
            //GRACEFUL GOOP / CAMERA
            List<string> mandatorySynergyItemsBehindTheGoops = new List<string>() { "nn:graceful_goop", "camera" };
            CustomSynergies.Add("Behind the Goops", mandatorySynergyItemsBehindTheGoops);
            //GRACEFUL GOOP / TEAR JERKER
            List<string> mandatorySynergyItemsDoomedDescent = new List<string>() { "nn:graceful_goop", "tear_jerker" };
            CustomSynergies.Add("The Doomed Descent Is Over", mandatorySynergyItemsDoomedDescent);
            //GRACEFUL GOOP / GOLDEN ITEMS
            List<string> mandatorySynergyItemsComedyGold = new List<string>() { "nn:graceful_goop" };
            List<string> optionalSynergyItemsComedyGold = new List<string>() { "au_gun", "gilded_hydra", "nn:golden_armour", "nn:gold_guon_stone", "old_goldie", "gilded_bullets", "gold_junk", "gold_ammolet" };
            CustomSynergies.Add("Comedy Gold", mandatorySynergyItemsComedyGold, optionalSynergyItemsComedyGold);
            //VOODOOLLETS / BADGE
            List<string> mandatorySynergyItemsSoiree = new List<string>() { "nn:voodoollets", "badge" };
            CustomSynergies.Add("Soiree", mandatorySynergyItemsSoiree);
            //VOODOOLLETS / CHARMING ROUNDS
            List<string> mandatorySynergyItemsTheLoveYouCannotShowMe = new List<string>() { "nn:voodoollets", "charming_rounds" };
            CustomSynergies.Add("The Love You Cannot Show Me", mandatorySynergyItemsTheLoveYouCannotShowMe);
            //VOODOOLLETS / SPICE / FACE MELTER / LUTE / GUNZHENG / GHOST BULLETS / GUNSLINGERS ASHES / SPECTRE BULLETS
            List<string> mandatorySynergyItemsInTheMovement = new List<string>() { "nn:voodoollets" };
            List<string> optionalSynergyItemsInTheMovement = new List<string>() { "spice", "face_melter", "really_special_lute", "gunzheng", "ghost_bullets", "gunslingers_ashes", "nn:spectre_bullets" };
            CustomSynergies.Add("In The Movement...", mandatorySynergyItemsInTheMovement, optionalSynergyItemsInTheMovement);
            //VOODOOLLETS / DECOY
            List<string> mandatorySynergyItemsElysianNight = new List<string>() { "nn:voodoollets", "decoy" };
            CustomSynergies.Add("Elysian Night...", mandatorySynergyItemsElysianNight);
            //FROST KEY / SNOWBALLER
            List<string> mandatorySynergyItemsFrostKeyTheSnowman = new List<string>() { "nn:frost_key", "snowballer" };
            CustomSynergies.Add("Frost-Key The Snowman", mandatorySynergyItemsFrostKeyTheSnowman);
            //FROST KEY / DRAGUNFIRE / RING OF FIRE RESISTANCE / STAFF OF FIREPOWER / HIGH DRAGUNFIRE
            List<string> mandatorySynergyItemsFrostAndGunfire = new List<string>() { "nn:frost_key" };
            List<string> optionalSynergyItemsFrostAndGunfire = new List<string>() { "high_dragunfire", "dragunfire", "ring_of_fire_resistance", "staff_of_firepower" };
            CustomSynergies.Add("Frost and Gunfire", mandatorySynergyItemsFrostAndGunfire, optionalSynergyItemsFrostAndGunfire);
            //SUPERSONIC SHOTS / FACE MELTER
            List<string> mandatorySynergyItemsUnderground = new List<string>() { "nn:supersonic_shots", "face_melter" };
            CustomSynergies.Add("Underground", mandatorySynergyItemsUnderground);
            //ACCELERANT / FLAME HAND / PITCHFORK / PHOENIX
            List<string> mandatorySynergyItemsBurnBabyBurn = new List<string>() { "nn:accelerant" };
            List<string> optionalSynergyItemsBurnBabyBurn = new List<string>() { "pitchfork", "flame_hand", "phoenix", };
            CustomSynergies.Add("Burn, Baby Burn!", mandatorySynergyItemsBurnBabyBurn, optionalSynergyItemsBurnBabyBurn);
            //MR. FAHRENHEIT / MR ACCRETION JR
            List<string> mandatorySynergyItemsMercury = new List<string>() { "mr_accretion_jr" };
            List<string> optionalSynergyItemsMercury = new List<string>() { "nn:mr._fahrenheit", };
            CustomSynergies.Add("Mercury", mandatorySynergyItemsMercury, optionalSynergyItemsMercury);
            //MR. FAHRENHEIT / SUPERSONIC SHOTS
            List<string> mandatorySynergyItemsSupersonicMan = new List<string>() { "nn:supersonic_shots", "nn:mr._fahrenheit" };
            CustomSynergies.Add("Supersonic Man", mandatorySynergyItemsSupersonicMan);
            //SILVER AMMOLET / SILVER BULLETS
            List<string> mandatorySynergyItemsBlessedAreTheShriek = new List<string>() { "nn:silver_ammolet", "silver_bullets" };
            CustomSynergies.Add("Blessed are The Shriek", mandatorySynergyItemsBlessedAreTheShriek);
            //GUNSHARK / FAT BULLETS / TITAN BULLETS
            List<string> mandatorySynergyItemsMegashark = new List<string>() { "nn:gunshark" };
            List<string> optionalSynergyItemsMegashark = new List<string>() { "fat_bullets", "nn:titan_bullets", "megahand" };
            CustomSynergies.Add("Megashark", mandatorySynergyItemsMegashark, optionalSynergyItemsMegashark);
            //GUNSHARK / COMPRESSED AIR TANK
            List<string> mandatorySynergyItemsBloodInTheWater = new List<string>() { "nn:gunshark", "compressed_air_tank" };
            CustomSynergies.Add("Blood In The Water", mandatorySynergyItemsBloodInTheWater);
            //LOVE PISTOL / LOVE ITEMS
            List<string> mandatorySynergyItemsEverlastingLove = new List<string>() { "nn:love_pistol" };
            List<string> optionalSynergyItemsEverlastingLove = new List<string>() { "charming_rounds", "shotgun_full_of_love", "charm_horn", "charmed_bow" };
            CustomSynergies.Add("Everlasting Love", mandatorySynergyItemsEverlastingLove, optionalSynergyItemsEverlastingLove);
            //LOVE PISTOL / TOXIC ITEMS
            List<string> mandatorySynergyItemsToxicLove = new List<string>() { "nn:love_pistol" };
            List<string> optionalSynergyItemsToxicLove = new List<string>() { "trashcannon", "shotgun_full_of_hate" };
            CustomSynergies.Add("Toxic Love", mandatorySynergyItemsToxicLove, optionalSynergyItemsToxicLove);
            //SUNLIGHT JAVELIN / GUN GREASE
            List<string> mandatorySynergyItemsGreaseLightning = new List<string>() { "nn:gun_grease", "sunlight_javelin" };
            CustomSynergies.Add("Grease Lightning", mandatorySynergyItemsGreaseLightning);
            //SUNLIGHT JAVELIN / MAMA
            List<string> mandatorySynergyItemsGunderboltsAndLightning = new List<string>() { "nn:mama", "sunlight_javelin" };
            CustomSynergies.Add("Gunderbolts and Lightning", mandatorySynergyItemsGunderboltsAndLightning);
            //DISC GUN / CIGARETTES / BOMBER JACKET / DOUBLE VISION
            List<string> mandatorySynergyItemsEvenWorseChoices = new List<string>() { "nn:disc_gun" };
            List<string> optionalSynergyItemsEvenWorseChoices = new List<string>() { "cigarettes", "nn:bomber_jacket", "double_vision" };
            CustomSynergies.Add("Even Worse Choices", mandatorySynergyItemsEvenWorseChoices, optionalSynergyItemsEvenWorseChoices);
            //JUSTICE / MOUSTACHE
            List<string> mandatorySynergyItemsShotkeeper = new List<string>() { "nn:justice", "mustache" };
            CustomSynergies.Add("Shotkeeper", mandatorySynergyItemsShotkeeper);
            //FINGER GUNS / LICHY TRIGGER FINGER / +1 BULLETS
            List<string> mandatorySynergyItemsPolydactyly = new List<string>() { "nn:finger_guns" };
            List<string> optionalSynergyItemsPolydactyly = new List<string>() { "+1_bullets", "lichy_trigger_finger" };
            CustomSynergies.Add("Polydactyly", mandatorySynergyItemsPolydactyly, optionalSynergyItemsPolydactyly);
            //DISC GUN / GONNE
            List<string> mandatorySynergyItemsDiscworld = new List<string>() { "nn:disc_gun", "nn:gonne" };
            CustomSynergies.Add("Discworld", mandatorySynergyItemsDiscworld);
            //DISC GUN SUPER DISC FORME
            List<string> mandatorySynergyItemsSuperDisc = new List<string>() { "nn:disc_gun" };
            List<string> optionalSynergyItemsSuperDisc = new List<string>() { "+1_bullets", "buzzkill" };
            CustomSynergies.Add("Super Disc", mandatorySynergyItemsSuperDisc, optionalSynergyItemsSuperDisc);
            //ORGUN / HEART ITEMS
            List<string> mandatorySynergyItemsHeartAttack = new List<string>() { "nn:orgun" };
            List<string> optionalSynergyItemsHeartAttack = new List<string>() { "heart_holster", "heart_purse", "heart_bottle", "heart_lunchbox", "heart_locket", "heart_synthesizer", "heart_of_ice", "nn:cheese_heart", "nn:forsaken_heart", "nn:heart_of_gold", "nn:heart_padlock" };
            CustomSynergies.Add("Heart Attack", mandatorySynergyItemsHeartAttack, optionalSynergyItemsHeartAttack);
            //ORGUN / HELMET ITEMS - HEADACHE
            List<string> mandatorySynergyItemsHeadache = new List<string>() { "nn:orgun" };
            List<string> optionalSynergyItemsHeadache = new List<string>() { "gunknight_helmet", "blast_helmet", "crown_of_guns", "stone_dome", "clown_mask", "old_knights_helm", "nn:horned_helmet" };
            CustomSynergies.Add("Headache", mandatorySynergyItemsHeadache, optionalSynergyItemsHeadache);
            //OCTAGUN / MUSICAL WEAPONS
            List<string> mandatorySynergyItemsShapesNBeats = new List<string>() { "nn:octagun" };
            List<string> optionalSynergyItemsShapesNBeats = new List<string>() { "gunzheng", "really_special_lute", "face_melter", "metronome" };
            CustomSynergies.Add("Shapes N' Beats", mandatorySynergyItemsShapesNBeats, optionalSynergyItemsShapesNBeats);
            //MAGIC MISSILE / MAGIC ITEMS
            List<string> mandatorySynergyItemsMagicerMissile = new List<string>() { "nn:magic_missile" };
            List<string> optionalSynergyItemsMagicerMissile = new List<string>() { "nn:magic_quiver", "magic_bullets", "witch_pistol", "bundle_of_wands" };
            CustomSynergies.Add("Magic-er Missile", mandatorySynergyItemsMagicerMissile, optionalSynergyItemsMagicerMissile);
            //GREY GUON STONE
            List<string> mandatorySynergyItemsGreyerGuonStone = new List<string>() { "nn:grey_guon_stone" };
            List<string> optionalSynergyItemsGreyerGuonStone = new List<string>() { "+1_bullets", "amulet_of_the_pit_lord", "bullet_idol" };
            CustomSynergies.Add("Greyer Guon Stone", mandatorySynergyItemsGreyerGuonStone, optionalSynergyItemsGreyerGuonStone);
            //GOLF RIFLE / PURPLER
            List<string> mandatorySynergyItemsBirdie = new List<string>() { "nn:golf_rifle", "nn:purpler" };
            CustomSynergies.Add("Birdie!", mandatorySynergyItemsBirdie);
            //PURPLER / PURPLE PROSE
            List<string> mandatorySynergyItemsPurplest = new List<string>() { "nn:purple_prose", "nn:purpler" };
            CustomSynergies.Add("Purplest", mandatorySynergyItemsPurplest);
            //PURPLE PROSE / REALLY SPECIAL LUTE
            List<string> mandatorySynergyItemsEternalProse = new List<string>() { "nn:purple_prose", "really_special_lute" };
            CustomSynergies.Add("Eternal Prose", mandatorySynergyItemsEternalProse);
            //MINI GUN / SHOTGUNS
            List<string> mandatorySynergyItemsMiniShotgun = new List<string>() { "nn:mini_gun" };
            List<string> optionalSynergyItemsMiniShotgun = new List<string>() { "regular_shotgun", "old_goldie", "huntsman" };
            CustomSynergies.Add("Mini Shotgun", mandatorySynergyItemsMiniShotgun, optionalSynergyItemsMiniShotgun);
            //HAND MORTAR / HAND CANNON
            List<string> mandatorySynergyItemsGoodOldGuns = new List<string>() { "nn:hand_cannon", "nn:hand_mortar" };
            CustomSynergies.Add("Good Old Guns", mandatorySynergyItemsGoodOldGuns);
            //HAND MORTAR / OLD GOLDIE
            List<string> mandatorySynergyItemsTheClassics = new List<string>() { "nn:hand_mortar", "old_goldie" };
            CustomSynergies.Add("The Classics", mandatorySynergyItemsTheClassics);
            //SPEAR OF JUSTICE / CLONE / PIG / GUN SOUL
            List<string> mandatorySynergyItemsUndying = new List<string>() { "nn:spear_of_justice" };
            List<string> optionalSynergyItemsUndying = new List<string>() { "clone", "gun_soul", "pig" };
            CustomSynergies.Add("Undying", mandatorySynergyItemsUndying, optionalSynergyItemsUndying);
            //SPEAR OF JUSTICE / HEAVY BOOTS / SHIELD OF THE MAIDEN
            List<string> mandatorySynergyItemsNoRunningAway = new List<string>() { "nn:spear_of_justice" };
            List<string> optionalSynergyItemsNoRunningAway = new List<string>() { "heavy_boots", "shield_of_the_maiden" };
            CustomSynergies.Add("No Running Away!", mandatorySynergyItemsNoRunningAway, optionalSynergyItemsNoRunningAway);
            //STUN GUN / TRANK GUN
            List<string> mandatorySynergyItemsNonLethalSolutions = new List<string>() { "nn:stun_gun", "trank_gun" };
            CustomSynergies.Add("Non Lethal Solutions", mandatorySynergyItemsNonLethalSolutions);
            //DIAMOND GUN - BANE OF ARTHROPODS
            List<string> mandatorySynergyItemsBaneOfArthropods = new List<string>() { "nn:diamond_gun" };
            List<string> optionalSynergyItemsBaneOfArthropods = new List<string>() { "shotgrub", "grappling_hook", "chaos_bullets", "bumbullets", "bee_hive", "jar_of_bees", "nn:hive_holster", "honeycomb" };
            CustomSynergies.Add("Bane of Arthropods", mandatorySynergyItemsBaneOfArthropods, optionalSynergyItemsBaneOfArthropods);
            //DIAMOND GUN - SMITE
            List<string> mandatorySynergyItemsSmite = new List<string>() { "nn:diamond_gun" };
            List<string> optionalSynergyItemsSmite = new List<string>() { "ghost_bullets", "zombie_bullets", "skull_spitter", "vertebraek47" };
            CustomSynergies.Add("Smite", mandatorySynergyItemsSmite, optionalSynergyItemsSmite);
            //DIAMOND GUN - FIRE ASPECT
            List<string> mandatorySynergyItemsFireAspect = new List<string>() { "nn:diamond_gun" };
            List<string> optionalSynergyItemsFireAspect = new List<string>() { "hot_lead", "nn:tracer_rounds", "copper_ammolet", "phoenix", "nn:mr._fahrenheit" };
            CustomSynergies.Add("Fire Aspect", mandatorySynergyItemsFireAspect, optionalSynergyItemsFireAspect);
            //DIAMOND GUN - KNOCKBACK
            List<string> mandatorySynergyItemsKnockback = new List<string>() { "nn:diamond_gun" };
            List<string> optionalSynergyItemsKnockback = new List<string>() { "casey", "nn:diamond_bracelet", "boxing_glove", "ruby_bracelet", "nn:pearl_bracelet" };
            CustomSynergies.Add("Knockback", mandatorySynergyItemsKnockback, optionalSynergyItemsKnockback);
            //DIAMOND GUN - SHARPNESS
            List<string> mandatorySynergyItemsSharpness = new List<string>() { "nn:diamond_gun" };
            List<string> optionalSynergyItemsSharpness = new List<string>() { "knife_shield", "katana_bullets", "vorpal_bullets", "excaliber" };
            CustomSynergies.Add("Sharpness", mandatorySynergyItemsSharpness, optionalSynergyItemsSharpness);
            //COPPER AMMOLET / LANTAKA - BROTHERS IN COPPER
            List<string> mandatorySynergyItemsBrothersInCopper = new List<string>() { "copper_ammolet" };
            List<string> optionalSynergyItemsBrothersInCopper = new List<string>() { "nn:lantaka" };
            CustomSynergies.Add("Brothers in Copper", mandatorySynergyItemsBrothersInCopper, optionalSynergyItemsBrothersInCopper);
            //BLOWGUN / POISON DART FROG
            List<string> mandatorySynergyItemsDartistry = new List<string>() { "nn:blowgun", "nn:poison_dart_frog" };
            CustomSynergies.Add("Dartistry", mandatorySynergyItemsDartistry);
            //BLOWGUN / DART RIFLE
            List<string> mandatorySynergyItemsOldAndNew = new List<string>() { "nn:blowgun", "nn:dart_rifle" };
            CustomSynergies.Add("Old and New", mandatorySynergyItemsOldAndNew);
            //KALASHNIRANG / BOOMERANG
            List<string> mandatorySynergyItemsRangaround = new List<string>() { "boomerang", "nn:kalashnirang" };
            CustomSynergies.Add("Rangaround", mandatorySynergyItemsRangaround);
            //BLASMASTER / GRAPPLING HOOK / BULLET TIME
            List<string> mandatorySynergyItemsPiratesLife = new List<string>() { "nn:blasmaster" };
            List<string> optionalSynergyItemsPiratesLife = new List<string>() { "grappling_hook", "bullet_time" };
            CustomSynergies.Add("Pirate's Life", mandatorySynergyItemsPiratesLife, optionalSynergyItemsPiratesLife);
            //DOGGUN / WOLF
            List<string> mandatorySynergyItemsDiscordAndRhyme = new List<string>() { "nn:doggun", "wolf" };
            CustomSynergies.Add("Discord and Rhyme", mandatorySynergyItemsDiscordAndRhyme);
            //DEMOLITIONIST / BOMB ITEMS
            List<string> mandatorySynergyItemsDemolitionMan = new List<string>() { "nn:demolitionist" };
            List<string> optionalSynergyItemsDemolitionMan = new List<string>() { "proximity_mine", "cluster_mine", "bomb", "nn:bombinomicon", "nn:bomber_jacket", "c4" };
            CustomSynergies.Add("Demolition Man", mandatorySynergyItemsDemolitionMan, optionalSynergyItemsDemolitionMan);
            //VISCERIFLE / OLD CREST
            List<string> mandatorySynergyItemsElderShot = new List<string>() { "nn:viscerifle", "old_crest" };
            CustomSynergies.Add("Elder Shot", mandatorySynergyItemsElderShot);
            //VISCERIFLE / MEAT SHIELD
            List<string> mandatorySynergyItemsBackMuscle = new List<string>() { "nn:viscerifle", "nn:meat_shield" };
            CustomSynergies.Add("Back Muscle", mandatorySynergyItemsBackMuscle);
            //VISCERIFLE / GREEN GUON STONE
            List<string> mandatorySynergyItemsDangerIsMyMiddleName = new List<string>() { "nn:viscerifle", "green_guon_stone" };
            CustomSynergies.Add("Danger Is My Middle Name", mandatorySynergyItemsDangerIsMyMiddleName);
            /*//BLANKANNON / GLASS AMMOLET
            List<string> mandatorySynergyItemsPanedExpression = new List<string>() { "nn:blankannon", "nn:glass_ammolet" };
            CustomSynergies.Add("Paned Expression", mandatorySynergyItemsPanedExpression);
            //BLANKANNON / ELDER BLANK
            List<string> mandatorySynergyItemsSecretsOfTheAncients = new List<string>() { "nn:blankannon", "elder_blank" };
            CustomSynergies.Add("Secrets of the Ancients", mandatorySynergyItemsSecretsOfTheAncients);*/
            //CLOWN SHOTGUN / CLOWN MASK
            List<string> mandatorySynergyItemsClownTown = new List<string>() { "nn:clown_shotgun" };
            List<string> optionalSynergyItemsClownTown = new List<string>() { "clown_mask" };
            CustomSynergies.Add("Clown Town", mandatorySynergyItemsClownTown, optionalSynergyItemsClownTown);
            //LOVE POTION / VOODOOLLETS
            List<string> mandatorySynergyItemsOohEeeOohAhAh = new List<string>() { "nn:love_potion" };
            List<string> optionalSynergyItemsOohEeeOohAhAh = new List<string>() { "nn:voodoollets" };
            CustomSynergies.Add("Ooh Eee Ooh Ah Ah!", mandatorySynergyItemsOohEeeOohAhAh, optionalSynergyItemsOohEeeOohAhAh);
            //LOVE POTION / CHARM ITEMS
            List<string> mandatorySynergyItemsNumber9 = new List<string>() { "nn:love_potion" };
            List<string> optionalSynergyItemsNumber9 = new List<string>() { "charm_horn", "charming_rounds", "charmed_bow", "nn:purple_prose", "nn:love_pistol" };
            CustomSynergies.Add("Number 9", mandatorySynergyItemsNumber9, optionalSynergyItemsNumber9);
            //PISTA / BOW
            List<string> mandatorySynergyItemsPistolsRequiem = new List<string>() { "nn:pista", "bow" };
            CustomSynergies.Add("Pistols Requiem", mandatorySynergyItemsPistolsRequiem);
            //RISK RIFLE / MEDKIT / RATION / ORANGE
            List<string> mandatorySynergyItemsZeroRisk = new List<string>() { "nn:risk_rifle" };
            List<string> optionalSynergyItemsZeroRisk = new List<string>() { "medkit", "ration", "orange" };
            CustomSynergies.Add("Zero Risk", mandatorySynergyItemsZeroRisk, optionalSynergyItemsZeroRisk);
            //ARMOURED ARMOUR / ARMOR SYNTHESIZER / MEAT SHIELD / GOLDEN ARMOUR / GUNKNIGHT ARMOUR / POWER ARMOUR / ARMOR OF THORNS
            List<string> mandatorySynergyItemsArmouredArmouredArmour = new List<string>() { "nn:armoured_armour" };
            List<string> optionalSynergyItemsArmouredArmouredArmour = new List<string>() { "armor_synthesizer", "nn:meat_shield", "nn:golden_armour", "gunknight_armor", "nn:power_armour", "armor_of_thorns" };
            CustomSynergies.Add("Armoured Armoured Armour", mandatorySynergyItemsArmouredArmouredArmour, optionalSynergyItemsArmouredArmouredArmour);
            //MULTIPLICATOR / TABLE TECHS
            List<string> mandatorySynergyItemsTimesTables = new List<string>() { "nn:multiplicator" };
            List<string> optionalSynergyItemsTimesTables = new List<string>() { "portable_table_device", "table_tech_sight", "table_tech_money", "table_tech_rocket", "table_tech_shotgun", "table_tech_heat", "table_tech_rage", "table_tech_blanks", "table_tech_stun", "nn:table_tech_table", "nn:table_tech_invulnerability", "nn:table_tech_speed", "nn:table_tech_ammo", "nn:table_tech_guon", "nn:tabullets" };
            CustomSynergies.Add("Times Tables", mandatorySynergyItemsTimesTables, optionalSynergyItemsTimesTables);
            //PENCIL / MIGHTIER THAN THE GUN
            List<string> mandatorySynergyItemsMightierThanTheGun = new List<string>() { "nn:pencil" };
            List<string> optionalSynergyItemsMightierThanTheGun = new List<string>() { "nn:gun_grease", "nn:paintball_gun", "ballot" };
            CustomSynergies.Add("Mightier Than The Gun", mandatorySynergyItemsMightierThanTheGun, optionalSynergyItemsMightierThanTheGun);
            //SHOWDOWN / CHARM ITEMS
            List<string> mandatorySynergyItemsFrenemies = new List<string>() { "nn:showdown" };
            List<string> optionalSynergyItemsFrenemies = new List<string>() { "charming_rounds", "charm_horn", "charmed_bow", "shotgun_full_of_love", "nn:love_pistol", "nn:purple_prose", "nn:love_potion" };
            CustomSynergies.Add("Frenemies", mandatorySynergyItemsFrenemies, optionalSynergyItemsFrenemies);
            //SHOWDOWN / SPITEFUL ITEMS
            List<string> mandatorySynergyItemsDirtyTricks = new List<string>() { "nn:showdown" };
            List<string> optionalSynergyItemsDirtyTricks = new List<string>() { "shotgun_full_of_hate", "poison_vial", "nn:mistake_bullets" };
            CustomSynergies.Add("Dirty Tricks", mandatorySynergyItemsDirtyTricks, optionalSynergyItemsDirtyTricks);
            //VACUUM GUN / TRANSMOG ITEMS
            List<string> mandatorySynergyItemsChickadoo = new List<string>() { "nn:vacuum_gun" };
            List<string> optionalSynergyItemsChickadoo = new List<string>() { "magic_bullets", "bundle_of_wands", "witch_pistol", "hexagun" };
            CustomSynergies.Add("Chickadoo", mandatorySynergyItemsChickadoo, optionalSynergyItemsChickadoo);
            //BOOKLLET / ANTIMAGIC ROUNDS / MAGIC BULLETS / +1 BULLETS / ALCHEMY CRUCIBLE
            List<string> mandatorySynergyItemsAdvancedAmmomancy = new List<string>() { "nn:bookllet" };
            List<string> optionalSynergyItemsAdvancedAmmomancy = new List<string>() { "magic_bullets", "nn:antimagic_rounds", "+1_bullets", "nn:alchemy_crucible" };
            CustomSynergies.Add("Advanced Ammomancy", mandatorySynergyItemsAdvancedAmmomancy, optionalSynergyItemsAdvancedAmmomancy);
            //REKEYTER / SHELLETON KEY
            List<string> mandatorySynergyItemsReShelletonKeyter = new List<string>() { "nn:rekeyter", "shelleton_key" };
            CustomSynergies.Add("ReShelletonKeyter", mandatorySynergyItemsReShelletonKeyter);
            //AM0 / Spread Items
            List<string> mandatorySynergyItemsSpreadshot = new List<string>() { "nn:am0" };
            List<string> optionalSynergyItemsSpreadshot = new List<string>() { "unity", "flak_bullets", "eyepatch", "sense_of_direction", "nn:clown_shotgun" };
            CustomSynergies.Add("Spreadshot", mandatorySynergyItemsSpreadshot, optionalSynergyItemsSpreadshot);
            //AM0 / MENGER AMMO BOX
            List<string> mandatorySynergyItemsMengerClip = new List<string>() { "nn:am0", "nn:menger_ammo_box" };
            CustomSynergies.Add("Menger Clip", mandatorySynergyItemsMengerClip);
            //BULLET BLADE / GHOST ITEMS
            List<string> mandatorySynergyItemsGhostSword = new List<string>() { "nn:bullet_blade" };
            List<string> optionalSynergyItemsGhostSword = new List<string>() { "ghost_bullets", "nn:spectre_bullets", "zombie_bullets", "nn:bombinomicon", "gunslingers_ashes" };
            CustomSynergies.Add("GHOST SWORD!!!", mandatorySynergyItemsGhostSword, optionalSynergyItemsGhostSword);
            //CARDINALS  MITRE / BOOTS
            List<string> mandatorySynergyItemsHolySocks = new List<string>() { "nn:cardinals_mitre" };
            List<string> optionalSynergyItemsHolySocks = new List<string>() { "ballistic_boots", "heavy_boots", "gunboots", "springheel_boots", "rat_boots", "nn:cyclone_boots", "nn:legboot", "nn:blank_boots" };
            CustomSynergies.Add("Holy Socks", mandatorySynergyItemsHolySocks, optionalSynergyItemsHolySocks);
            //DISPLACER CANNON  / DELETE THIS
            List<string> mandatorySynergyItemsDeleteThis = new List<string>() { "nn:displacer_cannon" };
            List<string> optionalSynergyItemsDeleteThis = new List<string>() { "chest_teleporter", "teleporter_prototype", "railgun", "prototype_railgun" };
            CustomSynergies.Add("Delete This", mandatorySynergyItemsDeleteThis, optionalSynergyItemsDeleteThis);
            //DISPLACER CANNON / LOOT VORTEX
            List<string> mandatorySynergyItemsLootVortex = new List<string>() { "nn:displacer_cannon" };
            List<string> optionalSynergyItemsLootVortex = new List<string>() { "nn:menger_ammo_box", "nn:bloody_ammo", "nn:spare_blank", "nn:spare_key", "nn:rusty_casing", "nn:loose_change" };
            CustomSynergies.Add("Loot Vortex", mandatorySynergyItemsLootVortex, optionalSynergyItemsLootVortex);
            //DESK FAN / BALLOON GUN
            List<string> mandatorySynergyItemsHighSetting = new List<string>() { "nn:desk_fan" };
            List<string> optionalSynergyItemsHighSetting = new List<string>() { "balloon_gun" };
            CustomSynergies.Add("Fresh Air", mandatorySynergyItemsHighSetting, optionalSynergyItemsHighSetting);
            //FALSE BLANK / GLASS AMMOLET
            List<string> mandatorySynergyItemsTransparentLies = new List<string>() { "nn:false_blank", "nn:glass_ammolet" };
            CustomSynergies.Add("Transparent Lies", mandatorySynergyItemsTransparentLies);
            //CHEM GRENADE / TOXIC ITEMS
            List<string> mandatorySynergyItemsToxicShock = new List<string>() { "nn:chem_grenade" };
            List<string> optionalSynergyItemsToxicShock = new List<string>() { "poison_vial", "irradiated_lead", "gamma_ray", "nn:graceful_goop", "gas_mask", "hazmat_suit", "monster_blood" };
            CustomSynergies.Add("Toxic Shock", mandatorySynergyItemsToxicShock, optionalSynergyItemsToxicShock);
            //BLOMBK / BLANK ITEMS
            List<string> mandatorySynergyItemsAtomicBlombk = new List<string>() { "nn:blombk" };
            List<string> optionalSynergyItemsAtomicBlombk = new List<string>() { "elder_blank", "nn:true_blank", "nn:false_blank", "blank_bullets", "full_metal_jacket", "nn:blank_boots", "big_boy" };
            CustomSynergies.Add("Atomic Blombk", mandatorySynergyItemsAtomicBlombk, optionalSynergyItemsAtomicBlombk);
            //TOOLBOX / RELIGIOUS ITEMS
            List<string> mandatorySynergyItemsHisGrace = new List<string>() { "nn:toolbox" };
            List<string> optionalSynergyItemsHisGrace = new List<string>() { "nn:cardinals_mitre", "holey_grail","blood_brooch", "yellow_chamber" };
            CustomSynergies.Add("His Grace", mandatorySynergyItemsHisGrace, optionalSynergyItemsHisGrace);
            //TOOLBOX / TOOL ITEMS
            List<string> mandatorySynergyItemsSharpestToolInTheShed = new List<string>() { "nn:toolbox" };
            List<string> optionalSynergyItemsSharpestToolInTheShed = new List<string>() { "drill", "h4mmer", "cobalt_hammer", "anvillain", "wood_beam", "nn:pencil" };
            CustomSynergies.Add("Sharpest Tool In The Shed", mandatorySynergyItemsSharpestToolInTheShed, optionalSynergyItemsSharpestToolInTheShed);
            //UNPREDICTABULLETS - CAUSE AND EFFECT
            List<string> mandatorySynergyItemsCauseAndEffect = new List<string>() { "nn:unpredictabullets" };
            List<string> optionalSynergyItemsCauseAndEffect = new List<string>() { "mass_shotgun", "chaos_bullets", "chaos_ammolet", "nn:protean" };
            CustomSynergies.Add("Cause And Effect", mandatorySynergyItemsCauseAndEffect, optionalSynergyItemsCauseAndEffect);
            //FUNGO CANNON / MYSHELLIUM
            List<string> mandatorySynergyItemsMyshellium = new List<string>() { "nn:fungo_cannon" };
            List<string> optionalSynergyItemsMyshellium = new List<string>() { "nn:error_shells", "nn:rando_rounds", "nn:shutdown_shells", "nn:blight_shell", "shell" };
            CustomSynergies.Add("Myshellium", mandatorySynergyItemsMyshellium, optionalSynergyItemsMyshellium);
            //FUNGO CANNON / HUNTER SPORES
            List<string> mandatorySynergyItemsHunterSpores = new List<string>() { "nn:fungo_cannon" };
            List<string> optionalSynergyItemsHunterSpores = new List<string>() { "wolf", "nn:doggun", "rattler", "trick_gun", "colt_1851" };
            CustomSynergies.Add("Hunter Spores", mandatorySynergyItemsHunterSpores, optionalSynergyItemsHunterSpores);
            //FUNGO CANNON / SPORE LAUNCHER
            List<string> mandatorySynergyItemsEnspore = new List<string>() { "nn:fungo_cannon", "nn:spore_launcher" };
            CustomSynergies.Add("Enspore!", mandatorySynergyItemsEnspore);
            //G20 / MULTIPLICATOR
            List<string> mandatorySynergyItemsRerollinRollinRollin = new List<string>() { "nn:g20" };
            List<string> optionalSynergyItemsRerollinRollinRollin = new List<string>() { "nn:multiplicator", "scouter" };
            CustomSynergies.Add("Rerollin Rollin Rollin", mandatorySynergyItemsRerollinRollinRollin, optionalSynergyItemsRerollinRollinRollin);
            //VOID MARSHAL / JUDGE - COURT MARSHAL
            List<string> mandatorySynergyItemsCourtMarshal = new List<string>() { "the_judge", "void_marshal" };
            CustomSynergies.Add("Court Marshal", mandatorySynergyItemsCourtMarshal) ;
            //GRANDFATHER GLOCK / SIX ITEMS
            List<string> mandatorySynergyItemsSixSharp = new List<string>() { "nn:grandfather_glock" };
            List<string> optionalSynergyItemsSixSharp = new List<string>() { "sixth_chamber", "high_kaliber" };
            CustomSynergies.Add("Six Sharp", mandatorySynergyItemsSixSharp, optionalSynergyItemsSixSharp);
            //GRANDFATHER GLOCK / ONE ITEMS
            List<string> mandatorySynergyItemsKellysEye = new List<string>() { "nn:grandfather_glock" };
            List<string> optionalSynergyItemsKellysEye = new List<string>() { "nn:1_shot", "+1_bullets" };
            CustomSynergies.Add("Kellys Eye", mandatorySynergyItemsKellysEye, optionalSynergyItemsKellysEye);
            //LOCKDOWN BULLETS / ALPHA BULLETS
            List<string> mandatorySynergyItemsAddedEffectLockdown = new List<string>() { "nn:lockdown_bullets", "alpha_bullets" };
            CustomSynergies.Add("Added Effect - Lockdown", mandatorySynergyItemsAddedEffectLockdown);
            //LOCKDOWN BULLETS / OMEGA BULLETS
            List<string> mandatorySynergyItemsMoonstoneWeapon = new List<string>() { "nn:lockdown_bullets", "omega_bullets" };
            CustomSynergies.Add("Moonstone Weapon", mandatorySynergyItemsMoonstoneWeapon);
            //BOOKLLET / LOREBOOK
            List<string> mandatorySynergyItemsLibrarian = new List<string>() { "nn:lorebook", "nn:bookllet" };
            CustomSynergies.Add("Librarian", mandatorySynergyItemsLibrarian);
            //LOREBOOK / KNIGHTLY ITEMS
            List<string> mandatorySynergyItemsLevel20Fighter = new List<string>() { "nn:lorebook" };
            List<string> optionalSynergyItemsLevel20Fighter = new List<string>() { "excaliber", "vorpal_gun", "vorpal_bullets", "knights_gun", "old_knights_flask", "nn:knightly_bullets" };
            CustomSynergies.Add("Level 20 Fighter", mandatorySynergyItemsLevel20Fighter, optionalSynergyItemsLevel20Fighter);
            //LOREBOOK / WIZARD ITEMS
            List<string> mandatorySynergyItemsLevel20Wizard = new List<string>() { "nn:lorebook" };
            List<string> optionalSynergyItemsLevel20Wizard = new List<string>() { "nn:magic_missile", "witch_pistol", "magic_bullets", "hexagun", "bundle_of_wands" };
            CustomSynergies.Add("Level 20 Wizard", mandatorySynergyItemsLevel20Wizard, optionalSynergyItemsLevel20Wizard);
            //LOREBOOK / BARD ITEMS
            List<string> mandatorySynergyItemsLevel20Bard = new List<string>() { "nn:lorebook" };
            List<string> optionalSynergyItemsLevel20Bard = new List<string>() { "really_special_lute", "face_melter", "metronome", "gunzheng" };
            CustomSynergies.Add("Level 20 Bard", mandatorySynergyItemsLevel20Bard, optionalSynergyItemsLevel20Bard);
            //LOREBOOK / ROGUE ITEMS
            List<string> mandatorySynergyItemsLevel20Rogue = new List<string>() { "nn:lorebook" };
            List<string> optionalSynergyItemsLevel20Rogue = new List<string>() { "smoke_bomb", "knife_shield", "kruller_glaive", "grappling_hook", "shadow_bullets", "box" };
            CustomSynergies.Add("Level 20 Rogue", mandatorySynergyItemsLevel20Rogue, optionalSynergyItemsLevel20Rogue);
            //RING OF AMMO REDEMTION / AMMO BELT / DRUM CLIP
            List<string> mandatorySynergyItemsAmmoEconomyInflation = new List<string>() { "nn:ring_of_ammo_redemption" };
            List<string> optionalSynergyItemsAmmoEconomyInflation = new List<string>() { "ammo_belt", "drum_clip", "utility_belt" };
            CustomSynergies.Add("Ammo Economy Inflation", mandatorySynergyItemsAmmoEconomyInflation, optionalSynergyItemsAmmoEconomyInflation);
            //RISKY RING / RISK RIFLE
            List<string> mandatorySynergyItemsDoubleRiskDoubleReward = new List<string>() { "nn:risky_ring", "nn:risk_rifle" };
            CustomSynergies.Add("Double Risk, Double Reward", mandatorySynergyItemsDoubleRiskDoubleReward);
            //RISKY RING / CROWN ITEMS
            List<string> mandatorySynergyItemsUltraMutation = new List<string>() { "nn:risky_ring" };
            List<string> optionalSynergyItemsUltraMutation = new List<string>() { "crown_of_guns", "nn:goomperors_crown" };
            CustomSynergies.Add("Ultra Mutation", mandatorySynergyItemsUltraMutation, optionalSynergyItemsUltraMutation);
            //WARP BULLETS / JOLTER / DISPLACER CANNON / TELEPORTER PROTOTYPE / CHEST TELEPORTER
            List<string> mandatorySynergyItemsLostInTheWarp = new List<string>() { "nn:warp_bullets" };
            List<string> optionalSynergyItemsLostInTheWarp = new List<string>() { "jolter", "nn:displacer_cannon", "teleporter_prototype", "chest_teleporter" };
            CustomSynergies.Add("Corrupted By Warp", mandatorySynergyItemsLostInTheWarp, optionalSynergyItemsLostInTheWarp);
            //ICICLE / COLD ITEMS
            List<string> mandatorySynergyItemsYouNeedToChill = new List<string>() { "nn:icicle" };
            List<string> optionalSynergyItemsYouNeedToChill = new List<string>() { "glacier", "frost_bullets", "nn:frost_key", "ice_cube", "snowballer" };
            CustomSynergies.Add("You Need To Chill", mandatorySynergyItemsYouNeedToChill, optionalSynergyItemsYouNeedToChill);
            //HOT GLUE GUN / STICK IN THE MUD
            List<string> mandatorySynergyItemsStickInTheMud = new List<string>() { "nn:hot_glue_gun" };
            List<string> optionalSynergyItemsStickInTheMud = new List<string>() { "pig", "nn:lockdown_bullets" };
            CustomSynergies.Add("Stick In The Mud", mandatorySynergyItemsStickInTheMud, optionalSynergyItemsStickInTheMud);
            //HOT GLUE GUN / HEAT STRESS
            List<string> mandatorySynergyItemsHeatStress = new List<string>() { "nn:hot_glue_gun" };
            List<string> optionalSynergyItemsHeatStress = new List<string>() { "hot_lead", "copper_ammolet", "nn:dragun_scale", "nn:flame_chamber", "phoenix" };
            CustomSynergies.Add("Heat Stress", mandatorySynergyItemsHeatStress, optionalSynergyItemsHeatStress);
            //HOT GLUE GUN / BALLOON GUN
            List<string> mandatorySynergyItemsGlueGunner = new List<string>() { "nn:hot_glue_gun" };
            List<string> optionalSynergyItemsGlueGunner = new List<string>() { "balloon_gun" };
            CustomSynergies.Add("Glue Gunner", mandatorySynergyItemsGlueGunner, optionalSynergyItemsGlueGunner);
            //FLAMECHAMBER / FLAME ITEMS
            List<string> mandatorySynergyItemsPyromaniac = new List<string>() { "nn:flame_chamber" };
            List<string> optionalSynergyItemsPyromaniac = new List<string>() { "hot_lead", "copper_ammolet", "phoenix" };
            CustomSynergies.Add("Pyromaniac", mandatorySynergyItemsPyromaniac, optionalSynergyItemsPyromaniac);
            //HONEY POT
            List<string> mandatorySynergyItemsHoneyImHome = new List<string>() { "nn:honey_pot" };
            List<string> optionalSynergyItemsHoneyImHome = new List<string>() { "bee_hive", "honeycomb", "nn:hive_holster" };
            CustomSynergies.Add("Honey, I'm Home!", mandatorySynergyItemsHoneyImHome, optionalSynergyItemsHoneyImHome);

            //SYNERGIES ADDED IN THE SYNERGISING
            //BOXING GLOVE / BASHING BULLETS
            List<string> mandatorySynergyItemsGunPunchMan = new List<string>() { "nn:bashing_bullets", "boxing_glove" };
            CustomSynergies.Add("Gun Punch Man", mandatorySynergyItemsGunPunchMan);
            //BLANK STARE / SPARE KEY
            List<string> mandatorySynergyItemsItsAKeyper = new List<string>() { "nn:blank_stare", "nn:spare_key" };
            CustomSynergies.Add("It's a Key-per!", mandatorySynergyItemsItsAKeyper);
            //BLANK KEY / SPARE BLANK
            List<string> mandatorySynergyItemsAnotherTime = new List<string>() { "nn:blank_key", "nn:spare_blank" };
            CustomSynergies.Add("...Another Time", mandatorySynergyItemsAnotherTime);
            //GLASS CHAMBER / GLASS ROUNDS
            List<string> mandatorySynergyItemsGlassworks = new List<string>() { "nn:glass_chamber", "nn:glass_rounds" };
            CustomSynergies.Add("Glassworks", mandatorySynergyItemsGlassworks);
            //HIVE HOLSTER / STINGER
            List<string> mandatorySynergyItemsStingInTheTail = new List<string>() { "nn:hive_holster", "stinger" };
            CustomSynergies.Add("Sting in the Tail", mandatorySynergyItemsStingInTheTail);
            //MUTAGEN / NANOMACHINES
            List<string> mandatorySynergyItemsNanobiology = new List<string>() { "nn:mutagen", "nanomachines" };
            CustomSynergies.Add("Nanobiology", mandatorySynergyItemsNanobiology);
            //MUTAGEN / ANTIBODY
            List<string> mandatorySynergyItemsUltraserum = new List<string>() { "nn:mutagen", "antibody" };
            CustomSynergies.Add("Ultraserum", mandatorySynergyItemsUltraserum);
            //SPARE KEY / SPARE BLANK
            List<string> mandatorySynergyItemsSpareProtection = new List<string>() { "nn:spare_blank", "nn:spare_key" };
            CustomSynergies.Add("Spare Protection", mandatorySynergyItemsSpareProtection);
            //LOOSE CHANGE / RUSTY CASING
            List<string> mandatorySynergyItemsLostNeverFound = new List<string>() { "nn:loose_change", "nn:rusty_casing" };
            CustomSynergies.Add("Lost, Never Found", mandatorySynergyItemsLostNeverFound);
            //BULLET KIN PLUSHIE / MICROTRANSACTION GUN / TSHIRT CANNON
            List<string> mandatorySynergyItemsMerchandising = new List<string>() { "nn:bullet_kin_plushie" };
            List<string> optionalSynergyItemsMerchandising = new List<string>() { "microtransaction_gun", "t_shirt_cannon" };
            CustomSynergies.Add("Merchandising", mandatorySynergyItemsMerchandising, optionalSynergyItemsMerchandising);

            // OLD SYNERGIES MIGRATED HERE
            List<string> mandatorySynergyItemsSoundBarrier = new List<string>() { "nn:table_tech_speed", "nn:speed_potion" };
            CustomSynergies.Add("Sound Barrier", mandatorySynergyItemsSoundBarrier);
            //YELLOW CHAMBER / KALIBERS EYE
            List<string> mandatorySynergyItemsAllSeeing = new List<string>() { "nn:kaliber's_eye", "yellow_chamber" };
            CustomSynergies.Add("All Seeing", mandatorySynergyItemsAllSeeing);
        }

    }
}
