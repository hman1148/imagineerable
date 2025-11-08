using Amazon.CDK;

namespace Imagineerable.ServiceEmail.Stacks
{
    public class ServiceEmailStack : Stack
    {
        public ServiceEmailStack(Construct scope, string id, IStackProps? props = null) : base(scope, id, props)
        {
            // Define your AWS resources here
            // Example:
            // var bucket = new Bucket(this, "MyBucket", new BucketProps
            // {
            //     Versioned = true,
            //     Encryption = BucketEncryption.S3_MANAGED
            // });
        }
    }
}
