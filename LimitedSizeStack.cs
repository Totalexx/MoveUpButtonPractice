using System.Collections.Generic;

namespace LimitedSizeStack;

public class LimitedSizeStack<T>
{
    private readonly LinkedList<T> store;
    private readonly int undoLimit;

    public LimitedSizeStack(int undoLimit)
    {
        store = new LinkedList<T>();
        this.undoLimit = undoLimit;
    }

    public void Push(T item)
    {
        if (undoLimit == 0) 
            return;

        if (store.Count == undoLimit)
            store.RemoveLast();

        store.AddFirst(item);
    }

    public T Pop()
    {
        var value = store.First.Value;
        store.RemoveFirst();

        return value;
    }
    
    public int Count => store.Count;
}