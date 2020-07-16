using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGraphGenerator.Model.DotElements
{
    public enum AttrType {Graph, Node, Edge, SpecificNode, SpecificEdge}

    public class Attr_stmt : Stmt
    {
        public AttrType type = AttrType.Graph;
        public Attr_list attr_list;

        public override string ID { get => "ATTRIBUTE_STATEMENT"; }

        public override StmtType Type { get => StmtType.Attribute; }

        public Attr_stmt(AttrType type, Attr_list attr_list)
        {
            this.type = type;
            this.attr_list = attr_list;
        }

        public override string ToCode()
        {
            StringBuilder str = new StringBuilder();

            switch (type)
            {
                case AttrType.Graph:
                    str.Append("graph");
                    break;
                case AttrType.Edge:
                    str.Append("edge");
                    break;
                case AttrType.Node:
                    str.Append("node");
                    break;
            }

            str.Append(attr_list.ToCode());

            return str.ToString();
        }
    }
}
