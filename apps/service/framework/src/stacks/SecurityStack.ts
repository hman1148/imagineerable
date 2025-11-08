import { CfnMapping, CfnOutput, Stack, StackProps, Tags } from 'aws-cdk-lib/core';
import { Construct } from 'constructs';

export class SecurityStack extends Stack {
    constructor(scope: Construct, id: string, props: StackProps) {
        super(scope, id, props);

        const env = this.node.tryGetContext('env') || 'dev';
        const sandbox = this.node.tryGetContext('sandbox') || '';

        const envMapping = new CfnMapping(this, 'EnvironmentMapping', {
            mapping: {
                dev: {
                    hostedZone: 'Z09011321LBDR3A2J6A1I',
                    domainName: 'dev.imagineerable.com',
                },
                test: {
                    hostedZone: 'Z09011321LBDR3A2J6A1I',
                    domainName: 'test.imagineerable.com',
                },
                prod: {
                    hostedZone: 'Z09011321LBDR3A2J6A1I',
                    domainName: 'imagineerable.com',
                },
            },
        });

        const hostedZoneId = envMapping.findInMap(env, 'hostedZone');
        const baseDomainName = envMapping.findInMap(env, 'domainName');

        Tags.of(this).add('Environment', env);
        if (sandbox) {
            Tags.of(this).add('Sandbox', sandbox);
        }

        //TODO: Add VPC security and Lambda security, etc...

        // Exports for use by other stacks
        new CfnOutput(this, 'SecurityExampleExport', {
            value: 'ExampleValue:' + hostedZoneId + baseDomainName,
            exportName: `${this.stackName}-SecurityExampleExport`,
            description: 'Security Stack Example Export Description!',
        });
    }
}
