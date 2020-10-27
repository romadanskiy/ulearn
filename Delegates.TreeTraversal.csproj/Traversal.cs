using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.TreeTraversal
{
    public static class Traversal
    {
        public static IEnumerable<Product> GetProducts(ProductCategory root)
        {
            return DoTraversal(
                root,
                r => r.Products.Count > 0,
                r => r.Products,
                r => r.Categories
            );
        }

        public static IEnumerable<Job> GetEndJobs(Job root)
        {
            return DoTraversal(
                root,
                r => r.Subjobs.Count == 0,
                r => new[] {r},
                r => r.Subjobs
            );
        }

        public static IEnumerable<T> GetBinaryTreeValues<T>(BinaryTree<T> root)
        {
            return DoTraversal(
                root,
                r => r.Left == null && r.Right == null,
                r => new[] {r.Value},
                r => new[] {r.Left, r.Right}
            );
        }


        private static IEnumerable<TValue> DoTraversal<TRoot, TValue>(TRoot root, 
            Func<TRoot, bool> check,
            Func<TRoot, IEnumerable<TValue>> valuesOf,
            Func<TRoot, IEnumerable<TRoot>> childrenOf)
        {
            if (root == null) 
                yield break;

            if (check(root))
                foreach (var value in valuesOf(root))
                    yield return value;

            foreach (var nextRoot in childrenOf(root))
                foreach (var value in DoTraversal(nextRoot, check, valuesOf, childrenOf))
                    yield return value;
        }
    }
}
