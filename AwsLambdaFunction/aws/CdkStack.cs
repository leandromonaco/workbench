using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Logs;
using Amazon.CDK.AWS.DynamoDB;
using Constructs;
using Amazon.CDK.AWS.CertificateManager;

namespace Cdk
{
    public class CdkStack : Stack
    {
        internal CdkStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            //https://www.alexdebrie.com/posts/api-gateway-access-logs/
         
            //    logGroup.set = "/cloudwatchlogs/dotnet6-app/";
            //Bucket logBucket = new Bucket(this, "S3 Bucket");

            // The code that defines your stack goes here
            var lambda = new Function(this, "MinimalApiNet6", new FunctionProps
            {
                Runtime = Runtime.DOTNET_6,
                Code = Code.FromAsset("../MinimalApi/bin/Debug/net6.0"),
                Handler = "MinimalApi",
                FunctionName = "minimalApiNet6",
                LogRetention = RetentionDays.ONE_MONTH
            });

            LogGroup logGroup = new LogGroup(this, "DevLogs");

            var api = new LambdaRestApi(this, "APIGatewayNet6", new LambdaRestApiProps
            {
                RestApiName = "APIGatewayNet6",
                Description = "A simple Minimal API with .NET 6",
                Handler = lambda,
                DeployOptions = new StageOptions()
                {
                    AccessLogDestination = new LogGroupLogDestination(logGroup),
                    AccessLogFormat = AccessLogFormat.JsonWithStandardFields(),
                },
                DomainName = new DomainNameOptions()
                {
                    DomainName = "minimalapi.net6.com",
                    //Certificate = new Certificate(this, "Certificate", new CertificateProps()
                    //{
                    //    SubjectAlternativeNames = new string[] { "minimalapi.net6.com" },
                    //}),
                    //SecurityPolicy = SecurityPolicy.TLS_1_2
                }
            });

            Deployment deployment = new Deployment(this, "Deployment", new DeploymentProps { Api = api });

            new Amazon.CDK.AWS.APIGateway.Stage(this, "dev", new Amazon.CDK.AWS.APIGateway.StageProps
            {
                Deployment = deployment,
                AccessLogDestination = new LogGroupLogDestination(logGroup),
                AccessLogFormat = AccessLogFormat.JsonWithStandardFields(new JsonWithStandardFieldProps
                {
                    Caller = false,
                    HttpMethod = true,
                    Ip = true,
                    Protocol = true,
                    RequestTime = true,
                    ResourcePath = true,
                    ResponseLength = true,
                    Status = true,
                    User = true
                })
            });

            new Amazon.CDK.AWS.APIGateway.Stage(this, "test", new Amazon.CDK.AWS.APIGateway.StageProps
            {
                Deployment = deployment,
                AccessLogDestination = new LogGroupLogDestination(logGroup),
                AccessLogFormat = AccessLogFormat.JsonWithStandardFields(new JsonWithStandardFieldProps
                {
                    Caller = false,
                    HttpMethod = true,
                    Ip = true,
                    Protocol = true,
                    RequestTime = true,
                    ResourcePath = true,
                    ResponseLength = true,
                    Status = true,
                    User = true
                })
            });

            var dynamoTable = new Table(this, "DynamoDBTable", new TableProps
            {
                TableName = "TMP_DYNAMODB_TABLE",
                PartitionKey = new Attribute { Name = "Id", Type = AttributeType.STRING },
                BillingMode = BillingMode.PAY_PER_REQUEST
            });

            dynamoTable.GrantFullAccess(lambda);

        }
    }
}
