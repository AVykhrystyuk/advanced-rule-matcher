using System.Buffers;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedRuleMatcher.Impl.Generic
{
    public class PrefixTreeSearchEngine<T> where T : class
    {
        private readonly Node root;

        public PrefixTreeSearchEngine(Node root) =>
            this.root = root;

        public Node? Match(IReadOnlyList<object> searchFields)
        {
            var node = root;

            foreach (var searchField in searchFields)
            {
                var nextNode = node.GetChildNodeOrDefault(searchField);
                if (nextNode == null)
                    return null;

                node = nextNode;
            }

            return node;
        }

        public static PrefixTreeSearchEngine<T> Make(IReadOnlyList<T> items, IReadOnlyList<LookupInfo<T>> lookups)
        {
            var rootNode = new Node("Root");
            PopulateNode(rootNode, items, lookups);
            return new PrefixTreeSearchEngine<T>(rootNode);
        }

        private static void PopulateNode(Node node, IReadOnlyList<T> items, IReadOnlyList<LookupInfo<T>> lookups, int lookupIndex = 0)
        {
            if (lookupIndex > 0)
            {
                var prevLookup = lookups[lookupIndex - 1];
                if (prevLookup.PayloadSelector != null)
                    node.Payload = prevLookup.PayloadSelector(items);
            }

            var isLeaf = lookupIndex == lookups.Count;
            if (isLeaf)
                return; 

            var lookup = lookups[lookupIndex];

            var itemsLookup = items.ToLookup(lookup.KeySelector);
            var anyItems = itemsLookup[lookup.AnyKey];
            var otherItems = itemsLookup.Where(group => group.Key != lookup.AnyKey);

            foreach (var group in otherItems)
            {
                var childNode = new Node(group.Key);
                PopulateNode(childNode, items: group.Concat(anyItems).ToArray(), lookups, lookupIndex + 1);
                node.AddChild(childNode);
            }

            node.DefaultNode = new Node(lookup.AnyKey);
            PopulateNode(node.DefaultNode, items: anyItems.ToArray(), lookups, lookupIndex + 1);
        }

        [System.Diagnostics.DebuggerDisplay("Key = {Key} | ChildrenCount = {children.Count}")]
        public class Node
        {
            private readonly Dictionary<object, Node> children = new();

            public Node(object key)
            {
                Key = key;
            }

            public object Key { get; }

            public T? Payload { get; set; }

            public Node? DefaultNode { get; set; }

            public Node? GetChildNodeOrDefault(object key) => children!.GetValueOrDefault(key, DefaultNode);

            public void AddChild(Node node) => children.Add(node.Key, node);
        }
    }
}
