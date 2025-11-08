using Amazon.CDK;
using Amazon.CDK.AWS.SES;
using Amazon.CDK.AWS.SNS;

namespace Imagineerable.ServiceEmail.Stacks
{
    public class ServiceEmailStack : Stack
    {
        public ServiceEmailStack(Construct scope, string id, IStackProps? props = null) : base(scope, id, props)
        {
            string emailyIdentity = "";


            CfnEmailIdentity domainIdentity = new CfnEmailIdentity(this, "ImagineeringDomainIdentity", new CfnEmailIdentityProps
            {
                EmailIdentity = emailyIdentity
            });

            Topic bounceTopic = new Topic(this, "SesBounceTopic", new TopicProps
            {
                DisplayName = "Imagineering Bounce Notifications"
            });
        }
    }
}
