using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOG
{
    class Program
    {
        static void Main(string[] args)
        {
            var fs = new FileStream("input.txt", FileMode.OpenOrCreate);
            //ShowCopyOfGraph(fs);
            //UnionGraph(fs);
            GetAmountComponents(fs);
            //ShowDegOfNodes(fs);
            //Relavent(fs);
        }

        public static void Relavent(FileStream fs)
        {
            bool oriented = true;
            var gr = new Graph(fs, oriented);
            gr.Show();
            try
            {
                gr.GlobalNode("F", "C").ForEach(x => Console.WriteLine(x.NameOfNode()));
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void GetAmountComponents(FileStream fs)
        {
            Graph gr = null;
            bool oriented = true;

            gr = new Graph(fs, oriented);
            gr.Show();

            try
            {
                Console.WriteLine(gr.GetNumOfLinkedElem());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //Set graph from filestream, make copy of it, than delete  
        //one edge and one node. Show that the initial graph dont change
        public static void ShowCopyOfGraph(FileStream fs)
        {
            bool oriented = false;
            var gr = new Graph(fs, oriented);
            gr.Show();
            Console.WriteLine();
            var gr2 = new Graph(gr, oriented);
            gr2.RemoveNode("B");
            gr2.RemoveEdge("A", "C");

            gr2.Show();
            Console.WriteLine();
            gr.Show();
        }
        public static void UnionGraph(FileStream fs)
        {
            bool oriented = true;
            var gr1 = new Graph(fs, oriented);
            var gr2 = new Graph(oriented);
            gr2.AddNode("A");
            gr2.AddNode("F");
            gr2.AddEdge("A", "F");
            gr1.AddEdge("A", "B");

            Console.WriteLine("Graph1");
            gr1.Show();

            Console.WriteLine();
            Console.WriteLine("Graph2");          
            gr2.Show();

            Console.WriteLine();
            Console.WriteLine("ResultGraph");
            (gr1 + gr2).Show();
        }
        //Show degree of the nodes
        public static void ShowDegOfNodes(FileStream fs)
        {
            bool oriented = true;
            var gr = new Graph(fs, oriented);
            gr.Show();
            gr.GetDegOfNodes().ForEach(x => Console.WriteLine(x));
        }

    }
}
