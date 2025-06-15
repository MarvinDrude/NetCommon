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

   public ref CodeTemporaryStore TemporaryStore
   {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => ref Unsafe.AsRef(ref _temporaryStore);
   }
   private CodeTemporaryStore _temporaryStore;
   
   public CodeBuilder(
      Span<byte> tempBuffer,
      Span<char> buffer,
      Span<char> indentBuffer)
   {
      _writer = new CodeTextWriter(
         buffer, indentBuffer);
      _temporaryStore = new CodeTemporaryStore(tempBuffer);

      ref var self = ref Unsafe.AsRef(ref this);
      
      _nameSpace = new NameSpaceModule(ref self);
      _class = new ClassModule(ref self);
   }
   
   public void Dispose()
   {
      _writer.Dispose();
   }
}