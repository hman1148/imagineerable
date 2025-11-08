using Amazon.CDK;
using System;
using System.Collections.Generic;
using ServiceEmail.Stacks;

namespace ServiceEmail
{
    public sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();

            // Get context values
            var env = app.Node.TryGetContext("env")?.ToString() ?? "dev";
            var sandbox = app.Node.TryGetContext("sandbox")?.ToString() ?? "";

            // Build stack name
            string stackName = $"ServiceEmail-{env}";
            if (!string.IsNullOrEmpty(sandbox))
            {
                stackName = $"{stackName}-{sandbox}";
            }

            new ServiceEmailStack(app, stackName, new StackProps
            {
                Env = new Environment
                {
                    Account = System.Environment.GetEnvironmentVariable("CDK_DEFAULT_ACCOUNT"),
                    Region = System.Environment.GetEnvironmentVariable("CDK_DEFAULT_REGION"),
                }
            });

            app.Synth();
        }
    }
}
