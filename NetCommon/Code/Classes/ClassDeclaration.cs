using System.Runtime.InteropServices;
using NetCommon.Code.Modules;

namespace NetCommon.Code.Classes;

[StructLayout(LayoutKind.Auto)]
public ref struct ClassDeclaration
{
   internal ClassModule Module;

   private ReadOnlySpan<char> _className;

   public ClassDeclaration()
   {
      
   }

   public void SetName(in ReadOnlySpan<char> name)
   {
      _className = name;
   }
}