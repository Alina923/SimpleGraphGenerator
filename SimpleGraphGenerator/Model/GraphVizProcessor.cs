using SimpleGraphGenerator.Model.DotElements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SimpleGraphGenerator.Model
{
    public class GraphVizProcessor
    {
        private BitmapImage _graphImage = null;
        private string _dotCode = null; 
        public Graph graph = null;

        public BitmapImage GraphImage { get => _graphImage; set => _graphImage = value; }
        public string DotCode { get => _dotCode; set => _dotCode = value; }

        //public void GraphToDotCode()
        //{
        //    DotCode = graph.ToCode();
        //}

        private BitmapImage LoadImage(string path)
        {
            if (!File.Exists(path))
            {
                path = @".\external\Error.png";
            }

            BitmapImage bmi = new BitmapImage();
            bmi.BeginInit();
            bmi.CacheOption = BitmapCacheOption.OnLoad;
            bmi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            bmi.UriSource = new Uri(System.IO.Path.GetFullPath(path), UriKind.Absolute);
            bmi.EndInit();
            return bmi;
        }

        /// <summary>
        /// Generates graph from DotCode and puts it into GraphImage.
        /// </summary>
        public void GenerateGraph()
        {
            string exec = @".\external\dot.exe";
            string dotPath = @".\external\dotInput";

            File.Delete(dotPath);
            File.Delete(dotPath + ".png");

            File.WriteAllText(dotPath, DotCode);

            System.Diagnostics.Process process = new System.Diagnostics.Process();

            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.StartInfo.FileName = exec;
            process.StartInfo.Arguments = "-Tpng -O " + dotPath;

            process.Start();
            process.WaitForExit();


            GraphImage = LoadImage(dotPath + ".png");
        }

        public void ExportImage(string path)
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(GraphImage));
            using (var fileStream = new System.IO.FileStream(path, System.IO.FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }

        public void RemoveNode(string id)
        {
            if(id != null)
            {
                List<Stmt> newStmts = new List<Stmt>();

                newStmts.AddRange(graph.stmt_list.stmts);

                foreach (var el in graph.stmt_list.stmts)
                {
                    if (el.Type == StmtType.Node && id == el.ID)
                    {
                        newStmts.Remove(el);
                        graph.stmt_list.stmts = newStmts;
                    }
                    else if (el.Type == StmtType.Edge && el.ID.Contains(id))
                    {
                        RemoveEdge(id);
                        newStmts.Clear();
                        newStmts.AddRange(graph.stmt_list.stmts);
                        RemoveNode(id);
                    }
                }
            }
        }

        public void ClearGraph()
        {
            graph.stmt_list.stmts.Clear();
        }

        public void RemoveEdge(string id1, string id2 = null)
        {
            if(id1 != null)
            {
                List<Stmt> newStmts = new List<Stmt>();

                newStmts.AddRange(graph.stmt_list.stmts);

                if (id2 != null)
                {
                    foreach (var el in graph.stmt_list.stmts)
                    {
                        if (el.Type == StmtType.Edge && el.ID.Contains(id1) && el.ID.Contains(id2))
                        {
                            newStmts.Remove(el);
                        }
                    }
                    graph.stmt_list.stmts = newStmts;

                    if (!NodeExists(id1))
                        graph.stmt_list.stmts.Add(new Node_stmt(new Node_id(id1)));
                    if (!NodeExists(id2))
                        graph.stmt_list.stmts.Add(new Node_stmt(new Node_id(id2)));
                }
                else
                {
                    foreach (var el in graph.stmt_list.stmts)
                    {
                        if (el.Type == StmtType.Edge && el.ID.Contains(id1))
                        {
                            string newId2 = el.ID.Replace(id1, "");
                            newId2 = newId2.Replace(",", "");
                            newStmts.Remove(el);
                            if (!NodeExists(id1))
                                newStmts.Add(new Node_stmt(new Node_id(id1)));
                            if (!NodeExists(newId2))
                                newStmts.Add(new Node_stmt(new Node_id(newId2)));

                        }
                    }

                    graph.stmt_list.stmts = newStmts;
                }
            }
        }

        public void AddNode(string id)
        {
            if (!NodeExists(id))
            {
                graph.stmt_list.stmts.Add(new Node_stmt(new Node_id(id)));
            }
        }

        public void AddEdge(string id1, string id2, Attr_list attrs = null)
        {
            if (!EdgeExists(id1, id2))
            {
                List<Stmt> newStmts = new List<Stmt>();

                newStmts.AddRange(graph.stmt_list.stmts);

                foreach (var el in graph.stmt_list.stmts)
                {
                    if ((el.Type == StmtType.Node && id1 == el.ID) || (el.Type == StmtType.Node && id2 == el.ID))
                    {
                        if ((el as Node_stmt).attr_list.attrs.Count < 1)
                            newStmts.Remove(el);
                    }
                }

                graph.stmt_list.stmts = newStmts;

                if (id1 != null && id2 != null)
                {
                    List<EdgeRHS> edge = new List<EdgeRHS>();
                    edge.Add(new EdgeRHS(graph.type, new Node_id(id2)));

                    if (attrs != null)
                    {
                        graph.stmt_list.stmts.Add(
                        new Edge_stmt(
                            new Node_id(id1),
                            edge,
                            attrs));
                    }
                    else
                    {
                        graph.stmt_list.stmts.Add(
                        new Edge_stmt(
                            new Node_id(id1),
                            edge));
                    }
                    
                }
            }     
        }

        public void ChangeGraphType(GraphType newType)
        {
            if (graph.type != newType)
            {
                graph.type = newType;
                foreach (var el in graph.stmt_list.stmts)
                {
                    if (el.Type == StmtType.Edge)
                    {
                        string id1 = el.ID.Split(',')[0];
                        string id2 = el.ID.Split(',')[1];
                        var attrs = (el as Edge_stmt).attr_list;

                        RemoveEdge(id1, id2);
                        AddEdge(id1, id2, attrs);
                    }
                }
            }
        }
        
        public bool NodeExists(string id)
        {
            if (id != null)
            {
                foreach (var el in graph.stmt_list.stmts)
                {
                    if (el.Type == StmtType.Node && id == el.ID)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool NodeExistsInEdge(string id)
        {
            if (id != null)
            {
                foreach (var el in graph.stmt_list.stmts)
                {
                    if (el.Type == StmtType.Edge && el.ID.Contains(id))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool EdgeExists(string id1, string id2)
        {
            if (id1 != null && id2 != null)
            {
                foreach (var el in graph.stmt_list.stmts)
                {
                    if (el.Type == StmtType.Edge && el.ID.Contains(id1) && el.ID.Contains(id2))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public Node_stmt FindNode(string id)
        {
            foreach (var el in graph.stmt_list.stmts)
            {
                if (el.Type == StmtType.Node && id == el.ID)
                {
                    return el as Node_stmt;
                }
            }
            return null;
        }

        public Edge_stmt FindEdge(string id1, string id2)
        {
            if (id1 != null && id2 != null)
            {
                foreach (var el in graph.stmt_list.stmts)
                {
                    if (el.Type == StmtType.Edge && el.ID.Contains(id1) && el.ID.Contains(id2))
                    {
                        return el as Edge_stmt;
                    }
                }
            }
            return null;
        }

        public bool AddAttribute(AttrType type, IDisID attr, string id1 = null, string id2= null)
        {
            bool result = false;

            List<IDisID> idisidList = new List<IDisID>();
            idisidList.Add(attr);

            A_list a_list = new A_list(idisidList);
            List<A_list> a_listList = new List<A_list>();
            a_listList.Add(a_list);

            Attr_list attr_list = new Attr_list(a_listList);

            if (type == AttrType.SpecificNode)
            {
                if (NodeExists(id1))
                {
                    Node_stmt focusNode = FindNode(id1);
                    focusNode.attr_list.attrs.AddRange(attr_list.attrs);
                    result = true;
                }
                else if(NodeExistsInEdge(id1))
                {
                    AddNode(id1);
                    Node_stmt focusNode = FindNode(id1);
                    focusNode.attr_list.attrs.AddRange(attr_list.attrs);
                    result = true;
                }
            }
            else if(type == AttrType.SpecificEdge)
            {
                if (EdgeExists(id1, id2))
                {
                    Edge_stmt focusEdge = FindEdge(id1, id2);
                    focusEdge.attr_list.attrs.AddRange(attr_list.attrs);
                    result = true;
                }                
            }
            else
            {
                graph.stmt_list.stmts.Insert(0,
                    new Attr_stmt(type, attr_list));
                result = true;
            }
            return result;
        }

        public bool RemoveAttribute(AttrType type, IDisID attr, string id1 = null, string id2 = null)
        {
            if (type == AttrType.SpecificNode)
            {
                foreach (var st in graph.stmt_list.stmts)
                {
                    if (st.Type == StmtType.Node && st.ID == (id1))
                    {
                        foreach (var at_ls in (st as Node_stmt).attr_list.attrs)
                        {
                            foreach (var at in at_ls.a_list)
                            {
                                if (at.ID == attr.ID)
                                {
                                    (st as Node_stmt).attr_list.attrs.Remove(at_ls);
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            else if (type == AttrType.SpecificEdge)
            {
                foreach (var st in graph.stmt_list.stmts)
                {
                    if (st.Type == StmtType.Edge && st.ID.Contains(id1) && st.ID.Contains(id2))
                    {
                        foreach (var at_ls in (st as Edge_stmt).attr_list.attrs)
                        {
                            foreach (var at in at_ls.a_list)
                            {
                                if (at.ID == attr.ID)
                                {
                                    (st as Edge_stmt).attr_list.attrs.Remove(at_ls);
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (var st in graph.stmt_list.stmts)
                {
                    if (st.Type == StmtType.Attribute)
                    {
                        foreach (var at_ls in (st as Attr_stmt).attr_list.attrs)
                        {
                            foreach (var at in at_ls.a_list)
                            {
                                if (at.ID == attr.ID)
                                {
                                    graph.stmt_list.stmts.Remove(st);
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
