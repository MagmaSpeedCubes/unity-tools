using System;

using UnityEngine;

using MagmaLabs.Utilities;

namespace MagmaLabs.UI{
    public interface IInfographic
    {
        void SetValue(float value);
        void SetRange(float min, float max);        
        void SetLabel(string label);                
        void SetUnit(string unit);                  

        void Refresh();                             

        float CurrentValue { get; }
        Range<float> ValueRange { get; }


    }

    public interface IInfographicEnhanced : IInfographic
    {
        void SetColor(Color color);                 
        void SetAnimationCurve(AnimationCurve curve); 
        void AnimateToValue(float targetValue, float duration); 
        void StopAnimation();
        void SetPrecision(int decimals);
        void SetFormat(Func<float, string> formatter);
        event Action<float> OnValueChanged;
        event Action OnAnimationComplete;

    }

    public class InfographicBase : MonoBehaviour, IInfographic
    {
        public virtual float CurrentValue { get; protected set; }
        public virtual Range<float> ValueRange { get; protected set; }

        public virtual void SetValue(float value)
        {
            CurrentValue = value;
            Refresh();
        }

        public virtual void SetRange(float min, float max) { }
        public virtual void SetLabel(string label) { }
        public virtual void SetUnit(string unit) { }
        public virtual void Refresh() { }

    }

    public class InfographicBaseEnhanced : InfographicBase, IInfographicEnhanced
    {
        public event Action<float> OnValueChanged;
        public event Action OnAnimationComplete;

        public virtual void SetColor(Color color) { }
        public virtual void SetAnimationCurve(AnimationCurve curve) { }
        public virtual void AnimateToValue(float targetValue, float duration) { }
        public virtual void StopAnimation() { }
        public virtual void SetPrecision(int decimals) { }
        public virtual void SetFormat(Func<float, string> formatter) { }

    }
}
