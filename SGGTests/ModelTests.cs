using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleGraphGenerator.Model;
using SimpleGraphGenerator.Model.DotElements;

namespace SGGTests
{
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public void Test_GenerateGraph()
        {
            GraphVizProcessor gvp = new GraphVizProcessor();
            gvp.DotCode = "digraph{a->b}";
            gvp.GenerateGraph();

            if (!File.Exists(@".\external\dotInput.png"))
                Assert.Fail("Output image not found.");
            if (gvp.GraphImage == null)
                Assert.Fail("GraphImage property is null.");
        }

        [TestMethod]
        public void Test_ExportImage()
        {
            GraphVizProcessor gvp = new GraphVizProcessor();
            gvp.DotCode = "digraph{a->b}";
            gvp.GenerateGraph();

            gvp.ExportImage(@".\external\testImage.png");

            if (!File.Exists(@".\external\testImage.png"))
                Assert.Fail("Exported image not found.");
        }

        [TestMethod]
        public void Test_ToCode()
        {
            List<Stmt> stmtList = new List<Stmt>();
            stmtList.Add(new Node_stmt(new Node_id("a")));
            Graph graph = new Graph(GraphType.Digraph, new Stmt_list(stmtList));

            string tmp = "digraph {\na;\n}";

            Assert.AreEqual(tmp, graph.ToCode(), message: "Wrong dot code.");
        }

        [TestMethod]
        public void Test_AddNode()
        {
            GraphVizProcessor gvp = new GraphVizProcessor();
            gvp.graph = new Graph(GraphType.Digraph, new Stmt_list());

            gvp.AddNode("testNode");
            string tmp = gvp.graph.ToCode();

            if (!tmp.Contains("testNode"))
                Assert.Fail("Node not found.");
        }

        [TestMethod]
        public void Test_AddEdge()
        {
            GraphVizProcessor gvp = new GraphVizProcessor();
            gvp.graph = new Graph(GraphType.Digraph, new Stmt_list());

            gvp.AddNode("tn1");
            gvp.AddNode("tn2");
            gvp.AddEdge("tn1", "tn2");
            gvp.AddEdge("tn3", "tn4");
            gvp.AddEdge("tn2", "tn3");
            string tmp = gvp.graph.ToCode();

            if (!tmp.Contains("tn1 -> tn2"))
                Assert.Fail("Edge between existing nodes not found.");
            if (!tmp.Contains("tn2 -> tn3"))
                Assert.Fail("Edge between two other edges not found.");
            if (!tmp.Contains("tn3 -> tn4"))
                Assert.Fail("Edge between new nodes not found.");
        }

        [TestMethod]
        public void Test_AddAttribute()
        {
            GraphVizProcessor gvp = new GraphVizProcessor();
            gvp.graph = new Graph(GraphType.Digraph, new Stmt_list());

            //Graph, Node, Edge attribute
            gvp.AddAttribute(AttrType.Graph, new IDisID("x", "y"));
            string tmp = gvp.graph.ToCode();
            if (!tmp.Contains("graph [x=y;]"))
                Assert.Fail("Attr_stmt type attribute not found.");

            //Specific node attribute
            gvp.AddNode("a");
            gvp.AddAttribute(AttrType.SpecificNode, new IDisID("x", "y"), "a");
            tmp = gvp.graph.ToCode();
            if (!tmp.Contains("a [x=y;]"))
                Assert.Fail("Node attribute not found.");

            //Specific node attribute
            gvp.AddEdge("a", "b");
            gvp.AddAttribute(AttrType.SpecificEdge, new IDisID("x", "y"), "a", "b");
            tmp = gvp.graph.ToCode();
            if (!tmp.Contains("a -> b [x=y;]"))
                Assert.Fail("Edge attribute not found.");
        }

        [TestMethod]
        public void Test_NodeExists()
        {
            GraphVizProcessor gvp = new GraphVizProcessor();
            gvp.graph = new Graph(GraphType.Digraph, new Stmt_list());

            //Node exists
            gvp.AddNode("a");
            if (!gvp.NodeExists("a"))
                Assert.Fail("Node not found.");

            //Node is part of an edge
            gvp.AddEdge("a", "b");
            if (!gvp.NodeExistsInEdge("a"))
                Assert.Fail("Node in an edge not found.");
        }

        [TestMethod]
        public void Test_EdgeExists()
        {
            GraphVizProcessor gvp = new GraphVizProcessor();
            gvp.graph = new Graph(GraphType.Digraph, new Stmt_list());

            gvp.AddEdge("a", "b");
            if (!gvp.EdgeExists("a", "b"))
                Assert.Fail("Edge not found.");
        }

