using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NetCommon.Buffers;

namespace NetCommon.Code.Modules;

[StructLayout(LayoutKind.Auto)]
public readonly ref struct ClassModule
{
   private readonly ByReferenceStack<CodeTextWriter> _writerReference;

   public ref CodeTextWriter Writer
   {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => ref _writerReference.AsRef();
   }
   
   public ClassModule(ref CodeTextWriter writer)
   {
      _writerReference = ByReferenceStack<CodeTextWriter>.Create(ref writer);
   }

   
}