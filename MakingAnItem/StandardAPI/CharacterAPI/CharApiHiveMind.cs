using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace CustomCharacters
{
    //WARNING DO NOT MODIFY THIS CLASS IT CAN CAUSE THE WHOLE API TO BREAK. Thanks :)

    //i know hivemind is one word and so it *should* be Hivemind but i like how HiveMind looks more
    class CharApiHiveMind : MonoBehaviour
    {
        public static void Init(string prefix)
        {
			modPrefix = prefix;
            if (ETGModMainBehaviour.Instance?.gameObject != null)
            {
                bool foundIt = false;
                //foreach (Component component in ETGModMainBehaviour.Instance.gameObject.GetComponents<Component>())
                if (ETGModMainBehaviour.Instance.gameObject.GetComponent("CharApiHiveMind") != null)
                {
                    //if (component.GetType().ToString().ToLower().Contains("charapihivemind"))
                    //{
                        var component = ETGModMainBehaviour.Instance.gameObject.GetComponent("CharApiHiveMind");

                        foundIt = true;

                        var _versionInternal = (float)ReflectionHelper.GetValue(component.GetType().GetField("versionInternal"), component);
                        var _modPrefix = (string)ReflectionHelper.GetValue(component.GetType().GetField("modPrefix"), component);
                        
                        if (version != _versionInternal)
                        {
                        Debug.LogWarning($"CharApi ({prefix}) - ({_modPrefix}): Warning! this mod's charapi version ({version}) dose not match the main CharApiHiveMind version ({_versionInternal})");
                        }
                        //this.trueHiveMindVersion = _trueHiveMindVersion;
                        Debug.Log($"CharApi ({prefix}): Hivemind has been found!");
                    //}
                }

                if (!foundIt)
                {
                    var hivemind = ETGModMainBehaviour.Instance?.gameObject.AddComponent<CharApiHiveMind>();
                    hivemind.modPrefixInternal = prefix;
                    hivemind.versionInternal = version;
                    Debug.Log($"CharApi ({prefix}): No Hivemind found so we're creating one");

                }
            }
        }



        public static bool AddNewCharacter(string prefix, PlayableCharacters character)
        {
            bool status = false;
            if (ETGModMainBehaviour.Instance?.gameObject != null)
            {
                foreach (Component component in ETGModMainBehaviour.Instance.gameObject.GetComponents<Component>())
                {
                    if (component.GetType().ToString().ToLower().Contains("charapihivemind"))
                    {                        
                        var _characters = (Dictionary<PlayableCharacters, string>)ReflectionHelper.GetValue(component.GetType().GetField("characters"), component);
                        var _modPrefix = (string)ReflectionHelper.GetValue(component.GetType().GetField("modPrefix"), component);


                        if (_characters.ContainsKey(character))
                        {
                            Debug.LogError($"CharApi ({prefix}): Warning! two characters have the same id ({(int)character})! this is very very bad please inform {prefix}/{_characters[character]}");
                        }
                        else
                        {
                            _characters.Add(character, prefix);
                            status = true;
                        }
                    }
                }
            }
            return status;
        }

        //here for backwards compatiblity
        private static IEnumerator Open(ArkController self, PlayerController interactor)
        {
            for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
            {
                if (GameManager.Instance.AllPlayers[i].healthHaver.IsAlive)
                {
                    GameManager.Instance.AllPlayers[i].SetInputOverride("ark");
                }
            }
            self.LidAnimator.Play();
            self.ChestAnimator.Play();
            self.PoofAnimator.PlayAndDisableObject(string.Empty, null);
            self.specRigidbody.Reinitialize();
            GameManager.Instance.MainCameraController.OverrideRecoverySpeed = 2f;
            GameManager.Instance.MainCameraController.OverridePosition = self.ChestAnimator.sprite.WorldCenter + new Vector2(0f, 2f);
            GameManager.Instance.MainCameraController.SetManualControl(true, true);
            self.StartCoroutine(typeof(ArkController).GetMethod("HandleLightSprite", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, null) as IEnumerator);
            while (self.LidAnimator.IsPlaying(self.LidAnimator.CurrentClip))
            {
                yield return null;
            }
            yield return self.StartCoroutine(typeof(ArkController).GetMethod("HandleGun", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { interactor }) as IEnumerator);
            yield return new WaitForSeconds(0.5f);
            Pixelator.Instance.DoFinalNonFadedLayer = true;
            yield return self.StartCoroutine((IEnumerator)typeof(ArkController).GetMethod("HandleClockhair", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { interactor }));
            interactor.ClearInputOverride("ark");
            yield break;
        }

        public static float version = 1.3f;

        public static string modPrefix;
        public string modPrefixInternal;
        public float versionInternal;
        public Dictionary<PlayableCharacters, string> characters = new Dictionary<PlayableCharacters, string>();
    }

	
}
