using System;
using System.Collections.Generic;
using UnityEngine;

namespace GungeonAPI
{
	// Token: 0x0200000C RID: 12
	public abstract class SimpleInteractable : BraveBehaviour
	{
		// Token: 0x0400002E RID: 46
		public Action<PlayerController, GameObject> OnAccept;

		// Token: 0x0400002F RID: 47
		public Action<PlayerController, GameObject> OnDecline;

		// Token: 0x04000030 RID: 48
		public List<string> conversation;
        public List<string> conversation2;

        // Token: 0x04000031 RID: 49
        public Func<PlayerController, GameObject, bool> CanUse;

		// Token: 0x04000032 RID: 50
		public Transform talkPoint;

		// Token: 0x04000033 RID: 51
		public string text;

		// Token: 0x04000034 RID: 52
		public string acceptText;
        public string acceptText2;

        // Token: 0x04000035 RID: 53
        public string declineText;
        public string declineText2;

        // Token: 0x04000036 RID: 54
        public bool isToggle;

		// Token: 0x04000037 RID: 55
		protected bool m_isToggled;

		// Token: 0x04000038 RID: 56
		protected bool m_canUse = true;
	}
}
