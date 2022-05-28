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

	//i know hivemind is one word and so it *should* he Hivemind but i like how HiveMind looks more
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
						ETGModConsole.Log($"CharApi ({prefix}) - ({_modPrefix}): Warning! this mod's charapi version ({version}) dose not match the main CharApiHiveMind version ({_versionInternal})");
					}
					//this.trueHiveMindVersion = _trueHiveMindVersion;
					ETGModConsole.Log($"CharApi ({prefix}): Hivemind has been found!");
					//}
				}

				if (!foundIt)
				{
					var hivemind = ETGModMainBehaviour.Instance?.gameObject.AddComponent<CharApiHiveMind>();
					hivemind.modPrefixInternal = prefix;
					hivemind.versionInternal = version;
					ETGModConsole.Log($"CharApi ({prefix}): No Hivemind found so we're creating one");

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
							ETGModConsole.Log($"CharApi ({prefix}): Warning! two characters have the same id ({(int)character})! this is very very bad please inform {prefix}/{_characters[character]}");
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
			yield return self.StartCoroutine(HandleClockhair(self, interactor));
			interactor.ClearInputOverride("ark");
			yield break;
		}

		private static IEnumerator HandleClockhair(ArkController self, PlayerController interactor)
		{

			FieldInfo _heldPastGun = typeof(ArkController).GetField("m_heldPastGun", BindingFlags.NonPublic | BindingFlags.Instance);

			Transform clockhairTransform = ((GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Clockhair", ".prefab"))).transform;
			ClockhairController clockhair = clockhairTransform.GetComponent<ClockhairController>();
			float elapsed = 0f;
			float duration = clockhair.ClockhairInDuration;
			Vector2 clockhairTargetPosition = interactor.CenterPosition;
			Vector2 clockhairStartPosition = clockhairTargetPosition + new Vector2(-20f, 5f);
			clockhair.renderer.enabled = true;
			clockhair.spriteAnimator.alwaysUpdateOffscreen = true;
			clockhair.spriteAnimator.Play("clockhair_intro");
			clockhair.hourAnimator.Play("hour_hand_intro");
			clockhair.minuteAnimator.Play("minute_hand_intro");
			clockhair.secondAnimator.Play("second_hand_intro");
			BraveInput currentInput = BraveInput.GetInstanceForPlayer(interactor.PlayerIDX);
			while (elapsed < duration)
			{
				typeof(ArkController).GetMethod("UpdateCameraPositionDuringClockhair", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { interactor.CenterPosition });


				if (GameManager.INVARIANT_DELTA_TIME == 0f)
				{
					elapsed += 0.05f;
				}
				elapsed += GameManager.INVARIANT_DELTA_TIME;
				float t = elapsed / duration;
				float smoothT = Mathf.SmoothStep(0f, 1f, t);
				if (currentInput == null)
				{
					ETGModConsole.Log("currentInput null");
				}

				if (clockhairTargetPosition == null)
				{
					ETGModConsole.Log("clockhairTargetPosition null");
				}
				clockhairTargetPosition = (Vector2)typeof(ArkController).GetMethod("GetTargetClockhairPosition", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { currentInput, clockhairTargetPosition });
				//clockhairTargetPosition = self.GetTargetClockhairPosition(currentInput, clockhairTargetPosition);
				Vector3 currentPosition = Vector3.Slerp(clockhairStartPosition, clockhairTargetPosition, smoothT);
				clockhairTransform.position = currentPosition.WithZ(0f);
				if (t > 0.5f)
				{
					clockhair.renderer.enabled = true;
				}
				if (t > 0.75f)
				{
					clockhair.hourAnimator.GetComponent<Renderer>().enabled = true;
					clockhair.minuteAnimator.GetComponent<Renderer>().enabled = true;
					clockhair.secondAnimator.GetComponent<Renderer>().enabled = true;
					GameCursorController.CursorOverride.SetOverride("ark", true, null);
				}
				clockhair.sprite.UpdateZDepth();
				typeof(ArkController).GetMethod("PointGunAtClockhair", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { interactor, clockhairTransform });
				yield return null;
			}
			clockhair.SetMotionType(1f);
			float shotTargetTime = 0f;
			float holdDuration = 4f;
			PlayerController shotPlayer = null;
			bool didShootHellTrigger = false;
			Vector3 lastJitterAmount = Vector3.zero;
			bool m_isPlayingChargeAudio = false;
			for (; ; )
			{
				typeof(ArkController).GetMethod("UpdateCameraPositionDuringClockhair", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { interactor.CenterPosition });
				clockhair.transform.position = clockhair.transform.position - lastJitterAmount;
				clockhair.transform.position = (Vector2)typeof(ArkController).GetMethod("GetTargetClockhairPosition", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { currentInput, clockhair.transform.position.XY() });
				clockhair.sprite.UpdateZDepth();
				bool isTargetingValidTarget = (bool)typeof(ArkController).GetMethod("CheckPlayerTarget", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { GameManager.Instance.PrimaryPlayer, clockhairTransform });
				shotPlayer = GameManager.Instance.PrimaryPlayer;
				if (!isTargetingValidTarget && GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
				{
					isTargetingValidTarget = (bool)typeof(ArkController).GetMethod("CheckPlayerTarget", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { GameManager.Instance.SecondaryPlayer, clockhairTransform });
					shotPlayer = GameManager.Instance.SecondaryPlayer;
				}
				if (!isTargetingValidTarget && GameStatsManager.Instance.AllCorePastsBeaten())
				{
					isTargetingValidTarget = (bool)typeof(ArkController).GetMethod("CheckHellTarget", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { self.HellCrackSprite, clockhairTransform });
					didShootHellTrigger = isTargetingValidTarget;
				}
				if (isTargetingValidTarget)
				{
					clockhair.SetMotionType(-10f);
				}
				else
				{
					clockhair.SetMotionType(1f);
				}
				if ((currentInput.ActiveActions.ShootAction.IsPressed || currentInput.ActiveActions.InteractAction.IsPressed) && isTargetingValidTarget)
				{
					if (!m_isPlayingChargeAudio)
					{
						m_isPlayingChargeAudio = true;
						AkSoundEngine.PostEvent("Play_OBJ_pastkiller_charge_01", self.gameObject);
					}
					shotTargetTime += BraveTime.DeltaTime;
				}
				else
				{
					shotTargetTime = Mathf.Max(0f, shotTargetTime - BraveTime.DeltaTime * 3f);
					if (m_isPlayingChargeAudio)
					{
						m_isPlayingChargeAudio = false;
						AkSoundEngine.PostEvent("Stop_OBJ_pastkiller_charge_01", self.gameObject);
					}
				}
				if ((currentInput.ActiveActions.ShootAction.WasReleased || currentInput.ActiveActions.InteractAction.WasReleased) && isTargetingValidTarget && shotTargetTime > holdDuration && !GameManager.Instance.IsPaused)
				{
					break;
				}
				if (shotTargetTime > 0f)
				{
					float distortionPower = Mathf.Lerp(0f, 0.35f, shotTargetTime / holdDuration);
					float distortRadius = 0.5f;
					float edgeRadius = Mathf.Lerp(4f, 7f, shotTargetTime / holdDuration);
					clockhair.UpdateDistortion(distortionPower, distortRadius, edgeRadius);
					float desatRadiusUV = Mathf.Lerp(2f, 0.25f, shotTargetTime / holdDuration);
					clockhair.UpdateDesat(true, desatRadiusUV);
					shotTargetTime = Mathf.Min(holdDuration + 0.25f, shotTargetTime + BraveTime.DeltaTime);
					float d = Mathf.Lerp(0f, 0.5f, (shotTargetTime - 1f) / (holdDuration - 1f));
					Vector3 vector = (UnityEngine.Random.insideUnitCircle * d).ToVector3ZUp(0f);
					BraveInput.DoSustainedScreenShakeVibration(shotTargetTime / holdDuration * 0.8f);
					clockhair.transform.position = clockhair.transform.position + vector;
					lastJitterAmount = vector;
					clockhair.SetMotionType(Mathf.Lerp(-10f, -2400f, shotTargetTime / holdDuration));
				}
				else
				{
					lastJitterAmount = Vector3.zero;
					clockhair.UpdateDistortion(0f, 0f, 0f);
					clockhair.UpdateDesat(false, 0f);
					shotTargetTime = 0f;
					BraveInput.DoSustainedScreenShakeVibration(0f);
				}
				typeof(ArkController).GetMethod("PointGunAtClockhair", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { interactor, clockhairTransform });
				yield return null;
			}
			BraveInput.DoSustainedScreenShakeVibration(0f);
			BraveInput.DoVibrationForAllPlayers(Vibration.Time.Normal, Vibration.Strength.Hard);
			clockhair.StartCoroutine(clockhair.WipeoutDistortionAndFade(0.5f));
			clockhair.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Unoccluded"));
			Pixelator.Instance.FadeToColor(1f, Color.white, true, 0.2f);
			Pixelator.Instance.DoRenderGBuffer = false;
			clockhair.spriteAnimator.Play("clockhair_fire");
			clockhair.hourAnimator.GetComponent<Renderer>().enabled = false;
			clockhair.minuteAnimator.GetComponent<Renderer>().enabled = false;
			clockhair.secondAnimator.GetComponent<Renderer>().enabled = false;
			yield return null;
			TimeTubeCreditsController ttcc = new TimeTubeCreditsController();
			bool isShortTunnel = didShootHellTrigger || shotPlayer.characterIdentity == PlayableCharacters.CoopCultist || (bool)typeof(ArkController).GetMethod("CharacterStoryComplete", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { shotPlayer.characterIdentity });
			UnityEngine.Object.Destroy((_heldPastGun.GetValue(self) as Transform).gameObject);
			interactor.ToggleGunRenderers(true, "ark");
			GameCursorController.CursorOverride.RemoveOverride("ark");
			Pixelator.Instance.LerpToLetterbox(0.35f, 0.25f);
			yield return self.StartCoroutine(ttcc.HandleTimeTubeCredits(clockhair.sprite.WorldCenter, isShortTunnel, clockhair.spriteAnimator, (!didShootHellTrigger) ? shotPlayer.PlayerIDX : 0, false));
			if (isShortTunnel)
			{
				Pixelator.Instance.FadeToBlack(1f, false, 0f);
				yield return new WaitForSeconds(1f);
			}
			if (didShootHellTrigger)
			{
				GameManager.DoMidgameSave(GlobalDungeonData.ValidTilesets.HELLGEON);
				GameManager.Instance.LoadCustomLevel("tt_bullethell");
			}
			else if (shotPlayer.characterIdentity == PlayableCharacters.CoopCultist)
			{
				GameManager.IsCoopPast = true;
				typeof(ArkController).GetMethod("ResetPlayers", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { false });

				GameManager.Instance.LoadCustomLevel("fs_coop");
			}
			else if ((bool)typeof(ArkController).GetMethod("CharacterStoryComplete", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { shotPlayer.characterIdentity }) && shotPlayer.characterIdentity == PlayableCharacters.Gunslinger)
			{
				GameManager.DoMidgameSave(GlobalDungeonData.ValidTilesets.FINALGEON);
				GameManager.IsGunslingerPast = true;
				typeof(ArkController).GetMethod("ResetPlayers", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { true });
				GameManager.Instance.LoadCustomLevel("tt_bullethell");
			}
			else if ((bool)typeof(ArkController).GetMethod("CharacterStoryComplete", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { shotPlayer.characterIdentity }))
			{
				bool flag = false;
				GameManager.DoMidgameSave(GlobalDungeonData.ValidTilesets.FINALGEON);
				if (shotPlayer.GetComponent<CustomCharacter>() != null)
				{
					flag = true;
					typeof(ArkController).GetMethod("ResetPlayers", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { false });
					GameManager.Instance.LoadCustomLevel(shotPlayer.GetComponent<CustomCharacter>().past);
				}
				else
				{
					switch (shotPlayer.characterIdentity)
					{
						case PlayableCharacters.Pilot:
							flag = true;
							typeof(ArkController).GetMethod("ResetPlayers", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { false });
							GameManager.Instance.LoadCustomLevel("fs_pilot");
							break;
						case PlayableCharacters.Convict:
							flag = true;
							typeof(ArkController).GetMethod("ResetPlayers", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { false });
							GameManager.Instance.LoadCustomLevel("fs_convict");
							break;
						case PlayableCharacters.Robot:
							flag = true;
							typeof(ArkController).GetMethod("ResetPlayers", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { false });
							GameManager.Instance.LoadCustomLevel("fs_robot");
							break;
						case PlayableCharacters.Soldier:
							flag = true;
							typeof(ArkController).GetMethod("ResetPlayers", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { false });
							GameManager.Instance.LoadCustomLevel("fs_soldier");
							break;
						case PlayableCharacters.Guide:
							flag = true;
							typeof(ArkController).GetMethod("ResetPlayers", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { false });
							GameManager.Instance.LoadCustomLevel("fs_guide");
							break;
						case PlayableCharacters.Bullet:
							flag = true;
							typeof(ArkController).GetMethod("ResetPlayers", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { false });
							GameManager.Instance.LoadCustomLevel("fs_bullet");
							break;
					}
				}
				if (!flag)
				{
					AmmonomiconController.Instance.OpenAmmonomicon(true, true);
				}
				else
				{
					GameUIRoot.Instance.ToggleUICamera(false);
				}
			}
			else
			{
				AmmonomiconController.Instance.OpenAmmonomicon(true, true);
			}
			for (; ; )
			{
				yield return null;
			}
			//yield break;
		}

		public static float version = 1.2f;

		public static string modPrefix;
		public string modPrefixInternal;
		public float versionInternal;
		public Dictionary<PlayableCharacters, string> characters = new Dictionary<PlayableCharacters, string>();
	}


}