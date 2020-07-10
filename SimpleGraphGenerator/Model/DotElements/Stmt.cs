using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGraphGenerator.Model.DotElements
{
    public enum StmtType {Node, Edge, Subgraph, Attribute, Other};

    public abstract class Stmt
    {
        public abstract string ID { get; }
        public abstract StmtType Type { get; }

        public abstract string ToCode();
    }
}
