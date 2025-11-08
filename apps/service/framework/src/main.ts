import { CertificateStack } from './stacks/CertificateStack';
import { NetworkingStack } from './stacks/NetworkingStack';
import { SecurityStack } from './stacks/SecurityStack';
import * as cdk from 'aws-cdk-lib/core';

const app = new cdk.App();

const account = process.env.CDK_DEFAULT_ACCOUNT;
const defaultRegion = process.env.CDK_DEFAULT_REGION;

// Get sandbox from context
const sandbox = app.node.tryGetContext('sandbox') || '';
const stackSuffix = sandbox ? `-${sandbox}` : '';

// Create certificate stack in us-west-1 for CloudFront
const certificateStack = new CertificateStack(app, `service-framework-certificate${stackSuffix}`, {
  env: { region: 'us-west-1', account },
  crossRegionReferences: true,
  description: `ACM certificates for CloudFront. Must be in us-west-1${sandbox ? ` (sandbox: ${sandbox})` : ''}`,
});

// Create main application stack in the default region
const networkingStack = new NetworkingStack(app, `service-framework-networking${stackSuffix}`, {
  env: { region: defaultRegion, account },
  crossRegionReferences: true,
  cloudFrontCertificateArn: certificateStack.wildCertificateArn,
  description: `Main networking infrastructure${sandbox ? ` (sandbox: ${sandbox})` : ''}`,
});

new SecurityStack(app, `service-framework-security${stackSuffix}`, {
  env: { region: defaultRegion, account },
  description: `Main security infrastructure${sandbox ? ` (sandbox: ${sandbox})` : ''}`,
});


// Ensure certificate stack is deployed before app stack
networkingStack.addDependency(certificateStack);
