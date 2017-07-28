using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Activities;
using WorkflowConsoleApplication1;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

namespace HelloWorkflow.Tests
{
    [TestClass]
    public class SayHelloFixture
    {
        [TestMethod]
        public void TestMethod1()
        {
        }

        [TestMethod]
        public void ShouldReturnGreetingWithName()
        {
            var output = WorkflowInvoker.Invoke(
              new SayHello()
              {
                  UserName = "Test"
              });
            Assert.AreEqual("Hello Test from Workflow 4", output["Greeting"]);
        }
        /// <summary>
        /// Verifies that the workflow returns an Out Argument
        /// Name:WorkflowThread
        /// Type:Int32
        /// Value:Non-Zero
        /// </summary>
        //[TestMethod]
        //public void ShouldReturnWorkflowThread()
        //{
        //    var output = WorkflowInvoker.Invoke(
        //     new SayHello()
        //     {
        //         UserName = "Test"
        //     });
        //    Assert.IsTrue(output.ContainsKey("WorkflowThread"),
        //      "SayHello must contain an OutArgument named WorkflowThread");
        //    // Don't know for sure what it is yet
        //    var outarg = output["WorkflowThread"];
        //    Assert.IsInstanceOfType(outarg, typeof(Int32),
        //      "WorkflowThread must be of type Int32");
        //    Assert.AreNotEqual(0, outarg,
        //      "WorkflowThread must not be zero");
        //    Debug.WriteLine("Test thread is " +
        //            Thread.CurrentThread.ManagedThreadId);
        //    Debug.WriteLine("Workflow thread is " + outarg.ToString());
        //}

        /// <summary>
        /// Verifies that the workflow returns an Out Argument
        /// Name:WorkflowThread
        /// Type:Int32
        /// Value:Non-Zero, matches thread used for Completed action
        /// </summary>
        [TestMethod]
        public void ShouldReturnWorkflowThread()
        {
            AutoResetEvent sync = new AutoResetEvent(false);
            Int32 actionThreadID = 0;
            IDictionary<string, object> output = null;
            WorkflowApplication workflowApp =
              new WorkflowApplication(
                   new SayHello()
                   {
                       UserName = "Test"
                   });
            // Create an Action<T> using a lambda expression
            // To be invoked when the workflow completes
            workflowApp.Completed = (e) =>
            {
                output = e.Outputs;
                actionThreadID = Thread.CurrentThread.ManagedThreadId;
                // Signal the test thread the workflow is done
                sync.Set();
            };
            workflowApp.Run();
            // Wait for the sync event for 1 second
            sync.WaitOne(TimeSpan.FromSeconds(1));
            Assert.IsNotNull(output,
              "output not set, workflow may have timed out");
            Assert.IsTrue(output.ContainsKey("WorkflowThread"),
              "SayHello must contain an OutArgument named WorkflowThread");
            // Don't know for sure what it is yet
            var outarg = output["WorkflowThread"];
            Assert.IsInstanceOfType(outarg, typeof(Int32),
              "WorkflowThread must be of type Int32");
            Assert.AreNotEqual(0, outarg,
              "WorkflowThread must not be zero");
            Debug.WriteLine("Test thread is " +
                    Thread.CurrentThread.ManagedThreadId);
            Debug.WriteLine("Workflow thread is " + outarg.ToString());
        }
    }
}
