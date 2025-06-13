using System.Runtime.CompilerServices;

namespace NetCommon.Buffers;

public ref struct TextWriterSlim : IDisposable
{
   private ArrayWriter<char> _buffer;

   public TextWriterSlim(Span<char> buffer)
   {
      _buffer = new ArrayWriter<char>(buffer);
   }

   public void WriteText(scoped ReadOnlySpan<char> text)
   {
      _buffer.Write(text);
   }

   public void Dispose()
   {
      _buffer.Dispose();
   }
}