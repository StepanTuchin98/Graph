using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOG
{
    public class Node
    {
        private String Name;
        private int indeg = 0;
        private int outdeg = 0;
        private bool Mark = false;

        public Node()
        {
        }

        public String NameOfNode()
        {
            return Name;
        }
        public Node(string name)
        {
            Name = name;
        }

        
        public void Show() => Console.Write("{0}", Name);

        public int GetInDeg() => indeg;

        public bool GetMark() => Mark;
        public void SetMark(bool mark) => Mark = mark;

        public int GetOutDeg() => outdeg;

        public void SetOutDeg(int OutDeg) => outdeg = OutDeg;

        public void IncInDeg() => indeg++;

        public override int GetHashCode() => 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);

        public override bool Equals(object obj)
        {
            var node = obj as Node;
            return node != null &&
                   Name == node.Name &&
                   indeg == node.indeg &&
                   outdeg == node.outdeg &&
                   Mark == node.Mark;
        }
    }
}
