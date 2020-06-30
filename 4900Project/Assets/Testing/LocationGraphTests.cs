using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;

namespace Tests
{
    public class LocationGraphUnitTests
    {
        [Test]
        public void TestAddNodesAndEdges()
        {
            // Create graph
            OverworldMap.LocationGraph graph;
            OverworldMap.LocationNode n1, n2, n3;

            n1 = new OverworldMap.LocationNode(1, "Town", OverworldMap.LocationType.TOWN, 0.0f, 0.0f);
            n2 = new OverworldMap.LocationNode(2, "Resource", OverworldMap.LocationType.RESOURCE, 1.0f, 1.0f);
            n3 = new OverworldMap.LocationNode(3, "Event", OverworldMap.LocationType.EVENT, 1.0f, -1.0f);

            graph = new OverworldMap.LocationGraph()
                .AddNode(n1).AddNode(n2).AddNode(n3)
                .AddEdge(n1, n2).AddEdge(n2, n3);

            // Check edges
            Assert.IsTrue(graph.HasEdge(n1, n2));
            Assert.IsTrue(graph.HasEdge(n2, n3));
            Assert.IsFalse(graph.HasEdge(n1, n3));

            // Run verify
            Assert.IsTrue(graph.VerifyGraph());
        }

        [Test]
        public void TestRemove()
        {
            // Create Graph
            OverworldMap.LocationGraph graph;
            OverworldMap.LocationNode n1, n2, n3;

            n1 = new OverworldMap.LocationNode(1, "Town", OverworldMap.LocationType.TOWN, 0.0f, 0.0f);
            n2 = new OverworldMap.LocationNode(2, "Resource", OverworldMap.LocationType.RESOURCE, 1.0f, 1.0f);
            n3 = new OverworldMap.LocationNode(3, "Event", OverworldMap.LocationType.EVENT, 1.0f, -1.0f);

            graph = new OverworldMap.LocationGraph()
                .AddNode(n1).AddNode(n2).AddNode(n3)
                .AddEdge(n1, n2).AddEdge(n2, n3);

            // Remove Edge
            Assert.AreEqual(2, graph.EdgeCount());
            graph.RemoveEdge(n2, n3);
            Assert.AreEqual(1, graph.EdgeCount());

            // Remove Node with no edges
            Assert.AreEqual(3, graph.NodeCount());
            graph.RemoveNode(n3);
            Assert.AreEqual(2, graph.NodeCount());

            // Remove Node with edges
            graph.RemoveNode(n2);
            Assert.AreEqual(1, graph.NodeCount());
            Assert.AreEqual(0, graph.EdgeCount());
        }

        [Test]
        public void TestEnumerables()
        {
            // Create graph
            OverworldMap.LocationGraph graph;
            OverworldMap.LocationNode n1, n2, n3;

            n1 = new OverworldMap.LocationNode(1, "Town", OverworldMap.LocationType.TOWN, 0.0f, 0.0f);
            n2 = new OverworldMap.LocationNode(2, "Resource", OverworldMap.LocationType.RESOURCE, 1.0f, 1.0f);
            n3 = new OverworldMap.LocationNode(3, "Event", OverworldMap.LocationType.EVENT, 1.0f, -1.0f);

            graph = new OverworldMap.LocationGraph()
                .AddNode(n1).AddNode(n2).AddNode(n3)
                .AddEdge(n1, n2).AddEdge(n2, n3);

            // List expectations
            List<int> expectedNodeIds = new List<int> { n1.Id, n2.Id, n3.Id };
            List<Tuple<int, int>> expectedEdges = new List<Tuple<int, int>> { new Tuple<int, int>(n1.Id, n2.Id), new Tuple<int, int>(n2.Id, n3.Id) };

            // Get actual
            List<int> nodeIds = new List<int>();
            foreach (var n in graph.GetNodeEnumerable())
            {
                nodeIds.Add(n.Id);
            }

            List<Tuple<int, int>> edges = new List<Tuple<int, int>>();
            foreach (var e in graph.GetEdgeEnumerable())
            {
                edges.Add(new Tuple<int, int>(e.Item1.Id, e.Item2.Id));
            }

            // Assert expected == actual
            Assert.AreEqual(3, nodeIds.Count);
            Assert.AreEqual(2, edges.Count);

            Assert.IsTrue(nodeIds.All(expectedNodeIds.Contains));
            Assert.IsTrue(edges.All(expectedEdges.Contains));
        }

        [Test]
        public void TestBadGraphFunctions()
        {
            // Create graph
            OverworldMap.LocationGraph graph;
            OverworldMap.LocationNode n1, n2, n3, n4;

            n1 = new OverworldMap.LocationNode(1, "Town", OverworldMap.LocationType.TOWN, 0.0f, 0.0f);
            n2 = new OverworldMap.LocationNode(2, "Resource", OverworldMap.LocationType.RESOURCE, 1.0f, 1.0f);
            n3 = new OverworldMap.LocationNode(3, "Event", OverworldMap.LocationType.EVENT, 1.0f, -1.0f);
            n4 = new OverworldMap.LocationNode(4, "Another Event", OverworldMap.LocationType.EVENT, 2.0f, -1.0f);

            graph = new OverworldMap.LocationGraph()
                .AddNode(n1).AddNode(n2).AddNode(n3)
                .AddEdge(n1, n2).AddEdge(n2, n3);

            // Run exception testing
            bool asserts;

            // Bad node add
            asserts = false;
            try { graph.AddNode(n1); } catch (Exception e) { asserts = true; }
            Assert.IsTrue(asserts);

            // Bad edge add
            asserts = false;
            try { graph.AddEdge(n1, n2); } catch (Exception e) { asserts = true; }
            Assert.IsTrue(asserts);

            // Bad node remove
            asserts = false;
            try { graph.RemoveNode(n4); } catch (Exception e) { asserts = true; }
            Assert.IsTrue(asserts);

            // Bad edge remove
            asserts = false;
            try { graph.RemoveEdge(n1, n3); } catch (Exception e) { asserts = true; }
            Assert.IsTrue(asserts);
        }
    }
}
