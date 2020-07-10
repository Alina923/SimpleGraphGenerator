using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGraphGenerator.Model.DotElements
{
    public class Node_id : Node
    {
        public string id = "";
        public Port port = null;

        public Node_id(string id, Port port = null)
        {
            this.id = id;
            this.port = port;
        }

        public override string ID { get => id; }

        public override StmtType Type { get => StmtType.Node; }

        public override string ToCode()
        {
            StringBuilder str = new StringBuilder();

            str.Append(id);

            if (port != null)
                str.Append(port.ToCode());

            return str.ToString();
        }
    }
}
