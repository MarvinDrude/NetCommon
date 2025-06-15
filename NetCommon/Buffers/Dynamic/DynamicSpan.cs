using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace NetCommon.Buffers.Dynamic;

public ref struct DynamicSpan<T>
   where T : struct, ISerializableStruct<T>, allows ref struct
{
   public readonly int SpaceLeft => _buffer.Length - _length;
   public readonly Span<byte> FreeBuffer => _buffer[_length..];
   public readonly Span<byte> WrittenSpan => _buffer[.._length];

   public readonly int Count => _itemLength;
   public readonly int Capacity => _buffer.Length;
   public readonly int Position => _length;
   
   private readonly Span<byte> _buffer;
   private int _length;
   private int _itemLength;
   
   public DynamicSpan(
      Span<byte> buffer)
   {
      _buffer = buffer;
      _length = 0;
      _itemLength = 0;
   }
   
   public DynamicSpan(
      Span<byte> buffer,
      int length,
      int itemLength)
   {
      _buffer = buffer;
      _length = length;
      _itemLength = itemLength;
   }

   public bool Add(scoped in T item)
   {
      var sizeRequired = T.GetSerializedSize(item);

      if (SpaceLeft < sizeRequired)
      {
         return false;
      }

      T.WriteTo(FreeBuffer[..sizeRequired], item);
      
      _length += sizeRequired;
      _itemLength++;
      
      return true;
   }

   public bool RemoveAt(int index)
   {
      if (index < 0 || index >= _itemLength)
      {
         return false;
      }

      var offset = 0;
      
      for (var current = 0; current < index; current++)
      {
         var length = BinaryPrimitives.ReadInt32BigEndian(_buffer.Slice(offset, sizeof(int)));
         offset += sizeof(int) + length;
         
         if (offset >= _length)
         {
            return false;
         }
      }
      
      var itemLength = BinaryPrimitives.ReadInt32BigEndian(_buffer.Slice(offset, sizeof(int)));
      var totalItemSize = sizeof(int) + itemLength;
      
      var remainingStart = offset + totalItemSize;
      var remainingLength = _length - remainingStart;

      if (remainingLength > 0)
      {
         _buffer.Slice(remainingStart, remainingLength)
            .CopyTo(_buffer.Slice(offset, remainingLength));
      }

      _length -= totalItemSize;
      _itemLength--;
      
      return true;
   }
   
   public bool TryGetByIndex(int index, out T item)
   {
      item = default;
      
      if (_itemLength <= index)
      {
         return false;
      }

      var offset = 0;
      
      for (var current = 0; current < index; current++)
      {
         var length = BinaryPrimitives.ReadInt32BigEndian(_buffer.Slice(offset, sizeof(int)));
         offset += sizeof(int) + length;
         
         if (offset >= _length)
         {
            return false;
         }
      }
      
      T.ReadFrom(_buffer[offset..], out item);
      return true;
   }

   public Enumerator GetEnumerator() => new(_buffer, _itemLength);
   
   public ref struct Enumerator
   {
      private readonly Span<byte> _buffer;
      private readonly int _itemLength;
      
      private int _index;
      private int _offset;
   
      private T _current;
   
      public Enumerator(Span<byte> buffer, int itemLength)
      {
         _buffer = buffer;
         _itemLength = itemLength;
         
         _index = -1;
         _offset = 0;
      }
   
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public bool MoveNext()
      {
         var index = _index + 1;
         if (index >= _itemLength)
         {
            return false;
         }
         
         _index = index;
         
         T.ReadFrom(_buffer[_offset..], out _current);
         var length = BinaryPrimitives.ReadInt32BigEndian(_buffer.Slice(_offset, sizeof(int)));
         
         _offset += sizeof(int) + length;
         
         return true;
   
      }
   
      public T Current => _current;
   }
}