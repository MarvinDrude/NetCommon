using NetCommon.Buffers.Dynamic;
using NetCommon.Code.Modifiers;

namespace NetCommon.Code.Classes;

public ref struct ClassDeclaration 
   : ISerializableStruct<ClassDeclaration>
{
   public StringView Name;
   public AccessModifier AccessModifier;
   public ClassModifier Modifiers;
   
   public static void WriteTo(scoped Span<byte> buffer, scoped in ClassDeclaration instance)
   {
      throw new NotImplementedException();
   }

   public static void ReadFrom(ReadOnlySpan<byte> buffer, out ClassDeclaration instance)
   {
      throw new NotImplementedException();
   }

   public static int GetSerializedSize(scoped in ClassDeclaration instance)
   {
      throw new NotImplementedException();
   }
}