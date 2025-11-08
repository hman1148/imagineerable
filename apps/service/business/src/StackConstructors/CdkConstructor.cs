using Amazon.CDK;
using Imagineerable.Service.Business.src.Models;
namespace Imagineerable.Service.Business.src.StackConstructors
{
    public static class CdkConstructor
    {
        /// <summary>
        /// Aws CDK Configuration
        /// </summary>
        private static CdkConfiguration _cdkConfiguration = default!;

        /// <summary>
        /// Creates a CdkConfiguration from the given App context
        /// </summary>
        /// <param name="app"><see cref="App"/></param>
        /// <returns><see cref="CdkConfiguration"/></returns>
        public static void FromApp(this App app)
        {
            string env = app.Node.TryGetContext("env")?.ToString() ?? "dev";
            string sandbox = app.Node.TryGetContext("sandbox")?.ToString() ?? "";
            string account = app.Node.TryGetContext("account")?.ToString() ?? "";
            string region = app.Node.TryGetContext("region")?.ToString() ?? "";

            string stackSuffix = $"-{env}";

            if (!string.IsNullOrEmpty(sandbox))
            {
                stackSuffix += $"-{sandbox}";
            }

            _cdkConfiguration = new CdkConfiguration
            {
                Environment = env,
                Sandbox = sandbox,
                Account = account,
                Region = region,
                StackSuffix = stackSuffix
            };
        }

        /// <summary>
        /// Creates an Aws CDK Stack of type T
        /// </summary>
        /// <typeparam name="T"><see cref="Stack"/></typeparam>
        /// <param name="app"><see cref="App"/></param>
        /// <param name="stackName">Stack name</param>
        /// <returns>Type of AWS Stack</returns>
        public static T CreateStack<T>(this App app, string stackName) where T : Stack
        {
            StackProps props = new StackProps
            {
                Env = new Amazon.CDK.Environment
                {
                    Account = _cdkConfiguration.Account,
                    Region = _cdkConfiguration.Region
                },
                Description = GetDescription(stackName)
            };

            return (T)Activator.CreateInstance(typeof(T), app, $"{stackName}{_cdkConfiguration.StackSuffix}", props)!;
        }

        /// <summary>
        /// Get the stack description
        /// </summary>
        /// <param name="stackName">Stack name</param>
        /// <returns>Statck description</returns>
        private static string GetDescription(string stackName)
        {
            var sandboxPart = string.IsNullOrEmpty(_cdkConfiguration.Sandbox) ? "" : $" (sandbox: {_cdkConfiguration.Sandbox})";
            return $"{stackName} infrastructure for {_cdkConfiguration.Environment} environment{sandboxPart}";
        }
    }
}
