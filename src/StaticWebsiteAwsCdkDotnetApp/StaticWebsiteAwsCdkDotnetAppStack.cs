using Amazon.CDK;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.S3;
using Amazon.CDK.AWS.S3.Deployment;
//using Constructs;
using System.Diagnostics;

namespace StaticWebsiteAwsCdkDotnetApp
{
    public class StaticWebsiteAwsCdkDotnetAppStack : Stack
    {
        internal StaticWebsiteAwsCdkDotnetAppStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            // The code that defines your stack goes here
            var bucket = new Bucket(this, "bucket", new BucketProps
            {
                BucketName = "okcdkdemo",
                AccessControl = BucketAccessControl.PUBLIC_READ,
                WebsiteIndexDocument = "index.html"
            });

            var policyStatement = new PolicyStatement(new PolicyStatementProps
            {
                Effect = Effect.ALLOW,
                Actions = new[] { "s3:GetObject" },
                Resources = new[] { bucket.ArnForObjects("*") },
                Principals = new IPrincipal[]
                {
                    new AnyPrincipal()

                }

            });

            bucket.AddToResourcePolicy(policyStatement);

            new BucketDeployment(this, "mydeployment", new BucketDeploymentProps
            {
                DestinationBucket = bucket,
                Sources = new[] {Source.Asset("./site-contents")}
            });

            new CfnOutput(this, "bucketurl", new CfnOutputProps
            {
                Value = bucket.BucketWebsiteUrl
            });
        }
    }
}
