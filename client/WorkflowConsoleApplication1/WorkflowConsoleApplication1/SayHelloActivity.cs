using System;
using System.Activities;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkflowConsoleApplication1
{
    public class SayHelloActivity : Activity
    {
        protected override Func<Activity> Implementation
        {
            get
            {
                // Return a Lambda expression
                // that creates an implementation for
                // the activity
                return () =>
                {
                    return new WriteLine()
                    {
                        Text = "Hello Workflow 4"
                    };
                };
            }
        }
    }
}
