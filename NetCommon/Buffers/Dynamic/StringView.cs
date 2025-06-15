namespace NetCommon.Buffers.Dynamic;

public readonly ref struct StringView
{
   public ReadOnlySpan<char> Span => _span;
   private readonly ReadOnlySpan<char> _span;
   
   public StringView(ReadOnlySpan<char> span)
   {
      _span = span;
   }

   public override string ToString()
   {
      return _span.ToString();
   }
}