using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageInfo<T>
{
    public int currentPage; 
    public int everyPageMax; 
    public List<T> pageItems;
}
