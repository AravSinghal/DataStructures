using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures
{
    /// <summary>
    /// Represents a self balanced sorted binary tree. A quintessential data structure with O(log N) look up, search, insertion and deletion complexity whilst using just O(N) space.
    /// </summary>
    /// <typeparam name="T">The type of data that will be stored in this tree. Must implement <see cref="IComparable{T}"/> so that node values can be compared.</typeparam>
    public class AVLTree<T> where T : IComparable<T>
    {
        private class Node
        {
            /// <summary>
            /// The left child node.
            /// </summary>
            public Node left;

            /// <summary>
            /// The right child node.
            /// </summary>
            public Node right;

            /// <summary>
            /// The data in the node.
            /// </summary>
            public T data;

            // We could use a getter property, but
            // We're storing heights instead for less computation
            /// <summary>
            /// The height of the subtree at this node.
            /// </summary>
            public byte height = 1;

            /// <summary>
            /// The difference in heights of the left child and the right child.
            /// </summary>
            public int BalanceFactor => (left?.height ?? 0) - (right?.height ?? 0);

            public Node(T data)
            {
                this.data = data;
            }

            /// <summary>
            /// Update the height of this node according to its child heights.
            /// </summary>
            public void UpdateHeight()
            {
                height = (byte)(Math.Max(left?.height ?? 0, right?.height ?? 0) + 1);
            }
        }

        /// <summary>
        /// The root node of the tree.
        /// </summary>
        private Node root;

        /// <summary>
        /// The number of values inserted into the tree.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Helper method that right rotates the subtree at <paramref name="root"/>
        /// </summary>
        /// <param name="root">The root of the subtree to rotate.</param>
        /// <returns>The root of the rotated subtree.</returns>
        private static Node RotateRight(Node root)
        {
            Node newRoot = root.left;

            // Make root.left (AKA newRoot) the new root instead
            root.left = newRoot.right;
            newRoot.right = root;

            // Update heights
            root.UpdateHeight();
            newRoot.UpdateHeight();

            return newRoot;
        }

        /// <summary>
        /// Helper method that left rotates the subtree at <paramref name="root"/>
        /// </summary>
        /// <param name="root">The root of the subtree to rotate.</param>
        /// <returns>The root of the rotated subtree.</returns>
        private static Node RotateLeft(Node root)
        {
            Node newRoot = root.right;

            // Make root.right (AKA newRoot) the new root instead
            root.right = newRoot.left;
            newRoot.left = root;

            // Update heights
            root.UpdateHeight();
            newRoot.UpdateHeight();

            return newRoot;
        }

        /// <summary>
        /// Helper method that recursively inserts <paramref name="data"/> in the subtree with root <paramref name="node"/>.
        /// </summary>
        /// <param name="node">The root of the subtree in which to insert the data.</param>
        /// <param name="data">The value to insert into the subtree.</param>
        /// <returns>The new root of the subtree.</returns>
        private static Node Insert(Node node, T data)
        {
            if (node == null)
                return new Node(data);

            if (data.CompareTo(node.data) < 0)
                node.left = Insert(node.left, data);
            else
                node.right = Insert(node.right, data);

            node.UpdateHeight();

            int balanceFactor = node.BalanceFactor;

            if (balanceFactor > 1)
            {
                // Left left case
                if (data.CompareTo(node.left.data) < 0)
                    return RotateRight(node);

                // Left right case
                node.left = RotateLeft(node.left);
                return RotateRight(node);
            }

            // Balance factor is in range [-1, 1], no modifications required
            if (balanceFactor >= -1)
                return node;

            // Right right case
            if (data.CompareTo(node.right.data) > 0)
                return RotateLeft(node);

            // Right left case
            node.right = RotateRight(node.right);
            return RotateLeft(node);
        }


        /// <summary>
        /// Inserts <paramref name="item"/> into the <see cref="AVLTree{T}"/>.
        /// </summary>
        /// <param name="item">The value to insert in the tree.</param>
        public void Insert(T item)
        {
            root = Insert(root, item);
            Count++;
        }

        /// <summary>
        /// Inserts all values in <paramref name="items"/> into the <see cref="AVLTree{T}"/>
        /// </summary>
        /// <param name="items">The values to insert in the tree.</param>
        public void InsertRange(params T[] items)
        {
            foreach (T item in items)
                Insert(item);
        }

        /// <summary>
        /// Determines whether a value satisfying <paramref name="comparer"/> exists in the <see cref="AVLTree{T}"/>.
        /// </summary>
        /// <param name="comparer">The comparer function that returns an<see cref="int"/>.</param>
        /// <returns>A <see cref="bool"/> stating whether a value satisfying <paramref name="comparer"/> exists in the tree.</returns>
        public bool Contains(Func<T, int> comparer)
        {
            Node node = root;

            while (node != null)
            {
                if (comparer(node.data) < 0)
                    node = node.left;
                else if (comparer(node.data) > 0)
                    node = node.right;
                else
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether <paramref name="item"/> exists in the <see cref="AVLTree{T}"/>.
        /// </summary>
        /// <param name="item">The item to search for in the tree.</param>
        /// <returns>A <see cref="bool"/> that states whether the value exists in the tree.</returns>
        public bool Contains(T item)
        {
            return Contains(item.CompareTo);
        }
    }
}