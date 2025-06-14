namespace NetCommon.Code.Modifiers;

[Flags]
public enum MethodModifier
{
   None = 0,
   Static = 1 << 0,
   Abstract = 1 << 1,
   Virtual = 1 << 2,
   Override = 1 << 3,
   Sealed = 1 << 4,
   Async = 1 << 5,
   Unsafe = 1 << 6,
   Extern = 1 << 7,
   Partial = 1 << 8,
   New = 1 << 9,
   ReadOnly = 1 << 10 
}