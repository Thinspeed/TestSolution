using System;
using System.Collections.Generic;

namespace testSolution;

public class Heap<T>
{
    private const int Compacity = 100;
    
    private T[] _data;
    private int _count = 0;
    private Comparer<T> _comparer;
    
    public Heap(Comparer<T> comparer)
    {
        _data = new T[Compacity];
        _comparer = comparer;
    }

    public void Add(T item)
    {
        if (_count == _data.Length)
        {
            var newData = new T[_data.Length * 2];
            _data.CopyTo(newData, 0);
            _data = newData;
        }
        
        _data[_count] = item;
        _count++;
        
        int i = _count - 1;
        int parent = (i - 1) / 2;

        while (i > 0 && _comparer.Compare(_data[parent], _data[i]) < 0)
        {
            (_data[i], _data[parent]) = (_data[parent], _data[i]);

            i = parent;
            parent = (i - 1) / 2;
        }
    }

    public T GetTop()
    {
        if (_count == 0) throw new InvalidOperationException("Heap is empty");
        
        T result = _data[0];
        
        _data[0] = _data[_count - 1];
        _count--;
        Heapify(0);
        
        return result;
    }
    
    private void Heapify(int i)
    {
        int leftChild;
        int rightChild;
        int largestChild;

        for (; ; )
        {
            leftChild = 2 * i + 1;
            rightChild = 2 * i + 2;
            largestChild = i;

            if (leftChild < _count && _comparer.Compare(_data[leftChild], _data[largestChild]) > 0) 
            {
                largestChild = leftChild;
            }

            if (rightChild < _count && _comparer.Compare(_data[rightChild], _data[largestChild]) > 0)
            {
                largestChild = rightChild;
            }

            if (largestChild == i) 
            {
                break;
            }

            (_data[i], _data[largestChild]) = (_data[largestChild], _data[i]);
            i = largestChild;
        }
    }
}