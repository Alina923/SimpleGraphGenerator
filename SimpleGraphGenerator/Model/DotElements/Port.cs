using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGraphGenerator.Model.DotElements
{
    public enum Compass_pt {N, Ne, E, Se, S, Sw, W, Nw, C, None, Null};

    public class Port
    {
        public string id = null;
        public Compass_pt compass_pt = Compass_pt.Null;

        public Port(string id, Compass_pt compass_pt = Compass_pt.Null)
        {
            this.id = id;
            this.compass_pt = compass_pt;
        }
        public Port(Compass_pt compass_pt)
        {
            this.compass_pt = compass_pt;
        }

        public string ToCode()
        {
            StringBuilder str = new StringBuilder();

            if (id != null)
                str.Append(":" + id);

            switch (compass_pt)
            {
                case Compass_pt.N:
                    str.Append(":n");
                    break;
                case Compass_pt.Ne:
                    str.Append(":ne");
                    break;
                case Compass_pt.E:
                    str.Append(":e");
                    break;
                case Compass_pt.Se:
                    str.Append(":se");
                    break;
                case Compass_pt.S:
                    str.Append(":s");
                    break;
                case Compass_pt.Sw:
                    str.Append(":sw");
                    break;
                case Compass_pt.W:
                    str.Append(":w");
                    break;
                case Compass_pt.Nw:
                    str.Append(":nw");
                    break;
                case Compass_pt.C:
                    str.Append(":c");
                    break;
                case Compass_pt.None:
                    str.Append(":_");
                    break;
            }

            return str.ToString();
        }
    }
}
