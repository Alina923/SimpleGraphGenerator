using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGraphGenerator.Model.DotElements
{
    public class Node_stmt : Stmt
    {
        public Node_id node_id = null;
        public Attr_list attr_list = new Attr_list();
        
        public override string ID { get => node_id.ID; }

        public override StmtType Type { get => StmtType.Node; }

        public Node_stmt(Node_id node_id, Attr_list attr_list)
        {
            this.node_id = node_id;
            this.attr_list = attr_list;
        }

        public Node_stmt(Node_id node_id)
        {
            this.node_id = node_id;
        }

        public override string ToCode()
        {
            StringBuilder str = new StringBuilder();

            str.Append(node_id.ToCode());

            if (attr_list.attrs.Count > 0)
                str.Append(attr_list.ToCode());

            return str.ToString();
        }

    }
}
