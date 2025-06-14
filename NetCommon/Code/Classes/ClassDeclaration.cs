using System.Runtime.InteropServices;
using NetCommon.Code.Modules;

namespace NetCommon.Code.Classes;

[StructLayout(LayoutKind.Auto)]
public ref struct ClassDeclaration
{
   internal ClassModule Module;

   public ClassDeclaration()
   {
      
   }
   
}