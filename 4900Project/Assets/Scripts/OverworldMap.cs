using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using UnityEngine;

/// <summary>
/// Model for the overworld map
/// </summary>
public class OverworldMap
{
    /// <summary>
    /// Specifies what exists at the location
    /// </summary>
    public enum LocationType
    {
        TOWN,
        RESOURCE,
        EVENT,
        NONE
    }

    /// <summary>
    /// Holds information about a single location
    /// </summary>
    public partial class LocationNode
    {
        private static int nextId = 0;
        public int Id { get; }

        public string Name { get; }
        public LocationType Type { get; }
        public int LocationId { get; }
        public float PosX { get; }
        public float PosY { get; }
        public Sprite Icon { get; }

        public LocationNode(int locationId, string name, LocationType type, float posX, float posY, Sprite icon = default(Sprite))
        {
            Id = nextId++;
            LocationId = locationId;
            Name = name;
            Type = type;
            PosX = posX;
            PosY = posY;
        }


        public LocationNode(int locationId, string name, float posX, float posY, int id, Sprite icon = default(Sprite))
        {
            Id = nextId++;
            LocationId = locationId;
            Name = name;
            Type = LocationType.TOWN;
            PosX = posX;
            PosY = posY;
            Icon = icon;
        }
    }

    /// <summary>
    /// A bi-directional graph for storing locations.
    /// Contains basic graph operations.
    /// 
    /// Currently does not prevent self-loops or repeat edges
    /// Also no checks to ensure node/edge is present when removing nodes/edges or when adding edges
    /// </summary>

    public partial class LocationGraph
    {
        private Dictionary<int, LocationNode> nodes;
        private Dictionary<int, List<int>> edges;
        private bool allowSelfEdges;

        public LocationGraph(bool allowSelfEdges = false)
        {
            nodes = new Dictionary<int, LocationNode>();
            edges = new Dictionary<int, List<int>>();
            this.allowSelfEdges = allowSelfEdges;
        }

        /// <summary>
        /// Add a new node to the graph. Optionally, you can include some initial edges.
        /// </summary>
        /// <param name="node">The node to add</param>
        /// <param name="adjacentNodeIds">Optional. Used to create edges from the node.</param>
        /// <returns>This instance</returns>
        public LocationGraph AddNode(LocationNode node, List<int> adjacentNodeIds = default(List<int>))
        {
            if (nodes.ContainsKey(node.Id)) throw new ArgumentException("LocationNode already exists in the graph");

            int id = node.Id;
            nodes.Add(id, node);
            edges.Add(id, new List<int>());

            if (adjacentNodeIds != null)
            {
                adjacentNodeIds.ForEach(anId => edges[id].Add(anId));
            }

            return this;
        }

        /// <summary>
        /// Remove a node from the graph.
        /// </summary>
        /// <param name="node"> The node to remove </param>
        /// <returns>This instance</returns>
        public LocationGraph RemoveNode(LocationNode node)
        {
            return RemoveNode(node.Id);
        }

        /// <summary>
        /// Remove a node from the graph.
        /// </summary>
        /// <param name="id"> The node id to remove </param>
        /// <returns>This instance</returns>
        public LocationGraph RemoveNode(int id)
        {
            if (!nodes.ContainsKey(id)) throw new KeyNotFoundException();

            // Remove edges listed as coming from this node
            edges.Remove(id);

            // Remove edges listed as coming from other nodes 
            foreach (int k in ReverseEdgeLookup(id))
            {
                edges[k].Remove(id);
            }

            //Remove node
            nodes.Remove(id);
            return this;
        }

        /// <summary>
        /// Finds all location node ids b s.t. edges has b:[...a...] where a is given by id 
        /// </summary>
        /// <param name="id">The node to search adjacencies for</param>
        /// <returns>A list of location node ids</returns>
        private IEnumerable<int> ReverseEdgeLookup(int id)
        {
            return edges.Where(p => p.Value.Contains(id)).Select(p => p.Key);
        }

        /// <summary>
        /// Add a new edge to the graph.
        /// 
        /// Assumes that the nodes were already added.
        /// Self-edges are allowed.
        /// </summary>
        /// <param name="locA">First location</param>
        /// <param name="locB">Second location</param>
        /// <returns>This instance</returns>
        public LocationGraph AddEdge(LocationNode locA, LocationNode locB)
        {
            return AddEdge(locA.Id, locB.Id);
        }

        /// <summary>
        /// Add a new edge to the graph.
        /// 
        /// Assumes that the nodes were already added.
        /// Self-edges are allowed.
        /// </summary>
        /// <param name="aId">First location id</param>
        /// <param name="bId">Second location id</param>
        /// <returns>This instance</returns>
        public LocationGraph AddEdge(int aId, int bId)
        {
            if (!nodes.ContainsKey(aId)) throw new KeyNotFoundException("aId does not exist in nodes");
            if (!nodes.ContainsKey(bId)) throw new KeyNotFoundException("bId does not exist in nodes");
            if (HasEdge(aId, bId)) throw new ArgumentException("Edge already exists");

            edges[aId].Add(bId);
            return this;
        }

        /// <summary>
        /// Remove an edge from the graph.
        /// 
        /// Currently assumes that both nodes exist in the graph already.
        /// </summary>
        /// <param name="locA">First location</param>
        /// <param name="locB">Second location</param>
        /// <returns>This instance</returns>
        public LocationGraph RemoveEdge(LocationNode locA, LocationNode locB)
        {
            return RemoveEdge(locA.Id, locB.Id);
        }

