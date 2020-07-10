using System.Collections.Generic;
using System.Text;

namespace SimpleGraphGenerator.Model.DotElements
{
    public class Stmt_list
    {
        public List<Stmt> stmts = new List<Stmt>();

        public Stmt_list() { }
        public Stmt_list(List<Stmt> stmts)
        {
            this.stmts = stmts;
        }

        public string ToCode()
        {
            StringBuilder str = new StringBuilder();

            foreach(var el in stmts)
            {
                str.Append(el.ToCode()+";\n");
            }

            return str.ToString();
        }
    }
}