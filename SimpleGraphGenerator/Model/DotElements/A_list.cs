using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGraphGenerator.Model.DotElements
{
    public class A_list
    {
        public List<IDisID> a_list = new List<IDisID>();

        public A_list() { }
        public A_list(List<IDisID> a_list)
        {
            this.a_list = a_list;
        }

        public string ToCode()
        {
            StringBuilder str = new StringBuilder();

            foreach (var el in a_list)
            {
                str.Append(el.ToCode() + ";");
            }

            return str.ToString();
        }
    }
}
