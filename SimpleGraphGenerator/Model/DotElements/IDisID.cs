using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGraphGenerator.Model.DotElements
{
    public class IDisID : Stmt
    {
        public string id1 = "", id2 = "";

        public IDisID(string id1, string id2)
        {
            this.id1 = id1;
            this.id2 = id2;
        }

        public override string ID { get => id1 + "," + id2; }

        public override StmtType Type { get => StmtType.Other; }

        public override string ToCode()
        {
            return id1 + "=" + id2;
        }
    }
}
