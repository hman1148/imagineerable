using Amazon.CDK;

namespace Imagineerable.Service.Business.Stacks
{
    public class ServiceCognitoStack : Stack
    {
        public ServiceCognitoStack(Construct scope, string id, IStackProps? props = null) : base(scope, id, props)
        {
            // Define your Cognito resources here

            // Example:
            // var userPool = new UserPool(this, "UserPool", new UserPoolProps
            // {
            //     UserPoolName = "imagineerable-users",
            //     SelfSignUpEnabled = true,
            //     SignInAliases = new SignInAliases { Email = true },
            //     AutoVerify = new AutoVerifiedAttrs { Email = true }
            // });
        }
    }
}
