using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGraphGenerator.Model.DotElements
{
    public enum GraphType { Graph, Digraph }

    public class Graph
    {
        public bool strict = false;
        public GraphType type = GraphType.Graph;
        public string id = "";
        public Stmt_list stmt_list;

        public Graph(GraphType type, Stmt_list stmt_list, string id = "", bool strict = false)
        {
            this.type = type;
            this.stmt_list = stmt_list;
            this.id = id;
            this.strict = strict;
        }

        public string ToCode()
        {
            StringBuilder str = new StringBuilder();

            str.Append(strict ? "strict " : "");

            switch (type)
            {
                case GraphType.Digraph:
                    str.Append("digraph ");
                    break;
                case GraphType.Graph:
                    str.Append("graph ");
                    break;
            }

            str.Append(id);

            str.Append("{\n" + stmt_list.ToCode() + "}");

            return str.ToString();
        }
    }
}