        /// <summary>
        /// Remove an edge from the graph.
        /// 
        /// Currently assumes that both nodes exist in the graph already.
        /// </summary>
        /// <param name="aId">First location</param>
        /// <param name="bId">Second location</param>
        /// <returns>This instance</returns>
        public LocationGraph RemoveEdge(int aId, int bId)
        {
            if (!HasEdge(aId, bId)) throw new ArgumentException("Edge does not exist in graph");

            if (edges.ContainsKey(aId))
            {
                edges[aId].Remove(bId);
            }
            else
            {
                edges[bId].Remove(aId);
            }

            return this;
        }

        /// <summary>
        /// Returns whether the graph maintains an edge between the two locations.
        /// </summary>
        /// <param name="locA">First location</param>
        /// <param name="locB">Second location</param>
        /// <returns>This instance</returns>
        public bool HasEdge(LocationNode locA, LocationNode locB)
        {
            return HasEdge(locA.Id, locB.Id);
        }

        /// <summary>
        /// Returns whether the graph maintains an edge between the two locations.
        /// </summary>
        /// <param name="aId">First location id</param>
        /// <param name="bId">Second location id</param>
        /// <returns>This instance</returns>
        public bool HasEdge(int aId, int bId)
        {
            bool edgeExists = false;
            List<int> adj;
            if (edges.TryGetValue(aId, out adj) && adj.Contains(bId))
            {
                edgeExists = true;
            }
            adj = null;
            if (!edgeExists && edges.TryGetValue(bId, out adj) && adj.Contains(aId))
            {
                edgeExists = true;
            }
            return edgeExists;
        }

        /// <summary>
        /// Allows the caller to access the location nodes an an enumerable.
        /// </summary>
        /// <returns>Location node enumerator</returns>
        public IEnumerable<LocationNode> GetNodeEnumerable()
        {
            return nodes.Values.AsEnumerable();
        }

        /// <summary>
        /// Allows the caller to access the location edges an an enumerable.
        /// </summary>
        /// <returns>Location edge enumerator</returns>
        public IEnumerable<Tuple<LocationNode, LocationNode>> GetEdgeEnumerable()
        {
            return edges.SelectMany(p => p.Value.Select(adjid => new Tuple<LocationNode, LocationNode>(nodes[p.Key], nodes[adjid])));
        }

        /// <summary>
        /// Get all nodes adjacent to the given node
        /// </summary>
        /// <param name="id">Node</param>
        /// <returns>Adjacent LocationNode enumerator</returns>
        public IEnumerable<LocationNode> GetAdjacentEnumerable(LocationNode node)
        {
            return GetAdjacentEnumerable(node.Id);
        }

        /// <summary>
        /// Get all nodes adjacent to the given node
        /// </summary>
        /// <param name="id">Node id</param>
        /// <returns>Adjacent LocationNode enumerator</returns>
        public IEnumerable<LocationNode> GetAdjacentEnumerable(int id)
        {
            return edges[id].ConvertAll(e => nodes[e]).Union(ReverseEdgeLookup(id).ToList().ConvertAll(e => nodes[e]));
        }

        /// <summary>
        /// Creates an id-ordered adjacency list of the following format:
        ///     0 : 1, 2 | 1 : 0, 2 | 2 : 0, 1
        /// </summary>
        /// <returns>A string representation of the graph</returns>
        public override string ToString()
        {
            return string.Join(" | ",
                nodes.Select(p => p.Key).ToList().ConvertAll(k => String.Format("{0} : {1}", k, // Create a formated string for each key
                string.Join(",", edges[k].ToArray().Union(ReverseEdgeLookup(k))                 // Combine a:[...b...] and b:[...a...] graph relationships
                .OrderBy(n => n)))).OrderBy(n => n));                                           // Order the nodes numerically and their adjaccent nodes numerically
        }

        /// <summary>
        /// Populate node with relevant object in data, should it exist.
        /// 
        /// To use:
        ///     LocationNode node;
        ///     if (GetNode(id, out node))
        ///     {
        ///         ...
        ///     }
        /// </summary>
        /// <param name="id">ID of the node</param>
        /// <param name="node">The node to populate</param>
        /// <returns>Whether or not the out node was populated</returns>
        public bool GetNode(int id, out LocationNode node)
        {
            return nodes.TryGetValue(id, out node);
        }

        /// <summary>
        /// Empty the graph
        /// </summary>
        public void Clear()
        {
            nodes.Clear();
            edges.Clear();
        }

        /// <summary>
        /// Get the number of nodes in the graph.
        /// </summary>
        /// <returns>Num of nodes</returns>
        public int NodeCount()
        {
            return nodes.Count;
        }

        /// <summary>
        /// Get the number of edges in the graph.
        /// </summary>
        /// <returns>Num of edges</returns>
        public int EdgeCount()
        {
            return edges.Sum(kvp => kvp.Value.Count);
        }

        public bool VerifyGraph()
        {
            bool valid = true;

            // Ensure the endpoints of all edges exist in nodes
            valid &= edges.Keys.All(nodes.Keys.Contains);
            valid &= edges.Values.SelectMany(adjs => adjs).All(nodes.Keys.Contains);

            // Ensure no duplicate edges exist, counting for self-edges
            List<Tuple<int, int>> edgePairs = edges.SelectMany(kvp => kvp.Value.Select(v => new Tuple<int, int>(kvp.Key, v))).ToList();
            for (int i = 0; i < edgePairs.Count; i++)
            {
                if (!valid) break;

                Tuple<int, int> outer = edgePairs[i];

                for (int j = 0; j < edgePairs.Count; j++)
                {
                    if (i == j) continue;

                    Tuple<int, int> inner = edgePairs[j];

                    if (inner.Item1 == outer.Item2 && inner.Item2 == outer.Item1)
                    {
                        valid = false;
                        break;
                    }
                }
            }

            return valid;
        }
    }
}