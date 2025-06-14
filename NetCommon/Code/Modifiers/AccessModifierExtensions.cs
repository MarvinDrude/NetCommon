namespace NetCommon.Code.Modifiers;

public static class AccessModifierExtensions
{
   public static void FillCharBuffer(this AccessModifier modifier, scoped Span<char> buffer)
   {
      ReadOnlySpan<char> target = modifier switch
      {
         AccessModifier.None => string.Empty,
         AccessModifier.PrivateProtected => "private protected",
         AccessModifier.ProtectedInternal => "protected internal",
         AccessModifier.Private => "private",
         AccessModifier.Protected => "protected",
         AccessModifier.Internal => "internal",
         AccessModifier.Public => "public",
         _ => string.Empty
      };
         
      target.CopyTo(buffer);
   }
   
   public static int GetCharBufferSize(this AccessModifier modifier)
   {
      return modifier switch
      {
         AccessModifier.None => 0,
         AccessModifier.Private => 7,
         AccessModifier.Protected => 9,
         AccessModifier.PrivateProtected => 17,
         AccessModifier.Internal => 8,
         AccessModifier.ProtectedInternal => 18,
         AccessModifier.Public => 6,
         _ => 0
      };
   }
}