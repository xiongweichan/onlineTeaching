using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Activities;

namespace WorkflowConsoleApplication1
{
    public class SayHelloInCode : CodeActivity
    {
        protected override void Execute(CodeActivityContext context)
        {
            Console.WriteLine("Hello Workflow 4 in code");
        }
    }
}
