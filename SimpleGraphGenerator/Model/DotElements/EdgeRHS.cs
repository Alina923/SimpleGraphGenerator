using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGraphGenerator.Model.DotElements
{ 
    public class EdgeRHS
    {
        public GraphType edgeop;
        public Node node;

        public string ID { get => node.ID; }

        /// <summary>
        /// Edgeop param must be same as current graph type.
        /// </summary>
        public EdgeRHS(GraphType edgeop, Node node)
        {
            this.edgeop = edgeop;
            this.node = node;
        }

        public string ToCode()
        {
            StringBuilder str = new StringBuilder();

            switch (edgeop)
            {
                case GraphType.Digraph:
                    str.Append(" -> ");
                    break;
                case GraphType.Graph:
                    str.Append(" -- ");
                    break;
            }

            str.Append(node.ToCode());

            return str.ToString();
        }
    }
}
