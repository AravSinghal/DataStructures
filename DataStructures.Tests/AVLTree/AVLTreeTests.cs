using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataStructures.Tests
{
    [TestClass]
    public class AVLTreeTests
    {
        [TestMethod]
        public void AVLTree_ContainsTest()
        {
            AVLTree<int> tree = new AVLTree<int>();

            tree.InsertRange(Enumerable.Range(1, 50000).ToArray());

            // Contains item test
            for (int i = 25; i <= 50125; i++)
            {
                bool expected = 1 <= i && i <= 50000;
                bool actual = tree.Contains(i);
                Assert.AreEqual(expected, actual, $"Contains returned {actual} instead of {expected} for Contains({i})");
            }

            // Contains comparer test
            int l = 15 ;
            int r = 25000;
            Assert.IsTrue(tree.Contains(delegate(int x)
            {
                if (x > r) return -1;
                if (x >= l) return 0;
                return 1;
            }));

            l = 50110;
            r = 51200;
            Assert.IsFalse(tree.Contains(delegate(int x)
            {
                if (x > r) return -1;
                if (x >= l) return 0;
                return 1;
            }));
            
        }
    }
}