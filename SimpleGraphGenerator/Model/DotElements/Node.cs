using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGraphGenerator.Model.DotElements
{
    public abstract class Node : Stmt
    {
        public override abstract string ToCode();
    }
}
