using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CSAcademyProject.Operators
{
    abstract class AbstractOperator
    {
        abstract public void Draw(Canvas canvas);
        abstract public void HandleMouseDown(int x, int y);
        abstract public void HandleMouseUp(int x, int y);
    }
}
