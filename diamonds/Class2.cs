using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace diamonds
{
    public class Class2
    {
            public void FileLoader(string path)
            {
            var list = new List<string[]>();
            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                var cells = line.Split(',');
                list.Add(cells);
            }
            return list;
        }
    }
}
