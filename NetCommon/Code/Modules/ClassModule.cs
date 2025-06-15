using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NetCommon.Buffers;

namespace NetCommon.Code.Modules;

[StructLayout(LayoutKind.Auto)]
public readonly ref struct ClassModule
{
   private readonly ByReferenceStack<CodeBuilder> _builderReference;

   public ref CodeBuilder Builder
   {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => ref _builderReference.AsRef();
   }
   
   public ClassModule(ref CodeBuilder builder)
   {
      _builderReference = ByReferenceStack<CodeBuilder>.Create(ref builder);
   }

   
}