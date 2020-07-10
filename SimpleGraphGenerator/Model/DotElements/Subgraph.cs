using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGraphGenerator.Model.DotElements
{
    public class Subgraph : Node
    {
        public string id = null;

        public Stmt_list stmt_list = new Stmt_list();
        
        public override string ID 
        {
            get {
                StringBuilder str = new StringBuilder();
                str.Append("SUBGRAPH " + id + "{");
                foreach (var el in stmt_list.stmts)
                {
                    str.Append(el.ID +  ",");
                }
                str.Append("}");
                return str.ToString();
            }
        }

        public override StmtType Type { get => StmtType.Subgraph; }

        public Subgraph(Stmt_list stmt_list, string id = null)
        {
            this.id = id;
            this.stmt_list = stmt_list;
        }

        public override string ToCode()
        {
            StringBuilder str = new StringBuilder();

            if (id != null)
                str.Append("subgraph " + id);

            str.Append("{" + stmt_list.ToCode() + "}");

            return str.ToString();
        }
    }
}
