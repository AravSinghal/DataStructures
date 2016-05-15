using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
            /// The parent of the node.
            /// </summary>
            public Node parent;

            /// <summary>
            /// The children of this node.
            /// </summary>
            public Node[] children = new Node[2];

            /// <summary>
            /// The left child node.
            /// </summary>
            public Node Left
            {
                get { return children[0]; }
                set { children[0] = value; }
            }

            /// <summary>
            /// The right child node.
            /// </summary>
            public Node Right
            {
                get { return children[1]; }
                set { children[1] = value; }
            }

            /// <summary>
            /// Gets the index of this node in the parent's children array.
            /// </summary>
            public int ParentIndex => parent?.Left == this ? 0 : 1;

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
            public int BalanceFactor => (Left?.height ?? 0) - (Right?.height ?? 0);

            public Node(T data)
            {
                this.data = data;
            }

            /// <summary>
            /// Update the height of this node according to its child heights.
            /// </summary>
            public void UpdateHeight()
            {
                height = (byte)(Math.Max(Left?.height ?? 0, Right?.height ?? 0) + 1);
            }
        }

        /// <summary>
        /// The root node of the tree.
        /// </summary>
        private Node root;

        public T RootData => root.data;

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
            Node newRoot = root.Left;

            // Make root.left (AKA newRoot) the new root instead
            root.Left = newRoot.Right;
            newRoot.Right = root;

            newRoot.parent = root.parent;
            root.parent = newRoot;

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
            Node newRoot = root.Right;

            // Make root.right (AKA newRoot) the new root instead
            root.Right = newRoot.Left;
            newRoot.Left = root;

            newRoot.parent = root.parent;
            root.parent = newRoot;

            // Update heights
            root.UpdateHeight();
            newRoot.UpdateHeight();

            return newRoot;
        }

        /// <summary>
        /// Helper method that iteratively balances the <see cref="AVLTree{T}"/>.
        /// </summary>
        /// <param name="node">The <see cref="Node"/> to start balancing from.</param>
        private void Balance(Node node)
        {
            while (node != null)
            {
                node.UpdateHeight();

                if (node.BalanceFactor > 1)
                {
                    // Too many nodes on the left!
                    if (node.Left.BalanceFactor > 0)
                    {
                        // Left Left case
                        if (node.parent != null)
                            node.parent.children[node.ParentIndex] = RotateRight(node);
                        else
                            root = RotateRight(node);
                    }
                    else
                    {
                        // Left right case
                        node.Left = RotateLeft(node.Left);
                        if (node.parent != null)
                            node.parent.children[node.ParentIndex] = RotateRight(node);
                        else
                            root = RotateRight(node);
                    }
                }
                else if (node.BalanceFactor < -1)
                {
                    // Too many nodes on the right!
                    if (node.Right.BalanceFactor < 0)
                    {
                        // Right right case
                        if (node.parent != null)
                            node.parent.children[node.ParentIndex] = RotateLeft(node);
                        else
                            root = RotateLeft(node);
                    }
                    else
                    {
                        // Right left case
                        node.Right = RotateRight(node.Right);
                        if (node.parent != null)
                            node.parent.children[node.ParentIndex] = RotateLeft(node);
                        else
                            root = RotateLeft(node);
                    }
                }

                node = node.parent;
            }
        }

        /// <summary>
        /// Iteratively inserts <paramref name="item"/> in the <see cref="AVLTree{T}"/>.
        /// </summary>
        /// <param name="item">The value to insert in the tree.</param>
        public void Insert(T item)
        {
            if (root == null)
            {
                root = new Node(item);
                return;
            }

            Node node = root;

            while (node != null)
            {
                if (item.CompareTo(node.data) < 0)
                {
                    if (node.Left != null)
                    {
                        node = node.Left;
                        continue;
                    }

                    node.Left = new Node(item) { parent = node };
                    Balance(node);
                }
                else if (item.CompareTo(node.data) > 0)
                {
                    if (node.Right != null)
                    {
                        node = node.Right;
                        continue;
                    }

                    node.Right = new Node(item) { parent = node };
                    Balance(node);
                }

                // If we reached here, it means two things:
                // 1. We have already inserted the value
                // 2. The value already exists in the array
                // In both cases, our work is done, so exit
                return;
            }
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
                    node = node.Left;
                else if (comparer(node.data) > 0)
                    node = node.Right;
                else
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether <paramref name="item"/> exists in the <see cref="AVLTree{T}"/>.
        /// </summary>
        /// <param name="item">The item to search for in the tree.</param>
        /// <returns>A <see cref="bool"/> that states whether the value exists in the tree.`</returns>
        public bool Contains(T item)
        {
            return Contains(item.CompareTo);
        }
    }
}