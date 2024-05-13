using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tools
{
    public class ClassGenerator
    {
        public void GenerateStatic(string className, IEnumerable<string> content, string path)
        {
            string fileName = Path.Combine(path, className + ".cs");
            string classContent = String.Empty;
            
            var namespaceName = path.Split("/").Where(x=>x!="Assets"&&x!="Scripts");
            var correctNamespaceName = String.Join(".", namespaceName);

            classContent += $"namespace {correctNamespaceName} \n{{\n";

            classContent += $"\tpublic static class {className}\n\t{{";

            
            foreach (string property in content)
            {
                string propertyName = new string(property.Where(char.IsLetter).ToArray());
                classContent += $"\n\t\tpublic static string {propertyName} = \"{property}\";";
            }

            classContent += $"\n\t}}\n}}";

            File.WriteAllText(fileName, classContent);
        }
    }
}