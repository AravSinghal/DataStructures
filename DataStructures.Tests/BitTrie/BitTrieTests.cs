using DataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataStructures.Tests
{
    [TestClass]
    public class BitTrieTests
    {
        [TestMethod]
        public void MaximizeXorTest()
        {
            ushort expected = 0x6;

            BitTrie trie = new BitTrie(3);
            trie.InsertRange(0x2, 0x7);

            ushort actual = trie.MaximizeXor(0x4);

            Assert.AreEqual(expected, actual, $"Wrong value returned: {actual} instead of {expected}");
        }

        [TestMethod]
        public void ContainsTest()
        {
            BitTrie trie = new BitTrie(3);
            trie.InsertRange(0x2, 0x5, 0x7);

            Assert.IsTrue(trie.Contains(0x5), $"Trie contains {0x5} but returned {false}");

            Assert.IsFalse(trie.Contains(0x6), $"Trie doesn't contain {0x6} but returned {true}");
        }
    }
}