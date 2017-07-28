using System;
using System.Linq;
using System.Activities;
using System.Activities.Statements;

using System.Activities.XamlIntegration;

namespace WorkflowConsoleApplication1
{

    class Program
    {
        static void Main(string[] args)
        {
            //Activity workflow1 = new SayHelloInCode();//new SayHelloActivity();//new SayHello();
            //WorkflowInvoker.Invoke(workflow1);
            WorkflowInvoker.Invoke(ActivityXamlServices.Load("SayHello.xaml"));
            Console.ReadKey(false);
        }
    }
}
