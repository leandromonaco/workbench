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
            var lambda = new Function(this, "MinimalApiNet6", new FunctionProps
            {
                Runtime = Runtime.DOTNET_6,
                Code = Code.FromAsset("../MinimalApi/bin/Debug/net6.0"),
                Handler = "MinimalApi",
                FunctionName = "minimalApiNet6"
            });

            var api = new LambdaRestApi(this, "APIGatewayNet6", new LambdaRestApiProps
            {
                RestApiName = "APIGatewayNet6",
                Description = "A simple Minimal API with .NET 6",
                Handler = lambda
            }); 
        }
    }
}
