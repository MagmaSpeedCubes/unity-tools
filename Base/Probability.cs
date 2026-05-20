using System;
using UnityEngine;
namespace MagmaLabs{
    [System.Serializable]
    public struct Probability {
        private float value;
        public Probability(float v) {
            if (v < 0 || v > 1) throw new ArgumentOutOfRangeException();
            value = v;
        }

        public bool Trial()
        {
            if (value <= 0f) return false;
            if (value >= 1f) return true;
            return UnityEngine.Random.value < value;
        }
        public Probability Not()
        {
            return new Probability(1-value);
        }

        public static Probability And(Probability[] events)
        {
            if (events == null) throw new ArgumentNullException(nameof(events));
            double product = 1.0;
            for (int i = 0; i < events.Length; i++)
            {
                product *= events[i].value;
                if (product <= 0.0) return new Probability(0f);
            }
            return new Probability((float)product);
        }

        public static Probability Or(Probability[] events)
        {
            if (events == null) throw new ArgumentNullException(nameof(events));
            double productOfComplements = 1.0;
            for (int i = 0; i < events.Length; i++)
            {
                productOfComplements *= 1.0 - events[i].value;
                if (productOfComplements <= 0.0) return new Probability(1f);
            }
            return new Probability((float)(1.0 - productOfComplements));
        }

        public static Probability Xor(Probability[] events)
        {
            if (events == null) throw new ArgumentNullException(nameof(events));
            double product = 1.0;
            for (int i = 0; i < events.Length; i++)
            {
                product *= 1.0 - (2.0 * events[i].value);
            }

            double p = (1.0 - product) / 2.0;
            if (p <= 0.0) return new Probability(0f);
            if (p >= 1.0) return new Probability(1f);
            return new Probability((float)p);
        }


    }

}
