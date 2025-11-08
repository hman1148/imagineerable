using Amazon.CDK;
using Imagineerable.Service.Business.Stacks;

App app = new App();

// Get context values
string env = app.Node.TryGetContext("env")?.ToString() ?? "dev";
string sandbox = app.Node.TryGetContext("sandbox")?.ToString() ?? "";
string account = app.Node.TryGetContext("account")?.ToString() ?? System.Environment.GetEnvironmentVariable("CDK_DEFAULT_ACCOUNT");
string region = app.Node.TryGetContext("region")?.ToString() ?? System.Environment.GetEnvironmentVariable("CDK_DEFAULT_REGION") ?? "us-west-1";

// Build stack suffix for environment and sandbox
string stackSuffix = $"-{env}";
if (!string.IsNullOrEmpty(sandbox))
{
    stackSuffix = $"{stackSuffix}-{sandbox}";
}

// Create Email stack
new ServiceEmailStack(app, $"ServiceEmail{stackSuffix}", new StackProps
{
    Env = new Amazon.CDK.Environment
    {
        Account = account,
        Region = region
    },
    Description = $"Email service infrastructure for {env} environment{(string.IsNullOrEmpty(sandbox) ? "" : $" (sandbox: {sandbox})")}"
});

// Create Cognito stack
new ServiceCognitoStack(app, $"ServiceCognito{stackSuffix}", new StackProps
{
    Env = new Amazon.CDK.Environment
    {
        Account = account,
        Region = region
    },
    Description = $"Cognito authentication infrastructure for {env} environment{(string.IsNullOrEmpty(sandbox) ? "" : $" (sandbox: {sandbox})")}"
});

app.Synth();
