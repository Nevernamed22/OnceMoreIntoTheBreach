using Alexandria.ItemAPI;
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
        public static GameObject RenderLaserSight(Vector2 position, float length, float width, float angle, bool alterColour = false, Color? colour = null)
        {
            GameObject gameObject = SpawnManager.SpawnVFX(SharedVFX.LaserSight, position, Quaternion.Euler(0, 0, angle));

            tk2dTiledSprite component2 = gameObject.GetComponent<tk2dTiledSprite>();
            float newWidth = 1f;
            if (width != -1) newWidth = width;
            component2.dimensions = new Vector2(length, newWidth);
            if (alterColour && colour != null)
            {
                component2.usesOverrideMaterial = true;
                component2.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
                component2.sprite.renderer.material.SetColor("_OverrideColor", (Color)colour);
                component2.sprite.renderer.material.SetColor("_EmissiveColor", (Color)colour);
                component2.sprite.renderer.material.SetFloat("_EmissivePower", 100);
                component2.sprite.renderer.material.SetFloat("_EmissiveColorPower", 1.55f);
            }
            return gameObject;
        }
        public static void GlitchScreenForSeconds(float seconds)
        {
            GameManager.Instance.StartCoroutine(DoScreenGlitch(seconds));
        }
        private static IEnumerator DoScreenGlitch(float seconds)
        {
            Material glitchPass = new Material(Shader.Find("Brave/Internal/GlitchUnlit"));
            Pixelator.Instance.RegisterAdditionalRenderPass(glitchPass);
            yield return new WaitForSeconds(seconds);
            Pixelator.Instance.DeregisterAdditionalRenderPass(glitchPass);
            yield break;
        }
        public static void DoRisingStringFade(string text, Vector2 point, Color colour, float heightOffGround = 3f, float opacity = 1f)
        {

            GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("DamagePopupLabel", ".prefab"), GameUIRoot.Instance.transform);

            dfLabel label = gameObject.GetComponent<dfLabel>();
            label.gameObject.SetActive(true);
            label.Text = text;
            label.Color = colour;
            label.Opacity = opacity;
            label.TextAlignment = TextAlignment.Center;
            label.Anchor = dfAnchorStyle.Bottom;
            label.Pivot = dfPivotPoint.BottomCenter;
            label.Invalidate();
            label.transform.position = dfFollowObject.ConvertWorldSpaces(point, GameManager.Instance.MainCameraController.Camera, GameManager.Instance.MainCameraController.Camera).WithZ(0f);
            label.transform.position = label.transform.position.QuantizeFloor(label.PixelsToUnits() / (Pixelator.Instance.ScaleTileScale / Pixelator.Instance.CurrentTileScale));

            label.StartCoroutine(HandleDamageNumberRiseCR(point, point.y - heightOffGround, label));
        }
        private static IEnumerator HandleDamageNumberRiseCR(Vector3 startWorldPosition, float worldFloorHeight, dfLabel damageLabel)
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

                    damageLabel.transform.position = dfFollowObject.ConvertWorldSpaces(new Vector3(worldPosition.x, worldPosition.y + (elapsed / duration)), mainCam, GameUIRoot.Instance.Manager.RenderCamera).WithZ(0f);
                }
                float t = elapsed / duration;
                damageLabel.Opacity = 1f - t;
                yield return null;
            }
            damageLabel.gameObject.SetActive(false);
            UnityEngine.Object.Destroy(damageLabel.gameObject, 1);
            yield break;
        }


        public static Shader GlitchShader;
        public static void BecomeGlitched(GameObject targetObject, float GlitchInterval = 0.1f, float DispProbability = 0.4f, float DispIntensity = 0.01f, float ColorProbability = 0.4f, float ColorIntensity = 0.04f)
        {
            if (targetObject == null) { return; }

            tk2dBaseSprite sprite = null;
            try
            {
                if (!targetObject.GetComponent<tk2dBaseSprite>()) { return; }
                sprite = targetObject.GetComponent<tk2dBaseSprite>();
            }
            catch { }
                ;
            if (sprite == null) { return; }

            if (targetObject.transform != null && targetObject.transform.position.GetAbsoluteRoom() != null)
            {
                if (GameManager.Instance.Dungeon.data.Entrance != null)
                {
                    if (targetObject.transform.position.GetAbsoluteRoom().GetRoomName().StartsWith(GameManager.Instance.Dungeon.data.Entrance.GetRoomName()))
                    {
                        return;
                    }
                }
            }
            if (string.IsNullOrEmpty(targetObject.name)) { return; }
            if (targetObject.name.StartsWith("SellPit")) { return; }
            if (targetObject.name.StartsWith("PitTop")) { return; }
            if (targetObject.name.StartsWith("PitBottom")) { return; }
            if (targetObject.name.StartsWith("NPC_PitDweller")) { return; }
            if (targetObject.name.StartsWith("player")) { return; }
            if (targetObject.name.StartsWith("BossStatuesDummy")) { return; }
            if (targetObject.GetComponentInChildren<BossStatueController>(true) != null | targetObject.GetComponent<BossStatueController>() != null) { return; }
            if (targetObject.GetComponentInChildren<BossStatuesController>(true) != null | targetObject.GetComponent<BossStatuesController>() != null) { return; }
            if (sprite.renderer.material.name.ToLower().StartsWith("glitchmaterial")) { return; }
            if (sprite.renderer.material.name.ToLower().StartsWith("hologrammaterial")) { return; }
            if (sprite.renderer.material.name.ToLower().StartsWith("galaxymaterial")) { return; }
            if (sprite.renderer.material.name.ToLower().StartsWith("spacematerial")) { return; }
            if (sprite.renderer.material.name.ToLower().StartsWith("paradoxmaterial")) { return; }
            if (sprite.renderer.material.name.ToLower().StartsWith("cosmichorrormaterial")) { return; }
            if (sprite.renderer.material.name.ToLower().StartsWith("rainbowmaterial")) { return; }
            if (targetObject.GetComponent<AIActor>() != null)
            {
                AIActor m_AIActor = targetObject.GetComponent<AIActor>();
                if (m_AIActor.GetActorName().StartsWith("Glitched") |
                    m_AIActor.ActorName.ToLower().StartsWith("glitched") | m_AIActor.IsBlackPhantom | m_AIActor.ActorName.StartsWith("Statue")
                   )
                {
                    return;
                }
            }

            ApplyGlitchShader(sprite, true, GlitchInterval, DispProbability, DispIntensity, ColorProbability, ColorIntensity);
        }
        public static void ApplyGlitchShader(tk2dBaseSprite sprite, bool usesOverrideMaterial = true, float GlitchInterval = 0.1f, float DispProbability = 0.4f, float DispIntensity = 0.01f, float ColorProbability = 0.4f, float ColorIntensity = 0.04f)
        {
            try
            {
                if (sprite == null) { return; }
                if (!GlitchShader) { GlitchShader = ShaderCache.Acquire("Brave/Internal/Glitch"); }
                Material m_cachedMaterial = new Material(GlitchShader);
                m_cachedMaterial.name = "GlitchMaterial";
                m_cachedMaterial.SetFloat("_GlitchInterval", GlitchInterval);
                m_cachedMaterial.SetFloat("_DispProbability", DispProbability);
                m_cachedMaterial.SetFloat("_DispIntensity", DispIntensity);
                m_cachedMaterial.SetFloat("_ColorProbability", ColorProbability);
                m_cachedMaterial.SetFloat("_ColorIntensity", ColorIntensity);

                m_cachedMaterial.SetFloat("_WrapDispCoords", 0);

                MeshRenderer spriteComponent = sprite.GetComponent<MeshRenderer>();
                if (spriteComponent == null) { return; }

                Material[] sharedMaterials = spriteComponent.sharedMaterials;
                if (sharedMaterials == null) { return; }
                if (sharedMaterials.Length > 0)
                {
                    foreach (Material material in sharedMaterials)
                    {
                        if (material.name.ToLower().StartsWith("glitchmaterial")) { return; }
                        if (material.name.ToLower().StartsWith("hologrammaterial")) { return; }
                        if (material.name.ToLower().StartsWith("galaxymaterial")) { return; }
                        if (material.name.ToLower().StartsWith("spacematerial")) { return; }
                        if (material.name.ToLower().StartsWith("paradoxmaterial")) { return; }
                        if (material.name.ToLower().StartsWith("cosmichorrormaterial")) { return; }
                        if (material.name.ToLower().StartsWith("rainbowmaterial")) { return; }
                    }
                }
                Array.Resize(ref sharedMaterials, sharedMaterials.Length + 1);
                // Material CustomMaterial = Instantiate(m_cachedGlitchMaterial);
                if (sharedMaterials[0].GetTexture("_MainTex") == null) { return; }
                m_cachedMaterial.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
                sharedMaterials[sharedMaterials.Length - 1] = m_cachedMaterial;
                spriteComponent.sharedMaterials = sharedMaterials;
                sprite.usesOverrideMaterial = usesOverrideMaterial;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                return;
            }
        }

        public static void ApplyGlitchShader(tk2dSprite sprite, float GlitchInterval = 0.1f, float DispProbability = 0.4f, float DispIntensity = 0.01f, float ColorProbability = 0.4f, float ColorIntensity = 0.04f)
        {

            try
            {
                if (sprite == null) { return; }
                // Material m_cachedMaterial = new Material(ShaderCache.Acquire("Brave/Internal/Glitch"));
                if (!GlitchShader) { GlitchShader = ShaderCache.Acquire("Brave/Internal/Glitch"); }
                Material m_cachedMaterial = new Material(GlitchShader);
                m_cachedMaterial.name = "GlitchMaterial";
                m_cachedMaterial.SetFloat("_GlitchInterval", GlitchInterval);
                m_cachedMaterial.SetFloat("_DispProbability", DispProbability);
                m_cachedMaterial.SetFloat("_DispIntensity", DispIntensity);
                m_cachedMaterial.SetFloat("_ColorProbability", ColorProbability);
                m_cachedMaterial.SetFloat("_ColorIntensity", ColorIntensity);
                m_cachedMaterial.SetFloat("_WrapDispCoords", 0);

                MeshRenderer spriteComponent = sprite.GetComponent<MeshRenderer>();
                Material[] sharedMaterials = spriteComponent.sharedMaterials;
                Array.Resize(ref sharedMaterials, sharedMaterials.Length + 1);
                m_cachedMaterial.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
                sharedMaterials[sharedMaterials.Length - 1] = m_cachedMaterial;
                spriteComponent.sharedMaterials = sharedMaterials;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                return;
            }
        }

        public static void ApplyGlitchShader(tk2dSlicedSprite sprite, float GlitchInterval = 0.1f, float DispProbability = 0.4f, float DispIntensity = 0.01f, float ColorProbability = 0.4f, float ColorIntensity = 0.04f)
        {
            try
            {
                if (sprite == null) { return; }
                Material m_cachedMaterial = new Material(ShaderCache.Acquire("Brave/Internal/Glitch"));
                m_cachedMaterial.name = "GlitchMaterial";
                m_cachedMaterial.SetFloat("_GlitchInterval", GlitchInterval);
                m_cachedMaterial.SetFloat("_DispProbability", DispProbability);
                m_cachedMaterial.SetFloat("_DispIntensity", DispIntensity);
                m_cachedMaterial.SetFloat("_ColorProbability", ColorProbability);
                m_cachedMaterial.SetFloat("_ColorIntensity", ColorIntensity);

                MeshRenderer spriteComponent = sprite.GetComponent<MeshRenderer>();
                Material[] sharedMaterials = spriteComponent.sharedMaterials;
                Array.Resize(ref sharedMaterials, sharedMaterials.Length + 1);
                m_cachedMaterial.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
                sharedMaterials[sharedMaterials.Length - 1] = m_cachedMaterial;
                spriteComponent.sharedMaterials = sharedMaterials;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                return;
            }
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

        public static GameObject CreateVFXBundle(string name, IntVector2 Dimensions, tk2dBaseSprite.Anchor anchor, bool usesZHeight, float zHeightOffset, float emissivePower = -1, Color? emissiveColour = null, bool persist = false)
        {
            GameObject Obj = new GameObject(name);
            VFXObject vfObj = new VFXObject();
            Obj.SetActive(false);
            FakePrefab.MarkAsFakePrefab(Obj);
            UnityEngine.Object.DontDestroyOnLoad(Obj);

            tk2dSpriteCollectionData VFXSpriteCollection = Initialisation.VFXCollection;
            tk2dSprite sprite = Obj.GetOrAddComponent<tk2dSprite>();
            tk2dSpriteAnimator animator = Obj.GetOrAddComponent<tk2dSpriteAnimator>();
            tk2dSpriteAnimation animation = Initialisation.vfxAnimationCollection;
            animator.Library = animation;
            sprite.collection = VFXSpriteCollection;

            Vector3[] colliderVertices = new Vector3[]{
                      new Vector3(0f, 0f, 0f),
                      new Vector3((Dimensions.x / 16), (Dimensions.y / 16), 0f)
                  };
            tk2dSpriteAnimationClip clip = animator.GetClipByName(name);

            List<tk2dSpriteDefinition> frames = new List<tk2dSpriteDefinition>();
            foreach (tk2dSpriteAnimationFrame frame in clip.frames) { frames.Add(VFXSpriteCollection.spriteDefinitions[frame.spriteId]); }

            foreach (tk2dSpriteDefinition frameDef in frames)
            {
                frameDef.ConstructOffsetsFromAnchor(anchor);
                frameDef.colliderVertices = colliderVertices;
                if (emissivePower > 0) frameDef.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
                if (emissivePower > 0) frameDef.material.SetFloat("_EmissiveColorPower", emissivePower);
                if (emissiveColour != null) frameDef.material.SetColor("_EmissiveColor", (Color)emissiveColour);
                if (emissivePower > 0) frameDef.materialInst.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
                if (emissivePower > 0) frameDef.materialInst.SetFloat("_EmissiveColorPower", emissivePower);
                if (emissiveColour != null) frameDef.materialInst.SetColor("_EmissiveColor", (Color)emissiveColour);
            }

            if (emissivePower > 0) sprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
            if (emissivePower > 0) sprite.renderer.material.SetFloat("_EmissiveColorPower", emissivePower);
            if (emissiveColour != null) sprite.renderer.material.SetColor("_EmissiveColor", (Color)emissiveColour);
            if (!persist)
            {
                SpriteAnimatorKiller kill = animator.gameObject.AddComponent<SpriteAnimatorKiller>();
                kill.fadeTime = -1f;
                kill.animator = animator;
                kill.delayDestructionTime = -1f;
            }
            animator.playAutomatically = true;
            animator.DefaultClipId = animator.GetClipIdByName(name);
            vfObj.attached = true;
            vfObj.persistsOnDeath = false;
            vfObj.usesZHeight = usesZHeight;
            vfObj.zHeight = zHeightOffset;
            vfObj.alignment = VFXAlignment.NormalAligned;
            vfObj.destructible = false;
            vfObj.effect = Obj;
            return Obj;
        }

        public static GameObject CreateVFXBundle(string name, bool usesZHeight, float zHeightOffset, float emissivePower = -1, float emissiveColourPower = -1, Color? emissiveColour = null, bool persist = false, tk2dSpriteCollectionData overrideCollection = null, tk2dSpriteAnimation overrideAnimationCollection = null)
        {
            GameObject Obj = new GameObject(name);
            VFXObject vfObj = new VFXObject();
            Obj.SetActive(false);
            FakePrefab.MarkAsFakePrefab(Obj);
            UnityEngine.Object.DontDestroyOnLoad(Obj);

            tk2dSpriteCollectionData VFXSpriteCollection = overrideCollection != null ? overrideCollection : Initialisation.VFXCollection;
            tk2dSprite sprite = Obj.GetOrAddComponent<tk2dSprite>();
            if (usesZHeight)
            {
                sprite.HeightOffGround = zHeightOffset;
            }
            tk2dSpriteAnimator animator = Obj.GetOrAddComponent<tk2dSpriteAnimator>();
            tk2dSpriteAnimation animation = overrideAnimationCollection != null ? overrideAnimationCollection : Initialisation.vfxAnimationCollection;
            animator.Library = animation;
            sprite.collection = VFXSpriteCollection;

            tk2dSpriteAnimationClip clip = animator.GetClipByName(name);

            List<tk2dSpriteDefinition> frames = new List<tk2dSpriteDefinition>();
            foreach (tk2dSpriteAnimationFrame frame in clip.frames) { frames.Add(VFXSpriteCollection.spriteDefinitions[frame.spriteId]); }

            sprite.usesOverrideMaterial = true;
            sprite.renderer.material.shader = ShaderCache.Acquire("tk2d/CutoutVertexColorTintableTilted");

            if (emissivePower > 0)
            {

                Material mat = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);
                mat.mainTexture = sprite.renderer.material.mainTexture;
                mat.SetColor("_EmissiveColor", (Color)emissiveColour); //RGB value of the color you want glowing
                mat.SetFloat("_EmissiveColorPower", emissiveColourPower); // no idea tbh
                mat.SetFloat("_EmissivePower", emissivePower); //brightness
                sprite.renderer.material = mat;
            }

            if (!persist)
            {
                SpriteAnimatorKiller kill = animator.gameObject.AddComponent<SpriteAnimatorKiller>();
                kill.fadeTime = -1f;
                kill.animator = animator;
                kill.delayDestructionTime = -1f;
            }
            animator.playAutomatically = true;
            animator.DefaultClipId = animator.GetClipIdByName(name);
            vfObj.attached = false;
            vfObj.persistsOnDeath = false;
            vfObj.usesZHeight = usesZHeight;
            vfObj.zHeight = zHeightOffset;
            vfObj.alignment = VFXAlignment.NormalAligned;
            vfObj.destructible = false;
            vfObj.effect = Obj;
            vfObj.orphaned = true;
            return Obj;
        }

        public static VFXPool CreateVFXPoolBundle(string name, bool usesZHeight, float zHeightOffset, VFXAlignment alignment = VFXAlignment.NormalAligned, float emissivePower = -1, Color? emissiveColour = null, bool persist = false, bool orphaned = false)
        {
            GameObject Obj = new GameObject(name);
            VFXPool pool = new VFXPool();
            pool.type = VFXPoolType.All;
            VFXComplex complex = new VFXComplex();
            VFXObject vfObj = new VFXObject();
            Obj.SetActive(false);
            FakePrefab.MarkAsFakePrefab(Obj);
            UnityEngine.Object.DontDestroyOnLoad(Obj);

            tk2dSpriteCollectionData VFXSpriteCollection = Initialisation.VFXCollection;
            tk2dSprite sprite = Obj.GetOrAddComponent<tk2dSprite>();
            tk2dSpriteAnimator animator = Obj.GetOrAddComponent<tk2dSpriteAnimator>();
            tk2dSpriteAnimation animation = Initialisation.vfxAnimationCollection;
            animator.Library = animation;

            if (emissivePower > 0)
            {
                sprite.usesOverrideMaterial = true;

                Material mat = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);
                mat.mainTexture = sprite.renderer.material.mainTexture;
                mat.SetColor("_EmissiveColor", (Color)emissiveColour); //RGB value of the color you want glowing
                mat.SetFloat("_EmissiveColorPower", emissivePower); // no idea tbh
                mat.SetFloat("_EmissivePower", emissivePower); //brightness
                sprite.renderer.material = mat;
            }
            if (!persist)
            {
                SpriteAnimatorKiller kill = animator.gameObject.AddComponent<SpriteAnimatorKiller>();
                kill.fadeTime = -1f;
                kill.animator = animator;
                kill.delayDestructionTime = -1f;
            }
            animator.playAutomatically = true;
            animator.DefaultClipId = animator.GetClipIdByName(name);
            vfObj.attached = true;
            vfObj.persistsOnDeath = persist;
            vfObj.usesZHeight = usesZHeight;
            vfObj.zHeight = zHeightOffset;
            vfObj.alignment = alignment;
            vfObj.destructible = false;
            vfObj.effect = Obj;
            vfObj.orphaned = orphaned;
            complex.effects = new VFXObject[] { vfObj };
            pool.effects = new VFXComplex[] { complex };
            return pool;
        }

        public static GameObject CreateVFX(string name, List<string> spritePaths, int fps, IntVector2 Dimensions, tk2dBaseSprite.Anchor anchor, bool usesZHeight, float zHeightOffset, float emissivePower = -1, Color? emissiveColour = null, tk2dSpriteAnimationClip.WrapMode wrap = tk2dSpriteAnimationClip.WrapMode.Once, bool persist = false, int loopStart = 0)
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
            clip.wrapMode = wrap;
            clip.loopStart = loopStart;
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
        public static VFXComplex CreateBlankVFXComplex()
        {
            return new VFXComplex() { effects = new VFXObject[] { } };
        }
        public static VFXPool CreateBlankVFXPool()
        {
            return new VFXPool { type = VFXPoolType.None, effects = new VFXComplex[] { new VFXComplex() { effects = new VFXObject[] { new VFXObject() { } } } } };
        }
        public static VFXPool CreateBlankVFXPool(GameObject effect, bool isDebris = false, bool attached = true)
        {

            if (isDebris)
            {
                return new VFXPool
                {
                    type = VFXPoolType.Single,
                    effects = new VFXComplex[] { new VFXComplex() { effects = new VFXObject[] {
                    new VFXObject() {
                    effect = effect,
                    alignment = VFXAlignment.Fixed,
                     attached = false,
                     destructible = false,
                     orphaned = true,
                     persistsOnDeath = false,
                    usesZHeight = false,
                    zHeight = 0,
            } } } }
                };
            }
            return new VFXPool { type = VFXPoolType.Single, effects = new VFXComplex[] { new VFXComplex() { effects = new VFXObject[] { new VFXObject() { effect = effect, attached = attached } } } } };
        }
        public static VFXPool CreateVFXPool(string name, List<string> spritePaths, int fps, IntVector2 Dimensions, tk2dBaseSprite.Anchor anchor, bool usesZHeight, float zHeightOffset, bool persist = false, VFXAlignment alignment = VFXAlignment.NormalAligned, float emissivePower = -1, Color? emissiveColour = null, tk2dSpriteAnimationClip.WrapMode wrapmode = tk2dSpriteAnimationClip.WrapMode.Once, int loopStart = 0)
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
            clip.wrapMode = wrapmode;
            clip.loopStart = loopStart;
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
    }
    public class TiledSpriteConnector : MonoBehaviour
    {
        private void Start()
        {
            tiledSprite = base.GetComponent<tk2dTiledSprite>();
        }
        private void Update()
        {
            if (sourceRigidbody)
            {
                Vector2 unitCenter = sourceRigidbody.UnitCenter;
                Vector2 unitCenter2 = Vector2.zero;
                if (usesVector && targetVector != Vector2.zero) unitCenter2 = targetVector;
                else if (targetRigidbody) unitCenter2 = targetRigidbody.UnitCenter;
                if (unitCenter2 != Vector2.zero)
                {
                    tiledSprite.transform.position = unitCenter;
                    Vector2 vector = unitCenter2 - unitCenter;
                    float num = BraveMathCollege.Atan2Degrees(vector.normalized);
                    int num2 = Mathf.RoundToInt(vector.magnitude / 0.0625f);
                    tiledSprite.dimensions = new Vector2((float)num2, tiledSprite.dimensions.y);
                    tiledSprite.transform.rotation = Quaternion.Euler(0f, 0f, num);
                    tiledSprite.UpdateZDepth();

                }
                else
                {
                    if (eraseSpriteIfTargetOrSourceNull) UnityEngine.Object.Destroy(tiledSprite.gameObject);
                    else if (eraseComponentIfTargetOrSourceNull) UnityEngine.Object.Destroy(this);
                }
            }
            else
            {
                if (eraseSpriteIfTargetOrSourceNull) UnityEngine.Object.Destroy(tiledSprite.gameObject);
                else if (eraseComponentIfTargetOrSourceNull) UnityEngine.Object.Destroy(this);
            }
        }

        public SpeculativeRigidbody sourceRigidbody;
        public SpeculativeRigidbody targetRigidbody;
        public Vector2 targetVector;
        public bool usesVector;
        public bool eraseSpriteIfTargetOrSourceNull;
        public bool eraseComponentIfTargetOrSourceNull;
        private tk2dTiledSprite tiledSprite;
    }
}
