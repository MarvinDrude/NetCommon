using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NetCommon.Buffers;
using NetCommon.Code.Classes;

namespace NetCommon.Code.Modules;

[StructLayout(LayoutKind.Auto)]
public readonly ref struct ClassModule
{
   private readonly ByReferenceStack<CodeTextWriter> _writerReference;

   private ref CodeTextWriter Writer
   {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => ref _writerReference.AsRef();
   }
   
   public ClassModule(ref CodeTextWriter writer)
   {
      _writerReference = ByReferenceStack<CodeTextWriter>.Create(ref writer);
   }

   public void Create(out ClassDeclaration declaration)
   {
      declaration = new ClassDeclaration
      {
         Module = this
      };
   }
}