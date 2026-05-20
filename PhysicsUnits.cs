using UnityEngine;

namespace MagmaLabs.Simulation
{
    [System.Serializable]
    public struct Seconds
    {
        public float value;
        public Seconds(float v)
        {
            value = v;
        }
    }
[System.Serializable]
    public struct Meters
    {
        public float value;
        public Meters(float v)
        {
            value = v;
        }
    }
[System.Serializable]
    public struct MetersPerSecond
    {
        public float value;

        public void Accelerate(MetersPerSecondSquared acceleration, Seconds deltaTime)
        {
            value += acceleration.value * deltaTime.value;
        }
    }
    [System.Serializable]
    public struct MetersPerSecondSquared
    {
        public float value;
    }
    [System.Serializable]
    public struct RadiansPerSecond
    {
        public float value;

        public void Accelerate(RadiansPerSecondSquared acceleration, Seconds deltaTime)
        {
            value += acceleration.value * deltaTime.value;
        }
    }
[System.Serializable]
    public struct RadiansPerSecondSquared
    {
        public float value;
        
    }

}
