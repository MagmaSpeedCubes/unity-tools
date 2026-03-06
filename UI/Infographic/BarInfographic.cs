using UnityEngine;

namespace MagmaLabs.UI{
    public class BarInfographic : InfographicBase{
        [SerializeField]protected Transform barScale;
        public override void Refresh()
        {
            barScale.localScale = new Vector3(GetPercentage(), 1, 1);
        }

    }
}
