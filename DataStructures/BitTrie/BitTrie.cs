using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures
{
    /// <summary>
    /// A binary tree with nodes serving as bits of the inserted values. One of the most useful data structures when you need to minimize or maximize a bitwise operation.
    /// </summary>
    public class BitTrie
    {
        private class Node
        {
            public Node[] children = new Node[2];
        }

        private readonly Node root = new Node();

        /// <summary>
        /// The number of bits in the maximum value that will be inserted in the <see cref="BitTrie"/>.
        /// </summary>
        private readonly byte maxDepth;

        /// <summary>
        /// Create a new instance of <see cref="BitTrie"/>.
        /// </summary>
        /// <param name="maxDepth">The number of bits in the maximum value that will be inserted in the <see cref="BitTrie"/>. Defaults to sizeof(<see cref="ushort"/>) * 8.</param>
        public BitTrie(byte maxDepth = sizeof(ushort) * 8)
        {
            this.maxDepth = maxDepth;
        }

        /// <summary>
        /// Inserts all the numbers in <paramref name="numbers"/> in the <see cref="BitTrie"/>.
        /// </summary>
        /// <param name="numbers">The numbers to be inserted.</param>
        public void InsertRange(params ushort[] numbers)
        {
            foreach (ushort number in numbers)
                Insert(number);
        }

        /// <summary>
        /// Insert a <paramref name="number"/> in the <see cref="BitTrie"/>.
        /// </summary>
        /// <param name="number">The number to be inserted.</param>
        public void Insert(ushort number)
        {
            Node node = root;

            for (byte d = maxDepth; d > 0; d--)
            {
                uint bit = GetBit(number, d);

                if (node.children[bit] == null)
                    node.children[bit] = new Node();

                node = node.children[bit];
            }
        }

        /// <summary>
        /// Determines whether <paramref name="number"/> is in the <see cref="BitTrie"/>.
        /// </summary>
        /// <param name="number">The number to be located in the <see cref="BitTrie"/>.</param>
        /// <returns><code>true</code> if <paramref name="number"/> is in the <see cref="BitTrie"/> else returns <code>false</code>.</returns>
        public bool Contains(ushort number)
        {
            Node node = root;

            for (byte d = maxDepth; d > 0; d--)
            {
                uint bit = GetBit(number, d);

                // The branch we need doesn't exist; therefore the number is not in the tree.
                if (node.children[bit] == null)
                    return false;

                node = node.children[bit];
            }

            return true;
        }

        /// <summary>
        /// Returns the maximum value after performing the XOR operation with <paramref name="q"/> and all the values in the <see cref="BitTrie"/>.
        /// </summary>
        /// <param name="q">The value to XOR the values in the <see cref="BitTrie"/> with.</param>
        /// <returns>The maximum XOR result.</returns>
        public ushort MaximizeXor(ushort q)
        {
            Node node = root;
            ushort max = 0;

            for (byte d = maxDepth; d > 0; d--)
            {
                uint bit = GetBit(q, d);

                // We always assume that the maximum XOR of values at this bit is 1 at first
                ushort val = 1; // Maximum value to be assigned to XOR result at bit
                bit ^= val; // Get required bit instead

                if (node.children[bit] == null)
                {
                    bit = 1 - bit; // Toggle bit
                    val = 0;
                }

                max |= (ushort)(val << (d - 1));
                node = node.children[bit];
            }

            return max;
        }

        /// <summary>
        /// Helper method which returns the <paramref name="i"/> th bit (starting from LSB) of <paramref name="n"/>.
        /// </summary>
        /// <param name="n">The number whose bit is to be extracted.</param>
        /// <param name="i">The 1 based (from LSB to MSB) index of the bit to be extracted.</param>
        /// <returns>The extracted bit.</returns>
        private static uint GetBit(ushort n, byte i)
        {
            ushort mask = (ushort)(1 << (i - 1));
            return ((uint)(mask & n) >> (i - 1));
        }
    }
}
