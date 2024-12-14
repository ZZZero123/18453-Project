using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISingleLinkedListFunction<T>
{
    /// <summary>
    /// </summary>
    SingleLinkedListNode<T> First { get; }

    /// <summary>
    /// </summary>
    SingleLinkedListNode<T> Last { get; }

    /// <summary>
    /// </summary>
    int Count { get; }
    /// <summary>
    /// </summary>
    bool IsEmpty { get; }

    /// <summary>
    /// </summary>
    void Clear();

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    bool Contains(T value);

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    SingleLinkedListNode<T> AddFirst(T value);

    /// <summary>
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    SingleLinkedListNode<T> AddFirst(SingleLinkedListNode<T> node);

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    SingleLinkedListNode<T> AddLast(T value);

    ///<summary>
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    SingleLinkedListNode<T> AddLast(SingleLinkedListNode<T> node);

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    SingleLinkedListNode<T> Insert(T value, int index);

    /// <summary>
    /// </summary>
    /// <param name="node"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    SingleLinkedListNode<T> Insert(SingleLinkedListNode<T> node, int index);

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    bool Delete(T value);

    /// <summary>
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    bool DeleteAt(int index);

    /// <summary>
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    bool Delete(SingleLinkedListNode<T> node);

    /// <summary>
    /// </summary>
    /// <returns></returns>
    bool DeleteFirst();

    /// <summary>
    /// </summary>
    /// <returns></returns>
    bool DeleteLast();

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    SingleLinkedListNode<T> Find(T value);

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    SingleLinkedListNode<T> FindPrevious(T value);

    /// <summary>
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    T this[int index] { get; }

    /// <summary>
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    T GetElement(int index);

    /// <summary>

    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    int IndexOf(T value);
}
