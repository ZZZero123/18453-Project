using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleLinkedList<T> : ISingleLinkedListFunction<T>
{
    private SingleLinkedListNode<T> first = null;
    /// <summary>
    /// </summary>
    public SingleLinkedListNode<T> First
    {
        get
        {
            return first;
        }
    }

    private SingleLinkedListNode<T> last = null;
    /// <summary>
    /// </summary>
    public SingleLinkedListNode<T> Last
    {
        get
        {
            return last;
        }
    }

    private int count;
    /// <summary>
    /// </summary>
    public int Count
    {
        get
        {
            count = GetCount();
            return count;
        }
    }

    private bool isEmpty;
    /// <summary>
    /// </summary>
    public bool IsEmpty
    {
        get
        {
            isEmpty = GetIsEmpty();
            return isEmpty;
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public T this[int index]
    {
        get
        {
            if (index >= Count || index < 0)
            {
                return default(T);
            }
            SingleLinkedListNode<T> tempNode = first;
            for (int i = 0; i < index; i++)
            {
                tempNode = tempNode.Next;
            }
            return tempNode.Value;
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public SingleLinkedListNode<T> AddFirst(T value)
    {
        SingleLinkedListNode<T> tempNode = new SingleLinkedListNode<T>();
        tempNode.Value = value;
        if (first == null)
        {
            first = tempNode;
            last = tempNode;
        }
        else
        {
            tempNode.Next = first;
            first = tempNode;
        }
        return tempNode;
    }

    /// <summary>
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public SingleLinkedListNode<T> AddFirst(SingleLinkedListNode<T> node)
    {
        SingleLinkedListNode<T> tempNode = CopyToFrom(node);
        tempNode.Next = null;
        if (first == null)
        {
            first = tempNode;
            last = tempNode;
        }
        else
        {
            tempNode.Next = first;
            first = tempNode;
        }
        return tempNode;
    }

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public SingleLinkedListNode<T> AddLast(T value)
    {
        SingleLinkedListNode<T> tempNode = new SingleLinkedListNode<T>();
        tempNode.Value = value;
        if (first == null)
        {
            first = tempNode;
            last = tempNode;
        }
        else
        {
            last.Next = tempNode;
            last = tempNode;
        }
        return tempNode;
    }

    /// <summary>
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public SingleLinkedListNode<T> AddLast(SingleLinkedListNode<T> node)
    {
        SingleLinkedListNode<T> tempNode = CopyToFrom(node);
        tempNode.Next = null;
        if (first == null)
        {
            first = tempNode;
            last = tempNode;
        }
        else
        {
            last.Next = tempNode;
            last = tempNode;
        }
        return tempNode;
    }

    /// <summary>
    /// </summary>
    public void Clear()
    {
        first = null;
        last = null;
    }

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Contains(T value)
    {
        if (first == null)
        {
            return false;
        }
        SingleLinkedListNode<T> tempNode = first;
        if (tempNode.Value.Equals(value))
        {
            return true;
        }
        while (true)
        {
            if (tempNode.Next != null || tempNode == last)
            {
                if (tempNode.Value.Equals(value))
                {
                    return true;
                }
                tempNode = tempNode.Next;

            }
            else
            {
                break;
            }
        }
        return false;
    }

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Delete(T value)
    {
        if (Contains(value))
        {
            SingleLinkedListNode<T> tempNode = first;
            if (tempNode.Value.Equals(value))
            {
                if (first == null)
                {
                    last = null;
                }
                first = first.Next;
                return true;
            }
            SingleLinkedListNode<T> tempPrevious = null;
            while (true)
            {
                if (tempNode.Next != null)
                {
                    tempPrevious = tempNode;
                    tempNode = tempNode.Next;
                    if (tempNode.Value.Equals(value))
                    {
                        tempPrevious.Next = tempNode.Next;
                        if (tempPrevious.Next == null)
                        {
                            last = tempPrevious;
                        }
                        return true;
                    }
                }
                else
                {
                    break;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool DeleteAt(int index)
    {
        if (index >= Count)
        {
            return false;
        }
        else
        {
            if (index == 0)
            {
                if (first == last)
                {
                    last = null;
                }
                first = first.Next;
                return true;
            }
            else
            {
                SingleLinkedListNode<T> tempNode = first;
                for (int i = 0; i < index - 1; i++)
                {
                    tempNode = tempNode.Next;
                }
                if (index == Count - 1)
                {
                    last = tempNode;
                }
                tempNode.Next = tempNode.Next.Next;
                return true;
            }
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public bool Delete(SingleLinkedListNode<T> node)
    {
        if (first == null)
        {
            return false;
        }
        else
        {
            SingleLinkedListNode<T> tempNode = first;
            if (tempNode.Equals(node))
            {
                if (first == last)
                {
                    last = null;
                }
                first = first.Next;
                return true;
            }
            SingleLinkedListNode<T> tempPrevious = null;
            while (true)
            {
                if (tempNode.Next != null)
                {
                    tempPrevious = tempNode;
                    tempNode = tempNode.Next;
                    if (tempNode.Equals(node))
                    {
                        tempPrevious.Next = tempNode.Next;
                        if (tempPrevious.Next == null)
                        {
                            last = tempPrevious;
                        }
                        return true;
                    }
                }
                else
                {
                    break;
                }
            }
            return false;
        }
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public bool DeleteFirst()
    {
        if (first == null)
        {
            return false;
        }
        else
        {
            if (first == last)
            {
                last = null;
            }
            first = first.Next;
            return true;
        }

    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public bool DeleteLast()
    {
        if (first == null)
        {
            return false;
        }
        else
        {
            if (first == last)
            {
                last = null;
                first = null;
            }
            else
            {
                SingleLinkedListNode<T> tempNode = first;
                SingleLinkedListNode<T> tempPrevious = null;
                while (true)
                {
                    if (tempNode.Next != null)
                    {
                        tempPrevious = tempNode;
                        tempNode = tempNode.Next;

                    }
                    else
                    {
                        tempPrevious.Next = null;
                        last = tempPrevious;
                        break;
                    }
                }
            }
            return true;
        }
    }
    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public SingleLinkedListNode<T> Find(T value)
    {
        if (Contains(value))
        {
            SingleLinkedListNode<T> tempNode = first;
            if (tempNode.Value.Equals(value))
            {
                return first;
            }
            while (true)
            {
                if (tempNode.Next != null || tempNode == last)
                {
                    if (tempNode.Value.Equals(value))
                    {
                        return tempNode;
                    }
                    tempNode = tempNode.Next;
                }
                else
                {
                    break;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public SingleLinkedListNode<T> FindPrevious(T value)
    {
        if (first == null)
        {
            return null;
        }

        SingleLinkedListNode<T> tempNode = first;
        if (tempNode.Value.Equals(value))
        {
            return null;
        }


        SingleLinkedListNode<T> tempPrevious = null;
        while (true)
        {
            if (tempNode.Next != null)
            {

                tempPrevious = tempNode;
                tempNode = tempNode.Next;
                if (tempNode.Value.Equals(value))
                {
                    return tempPrevious;
                }
            }
            else
            {
                break;
            }
        }
        return null;
    }

    /// <summary>
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public T GetElement(int index)
    {
        if (index >= Count)
        {
            return default(T);
        }
        else
        {
            if (index == 0)
            {
                return first.Value;
            }
            else
            {
                SingleLinkedListNode<T> tempNode = first;
                for (int i = 0; i < index; i++)
                {
                    tempNode = tempNode.Next;
                }
                return tempNode.Value;
            }
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public int IndexOf(T value)
    {
        if (Contains(value))
        {
            int tempIndex = 0;
            SingleLinkedListNode<T> tempNode = first;
            if (tempNode.Value.Equals(value))
            {
                return tempIndex;
            }
            while (true)
            {
                if (tempNode.Next != null)
                {
                    if (tempNode.Value.Equals(value))
                    {
                        return tempIndex;
                    }
                    tempNode = tempNode.Next;
                    tempIndex++;
                }
                else
                {
                    break;
                }
            }
        }
        return -1;
    }

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public SingleLinkedListNode<T> Insert(T value, int index)
    {
        if (index > Count || index < 0)
        {
            return null;
        }
        if (index == 0)
        {
            return AddFirst(value);
        }
        else if (index == Count)
        {
            return AddLast(value);
        }
        else
        {
            if (first == null) return null;

            SingleLinkedListNode<T> tempNode = first;
            for (int i = 0; i < index - 1; i++)
            {
                tempNode = tempNode.Next;
            }
            SingleLinkedListNode<T> newNode = new SingleLinkedListNode<T>();
            newNode.Value = value;
            newNode.Next = tempNode.Next;
            tempNode.Next = newNode;
            return newNode;
        }


    }

    /// <summary>
    /// </summary>
    /// <param name="node"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public SingleLinkedListNode<T> Insert(SingleLinkedListNode<T> node, int index)
    {
        if (index > Count || index < 0)
        {
            return null;
        }
        if (index == 0)
        {
            return AddFirst(node);
        }
        else if (index == Count)
        {
            return AddLast(node);
        }
        else
        {
            if (first == null) return null;

            SingleLinkedListNode<T> tempNode = first;
            for (int i = 0; i < index - 1; i++)
            {
                tempNode = tempNode.Next;
            }
            SingleLinkedListNode<T> newNode = CopyToFrom(node);
            newNode.Next = tempNode.Next;
            tempNode.Next = newNode;
            return newNode;
        }
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        string str = "linkedlist info";
        if (first == null)
        {
            Debug.Log("empty linkedlist");
            return str;
        }
        SingleLinkedListNode<T> tempNode = first;
        str += tempNode.Value.ToString();
        while (true)
        {
            if (tempNode.Next != null)
            {
                tempNode = tempNode.Next;
                str += tempNode.Value.ToString();

            }
            else
            {

                break;
            }
        }
        Debug.Log(str);
        return str;
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    private int GetCount()
    {
        if (first == null) return 0;

        int tempCount = 1;
        SingleLinkedListNode<T> tempNode = first;
        while (true)
        {
            if (tempNode.Next != null)
            {
                tempNode = tempNode.Next;
                tempCount++;
            }
            else
            {
                break;
            }
        }
        return tempCount;
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    private bool GetIsEmpty()
    {
        return first == null;
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    private SingleLinkedListNode<T> CopyToFrom(SingleLinkedListNode<T> node)
    {
        SingleLinkedListNode<T> tempNode = new SingleLinkedListNode<T>();
        tempNode.Value = node.Value;
        tempNode.Next = tempNode.Next;
        return tempNode;
    }
}
