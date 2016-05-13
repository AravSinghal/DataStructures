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
            trie.Insert(0x2);
            trie.Insert(0x7);

            ushort actual = trie.MaximizeXor(0x4);
            
            Assert.AreEqual(expected, actual, $"Wrong value returned: {actual} instead of {expected}");
        }
    }
}