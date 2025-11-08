namespace Imagineerable.Service.Business.src.Models
{
    /// <summary>
    /// Aws CDK Configuration
    /// </summary>
    public class CdkConfiguration
    {
        /// <summary>
        /// Aws Environment (e.g., "dev", "prod")
        /// </summary>
        public required string Environment { get; init; }

        /// <summary>
        /// Aws Sandbox (e.g., "sandbox1", "sandbox2")
        /// </summary>
        public required string Sandbox { get; init; }

        /// <summary>
        /// Aws Account ID
        /// </summary>
        public required string Account { get; init; }

        /// <summary>
        /// Aws Region (e.g., "us-west-1")
        /// </summary>
        public required string Region { get; init; }

        /// <summary>
        /// Aws Stack Suffix (e.g., "-dev-sandbox1")
        /// </summary>
        public required string StackSuffix { get; init; }
    }
}
