using System.Runtime.CompilerServices;

namespace NetCommon.Buffers;

public ref struct CodeTextWriter : IDisposable
{
   private const char DefaultIndent = '\t';
   private const char DefaultNewLine = '\n';

   private readonly char _indentCharacter;
   private readonly char _newLineCharacter;
   
   private ArrayWriter<char> _indentCache;
   private int _currentLevel;
   private ReadOnlySpan<char> _currentLevelBuffer;

   private ArrayWriter<char> _buffer;
   
   public CodeTextWriter(
      Span<char> buffer,
      Span<char> indentBuffer,
      char indentCharacter = DefaultIndent,
      char newLineCharacter = DefaultNewLine)
   {
      _indentCharacter = indentCharacter;
      _newLineCharacter = newLineCharacter;

      _buffer = new ArrayWriter<char>(buffer);
      
      _currentLevel = 0;
      _indentCache = new ArrayWriter<char>(indentBuffer);
      for (var e = 0; e < indentBuffer.Length; e++)
      {
         _indentCache.Write(_indentCharacter);
      }
   }

   public void WriteLine()
   {
      _buffer.Write(_newLineCharacter);
   }

   public void WriteLineIf(bool condition)
   {
      if (condition)
      {
         WriteLine();
      }
   }

   public void WriteLineIf(bool condition, scoped ReadOnlySpan<char> content, bool multiLine = false)
   {
      if (condition)
      {
         WriteLine(content, multiLine);
      }
   }
   
   public void WriteLine(scoped ReadOnlySpan<char> content, bool multiLine = false)
   {
      Write(content, multiLine);
      WriteLine();
   }
   
   public void WriteText(string text)
   {
      WriteText(text.AsSpan());
   }
   
   public void WriteText(scoped ReadOnlySpan<char> text)
   {
      AddIndentOnDemand();
      _buffer.Write(text);
   }

   public void Write(scoped ReadOnlySpan<char> text, bool multiLine = false)
   {
      if (!multiLine)
      {
         WriteText(text);
      }
      else
      {
         while (text.Length > 0)
         {
            var newLinePos = text.IndexOf(_newLineCharacter);

            if (newLinePos >= 0)
            {
               var line = text[..newLinePos];
               
               WriteIf(!line.IsEmpty, line);
               WriteLine();

               text = text[(newLinePos + 1)..];
            }
            else
            {
               WriteText(text);
               break;
            }
         }
      }
   }

   public void WriteIf(bool condition, scoped ReadOnlySpan<char> content, bool multiLine = false)
   {
      if (condition)
      {
         Write(content, multiLine);
      }  
   }
   
   public void UpIndent()
   {
      _currentLevel++;
      _currentLevelBuffer = GetCurrentIndentBuffer();
   }

   public void DownIndent()
   {
      _currentLevel--;
      ArgumentOutOfRangeException.ThrowIfLessThan(_currentLevel, 0, nameof(_currentLevel));
      
      _currentLevelBuffer = GetCurrentIndentBuffer();
   }
   
   private ReadOnlySpan<char> GetCurrentIndentBuffer()
   {
      if (_currentLevel == 0)
      {
         return [];
      }

      while (_indentCache.Position < _currentLevel)
      {
         _indentCache.Write(_indentCharacter);
      }

      return _indentCache.WrittenSpan[.._currentLevel];
   }

   private void AddIndentOnDemand()
   {
      if (_currentLevelBuffer.IsEmpty)
      {
         return;
      }
      
      if (_buffer.Position == 0 || _buffer.WrittenSpan[^1] == _newLineCharacter)
      {
         _buffer.Write(_currentLevelBuffer);
      }
   }

   public override string ToString()
   {
      return _buffer.WrittenSpan.Trim().ToString();
   }

   public void Dispose()
   {
      _buffer.Dispose();
      _indentCache.Dispose();
   }
}