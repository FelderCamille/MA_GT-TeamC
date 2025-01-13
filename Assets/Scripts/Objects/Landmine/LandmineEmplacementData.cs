using System;
using Unity.Netcode;

namespace Objects
{
    [Serializable]
    public class LandmineEmplacementData : INetworkSerializable, IEquatable<LandmineEmplacementData>
    {
        public bool[] emplacements;
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref emplacements);
        }
        
        // Implementing IEquatable<LandmineEmplacementData>
        public bool Equals(LandmineEmplacementData other)
        {
            if (other == null) return false;

            // Check if the emplacements arrays are equal
            if (emplacements == null && other.emplacements == null) return true;
            if (emplacements == null || other.emplacements == null) return false;
            if (emplacements.Length != other.emplacements.Length) return false;

            for (int i = 0; i < emplacements.Length; i++)
            {
                if (emplacements[i] != other.emplacements[i])
                    return false;
            }

            return true;
        }

        // Overriding Object.Equals for consistency
        public override bool Equals(object obj)
        {
            return Equals(obj as LandmineEmplacementData);
        }
    }
}