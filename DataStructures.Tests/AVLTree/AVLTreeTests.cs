using System;
using DataStructures;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataStructures.Tests
{
    [TestClass]
    public class AVLTreeTests
    {
        public static T[] Randomize<T>(T[] items)
        {
            Random rand = new Random();

            // For each spot in the array, pick
            // a random item to swap into that spot.
            for (int i = 0; i < items.Length - 1; i++)
            {
                int j = rand.Next(i, items.Length);
                T temp = items[i];
                items[i] = items[j];
                items[j] = temp;
            }

            return items;
        }

        [TestMethod]
        public void AVLTree_ContainsTest()
        {
            AVLTree<int> tree = new AVLTree<int>();

            tree.InsertRange(Randomize(Enumerable.Range(1, 50000).ToArray()));

            // Contains item test
            
            for (int i = 25; i <= 50125; i++)
            {
                bool expected = 1 <= i && i <= 50000;
                bool actual = tree.Contains(i);
                Assert.AreEqual(expected, actual, $"Contains returned {actual} instead of {expected} for Contains({i})");
            }

            // Contains comparer test
            int l = 15;
            int r = 25000;
            Assert.IsTrue(tree.Contains(delegate (int x)
            {
                if (x > r) return -1;
                if (x >= l) return 0;
                return 1;
            }));
            
            l = 50110;
            r = 51200;
            Assert.IsFalse(tree.Contains(delegate (int x)
            {
                if (x > r) return -1;
                if (x >= l) return 0;
                return 1;
            }));
            
        }

        [TestMethod]
        public void AVLTree_InsertTest()
        {
            AVLTree<int> tree = new AVLTree<int>();

            tree.Insert(4);
            tree.Insert(6);
            tree.Insert(3);
            tree.Insert(5);
            tree.Insert(1);
            tree.Insert(7);
            tree.Insert(2);

            Assert.AreEqual(4, tree.RootData);
        }
    }
}