        [TestMethod]
        public void Test_FindNode()
        {
            GraphVizProcessor gvp = new GraphVizProcessor();
            gvp.graph = new Graph(GraphType.Digraph, new Stmt_list());

            gvp.AddNode("a");

            if (gvp.FindNode("a") == null)
                Assert.Fail("Node not found.");
        }

        [TestMethod]
        public void Test_FindEdge()
        {
            GraphVizProcessor gvp = new GraphVizProcessor();
            gvp.graph = new Graph(GraphType.Digraph, new Stmt_list());

            gvp.AddEdge("a", "b");

            if (gvp.FindEdge("a", "b") == null)
                Assert.Fail("Edge not found.");
        }

        [TestMethod]
        public void Test_RemoveNode()
        {
            GraphVizProcessor gvp = new GraphVizProcessor();
            gvp.graph = new Graph(GraphType.Digraph, new Stmt_list());

            gvp.AddNode("a");
            gvp.RemoveNode("a");
            if (gvp.NodeExists("a"))
                Assert.Fail("Node found after removal.");

            gvp.AddEdge("a", "b");
            gvp.RemoveNode("a");
            if (gvp.NodeExists("a"))
                Assert.Fail("Node found after removal.");
            if (!gvp.NodeExists("b"))
                Assert.Fail("Second node of and edge not found.");
        }

        [TestMethod]
        public void Test_RemoveEdge()
        {
            GraphVizProcessor gvp = new GraphVizProcessor();
            gvp.graph = new Graph(GraphType.Digraph, new Stmt_list());

            gvp.AddEdge("a", "b");
            gvp.RemoveEdge("a", "b");

            if (gvp.EdgeExists("a", "b"))
                Assert.Fail("Edge found after removal.");
            if (!gvp.NodeExists("a"))
                Assert.Fail("Node not found after edge removal.");
            if (!gvp.NodeExists("b"))
                Assert.Fail("Node not found after edge removal.");
        }

        [TestMethod]
        public void Test_RemoveAttribute()
        {
            GraphVizProcessor gvp = new GraphVizProcessor();
            gvp.graph = new Graph(GraphType.Digraph, new Stmt_list());

            //Graph, Node, Edge attribute
            gvp.AddAttribute(AttrType.Graph, new IDisID("x", "y"));
            gvp.RemoveAttribute(AttrType.Graph, new IDisID("x", "y"));
            string tmp = gvp.graph.ToCode();
            if (tmp.Contains("graph [x=y;]"))
                Assert.Fail("Attr_stmt type attribute found after removal.");

            //Specific node attribute
            gvp.AddNode("a");
            gvp.AddAttribute(AttrType.SpecificNode, new IDisID("x", "y"), "a");
            gvp.RemoveAttribute(AttrType.SpecificNode, new IDisID("x", "y"), "a");
            tmp = gvp.graph.ToCode();
            if (tmp.Contains("a [x=y;]"))
                Assert.Fail("Node attribute found after removal.");

            //Specific node attribute
            gvp.AddEdge("a", "b");
            gvp.AddAttribute(AttrType.SpecificEdge, new IDisID("x", "y"), "a", "b");
            gvp.RemoveAttribute(AttrType.SpecificEdge, new IDisID("x", "y"), "a", "b");
            tmp = gvp.graph.ToCode();
            if (tmp.Contains("a -> b [x=y;]"))
                Assert.Fail("Edge attribute found after removal.");
        }

        
        [TestMethod]
        public void Test_ChangeGraphType()
        {
            GraphVizProcessor gvp = new GraphVizProcessor();
            gvp.graph = new Graph(GraphType.Digraph, new Stmt_list());
            gvp.AddEdge("a","b");

            if (!gvp.graph.ToCode().Contains("->"))
                Assert.Fail("Edge type is not equal to grapf type before change.");

            gvp.ChangeGraphType(GraphType.Graph);

            if (!gvp.graph.ToCode().Contains("--"))
                Assert.Fail("Edge type is not equal to graph type after change.");
        }

        [TestMethod]
        public void Test_ClearGraph()
        {
            GraphVizProcessor gvp = new GraphVizProcessor();
            gvp.graph = new Graph(GraphType.Digraph, new Stmt_list());
            
            gvp.AddNode("a");

            gvp.ClearGraph();

            if (gvp.NodeExists("a"))
                Assert.Fail("Node found after clearing graph.");
        }
    }
}
