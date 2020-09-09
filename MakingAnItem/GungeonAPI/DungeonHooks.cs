using System;
using System.Diagnostics;
using System.Reflection;
using Dungeonator;
using MonoMod.RuntimeDetour;


namespace GungeonAPI
{
	// Token: 0x02000004 RID: 4
	public static class DungeonHooks
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000017 RID: 23 RVA: 0x00002F10 File Offset: 0x00001110
		// (remove) Token: 0x06000018 RID: 24 RVA: 0x00002F44 File Offset: 0x00001144
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<LoopDungeonGenerator, Dungeon, DungeonFlow, int> OnPreDungeonGeneration;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000019 RID: 25 RVA: 0x00002F78 File Offset: 0x00001178
		// (remove) Token: 0x0600001A RID: 26 RVA: 0x00002FAC File Offset: 0x000011AC
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action OnPostDungeonGeneration;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600001B RID: 27 RVA: 0x00002FE0 File Offset: 0x000011E0
		// (remove) Token: 0x0600001C RID: 28 RVA: 0x00003014 File Offset: 0x00001214
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action OnFoyerAwake;

		// Token: 0x0600001D RID: 29 RVA: 0x00003047 File Offset: 0x00001247
		public static void FoyerAwake(Action<MainMenuFoyerController> orig, MainMenuFoyerController self)
		{
			orig(self);
			Action onFoyerAwake = DungeonHooks.OnFoyerAwake;
			if (onFoyerAwake != null)
			{
				onFoyerAwake();
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00003064 File Offset: 0x00001264
		public static void LoopGenConstructor(Action<LoopDungeonGenerator, Dungeon, int> orig, LoopDungeonGenerator self, Dungeon dungeon, int dungeonSeed)
		{
			Tools.Print<string>("-Loop Gen Called-", "5599FF", false);
			orig(self, dungeon, dungeonSeed);
			bool flag = GameManager.Instance != null && GameManager.Instance != DungeonHooks.targetInstance;
			if (flag)
			{
				DungeonHooks.targetInstance = GameManager.Instance;
				DungeonHooks.targetInstance.OnNewLevelFullyLoaded += DungeonHooks.OnLevelLoad;
			}
			DungeonFlow arg = (DungeonFlow)DungeonHooks.m_assignedFlow.GetValue(self);
			Action<LoopDungeonGenerator, Dungeon, DungeonFlow, int> onPreDungeonGeneration = DungeonHooks.OnPreDungeonGeneration;
			if (onPreDungeonGeneration != null)
			{
				onPreDungeonGeneration(self, dungeon, arg, dungeonSeed);
			}
			dungeon = null;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000030FC File Offset: 0x000012FC
		public static void OnLevelLoad()
		{
			Tools.Print<string>("-Post Gen Called-", "5599FF", false);
			Action onPostDungeonGeneration = DungeonHooks.OnPostDungeonGeneration;
			if (onPostDungeonGeneration != null)
			{
				onPostDungeonGeneration();
			}
		}

		// Token: 0x0400000A RID: 10
		private static GameManager targetInstance;

		// Token: 0x0400000B RID: 11
		public static FieldInfo m_assignedFlow = typeof(LoopDungeonGenerator).GetField("m_assignedFlow", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x0400000C RID: 12
		private static Hook preDungeonGenHook = new Hook(typeof(LoopDungeonGenerator).GetConstructor(new Type[]
		{
			typeof(Dungeon),
			typeof(int)
		}), typeof(DungeonHooks).GetMethod("LoopGenConstructor"));

		// Token: 0x0400000D RID: 13
		private static Hook foyerAwakeHook = new Hook(typeof(MainMenuFoyerController).GetMethod("Awake", BindingFlags.Instance | BindingFlags.NonPublic), typeof(DungeonHooks).GetMethod("FoyerAwake"));
	}
}
