using Amazon.CDK;
using Amazon.CDK.AWS.SES;
using Amazon.CDK.AWS.SNS;

namespace Imagineerable.Service.Business.Stacks
{
    public class ServiceEmailStack : Stack
    {
        public ServiceEmailStack(Construct scope, string id, IStackProps? props = null) : base(scope, id, props)
        {
            string emailIdentity = "";

            CfnEmailIdentity domainIdentity = new CfnEmailIdentity(this, "ImagineeringDomainIdentity", new CfnEmailIdentityProps
            {
                EmailIdentity = emailIdentity
            });

            Topic bounceTopic = new Topic(this, "SesBounceTopic", new TopicProps
            {
                DisplayName = "Imagineering Bounce Notifications"
            });
        }
    }
}
