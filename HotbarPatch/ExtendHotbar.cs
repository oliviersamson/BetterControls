using System.Runtime.CompilerServices;

namespace UnityEngine
{
    public static class ExtendHotbar
    {
        private static readonly ConditionalWeakTable<Hotbar, LastActiveValue> LastActive = new ConditionalWeakTable<Hotbar, LastActiveValue>();

        public class LastActiveValue
        {
            public int Value;
        }

        public static LastActiveValue GetLastActive(this Hotbar hotbar)
        {
            return LastActive.GetOrCreateValue(hotbar);
        }

        public static void SetLastActive(this Hotbar hotbar, int value)
        {
            LastActive.GetOrCreateValue(hotbar).Value = value;
        }
    }
}
