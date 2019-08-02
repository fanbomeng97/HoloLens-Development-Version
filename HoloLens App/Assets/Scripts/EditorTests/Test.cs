using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Test
{
    // A Test behaves as an ordinary method
    [Test]
    public void TestSimplePasses()
    {
        bool isActive = false;
        Assert.AreEqual(false, isActive);
        // Use the Assert class to test conditions
    }
}

