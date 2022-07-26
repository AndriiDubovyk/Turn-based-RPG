using System;
using System.Collections.Generic;

class MinPriorityQueue<T> where T : IComparable
{
    private List<T> data;
    public int Count { get { return data.Count; } }

    public MinPriorityQueue()
    {
        data = new List<T>();
    }

    public void Enqueue(T element)
    {
        data.Add(element);
        int i = Count - 1;

        while (i > 0)
        {
            int parent = (i - 1) / 2;
            // go up if parent is greater then element 
            if (data[parent].CompareTo(element) > 0)
            {
                data[i] = data[parent];
                i = parent;
            }
            else // if element is less or equals parent 
            {
                break;
            }
           
        }
        data[i] = element;
    }

    public T Dequeue()
    {
        if (Count == 0)
        {
            throw new InvalidOperationException("Queue is empty");
        }
        T target = data[0];


        // Heapify the tree
        T root = data[Count - 1];
        data.RemoveAt(Count - 1);

        int i = 0;
        while (i * 2 + 1 < Count)
        {
            int leftChild = i * 2 + 1;
            int rightChild = i * 2 + 2;
            int minChild = rightChild < Count && data[rightChild].CompareTo(data[leftChild]) < 0 ? rightChild : leftChild;

            if (data[minChild].CompareTo(root) >= 0) break;
            data[i] = data[minChild];
            i = minChild;
        }

        if (Count > 0) data[i] = root;
        return target;
    }

    public bool Contains(T element)
    {
        return data.Contains(element);
    }

}