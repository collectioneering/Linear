using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Linear;

/// <summary>
/// Stores circular buffer of elements
/// </summary>
/// <typeparam name="T">Element type</typeparam>
public class CircleBuffer<T> : IList<T>
{
    /// <summary>
    /// Maximum capacity
    /// </summary>
    public int Capacity { get; }

    private readonly T[] _entries;
    private int _first;
    private int _count;

    /// <summary>
    /// Create new instance of <see cref="CircleBuffer{T}"/>
    /// </summary>
    /// <param name="capacity"></param>
    public CircleBuffer(int capacity)
    {
        Capacity = capacity;
        _entries = new T[capacity];
        _first = 0;
    }

    /// <inheritdoc />
    public T this[int i]
    {
        get
        {
            RangeThrow(i);
            return _entries[Index(i)];
        }
        set
        {
            RangeThrow(i);
            _entries[Index(i)] = value;
        }
    }

    /// <inheritdoc />
    public void RemoveAt(int i)
    {
        RangeThrow(i);
        if (i < _count / 2)
        {
            // Better to move elements below
            for (int j = i; j > 0; j--) _entries[Index(j)] = _entries[Index(j - 1)];
            _entries[_first] = default!;
            // Move bottom up
            _first = (_first + 1) % Capacity;
        }
        else
        {
            // Better to move elements above
            int top = _count - 1;
            for (int j = i; j < top; j++) _entries[Index(j)] = _entries[Index(j + 1)];
            _entries[Index(_count - 1)] = default!;
        }

        _count--;
    }

    /// <inheritdoc />
    public void Insert(int index, T value)
    {
        if (_count == Capacity)
            throw new ArgumentException($"Cannot push with length {_count} and capacity {Capacity}");
        _count++;
        RangeThrow(index);
        if (index < _count / 2)
        {
            _first = (Capacity + _first - 1) % Capacity;
            // Better to move elements below
            for (int j = 0; j < index; j++) _entries[Index(j)] = _entries[Index(j + 1)];
        }
        else
        {
            // Better to move elements above
            for (int j = _count - 1; j > index; j--) _entries[Index(j)] = _entries[Index(j - 1)];
        }

        _entries[Index(index)] = value;
    }

    private void RangeThrow(int i)
    {
        if (i < 0 || i >= _count) throw new ArgumentException($"Invalid index {i} for list of length {_count}");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int Index(int i) => (_first + i) % Capacity;

    /// <inheritdoc />
    public int IndexOf(T item)
    {
        for (int i = 0; i < _count; i++)
            if (_entries[Index(i)]?.Equals(item) ?? item == null)
            {
                return i;
            }

        return -1;
    }

    /// <inheritdoc />
    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < _count; i++)
            yield return _entries[Index(i)];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    public void Add(T item)
    {
        Insert(_count, item);
    }

    /// <inheritdoc />
    public void Clear()
    {
        for (int i = 0; i < _count; i++)
            _entries[Index(i)] = default!;
        _count = 0;
    }

    /// <inheritdoc />
    public bool Contains(T item)
    {
        for (int i = 0; i < _count; i++)
            if (_entries[Index(i)]?.Equals(item) ?? item == null)
            {
                return true;
            }

        return false;
    }

    /// <inheritdoc />
    public void CopyTo(T[] array, int arrayIndex)
    {
        if (array == null) throw new ArgumentNullException();
        if (arrayIndex < 0) throw new ArgumentOutOfRangeException();
        if (array.Length - arrayIndex < _count) throw new ArgumentException();
        int a = Math.Min(_count, Capacity - _first);
        Array.Copy(_entries, _first, array, arrayIndex, a);
        if (a != _count)
            Array.Copy(_entries, 0, array, arrayIndex + a, _count - a);
    }

    /// <inheritdoc />
    public bool Remove(T item)
    {
        for (int i = 0; i < _count; i++)
            if (_entries[Index(i)]?.Equals(item) ?? item == null)
            {
                RemoveAt(i);
                return true;
            }

        return false;
    }

    /// <inheritdoc />
    public int Count => _count;

    /// <inheritdoc />
    public bool IsReadOnly => false;
}
