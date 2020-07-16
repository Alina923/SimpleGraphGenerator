using Microsoft.Win32;
using SimpleGraphGenerator.Model;
using SimpleGraphGenerator.Model.DotElements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SimpleGraphGenerator.ViewModel
{
    class ViewModelMain : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private GraphVizProcessor gvp = new GraphVizProcessor();
        private string _nodeFocus;
        private string _EdgeFocus1;
        private string _EdgeFocus2;
        private string _EdgeArrowType;
        private string _EdgeArrowType2;
        private bool _firstAttrTextBoxEnabled;
        private bool _secondAttrTextBoxEnabled;
        private string _attrFocus1;
        private string _attrFocus2;
        private string _newAttrType;
        private string _attrValue;
        private string _attrName;

        public struct AttrEntity
        {
            public string name;
            public string value;
            public string focusNode1;
            public string focusNode2;
            public AttrType type;

            public AttrEntity(AttrType type, string name, string value, string focusNode1 = null, string focusNode2 = null)
            {
                this.name = name;
                this.value = value;
                this.focusNode1 = focusNode1;
                this.focusNode2 = focusNode2;
                this.type = type;
            }

            public string GetVal()
            {
                StringBuilder str = new StringBuilder();

                switch (type)
                {
                    case AttrType.Edge:
                        str.Append("All edges");
                        break;
                    case AttrType.Graph:
                        str.Append("Graph");
                        break;
                    case AttrType.Node:
                        str.Append("All nodes");
                        break;
                    case AttrType.SpecificEdge:
                        str.Append("Edge " + focusNode1 + " - " + focusNode2);
                        break;
                    case AttrType.SpecificNode:
                        str.Append("Node " + focusNode1);
                        break;
                }

                str.Append(" " + name + " = " + value);

                return str.ToString();
            }
        }

        public BitmapImage GraphImage { get => gvp.GraphImage; }
        public DelegateCommand GenerateGraph
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    gvp.GenerateGraph();
                    OnPropertyChanged(nameof(GraphImage));
                });
            }
        }

        public string DotCode
        {
            get => gvp.DotCode;
            set
            {
                if (gvp.DotCode != value)
                {
                    gvp.DotCode = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Graph_type
        {
            get => gvp.graph.type == GraphType.Graph ? "Undirected" : "Directed";
            set
            {
                var val = value.Split().Count() < 2 ? value : value.Split()[1];

                GraphType gt = val == "Undirected" ? GraphType.Graph : GraphType.Digraph;

                if (gvp.graph.type != gt)
                {
                    gvp.ChangeGraphType(gt);
                    DotCode = gvp.graph.ToCode();
                    OnPropertyChanged();
                }
            }
        }

        public string EdgeArrowType
        {
            get => _EdgeArrowType;

            set
            {
                string at = value == "Undirected" ? "--" : "->";

                if (_EdgeArrowType != at)
                {
                    _EdgeArrowType = at;
                    OnPropertyChanged();
                }

            }
        }

        public string Graph_id
        {
            get => gvp.graph.id;
            set
            {
                if (gvp.graph.id != value)
                {
                    gvp.graph.id = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool Graph_strict
        {
            get => gvp.graph.strict;
            set
            {
                if (gvp.graph.strict != value)
                {
                    gvp.graph.strict = value;
                    OnPropertyChanged();
                }
            }
        }

        public string FocusNode { get => _nodeFocus; set => _nodeFocus = value; }

        public DelegateCommand AddNode
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    gvp.AddNode(FocusNode);
                    OnPropertyChanged();
                });
            }
        }
        public DelegateCommand RemoveNode
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    gvp.RemoveNode(FocusNode);
                    OnPropertyChanged();
                });
            }
        }

        public string ConFocus1 { get => _EdgeFocus1; set => _EdgeFocus1 = value; }

        public string ConFocus2 { get => _EdgeFocus2; set => _EdgeFocus2 = value; }

        public DelegateCommand AddEdge
        {
            get
            {
                return new DelegateCommand((obj) =>
                {

                    gvp.AddEdge(ConFocus1, ConFocus2);
                    OnPropertyChanged();
                });
            }
        }

        public DelegateCommand RemoveEdge
        {
            get
            {
                return new DelegateCommand((obj) =>
                {

                    gvp.RemoveEdge(ConFocus1, ConFocus2);

                    OnPropertyChanged();
                });
            }
        }

        public DelegateCommand ClearGraph
        {
            get
            {
                return new DelegateCommand((obj) =>
                {

                    gvp.ClearGraph();

                    OnPropertyChanged();
                });
            }
        }

        public string EdgeArrowType2
        {
            get => _EdgeArrowType2;

            set
            {
                if (NewAttrType != null)
                {
                    string at;

                    var attrType = NewAttrType.Split().Count() < 2 ? NewAttrType : NewAttrType.Split()[1];
                    if (attrType == "Edge")
                    {
                        if (value == "Undirected")
                            at = "--";
                        else at = "->";
                    }
                    else
                        at = "";

                    if (_EdgeArrowType2 != at)
                    {
                        _EdgeArrowType2 = at;
                        OnPropertyChanged();
                    }
                }
            }
        }

        public string NewAttrType
        {
            get => _newAttrType;
            set
            {
                if (_newAttrType != value)
                {
                    _newAttrType = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool FirstAttrTextBoxEnabled
        {
            get => _firstAttrTextBoxEnabled;
            set
            {
                if (NewAttrType != null)
                {
                    bool at;

                    var attrType = NewAttrType.Split().Count() < 2 ? NewAttrType : NewAttrType.Split()[1];
                    if (attrType == "Node" || attrType == "Edge")
                        at = true;
                    else
                        at = false;

                    if (_firstAttrTextBoxEnabled != at)
                    {
                        _firstAttrTextBoxEnabled = at;
                        OnPropertyChanged();
                    }
                }
            }
        }

        public bool SecondAttrTextBoxEnabled
        {
            get => _secondAttrTextBoxEnabled;
            set
            {
                if (NewAttrType != null)
                {
                    bool at;

                    var attrType = NewAttrType.Split().Count() < 2 ? NewAttrType : NewAttrType.Split()[1];
                    if (attrType == "Edge")
                        at = true;
                    else
                        at = false;

                    if (_secondAttrTextBoxEnabled != at)
                    {
                        _secondAttrTextBoxEnabled = at;
                        OnPropertyChanged();
                    }
                }
            }
        }

        public string AttrFocus1 { get => _attrFocus1; set => _attrFocus1 = value; }

        public string AttrFocus2 { get => _attrFocus2; set => _attrFocus2 = value; }

        public string AttrValue { get => _attrValue; set => _attrValue = value; }

        public string AttrName { get => _attrName; set => _attrName = value; }

        public DelegateCommand AddNewAttribute
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    if (NewAttrType != null)
                    {
                        bool result = false;
                        var attrType = NewAttrType.Split()[NewAttrType.Split().Count() - 1];
                        switch (attrType)
                        {
                            case "Graph":
                                result = gvp.AddAttribute(AttrType.Graph, new IDisID(AttrName, AttrValue));
                                break;
                            case "nodes":
                                result = gvp.AddAttribute(AttrType.Node, new IDisID(AttrName, AttrValue));
                                break;
                            case "edges":
                                result = gvp.AddAttribute(AttrType.Edge, new IDisID(AttrName, AttrValue));
                                break;
                            case "Node":
                                result = gvp.AddAttribute(AttrType.SpecificNode, new IDisID(AttrName, AttrValue), AttrFocus1);
                                break;
                            case "Edge":
                                result = gvp.AddAttribute(AttrType.SpecificEdge, new IDisID(AttrName, AttrValue), AttrFocus1, AttrFocus2);
                                break;

                        }
                        if (result)
                            AddNewAttrToList();
                        OnPropertyChanged();
                    }
                });


            }
        }

        private List<AttrEntity> _attrList;

        public ObservableCollection<string> ViewAttrList { get; set; }

        public string SelectedAttr { get; set; }

        public DelegateCommand RemoveAttribute
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    if (SelectedAttr != null)
                    {
                        string[] str = SelectedAttr.Split();
                        bool result = false;
                        switch (str[0])
                        {
                            case "Graph":
                                result = gvp.RemoveAttribute(AttrType.Graph, new IDisID(str[1], str[3]));
                                break;
                            case "Edge":
                                result = gvp.RemoveAttribute(AttrType.SpecificEdge, new IDisID(str[4], str[6]), str[1], str[3]);
                                break;
                            case "Node":
                                result = gvp.RemoveAttribute(AttrType.SpecificNode, new IDisID(str[2], str[4]), str[1]);
                                break;
                            case "All":
                                if (str[1] == "nodes")
                                    result = gvp.RemoveAttribute(AttrType.Node, new IDisID(str[2], str[4]));
                                else
                                    result = gvp.RemoveAttribute(AttrType.Edge, new IDisID(str[2], str[4]));
                                break;
                        }
                        if (result)
                        {
                            List<AttrEntity> newAttrList = new List<AttrEntity>();
                            newAttrList.AddRange(_attrList);
                            foreach (var el in _attrList)
                            {
                                if (el.GetVal() == SelectedAttr)
                                    newAttrList.Remove(el);
                            }
                            _attrList = newAttrList;
                        }

                        OnPropertyChanged();
                    }
                });
            }
        }

        public DelegateCommand OpenHelp
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    System.Diagnostics.Process.Start("https://graphviz.org/doc/info/");
                });
            }
        }

        public DelegateCommand ExportGraph 
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Image (*.png) | *.png";
                    if (saveFileDialog.ShowDialog( ) == true)
                        gvp.ExportImage(saveFileDialog.FileName);
                });
            }
        }

        public ViewModelMain()
        {
            gvp.graph = new Graph(GraphType.Graph, new Stmt_list());
            DotCode = gvp.graph.ToCode();
            PropertyChanged += ChangeDotCode;
            PropertyChanged += ChangeEdgeType;
            PropertyChanged += ChangeNewAttrType;
            PropertyChanged += ChangeAttrList;

            _attrList = new List<AttrEntity>();
        }

        private void OnPropertyChanged([CallerMemberName] string properties = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(properties));
        }

        public void ChangeDotCode(object sender, PropertyChangedEventArgs properties)
        {
            if (properties.PropertyName != nameof(DotCode) && properties.PropertyName != nameof(GraphImage))
            {
                DotCode = gvp.graph.ToCode();
                gvp.GenerateGraph();
                OnPropertyChanged(nameof(GraphImage));
            }
        }

        public void ChangeEdgeType(object sender, PropertyChangedEventArgs properties)
        {
            EdgeArrowType = Graph_type;
            EdgeArrowType2 = Graph_type;
        }

        public void ChangeNewAttrType(object sender, PropertyChangedEventArgs properties)
        {
            FirstAttrTextBoxEnabled = true;
            SecondAttrTextBoxEnabled = true;
        }

        public void AddNewAttrToList()
        {
            if (NewAttrType != null)
            {
                var attrType = NewAttrType.Split()[NewAttrType.Split().Count() - 1];
                switch (attrType)
                {
                    case "Graph":
                        _attrList.Add(new AttrEntity(AttrType.Graph, AttrName, AttrValue));
                        break;
                    case "nodes":
                        _attrList.Add(new AttrEntity(AttrType.Node, AttrName, AttrValue));
                        break;
                    case "edges":
                        _attrList.Add(new AttrEntity(AttrType.Edge, AttrName, AttrValue));
                        break;
                    case "Node":
                        _attrList.Add(new AttrEntity(AttrType.SpecificNode, AttrName, AttrValue, AttrFocus1));
                        break;
                    case "Edge":
                        _attrList.Add(new AttrEntity(AttrType.SpecificEdge, AttrName, AttrValue, AttrFocus1, AttrFocus2));
                        break;

                }
            }
        }

        public void ChangeAttrList(object sender, PropertyChangedEventArgs properties)
        {
            if(properties.PropertyName == nameof(AddNewAttribute) || properties.PropertyName == nameof(RemoveAttribute))
            {
                ObservableCollection<string> data = new ObservableCollection<string>();
                foreach (var el in _attrList)
                {
                    data.Add(el.GetVal());
                }
                ViewAttrList = data;
                OnPropertyChanged(nameof(ViewAttrList));
            }
        }
    }
}
