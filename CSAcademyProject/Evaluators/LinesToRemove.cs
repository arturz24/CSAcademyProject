using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSAcademyProject.Evaluators
{
    class LinesToRemove
    {
        public List<int> RowIndexes { get; }
        public List<int> ColumnIndexes { get; }

        public LinesToRemove()
        {
            RowIndexes = new List<int>();
            ColumnIndexes = new List<int>();
        }
    }
}
