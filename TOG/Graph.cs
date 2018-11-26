using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOG
{
    class Graph
    {
        Dictionary<Node, List<Edge>> graph = new Dictionary<Node, List<Edge>>();
        Random R = new Random();
        Dictionary<Node, List<Edge>> graphT = new Dictionary<Node, List<Edge>>();

        bool oriented;

        private List<Node> order = new List<Node>();

        #region ctors
        public Graph(Dictionary<Node, List<Edge>> graph, bool oriented)
        {
            this.graph = graph;
            this.oriented = oriented;
        }

        public Graph() { }

        public Graph(bool oriented) {
            this.oriented = oriented;
        }

        public Graph(FileStream input, bool oriented)
        {
            StreamReader inputStream = new StreamReader(input);
            while (!inputStream.EndOfStream)
            {
                string line = inputStream.ReadLine();
                string[] str = line.Split(':');
                string key = str[0];
                string[] values = str[1].Trim().Split(' ');
                List<Edge> res = new List<Edge>();

                foreach (var v in values)
                    if (!v.Equals(""))
                    {
                        string[] temp = v.Split('-');
                        Node x = AddNode(temp[0]);
                        if(v.Length > 1)
                        res.Add(new Edge(x, int.Parse(temp[1])));
                    }

                AddNode(key, res);
            }

            this.oriented = oriented;
            if (!oriented)
                ToNotOrient();    
        }
        
        public Graph(Graph G, bool oriented)
        {
            
            foreach (var n in G.graph.Keys)
                AddNode(n.NameOfNode());

            foreach (var n in G.graph)
            {
                foreach (var e in n.Value)
                {
                    AddEdge(n.Key, e.GetNode());
                }
            }
            this.oriented = oriented;
        }
        /*
        public Graph(int N, int M, bool oriented = false)//oriented == true - граф ориентированный, иначе - неориентированный
        {
            //заполнение списока смежности
            //заполнение вершин
            for (int i = 0; i < N; i++)
                AddNode(new Node(i));
            //заполнение ребер
            //если число ребер превышает допустимое значение - установить максимум
            if (oriented)//для ориентированного
            {
                if (M > N * (N - 1))
                    M = N * (N - 1);
            }
            //для неориентированного
            else
            {
                if (M > N * (N - 1) / 2)
                    M = N * (N - 1) / 2;
            }
            for (int i = 0; i < M; i++)
            {
                //взять 2 несовпадающие вершины       
                int r1 = R.Next(N);
                // Здесь следить, чтобы R.Next не оказался == предыдущему
                int r2 = R.Next(N);
                while (r1 == r2)
                    r2 = R.Next(N);

                KeyValuePair<Node, List<Edge>> K = graph.ElementAt(0);
                //Найти такую, чтобы вершина была r1
                foreach (var v in graph)
                    if (v.Key.NameOfNode() == r1)
                        K = v;

                //найти ребро r2
                bool exists = false;
                //выполняется проверка наличия ребра
                foreach (var l in K.Value)
                    if (l.to.NameOfNode() == r2)
                        exists = true;
                if (exists)
                {
                    i--;
                    continue;
                }
                //Добавить ребро "туда" и "обратно", если надо

                int Weight = R.Next(10) + 1;
                Edge edg = new Edge(r1, r2, Weight, graph);

                AddEdge(edg);

                if (!oriented) //Не ориенированный - значит, и обратное ребро тоже есть
                {
                    edg = new Edge(r2, r1, Weight, VertexWeight);
                    AddEdge(edg);
                }
            }
            //заполнить список ребер
            VertexWeightToEdge();
            Save(); //Последний сгенерированный граф
        }*/

        #endregion ctors

        #region SearchNodesAndEdges
        public Node SearchNode(string node)
        {
            foreach (var n in graph.Keys)
                if (n.NameOfNode().Equals(node))
                    return n;
            return null;
        }

        public Node SearchNode(string node, Dictionary<Node, List<Edge>> graphT)
        {
            foreach (var n in graphT.Keys)
                if (n.NameOfNode().Equals(node))
                    return n;
            return null;
        }

        private Edge SearchEdge(Node Out, Node In)
        {
            foreach (var n in graph[In])
                if (n.NameOfNode().Equals(Out.NameOfNode()))
                    return n;
            return null;
        }

        private Edge SearchEdge(string Node)
        {
            foreach (var li in graph.Values)
                foreach (Edge n in li)
                    if (n.NameOfNode().Equals(Node))
                        return n;
            return null;
        }

        private Edge SearchEdge(string Node, string where)
        {
            Node whr = SearchNode(where);
            foreach (var n in graph[whr])
                if (n.NameOfNode().Equals(Node))
                    return n;
            return null;
        }

        #endregion SearchNodesAndEdges

        #region AddNodesAndEdges
        public Node AddNode(string Node)
        {
            Node temp = SearchNode(Node);
            if (temp == null)
            {
                temp = new Node(Node);
                graph.Add(temp, new List<Edge>());
            }
            return temp;
        }

        public void AddNode(string Node, List<Edge> list)
        {
            Node temp = SearchNode(Node);
            if (temp == null)
                graph.Add(new Node(Node), list);
            else
                graph[temp] = list;  
        }

        public void AddNode(Node node, List<Edge> list)
        {
            graph.Add(node, list);
           // if (!oriented)
              //  ToNotOrient();
        }

        public void AddEdge(Node NodeOut, Node NodeIn)
        {
            if (SearchEdge(NodeIn.NameOfNode(), NodeOut.NameOfNode()) == null)
                graph[NodeOut].Add(new Edge(NodeIn));
            else
                Console.WriteLine("Such edge already exists");

            if (!oriented && SearchEdge(NodeOut, NodeIn) == null)
                graph[NodeIn].Add(new Edge(NodeOut));
        }

        public void AddEdge(string NodeOut, string NodeIn)
        {
            Node o = SearchNode(NodeOut);
            Node i = SearchNode(NodeIn);
            if(SearchEdge(i,o) == null)
            graph[o].Add(new Edge(i));
            else
                Console.WriteLine("Such edge already exists");

            if (!oriented && SearchEdge(o, i) == null)
                graph[i].Add(new Edge(o));
        }

        #endregion AddNodesAndEdges

        #region RemoveNodesAndEdges
        public void RemoveNode(string Node)
        {
            Node t = SearchNode(Node);
            graph.Remove(t);
            foreach (var li in graph.Values)
            {
                RemoveNodeInList(t, li);
            }
        }

        public void RemoveNodeInList(Node node, List<Edge> list)
        {
            list.Remove(SearchEdge(node.NameOfNode()));
        }

        public void RemoveEdge(string NodeOut, string NodeIn)
        {
            Node no = SearchNode(NodeOut);
            Edge ei = SearchEdge(NodeIn);

            if (no != null && ei != null)
            {
                graph[no].Remove(ei);
                if (!oriented)
                    graph[SearchNode(NodeIn)].Remove(SearchEdge(NodeOut));
            }
            else Console.WriteLine("There is no such edge!");
        }

        #endregion RemoveNodesAndEdges

        #region TaskMethods
        private Dictionary<Node, List<Edge>> GetGraph() => graph;

        private Dictionary<Node, List<Edge>> GetGraphT()
        {
            SetGraphT();
            return graphT;
        }

        private void SetGraphT()
        {
            foreach (var item in graph.Keys)
            {
                graphT.Add(new Node(item.NameOfNode()), new List<Edge>());
            }

            foreach (var item in graph)
            {
                foreach (var edge in item.Value)
                {
                    graphT[SearchNode(edge.NameOfNode())].Add(new Edge(item.Key));
                }
            }
            
        }

        void CheckVertex(KeyValuePair<Node, List<Edge>> K)
        {
            //Если уже посещенна - то просто вернуться
            if (K.Key.GetMark())
                return;
            //Отметить себя как посещенную
            K.Key.SetMark(true);

            foreach (var e in K.Value)
            {
                //Найти такую пару, чтобы ее вершина была e.to
                foreach (var v in graph)
                {
                    if (v.Key.NameOfNode().Equals(e.to.NameOfNode()))
                        //и проверить ее
                        CheckVertex(v);
                }
            }

        }

        bool IsLinked()//проверить, связный ли граф
        {
            //Пометить все false
            //Проверить вершину номер 0
            CheckVertex(graph.ElementAt(0));
            //Проверить, нет ли неотмеченных верщшин
            foreach (var v in graph)
                if (!v.Key.GetMark())
                    return false;
            return true;
        }

        public Graph Boruvki()
        {
            //проверить на связанность графа
            //если нет - вернуть null
            if (!IsLinked())
                return null;
            return new Graph();
        }

        public List<Node> GlobalNode(string NodeU, string NodeV)
        {
            Node nodeU = SearchNode(NodeU);
            Node nodeV = SearchNode(NodeV);
            if (NodeU != null && NodeV != null)
            {
                List<Node> res = new List<Node>();

                foreach (var i in graph[nodeU])
                {
                    foreach (var j in graph[nodeV])
                    {
                        if (i.NameOfNode().Equals(j.NameOfNode()))
                            res.Add(i.GetNode());

                    }
                }
                return res;
            }
            throw new Exception("No such nodes");
        }

        public void ToNotOrient()
        { 
            foreach (var n in graph)
                foreach (var li in n.Value)
                    if (SearchEdge(n.Key, li.GetNode()) == null)
                        graph[li.GetNode()].Add(new Edge(n.Key));
        }

        public List<int> GetDegOfNodes()
        {
            SetDegOfNodes();
            List<int> res = new List<int>();
            foreach (var n in graph.Keys)
            {
                if(!oriented)
                    res.Add(n.GetOutDeg());
                else
                    res.Add(n.GetInDeg() + n.GetOutDeg());
            }
            return res;

        }

        private void SetDegOfNodes()
        {
            foreach (var el in graph)
            {
                el.Key.SetOutDeg(el.Value.Count);
            }

                foreach (var li in graph.Values)
                {
                    foreach (var t in li)
                    {
                        SearchNode(t.NameOfNode()).IncInDeg();
                    }
                }
            }

        private List<Node> GetNodes()
        {
            return this.graph.Keys.ToList();
        }

        public static Graph operator +(Graph g1, Graph g2)
        {
            Graph newGraph = new Graph(true);

            if (g1.oriented && g2.oriented)
            {
                HashSet<string> f = new HashSet<string>();
                g1.GetNodes().ForEach(x => f.Add(x.NameOfNode()));
                HashSet<string> s = new HashSet<string>();
                g2.GetNodes().ForEach(x => s.Add(x.NameOfNode()));

                f.UnionWith(s);
               
                foreach (var x in f)
                {
                    Node n1 = g1.SearchNode(x);
                    Node n2 = g2.SearchNode(x);
         
                    if(n1 != null && n2 != null)
                        newGraph.AddNode(new Node(x), Difference(g1.graph[n1], g2.graph[n2]));
                    else if(n1 == null)
                        newGraph.AddNode(new Node(x), g2.graph[n2]);
                    else
                        newGraph.AddNode(new Node(x), g1.graph[n1]);
                }

            }

            return newGraph;
        }

        private static List<Edge> Difference(List<Edge> F, List<Edge> S)
        {
            HashSet<string> f = new HashSet<string>();
            F.ForEach(x => f.Add(x.NameOfNode()));
            HashSet<string> s = new HashSet<string>();
            S.ForEach(x => s.Add(x.NameOfNode()));

            f.UnionWith(s);
            
            List<Edge> res = new List<Edge>();
            foreach (var x in f)
            {
                res.Add(new Edge(new Node(x)));
            }
            
            return res;
        }

        private void dfs(string n)
        {
            Node node = SearchNode(n);
            node.SetMark(true);
            foreach (var e in graph[node])
                if (!e.GetNode().GetMark())
                    dfs(e.NameOfNode());
            order.Add(node);
        }
        

        private void dfs2(string n)
        {
            
            Node node = SearchNode(n, graphT);
            node.SetMark(true);
            //C.Add(node);

            foreach (var e in graphT[node])
                if (!e.GetNode().GetMark())
                {
                    e.GetNode().SetMark(true);
                    dfs2(e.NameOfNode());
                }
        }

        private void SetNodesMarkFalse()
        {
            foreach (var k in graph.Keys)
            {
                k.SetMark(false);
                foreach (var val in graph[k])
                {
                    val.GetNode().SetMark(false);
                }
            }
        }

        public int GetEdgeWeight(string n1, string n2) => SearchEdge(n2, n1).weight;


        public int GetNumOfLinkedElem()//проверить, связный ли граф
        {
            if (oriented)
            {
                List<Node> component = new List<Node>();
                foreach (var n in graph.Keys)
                    if (!n.GetMark())
                        dfs(n.NameOfNode());
                SetNodesMarkFalse();
                SetGraphT();
                int count = 0;
                for (int i = 0; i < graphT.Keys.Count; i++)
                {
                    Node v = order.ElementAt(graphT.Keys.Count - 1 - i);

                    if (!v.GetMark())
                    {
                        v.SetMark(true);
                        dfs2(v.NameOfNode());
                        count++;
                        //component.clear();
                    }
                }
                return count;
            }
            throw new Exception("Not oriented Graph");
        }
        public void Show()
        {
            foreach(var k in graph.Keys)
            {
                Console.Write("{0} :", k.NameOfNode());
                foreach (var val in graph[k])
                {
                    Console.Write("{0}-{1} ", val.NameOfNode(), val.GetWeight());
                }
                Console.WriteLine();
            }
        }

        #endregion TaskMethods
    }
}
