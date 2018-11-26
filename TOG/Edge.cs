using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOG
{

    public class Edge
    {
        public Node to; //куда
        public int weight = 1; //вес

        public Edge(Node To, int Weight)//конструктор
        {
            to = To;
            weight = Weight;
        }
        public Edge(Node To)//конструктор
        {
            to = To;
        }
         public Node GetNode()
        {
            return to;
        }
        public string NameOfNode()
        {
            return to.NameOfNode();
        }
        public int GetWeight()
        {
            return weight;
        }


    }

}
