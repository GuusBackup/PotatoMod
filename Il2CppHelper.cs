using UnityEngine.Events;

namespace PotatoMod;
public static class Il2CppHelper
{
    public static UnityAction<T> ToUnityAction<T>(this Action<T> action)
    {

#if Mono
        return (b) => { action(b); };
#else
        return action;
#endif
    }
}
