using ItemAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace NevernamedsItems
{
    public class GameActorDecorationEffect : GameActorEffect
    {
        public GameActorDecorationEffect()
        {

        }
    }
    class VFXToolbox
    {
        private static GameObject VFXScapeGoat;
        private static tk2dSpriteCollectionData PrivateVFXCollection;
        public static tk2dSpriteCollectionData VFXCollection
        {
            get
            {
                return PrivateVFXCollection;
            }
        }
        public static void InitVFX()
        {
            VFXScapeGoat = new GameObject();
            UnityEngine.Object.DontDestroyOnLoad(VFXScapeGoat);
            PrivateVFXCollection = SpriteBuilder.ConstructCollection(VFXScapeGoat, "OMITBVFXCollection");

            GameObject errorshellsvfx = SpriteBuilder.SpriteFromResource("NevernamedsItems/Resources/MiscVFX/errorshellsoverhead_vfx", new GameObject("ErrorShellsIcon"));
            errorshellsvfx.SetActive(false);
            tk2dBaseSprite vfxSprite = errorshellsvfx.GetComponent<tk2dBaseSprite>();
            vfxSprite.GetCurrentSpriteDef().ConstructOffsetsFromAnchor(tk2dBaseSprite.Anchor.LowerCenter, vfxSprite.GetCurrentSpriteDef().position3);
            FakePrefab.MarkAsFakePrefab(errorshellsvfx);
            UnityEngine.Object.DontDestroyOnLoad(errorshellsvfx);
            EasyVFXDatabase.ERRORShellsOverheadVFX = errorshellsvfx;

            List<string> SpareVFXPaths = new List<string>()
            {
                "NevernamedsItems/Resources/MiscVFX/spared_vfx_001",
                "NevernamedsItems/Resources/MiscVFX/spared_vfx_002",
                "NevernamedsItems/Resources/MiscVFX/spared_vfx_003",
                "NevernamedsItems/Resources/MiscVFX/spared_vfx_004",
                "NevernamedsItems/Resources/MiscVFX/spared_vfx_005",
                "NevernamedsItems/Resources/MiscVFX/spared_vfx_006",
                "NevernamedsItems/Resources/MiscVFX/spared_vfx_007",
                "NevernamedsItems/Resources/MiscVFX/spared_vfx_008",
                "NevernamedsItems/Resources/MiscVFX/spared_vfx_009",
                "NevernamedsItems/Resources/MiscVFX/spared_vfx_010",
                "NevernamedsItems/Resources/MiscVFX/spared_vfx_011",
            };
            GameObject spareVFX = CreateVFX("GundertaleSpare", SpareVFXPaths, 16, new IntVector2(34, 14), tk2dBaseSprite.Anchor.LowerCenter, true, 0.18f, 100, Color.yellow);
            EasyVFXDatabase.GundetaleSpareVFX = spareVFX;

            List<string> SpeedUpVFX = new List<string>()
            {
                "NevernamedsItems/Resources/StatVFX/speedup_vfx_001",
                "NevernamedsItems/Resources/StatVFX/speedup_vfx_002",
                "NevernamedsItems/Resources/StatVFX/speedup_vfx_003",
                "NevernamedsItems/Resources/StatVFX/speedup_vfx_004",
                "NevernamedsItems/Resources/StatVFX/speedup_vfx_005",
                "NevernamedsItems/Resources/StatVFX/speedup_vfx_006",
                "NevernamedsItems/Resources/StatVFX/speedup_vfx_007",
            };
            GameObject SpeedUpVFXObj = CreateVFX("Speed Up VFX", SpeedUpVFX, 16, new IntVector2(27, 17), tk2dBaseSprite.Anchor.LowerCenter, true, 0.18f, 100, Color.yellow);
            EasyVFXDatabase.SpeedUpVFX = SpeedUpVFXObj;

            #region RainbowGuonPoofs
            //RED
            List<string> RedPoofPaths = new List<string>()
            {
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/redpoof_001",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/redpoof_002",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/redpoof_003",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/redpoof_004",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/redpoof_005",
            };
            RainbowGuonStone.RedGuonTransitionVFX = CreateVFX("RedGuonPoof", RedPoofPaths, 14, new IntVector2(21, 22), tk2dBaseSprite.Anchor.MiddleCenter, false, 0);
            //ORANGE
            List<string> OrangePoofPaths = new List<string>()
            {
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/orangepoof_001",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/orangepoof_002",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/orangepoof_003",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/orangepoof_004",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/orangepoof_005",
            };
            RainbowGuonStone.OrangeGuonTransitionVFX = CreateVFX("OrangeGuonPoof", OrangePoofPaths, 14, new IntVector2(21, 22), tk2dBaseSprite.Anchor.MiddleCenter, false, 0);
            //YELLOW
            List<string> YellowPoofPaths = new List<string>()
            {
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/yellowpoof_001",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/yellowpoof_002",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/yellowpoof_003",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/yellowpoof_004",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/yellowpoof_005",
            };
            RainbowGuonStone.YellowGuonTransitionVFX = CreateVFX("YellowGuonPoof", YellowPoofPaths, 14, new IntVector2(21, 22), tk2dBaseSprite.Anchor.MiddleCenter, false, 0);
            //GREEN
            List<string> GreenPoofPaths = new List<string>()
            {
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/greenpoof_001",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/greenpoof_002",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/greenpoof_003",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/greenpoof_004",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/greenpoof_005",
            };
            RainbowGuonStone.GreenGuonTransitionVFX = CreateVFX("GreenGuonPoof", GreenPoofPaths, 14, new IntVector2(21, 22), tk2dBaseSprite.Anchor.MiddleCenter, false, 0);
            //BLUE
            List<string> BluePoofPaths = new List<string>()
            {
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/bluepoof_001",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/bluepoof_002",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/bluepoof_003",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/bluepoof_004",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/bluepoof_005",
            };
            RainbowGuonStone.BlueGuonTransitionVFX = CreateVFX("BlueGuonPoof", BluePoofPaths, 14, new IntVector2(21, 22), tk2dBaseSprite.Anchor.MiddleCenter, false, 0);
            //WHITE
            List<string> WhitePoofPaths = new List<string>()
            {
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/whitepoof_001",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/whitepoof_002",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/whitepoof_003",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/whitepoof_004",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/whitepoof_005",
            };
            RainbowGuonStone.WhiteGuonTransitionVFX = CreateVFX("WhiteGuonPoof", WhitePoofPaths, 14, new IntVector2(21, 22), tk2dBaseSprite.Anchor.MiddleCenter, false, 0);
            //CYAN
            List<string> CyanPoofPaths = new List<string>()
            {
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/cyanpoof_001",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/cyanpoof_002",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/cyanpoof_003",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/cyanpoof_004",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/cyanpoof_005",
            };
            RainbowGuonStone.CyanGuonTransitionVFX = CreateVFX("CyanGuonPoof", CyanPoofPaths, 14, new IntVector2(21, 22), tk2dBaseSprite.Anchor.MiddleCenter, false, 0);
            //GOLD
            List<string> GoldPoofPaths = new List<string>()
            {
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/goldpoof_001",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/goldpoof_002",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/goldpoof_003",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/goldpoof_004",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/goldpoof_005",
            };
            RainbowGuonStone.GoldGuonTransitionVFX = CreateVFX("GoldGuonPoof", GoldPoofPaths, 14, new IntVector2(21, 22), tk2dBaseSprite.Anchor.MiddleCenter, false, 0);
            //GREY
            List<string> GreyPoofPaths = new List<string>()
            {
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/greypoof_001",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/greypoof_002",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/greypoof_003",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/greypoof_004",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/greypoof_005",
            };
            RainbowGuonStone.GreyGuonTransitionVFX = CreateVFX("GreyGuonPoof", GreyPoofPaths, 14, new IntVector2(21, 22), tk2dBaseSprite.Anchor.MiddleCenter, false, 0);
            //BROWN
            List<string> BrownPoofPaths = new List<string>()
            {
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/brownpoof_001",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/brownpoof_002",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/brownpoof_003",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/brownpoof_004",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/brownpoof_005",
            };
            RainbowGuonStone.BrownGuonTransitionVFX = CreateVFX("BrownGuonPoof", BrownPoofPaths, 14, new IntVector2(21, 22), tk2dBaseSprite.Anchor.MiddleCenter, false, 0);
            //INDIGO
            List<string> IndigoPoofPaths = new List<string>()
            {
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/indigopoof_001",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/indigopoof_002",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/indigopoof_003",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/indigopoof_004",
                "NevernamedsItems/Resources/MiscVFX/RainbowGuonPoofs/indigopoof_005",
            };
            RainbowGuonStone.IndigoGuonTransitionVFX = CreateVFX("IndigoGuonPoof", IndigoPoofPaths, 14, new IntVector2(21, 22), tk2dBaseSprite.Anchor.MiddleCenter, false, 0);
            #endregion
        }
        
        public static void DoStringSquirt(string text, Vector2 point, Color colour, float heightOffGround = 3f, float opacity = 1f)
        {

            GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("DamagePopupLabel", ".prefab"), GameUIRoot.Instance.transform);

            dfLabel label = gameObject.GetComponent<dfLabel>();
            label.gameObject.SetActive(true);
            label.Text = text;
            label.Color = colour;
            label.Opacity = opacity;
            label.TextAlignment = TextAlignment.Center;

            label.transform.position = point;
            Vector2 point2 = new Vector2(label.transform.position.x - (label.GetCenter().x - label.transform.position.x), point.y);
            label.transform.position = label.transform.position.QuantizeFloor(label.PixelsToUnits() / (Pixelator.Instance.ScaleTileScale / Pixelator.Instance.CurrentTileScale));
            label.StartCoroutine(HandleDamageNumberCR(point2, point2.y - heightOffGround, label));
        }
        private static IEnumerator HandleDamageNumberCR(Vector3 startWorldPosition, float worldFloorHeight, dfLabel damageLabel)
        {
            float elapsed = 0f;
            float duration = 1.5f;
            float holdTime = 0f;
            Camera mainCam = GameManager.Instance.MainCameraController.Camera;
            Vector3 worldPosition = startWorldPosition;
            Vector3 lastVelocity = new Vector3(Mathf.Lerp(-8f, 8f, UnityEngine.Random.value), UnityEngine.Random.Range(15f, 25f), 0f);
            while (elapsed < duration)
            {
                float dt = BraveTime.DeltaTime;
                elapsed += dt;
                if (GameManager.Instance.IsPaused)
                {
                    break;
                }
                if (elapsed > holdTime)
                {
                    lastVelocity += new Vector3(0f, -50f, 0f) * dt;
                    Vector3 vector = lastVelocity * dt + worldPosition;
                    if (vector.y < worldFloorHeight)
                    {
                        float num = worldFloorHeight - vector.y;
                        float num2 = worldFloorHeight + num;
                        vector.y = num2 * 0.5f;
                        lastVelocity.y *= -0.5f;
                    }
                    worldPosition = vector;
                    damageLabel.transform.position = dfFollowObject.ConvertWorldSpaces(worldPosition, mainCam, GameUIRoot.Instance.Manager.RenderCamera).WithZ(0f);
                }
                float t = elapsed / duration;
                damageLabel.Opacity = 1f - t;
                yield return null;
            }
            damageLabel.gameObject.SetActive(false);
            UnityEngine.Object.Destroy(damageLabel.gameObject, 1);
            yield break;
        }
        public static GameObject CreateVFX(string name, List<string> spritePaths, int fps, IntVector2 Dimensions, tk2dBaseSprite.Anchor anchor, bool usesZHeight, float zHeightOffset, float emissivePower = -1, Color? emissiveColour = null)
        {
            GameObject Obj = new GameObject(name);
            VFXObject vfObj = new VFXObject();
            Obj.SetActive(false);
            FakePrefab.MarkAsFakePrefab(Obj);
            UnityEngine.Object.DontDestroyOnLoad(Obj);

            tk2dSpriteCollectionData VFXSpriteCollection = SpriteBuilder.ConstructCollection(Obj, (name + "_Collection"));
            int spriteID = SpriteBuilder.AddSpriteToCollection(spritePaths[0], VFXSpriteCollection);

            tk2dSprite sprite = Obj.GetOrAddComponent<tk2dSprite>();
            sprite.SetSprite(VFXSpriteCollection, spriteID);
            tk2dSpriteDefinition defaultDef = sprite.GetCurrentSpriteDef();
            defaultDef.colliderVertices = new Vector3[]{
                      new Vector3(0f, 0f, 0f),
                      new Vector3((Dimensions.x / 16), (Dimensions.y / 16), 0f)
                  };

            tk2dSpriteAnimator animator = Obj.GetOrAddComponent<tk2dSpriteAnimator>();
            tk2dSpriteAnimation animation = Obj.GetOrAddComponent<tk2dSpriteAnimation>();
            animation.clips = new tk2dSpriteAnimationClip[0];
            animator.Library = animation;
            tk2dSpriteAnimationClip clip = new tk2dSpriteAnimationClip() { name = "start", frames = new tk2dSpriteAnimationFrame[0], fps = fps };
            List<tk2dSpriteAnimationFrame> frames = new List<tk2dSpriteAnimationFrame>();
            for (int i = 0; i < spritePaths.Count; i++)
            {
                tk2dSpriteCollectionData collection = VFXSpriteCollection;
                int frameSpriteId = SpriteBuilder.AddSpriteToCollection(spritePaths[i], collection);
                tk2dSpriteDefinition frameDef = collection.spriteDefinitions[frameSpriteId];
                frameDef.ConstructOffsetsFromAnchor(anchor);
                frameDef.colliderVertices = defaultDef.colliderVertices;
                if (emissivePower > 0) frameDef.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
                if (emissivePower > 0) frameDef.material.SetFloat("_EmissiveColorPower", emissivePower);
                if (emissiveColour != null) frameDef.material.SetColor("_EmissiveColor", (Color)emissiveColour);
                if (emissivePower > 0) frameDef.materialInst.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
                if (emissivePower > 0) frameDef.materialInst.SetFloat("_EmissiveColorPower", emissivePower);
                if (emissiveColour != null) frameDef.materialInst.SetColor("_EmissiveColor", (Color)emissiveColour);
                frames.Add(new tk2dSpriteAnimationFrame { spriteId = frameSpriteId, spriteCollection = collection });
            }
            if (emissivePower > 0) sprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
            if (emissivePower > 0) sprite.renderer.material.SetFloat("_EmissiveColorPower", emissivePower);
            if (emissiveColour != null) sprite.renderer.material.SetColor("_EmissiveColor", (Color)emissiveColour);
            clip.frames = frames.ToArray();
            clip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
            animation.clips = animation.clips.Concat(new tk2dSpriteAnimationClip[] { clip }).ToArray();
            SpriteAnimatorKiller kill = animator.gameObject.AddComponent<SpriteAnimatorKiller>();
            kill.fadeTime = -1f;
            kill.animator = animator;
            kill.delayDestructionTime = -1f;
            animator.playAutomatically = true;
            animator.DefaultClipId = animator.GetClipIdByName("start");
            vfObj.attached = true;
            vfObj.persistsOnDeath = false;
            vfObj.usesZHeight = usesZHeight;
            vfObj.zHeight = zHeightOffset;
            vfObj.alignment = VFXAlignment.NormalAligned;
            vfObj.destructible = false;
            vfObj.effect = Obj;
            return Obj;
        }
        public static VFXComplex CreateVFXComplex(string name, List<string> spritePaths, int fps, IntVector2 Dimensions, tk2dBaseSprite.Anchor anchor, bool usesZHeight, float zHeightOffset, bool persist = false, float emissivePower = -1, Color? emissiveColour = null)
        {
            GameObject Obj = new GameObject(name);
            VFXPool pool = new VFXPool();
            pool.type = VFXPoolType.All;
            VFXComplex complex = new VFXComplex();
            VFXObject vfObj = new VFXObject();
            Obj.SetActive(false);
            FakePrefab.MarkAsFakePrefab(Obj);
            UnityEngine.Object.DontDestroyOnLoad(Obj);

            tk2dSpriteCollectionData VFXSpriteCollection = SpriteBuilder.ConstructCollection(Obj, (name + "_Collection"));
            int spriteID = SpriteBuilder.AddSpriteToCollection(spritePaths[0], VFXSpriteCollection);

            tk2dSprite sprite = Obj.GetOrAddComponent<tk2dSprite>();
            sprite.SetSprite(VFXSpriteCollection, spriteID);
            tk2dSpriteDefinition defaultDef = sprite.GetCurrentSpriteDef();
            defaultDef.colliderVertices = new Vector3[]{
                      new Vector3(0f, 0f, 0f),
                      new Vector3((Dimensions.x / 16), (Dimensions.y / 16), 0f)
                  };

            tk2dSpriteAnimator animator = Obj.GetOrAddComponent<tk2dSpriteAnimator>();
            tk2dSpriteAnimation animation = Obj.GetOrAddComponent<tk2dSpriteAnimation>();
            animation.clips = new tk2dSpriteAnimationClip[0];
            animator.Library = animation;
            tk2dSpriteAnimationClip clip = new tk2dSpriteAnimationClip() { name = "start", frames = new tk2dSpriteAnimationFrame[0], fps = fps };
            List<tk2dSpriteAnimationFrame> frames = new List<tk2dSpriteAnimationFrame>();
            for (int i = 0; i < spritePaths.Count; i++)
            {
                tk2dSpriteCollectionData collection = VFXSpriteCollection;
                int frameSpriteId = SpriteBuilder.AddSpriteToCollection(spritePaths[i], collection);
                tk2dSpriteDefinition frameDef = collection.spriteDefinitions[frameSpriteId];
                frameDef.ConstructOffsetsFromAnchor(anchor);
                frameDef.colliderVertices = defaultDef.colliderVertices;
                if (emissivePower > 0) frameDef.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
                if (emissivePower > 0) frameDef.material.SetFloat("_EmissiveColorPower", emissivePower);
                if (emissiveColour != null) frameDef.material.SetColor("_EmissiveColor", (Color)emissiveColour);
                if (emissivePower > 0) frameDef.materialInst.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
                if (emissivePower > 0) frameDef.materialInst.SetFloat("_EmissiveColorPower", emissivePower);
                if (emissiveColour != null) frameDef.materialInst.SetColor("_EmissiveColor", (Color)emissiveColour);
                frames.Add(new tk2dSpriteAnimationFrame { spriteId = frameSpriteId, spriteCollection = collection });
            }
            if (emissivePower > 0) sprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
            if (emissivePower > 0) sprite.renderer.material.SetFloat("_EmissiveColorPower", emissivePower);
            if (emissiveColour != null) sprite.renderer.material.SetColor("_EmissiveColor", (Color)emissiveColour);
            clip.frames = frames.ToArray();
            clip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
            animation.clips = animation.clips.Concat(new tk2dSpriteAnimationClip[] { clip }).ToArray();
            SpriteAnimatorKiller kill = animator.gameObject.AddComponent<SpriteAnimatorKiller>();
            kill.fadeTime = -1f;
            kill.animator = animator;
            kill.delayDestructionTime = -1f;
            animator.playAutomatically = true;
            animator.DefaultClipId = animator.GetClipIdByName("start");
            vfObj.attached = true;
            vfObj.persistsOnDeath = persist;
            vfObj.usesZHeight = usesZHeight;
            vfObj.zHeight = zHeightOffset;
            vfObj.alignment = VFXAlignment.NormalAligned;
            vfObj.destructible = false;
            vfObj.effect = Obj;
            complex.effects = new VFXObject[] { vfObj };
            pool.effects = new VFXComplex[] { complex };
            return complex;
        }
        public static VFXPool CreateVFXPool(string name, List<string> spritePaths, int fps, IntVector2 Dimensions, tk2dBaseSprite.Anchor anchor, bool usesZHeight, float zHeightOffset, bool persist = false, VFXAlignment alignment = VFXAlignment.NormalAligned, float emissivePower = -1, Color? emissiveColour = null)
        {
            GameObject Obj = new GameObject(name);
            VFXPool pool = new VFXPool();
            pool.type = VFXPoolType.All;
            VFXComplex complex = new VFXComplex();
            VFXObject vfObj = new VFXObject();
            Obj.SetActive(false);
            FakePrefab.MarkAsFakePrefab(Obj);
            UnityEngine.Object.DontDestroyOnLoad(Obj);

            tk2dSpriteCollectionData VFXSpriteCollection = SpriteBuilder.ConstructCollection(Obj, (name + "_Collection"));
            int spriteID = SpriteBuilder.AddSpriteToCollection(spritePaths[0], VFXSpriteCollection);

            tk2dSprite sprite = Obj.GetOrAddComponent<tk2dSprite>();
            sprite.SetSprite(VFXSpriteCollection, spriteID);
            tk2dSpriteDefinition defaultDef = sprite.GetCurrentSpriteDef();
            defaultDef.colliderVertices = new Vector3[]{
                      new Vector3(0f, 0f, 0f),
                      new Vector3((Dimensions.x / 16), (Dimensions.y / 16), 0f)
                  };

            tk2dSpriteAnimator animator = Obj.GetOrAddComponent<tk2dSpriteAnimator>();
            tk2dSpriteAnimation animation = Obj.GetOrAddComponent<tk2dSpriteAnimation>();
            animation.clips = new tk2dSpriteAnimationClip[0];
            animator.Library = animation;
            tk2dSpriteAnimationClip clip = new tk2dSpriteAnimationClip() { name = "start", frames = new tk2dSpriteAnimationFrame[0], fps = fps };
            List<tk2dSpriteAnimationFrame> frames = new List<tk2dSpriteAnimationFrame>();
            for (int i = 0; i < spritePaths.Count; i++)
            {
                tk2dSpriteCollectionData collection = VFXSpriteCollection;
                int frameSpriteId = SpriteBuilder.AddSpriteToCollection(spritePaths[i], collection);
                tk2dSpriteDefinition frameDef = collection.spriteDefinitions[frameSpriteId];
                frameDef.ConstructOffsetsFromAnchor(anchor);
                frameDef.colliderVertices = defaultDef.colliderVertices;
                if (emissivePower > 0) frameDef.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
                if (emissivePower > 0) frameDef.material.SetFloat("_EmissiveColorPower", emissivePower);
                if (emissiveColour != null) frameDef.material.SetColor("_EmissiveColor", (Color)emissiveColour);
                if (emissivePower > 0) frameDef.materialInst.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
                if (emissivePower > 0) frameDef.materialInst.SetFloat("_EmissiveColorPower", emissivePower);
                if (emissiveColour != null) frameDef.materialInst.SetColor("_EmissiveColor", (Color)emissiveColour);
                frames.Add(new tk2dSpriteAnimationFrame { spriteId = frameSpriteId, spriteCollection = collection });
            }
            if (emissivePower > 0) sprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
            if (emissivePower > 0) sprite.renderer.material.SetFloat("_EmissiveColorPower", emissivePower);
            if (emissiveColour != null) sprite.renderer.material.SetColor("_EmissiveColor", (Color)emissiveColour);
            clip.frames = frames.ToArray();
            clip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
            animation.clips = animation.clips.Concat(new tk2dSpriteAnimationClip[] { clip }).ToArray();
            if (!persist)
            {
                SpriteAnimatorKiller kill = animator.gameObject.AddComponent<SpriteAnimatorKiller>();
                kill.fadeTime = -1f;
                kill.animator = animator;
                kill.delayDestructionTime = -1f;
            }
            animator.playAutomatically = true;
            animator.DefaultClipId = animator.GetClipIdByName("start");
            vfObj.attached = true;
            vfObj.persistsOnDeath = persist;
            vfObj.usesZHeight = usesZHeight;
            vfObj.zHeight = zHeightOffset;
            vfObj.alignment = alignment;
            vfObj.destructible = false;
            vfObj.effect = Obj;
            complex.effects = new VFXObject[] { vfObj };
            pool.effects = new VFXComplex[] { complex };
            return pool;
        }
        public static VFXPool CreateMuzzleflash(string name, List<string> spriteNames, int fps, List<IntVector2> spriteSizes, List<tk2dBaseSprite.Anchor> anchors, List<Vector2> manualOffsets, bool orphaned, bool attached, bool persistsOnDeath,
            bool usesZHeight, float zHeight, VFXAlignment alignment, bool destructible, List<float> emissivePowers, List<Color> emissiveColors)
        {
            VFXPool pool = new VFXPool();
            pool.type = VFXPoolType.All;
            VFXComplex complex = new VFXComplex();
            VFXObject vfObj = new VFXObject();
            GameObject obj = new GameObject(name);
            obj.SetActive(false);
            FakePrefab.MarkAsFakePrefab(obj);
            UnityEngine.Object.DontDestroyOnLoad(obj);
            tk2dSprite sprite = obj.AddComponent<tk2dSprite>();
            tk2dSpriteAnimator animator = obj.AddComponent<tk2dSpriteAnimator>();
            tk2dSpriteAnimationClip clip = new tk2dSpriteAnimationClip();
            clip.fps = fps;
            clip.frames = new tk2dSpriteAnimationFrame[0];
            for (int i = 0; i < spriteNames.Count; i++)
            {
                string spriteName = spriteNames[i];
                IntVector2 spriteSize = spriteSizes[i];
                tk2dBaseSprite.Anchor anchor = anchors[i];
                Vector2 manualOffset = manualOffsets[i];
                float emissivePower = emissivePowers[i];
                Color emissiveColor = emissiveColors[i];
                tk2dSpriteAnimationFrame frame = new tk2dSpriteAnimationFrame();
                frame.spriteId = VFXCollection.GetSpriteIdByName(spriteName);
                tk2dSpriteDefinition def = VFXToolbox.SetupDefinitionForShellSprite(spriteName, frame.spriteId, spriteSize.x, spriteSize.y);
                def.ConstructOffsetsFromAnchor(anchor, def.position3);
                def.MakeOffset(manualOffset);
                def.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
                def.material.SetFloat("_EmissiveColorPower", emissivePower);
                def.material.SetColor("_EmissiveColor", emissiveColor);
                def.materialInst.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
                def.materialInst.SetFloat("_EmissiveColorPower", emissivePower);
                def.materialInst.SetColor("_EmissiveColor", emissiveColor);
                frame.spriteCollection = VFXCollection;
                clip.frames = clip.frames.Concat(new tk2dSpriteAnimationFrame[] { frame }).ToArray();
            }
            sprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
            sprite.renderer.material.SetFloat("_EmissiveColorPower", emissivePowers[0]);
            sprite.renderer.material.SetColor("_EmissiveColor", emissiveColors[0]);
            clip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
            clip.name = "start";
            animator.spriteAnimator.Library = animator.gameObject.AddComponent<tk2dSpriteAnimation>();
            animator.spriteAnimator.Library.clips = new tk2dSpriteAnimationClip[] { clip };
            animator.spriteAnimator.Library.enabled = true;
            SpriteAnimatorKiller kill = animator.gameObject.AddComponent<SpriteAnimatorKiller>();
            kill.fadeTime = -1f;
            kill.animator = animator;
            kill.delayDestructionTime = -1f;
            vfObj.orphaned = orphaned;
            vfObj.attached = attached;
            vfObj.persistsOnDeath = persistsOnDeath;
            vfObj.usesZHeight = usesZHeight;
            vfObj.zHeight = zHeight;
            vfObj.alignment = alignment;
            vfObj.destructible = destructible;
            vfObj.effect = obj;
            complex.effects = new VFXObject[] { vfObj };
            pool.effects = new VFXComplex[] { complex };
            animator.playAutomatically = true;
            animator.DefaultClipId = animator.GetClipIdByName("start");
            return pool;
        }
        public static GameObject CreateCustomClip(string spriteName, int pixelWidth, int pixelHeight)
        {
            GameObject clip = UnityEngine.Object.Instantiate((PickupObjectDatabase.GetById(95) as Gun).clipObject);
            clip.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(clip.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(clip);
            clip.GetComponent<tk2dBaseSprite>().spriteId = VFXCollection.inst.GetSpriteIdByName(spriteName);
            VFXToolbox.SetupDefinitionForClipSprite(spriteName, clip.GetComponent<tk2dBaseSprite>().spriteId, pixelWidth, pixelHeight);
            return clip;
        }
        public static void SetupDefinitionForClipSprite(string name, int id, int pixelWidth, int pixelHeight)
        {
            float thing = 14;
            float trueWidth = (float)pixelWidth / thing;
            float trueHeight = (float)pixelHeight / thing;
            tk2dSpriteDefinition def = VFXCollection.inst.spriteDefinitions[(PickupObjectDatabase.GetById(47) as Gun).clipObject.GetComponent<tk2dBaseSprite>().spriteId].CopyDefinitionFrom();
            def.boundsDataCenter = new Vector3(trueWidth / 2f, trueHeight / 2f, 0f);
            def.boundsDataExtents = new Vector3(trueWidth, trueHeight, 0f);
            def.untrimmedBoundsDataCenter = new Vector3(trueWidth / 2f, trueHeight / 2f, 0f);
            def.untrimmedBoundsDataExtents = new Vector3(trueWidth, trueHeight, 0f);
            def.position0 = new Vector3(0f, 0f, 0f);
            def.position1 = new Vector3(0f + trueWidth, 0f, 0f);
            def.position2 = new Vector3(0f, 0f + trueHeight, 0f);
            def.position3 = new Vector3(0f + trueWidth, 0f + trueHeight, 0f);
            def.colliderVertices[1].x = trueWidth;
            def.colliderVertices[1].y = trueHeight;
            def.name = name;
            VFXCollection.spriteDefinitions[id] = def;
        }
        public static GameObject CreateCustomShellCasing(string spriteName, int pixelWidth, int pixelHeight)
        {
            GameObject casing = UnityEngine.Object.Instantiate((PickupObjectDatabase.GetById(202) as Gun).shellCasing);
            casing.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(casing.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(casing);
            casing.GetComponent<tk2dBaseSprite>().spriteId = VFXCollection.inst.GetSpriteIdByName(spriteName);
            VFXToolbox.SetupDefinitionForShellSprite(spriteName, casing.GetComponent<tk2dBaseSprite>().spriteId, pixelWidth, pixelHeight);
            return casing;
        }
        public static tk2dSpriteDefinition SetupDefinitionForShellSprite(string name, int id, int pixelWidth, int pixelHeight, tk2dSpriteDefinition overrideToCopyFrom = null)
        {
            float thing = 14;
            float trueWidth = (float)pixelWidth / thing;
            float trueHeight = (float)pixelHeight / thing;
            tk2dSpriteDefinition def = overrideToCopyFrom ?? VFXCollection.inst.spriteDefinitions[(PickupObjectDatabase.GetById(202) as Gun).shellCasing.GetComponent<tk2dBaseSprite>().spriteId].CopyDefinitionFrom();
            def.boundsDataCenter = new Vector3(trueWidth / 2f, trueHeight / 2f, 0f);
            def.boundsDataExtents = new Vector3(trueWidth, trueHeight, 0f);
            def.untrimmedBoundsDataCenter = new Vector3(trueWidth / 2f, trueHeight / 2f, 0f);
            def.untrimmedBoundsDataExtents = new Vector3(trueWidth, trueHeight, 0f);
            def.position0 = new Vector3(0f, 0f, 0f);
            def.position1 = new Vector3(0f + trueWidth, 0f, 0f);
            def.position2 = new Vector3(0f, 0f + trueHeight, 0f);
            def.position3 = new Vector3(0f + trueWidth, 0f + trueHeight, 0f);
            def.name = name;
            VFXCollection.spriteDefinitions[id] = def;
            return def;
        }
    }
}
