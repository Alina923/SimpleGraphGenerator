using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGraphGenerator.Model.DotElements
{
    class Edge_stmt : Stmt
    {
        public Node node;
        public List<EdgeRHS> edgeRHS = new List<EdgeRHS>();
        public Attr_list attr_list = new Attr_list();

        public override string ID 
        {
            get 
            {
                StringBuilder str = new StringBuilder();
                str.Append(node.ID);
                foreach (var el in edgeRHS)
                {
                    str.Append("," + el.ID);
                }
                return str.ToString();
            } 
        }

        public override StmtType Type { get => StmtType.Edge; }

        public Edge_stmt(Node node, List<EdgeRHS> edgeRHS, Attr_list attr_list)
        {
            this.node = node;
            this.edgeRHS = edgeRHS;
            this.attr_list = attr_list;
        }

        public Edge_stmt(Node node, List<EdgeRHS> edgeRHS)
        {
            this.node = node;
            this.edgeRHS = edgeRHS;
        }

        public override string ToCode()
        {
            StringBuilder str = new StringBuilder();

            str.Append(node.ToCode() + " ");
            
            foreach(var el in edgeRHS)
            {
                str.Append(el.ToCode());
            }
            

            if(attr_list != null && attr_list.attrs.Count > 0)
                str.Append(attr_list.ToCode());

            return str.ToString();
        }
    }
}
