using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures
{
    public class BitTrie
    {
        public class Node
        {
            public Node[] children = new Node[2];
        }

        private Node root = new Node();

        /// <summary>
        /// The number of bits in the maximum number that will be inserted into the tree.
        /// </summary>
        private readonly byte maxDepth;

        public BitTrie(byte maxDepth = sizeof(ushort) * 8)
        {
            this.maxDepth = maxDepth;
        }

        public void Insert(ushort number)
        {
            Node node = root;

            for (byte d = maxDepth; d > 0; d--)
            {
                ushort mask = (ushort)(1 << (d - 1));
                ushort bit = (ushort)((uint)(mask & number) >> (d - 1));

                if (node.children[bit] == null)
                    node.children[bit] = new Node();

                node = node.children[bit];
            }
        }

        public ushort MaximizeXor(ushort q)
        {
            Node node = root;
            ushort max = 0;

            for (byte d = maxDepth; d > 0; d--)
            {
                ushort mask = (ushort)(1 << (d - 1));
                ushort bit = (ushort)((uint)(mask & q) >> (d - 1));

                // We always assume that the maximum XOR of values at this bit is 1 at first
                ushort val = 1; // Maximum value to be assigned to XOR result at bit
                bit ^= val; // Get required bit instead

                if (node.children[bit] == null)
                {
                    bit = (ushort)(1 - bit); // Toggle bit
                    val = 0;
                }

                max += (ushort)(val << (d - 1));
                node = node.children[bit];
            }

            return max;
        }
    }
}
