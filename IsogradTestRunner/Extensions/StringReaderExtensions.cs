using System.Collections.Generic;
using System.IO;

namespace IsogradTestRunner.Extensions
{
    public static class StringReaderExtensions
    {
        public static IEnumerable<string> ReadAllLines(this StringReader stringReader)
        {
            string line;
            while ((line = stringReader.ReadLine()) != null)
            {
                yield return line;
            }
        }
    }
}
