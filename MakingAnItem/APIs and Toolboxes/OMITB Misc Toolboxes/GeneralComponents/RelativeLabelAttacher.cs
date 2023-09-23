using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsItems
{
    class RelativeLabelAttacher : MonoBehaviour
    {
        public RelativeLabelAttacher()
        {
            labelValue = "Lorem Ipsum";
            colour = Color.red;
            offset = Vector2.zero;
            centered = false;
        }
        private void Start()
        {
            self = base.gameObject;
            GameObject labelObj = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("DamagePopupLabel", ".prefab"), GameUIRoot.Instance.transform);
            label = labelObj.GetComponent<dfLabel>();
            label.gameObject.SetActive(true);
            label.Text = labelValue;
            label.Color = colour;
            label.Opacity = 1;
            label.TextAlignment = TextAlignment.Center;

            extantLabel = labelObj;
            extantLabel.transform.position = self.transform.position + offset;
            Vector2 midSet = extantLabel.transform.position.QuantizeFloor(extantLabel.GetComponent<dfLabel>().PixelsToUnits() / (Pixelator.Instance.ScaleTileScale / Pixelator.Instance.CurrentTileScale));
            extantLabel.transform.position = dfFollowObject.ConvertWorldSpaces(midSet, GameManager.Instance.MainCameraController.Camera, GameUIRoot.Instance.Manager.RenderCamera).WithZ(0f);    
        }
        private void OnDestroy()
        {
            if (extantLabel)
            {
                extantLabel.gameObject.SetActive(false);
                UnityEngine.Object.Destroy(extantLabel);
                extantLabel = null;
            }
        }
        private void Update()
        {
            if (extantLabel && self)
            {
                Vector3 position = self.transform.position + offset;
                if (centered) { position.x -= (label.Width * 0.02f) * 0.5f; }
                extantLabel.transform.position = position;


                Vector2 midSet = extantLabel.transform.position.QuantizeFloor(extantLabel.GetComponent<dfLabel>().PixelsToUnits() / (Pixelator.Instance.ScaleTileScale / Pixelator.Instance.CurrentTileScale));
                extantLabel.transform.position = dfFollowObject.ConvertWorldSpaces(midSet, GameManager.Instance.MainCameraController.Camera, GameUIRoot.Instance.Manager.RenderCamera).WithZ(0f);
                
                label.text = labelValue;
            }
        }
        public bool centered;
        private dfLabel label;
        private GameObject self;
        private GameObject extantLabel;
        public string labelValue;
        public Color colour;
        public Vector3 offset;
    }
}
