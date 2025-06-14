namespace NetCommon.Code.Modifiers;

[Flags]
public enum ClassModifier
{
   None = 0,
   Abstract = 1 << 0,
   Sealed = 1 << 1,
   Static = 1 << 2,
   Partial = 1 << 3,
   Unsafe = 1 << 4
}