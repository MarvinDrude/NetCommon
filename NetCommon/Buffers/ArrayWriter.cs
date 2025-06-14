﻿
using System.Runtime.InteropServices;

namespace NetCommon.Buffers;

[StructLayout(LayoutKind.Auto)]
public ref struct ArrayWriter<T> : IDisposable
{
   public readonly int Capacity => _usesPoolMemory ? _growBuffer.Length : _buffer.Length;
   public readonly int FreeCapacity => Capacity - _position;

   public readonly ReadOnlySpan<T> WrittenSpan => Buffer[.._position];
   
   public int Position
   {
      readonly get => _position;
      set
      {
         if (value > Capacity || value < 0)
         {
            throw new ArgumentOutOfRangeException(nameof(Position));
         }
         _position = value;
      }
   }

   private readonly Span<T> Buffer => _usesPoolMemory ? _growBuffer.Span : _buffer;
   private readonly Span<T> _buffer;

   private ArrayPoolAllocation<T> _growBuffer;
   private int _position;
   private int _initialCapacity;

   private readonly bool _usesPoolMemory => _growBuffer.Length > 0;
   
   public ArrayWriter(Span<T> buffer, int capacity = -1)
   {
      _buffer = buffer;
      _position = 0;
      _initialCapacity = capacity;
   }

   public void AddReference(ref T reference)
   {
      Add() = reference;
   }
   
   public void Add(T value)
   {
      Add() = value;
   }
   
   private ref T Add()
   {
      ref var temp = ref MemoryMarshal.GetReference(GetUsableSpan(1));
      _position++;
      return ref temp;
   }
   
   public void Write(in T value)
   {
      GetUsableSpan(1)[0] = value;
      _position++;
   }
   
   public void Write(scoped ReadOnlySpan<T> span)
   {
      if (span.IsEmpty)
      {
         return;
      }
      
      span.CopyTo(GetUsableSpan(span.Length));
      _position += span.Length;
   }
   
   private Span<T> GetUsableSpan(int requestedSize)
   {
      Span<T> result;
      
      if (!_growBuffer.Memory.IsEmpty)
      {
         if (CalculateSize(requestedSize, _growBuffer.Length, _position, out var grownSize))
         {
            ArrayPoolAllocator<T>.Create(grownSize, out var newAllocation);
            _growBuffer.CopyTo(newAllocation);
            _growBuffer.Dispose();

            _growBuffer = newAllocation;
         }
         
         result = _growBuffer.Span;
      }
      else
      {
         if (CalculateSize(requestedSize, _buffer.Length, _position, out var grownSize))
         {
            if (_initialCapacity != -1 && _initialCapacity >= grownSize)
            {
               grownSize = _initialCapacity;
            }
            
            ArrayPoolAllocator<T>.Create(grownSize, out _growBuffer);
            _buffer.CopyTo(_growBuffer.Span);

            result = _growBuffer.Span;
         }
         else
         {
            result = _buffer;
         }
      }

      return result[_position..];
   }

   public readonly void Dispose()
   {
      if (_growBuffer.Memory.IsEmpty)
      {
         return;
      }
      
      _growBuffer.Dispose();
   }

   private static bool CalculateSize(int requestedSize, int capacity, int position, out int grownSize)
   {
      var left = capacity - position;
      
      if (requestedSize > left)
      {
         var growBy = capacity > 0 ? Math.Max(capacity, requestedSize) : DefaultArraySize;
         grownSize = capacity + growBy;
         
         return true;
      }

      grownSize = -1;
      return false;
   }

   private const int DefaultArraySize = 1024;
}