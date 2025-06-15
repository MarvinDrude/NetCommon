using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NetCommon.Buffers;
using NetCommon.Code.Modules;

namespace NetCommon.Code;

[StructLayout(LayoutKind.Auto)]
public ref struct CodeBuilder : IDisposable
{
   public ref NameSpaceModule NameSpace
   {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => ref Unsafe.AsRef(ref _nameSpace);
   }
   private NameSpaceModule _nameSpace;
   
   public ref ClassModule Class
   {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => ref Unsafe.AsRef(ref _class);
   }
   private ClassModule _class;

   public ref CodeTextWriter Writer
   {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => ref Unsafe.AsRef(ref _writer);
   }
   private CodeTextWriter _writer;

   private CodeTemporaryStore _temporaryStore;
   
   public CodeBuilder(
      Span<char> buffer,
      Span<char> indentBuffer)
   {
      _writer = new CodeTextWriter(
         buffer, indentBuffer);

      ref var writer = ref Unsafe.AsRef(ref _writer);
      
      _nameSpace = new NameSpaceModule(ref writer);
      _class = new ClassModule(ref writer);
   }
   
   public void Dispose()
   {
      _writer.Dispose();
   }
}