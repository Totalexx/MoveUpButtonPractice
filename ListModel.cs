using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

public class ListModel<TItem>
{
	public List<TItem> Items { get; }
	public int UndoLimit;
	private LimitedSizeStack<Action> actions;

	public ListModel(int undoLimit) : this(new List<TItem>(), undoLimit)
	{
	}

	public ListModel(List<TItem> items, int undoLimit)
	{
		Items = items;
		UndoLimit = undoLimit;
		actions = new LimitedSizeStack<Action>(undoLimit);
	}

	public void AddItem(TItem item)
	{
		actions.Push(new AddAction(Items, item));
		Items.Add(item);
	}

	public void RemoveItem(int index)
	{
		actions.Push(new RemoveAction(Items, Items[index], index));
		Items.RemoveAt(index);
	}

	public bool CanUndo()
	{
		return actions.Count > 0;
	}

	public void Undo()
	{
		actions.Pop().DoUndoAction();
	}
	
	public void MoveUpItem(int index)
	{
		var thisItem = Items[index];
		var nextItem = Items[index - 1];

		Items[index] = nextItem;
		Items[index - 1] = thisItem;
	}
	
	private abstract class Action
	{
		protected readonly List<TItem> items;
		protected readonly TItem item;

		protected Action(List<TItem> container, TItem item)
		{
			items = container;
			this.item = item;
		}

		public abstract void DoUndoAction();
	}
	
	private class AddAction : Action
	{
		public AddAction(List<TItem> container, TItem item) : base(container, item)
		{
		}
		
		public override void DoUndoAction()
		{
			items.RemoveAt(items.Count - 1);
		}
	}
	
	private class RemoveAction : Action
	{
		private readonly int itemIndex;
		
		public RemoveAction(List<TItem> container, TItem item, int itemIndex) : base(container, item)
		{
			this.itemIndex = itemIndex;
		}
		
		public override void DoUndoAction()
		{
			items.Insert(itemIndex, item);
		}
	}
}