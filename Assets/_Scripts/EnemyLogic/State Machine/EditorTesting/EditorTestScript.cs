using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TestBehaviourEditor : MonoBehaviour
{
    [Randomize(0, 10)]
    public float RandomValue;
    public List<ClassStuff> classList;
}

[Serializable]
public class ClassStuff
{
    public List<SubClassStuff> subclass;
}

[Serializable]
public class SubClassStuff
{
    public List<int> ints;
}
