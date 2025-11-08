# ServiceEmail

C# CDK application for service-email.

## Prerequisites

- .NET 8.0 SDK or later
- AWS CDK CLI (`npm install -g aws-cdk`)
- AWS credentials configured

## Commands

### Build

```bash
nx build service-email
```

### Synthesize CloudFormation template

```bash
nx synth service-email
```

### Deploy to AWS

```bash
nx deploy service-email
```

### View differences

```bash
nx diff service-email
```

### Destroy stack

```bash
nx destroy service-email
```

### Run tests

```bash
nx test service-email
```

## Context Variables

- `env`: Environment name (default: "dev")
- `sandbox`: Sandbox identifier (default: "")

Example with custom context:

```bash
nx deploy service-email --env=prod --sandbox=feature-branch
```
