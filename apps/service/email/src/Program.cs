using Amazon.CDK;

App app = new App();

// Get context values
string env = app.Node.TryGetContext("env")?.ToString() ?? "dev";
string sandbox = app.Node.TryGetContext("sandbox")?.ToString() ?? "";

// Build stack name
string stackName = $"ServiceEmail-{env}";

if (!string.IsNullOrEmpty(sandbox))
{
    stackName = $"{stackName}-{sandbox}";
}

app.Synth();
