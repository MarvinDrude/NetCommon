using NetCommon.Buffers;

namespace NetCommon.Code;

public ref struct CodeBuilder
{
   private CodeTextWriter _writer;

   public CodeBuilder(
      Span<char> buffer,
      Span<char> indentBuffer)
   {
      _writer = new CodeTextWriter(
         buffer, indentBuffer);
   }
   
   
}