using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.Lambda;
using Constructs;

namespace Cdk
{
    public class CdkStack : Stack
    {
        internal CdkStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            // The code that defines your stack goes here
            var lambda = new Function(this, "HelloLambda", new FunctionProps
            {
                Runtime = Runtime.DOTNET_6,
                Code = Code.FromAsset("../MinimalApi/bin/Debug/net6.0"),
                Handler = "MinimalApi",
                FunctionName = "helloLambda"
            });

            var api = new LambdaRestApi(this, "HelloApi", new LambdaRestApiProps
            {
                RestApiName = "HelloApi",
                Description = "A simple hello world API",
                Handler = lambda
            }); 
        }
    }
}
