using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGraphGenerator.Model.DotElements
{
    public class Attr_list
    {
        public List<A_list> attrs = new List<A_list>();

        public Attr_list() { }
        public Attr_list(List<A_list> attrs)
        {
            this.attrs = attrs;
        }

        public string ToCode()
        {
            StringBuilder str = new StringBuilder();

            foreach (var el in attrs)
            {
                str.Append("[" + el.ToCode() + "]");
            }

            return str.ToString();
        }
    }
}
