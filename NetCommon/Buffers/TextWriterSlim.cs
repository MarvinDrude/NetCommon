using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetCommon.Buffers;

[StructLayout(LayoutKind.Auto)]
public ref struct TextWriterSlim : IDisposable
{
   private ArrayWriter<char> _buffer;

   public TextWriterSlim(Span<char> buffer)
   {
      _buffer = new ArrayWriter<char>(buffer);
   }

   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public void WriteText(scoped ReadOnlySpan<char> text)
   {
      _buffer.Write(text);
   }

   public void Dispose()
   {
      _buffer.Dispose();
   }
}