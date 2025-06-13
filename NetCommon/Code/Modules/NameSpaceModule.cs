using System.Runtime.CompilerServices;
using NetCommon.Buffers;

namespace NetCommon.Code.Modules;

public readonly ref struct NameSpaceModule
{
   private readonly ByReferenceStack<CodeTextWriter> _writerReference;
   private ref CodeTextWriter Writer => ref _writerReference.AsRef();
   
   public NameSpaceModule(ref CodeTextWriter writer)
   {
      _writerReference = ByReferenceStack<CodeTextWriter>.Create(ref writer);
   }
   
   public void EnableNullable(bool extraLine)
   {
      ref var writer = ref Writer;
      
      writer.WriteLine("#nullable enable");
      writer.WriteLineIf(extraLine);
   }
   
   
}