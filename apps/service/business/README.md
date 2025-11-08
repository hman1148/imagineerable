# Service Business - Unified C# CDK Stacks

This project contains all C# CDK stacks for business logic services (Email, Cognito, etc.) in a single unified project.

## Project Structure

```
apps/service/business/
├── src/
│   ├── Program.cs                      # CDK app entry point - instantiates all stacks
│   └── Stacks/
│       ├── ServiceEmailStack.cs        # Email service infrastructure
│       └── ServiceCognitoStack.cs      # Cognito authentication infrastructure
├── business.csproj                     # .NET project file with all dependencies
├── cdk.json                            # CDK configuration
├── global.json                         # .NET SDK configuration
└── project.json                        # Nx project configuration
```

## Available Commands

All commands are run through Nx from the repository root:

```bash
# Build the project
nx build service-business

# Synthesize CloudFormation templates for all stacks
nx synth service-business

# Show differences with deployed stacks
nx diff service-business

# Deploy ALL stacks (EmailStack + CognitoStack)
nx deploy service-business

# Destroy ALL stacks
nx destroy service-business

# Run tests
nx test service-business
```

## Environment and Sandbox Support

This project supports environment and sandbox contexts:

```bash
# Deploy to development environment (default)
nx deploy service-business --env=dev

# Deploy to production
nx deploy service-business --env=prod

# Deploy to a sandbox
nx deploy service-business --env=dev --sandbox=feature-123

# Stack names will be formatted as:
# - ServiceEmail-dev
# - ServiceCognito-dev
# - ServiceEmail-dev-feature-123 (with sandbox)
# - ServiceCognito-dev-feature-123 (with sandbox)
```

## Adding New Stacks

To add a new C# stack to this project:

1. **Create a new stack class** in `src/Stacks/`:

```csharp
// src/Stacks/ServiceMyNewStack.cs
using Amazon.CDK;

namespace Imagineerable.Service.Business.Stacks
{
    public class ServiceMyNewStack : Stack
    {
        public ServiceMyNewStack(Construct scope, string id, IStackProps? props = null)
            : base(scope, id, props)
        {
            // Define your AWS resources here
        }
    }
}
```

2. **Instantiate it in Program.cs**:

```csharp
// Add to src/Program.cs
new ServiceMyNewStack(app, $"ServiceMyNew{stackSuffix}", new StackProps
{
    Env = new Amazon.CDK.Environment
    {
        Account = account,
        Region = region
    },
    Description = $"My new service for {env} environment{(string.IsNullOrEmpty(sandbox) ? "" : $" (sandbox: {sandbox})")}"
});
```

3. **Add required NuGet packages** to `business.csproj`:

```xml
<ItemGroup>
  <!-- Existing packages... -->
  <PackageReference Include="Amazon.CDK.AWS.YourService" Version="1.204.0" />
</ItemGroup>
```

4. **Build and deploy**:

```bash
nx build service-business
nx deploy service-business
```

## Adding AWS CDK Packages

This project uses AWS CDK v2 (Amazon.CDK.Lib 2.222.0), but some services still use v1 packages. To add packages:

```xml
<ItemGroup>
  <!-- For CDK v2 services (most common) -->
  <PackageReference Include="Amazon.CDK.Lib" Version="2.222.0" />

  <!-- For services not yet in v2, use v1 packages -->
  <PackageReference Include="Amazon.CDK.AWS.Lambda" Version="1.204.0" />
  <PackageReference Include="Amazon.CDK.AWS.DynamoDB" Version="1.204.0" />
</ItemGroup>
```

Then run `dotnet restore` or `nx build service-business`.

## Comparison with TypeScript Framework

This C# project mirrors the structure of the TypeScript framework:

| TypeScript Framework | C# Business |
|---------------------|-------------|
| `apps/service/framework/` | `apps/service/business/` |
| `src/main.ts` | `src/Program.cs` |
| `src/stacks/*.ts` | `src/Stacks/*.cs` |
| `nx deploy service-framework` | `nx deploy service-business` |

Both deploy **all** their stacks with a single command!

## Prerequisites

- .NET 8.0 SDK
- AWS credentials configured (`aws configure`)
- AWS CDK CLI (`npm install -g aws-cdk`)

## Troubleshooting

### .NET Version Issues

If you encounter .NET version errors, check your installed version:

```bash
dotnet --version
```

Update `global.json` if needed to match your installed version.

### CDK Bootstrap

If this is your first deployment, bootstrap CDK in your account:

```bash
cd apps/service/business
cdk bootstrap
```
