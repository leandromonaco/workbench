using EnvironmentInitializer;
using Microsoft.Extensions.Configuration;

var debugEnabled = args.Length > 0 && !string.IsNullOrEmpty(args[0]) && args[0].ToLower().Equals("--debug");

var exitEnabled = args.Length > 0 && !string.IsNullOrEmpty(args[0]) && args[0].ToLower().Equals("--exit");

var configuration = new ConfigurationBuilder()
                            .SetBasePath(Environment.CurrentDirectory)
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true)
                            .Build();

var dynamoDbLocalUrl = configuration["DynamoDbLocalUrl"];
var dynamoDbTables = configuration.GetSection("DynamoDbTables").Get<List<DynamoDbTable>>();
var cognitoLocalDbFolder = configuration["CognitoLocalDbFolder"];
var cognitoContainerId = string.Empty;

Console.WriteLine("Cleaning up docker containers...");
Helper.KillProcess("tye");
//TODO: Add all processes here
Helper.KillProcess("Mock.API");
Helper.RunPowerShellCommand(@"docker kill $(docker ps -q)");
Helper.RunPowerShellCommand(@"docker rm --force $(docker ps -a -q)");
Helper.RunPowerShellCommand(@"docker network prune --force");

if (exitEnabled)
{
    Console.WriteLine("Environment is no longer running.");
    return;
}

var tyeYmlFolder = @"C:\Dev\GitHub\Workbench\MicroserviceTemplate";

if (debugEnabled)
{
    Console.WriteLine("Running Tye in debug mode... do not forget to attach the debugger!");
    Helper.RunPowerShellCommand(@"tye run --port 10000 --dashboard --debug *", tyeYmlFolder, false);
}
else
{
    Console.WriteLine("Running Tye...");
    Helper.RunPowerShellCommand(@"tye run --port 10000 --dashboard", tyeYmlFolder, false);
}

Console.WriteLine("Spining up new docker containers...");
var sourceJson = string.Empty;
var cognitoDockerState = string.Empty;
var dynamoDbDockerState = string.Empty;
while (!cognitoDockerState.Equals("running") &&
       !dynamoDbDockerState.Equals("running"))
{
    Thread.Sleep(60000);
    sourceJson = Helper.RunCmdCommand(@"docker container ls --filter ""name = cognito*"" --format=""{{json .}}""");
    cognitoDockerState = Helper.GetJsonPropertyValue("State", sourceJson);
    cognitoContainerId = Helper.GetJsonPropertyValue("ID", sourceJson);
    sourceJson = Helper.RunCmdCommand(@"docker container ls --filter ""name = dynamodb*"" --format=""{{json .}}""");
    dynamoDbDockerState = Helper.GetJsonPropertyValue("State", sourceJson);
}

Console.WriteLine("Starting local environment configuration...");

//DynamoDB
Console.WriteLine("Configuring DynamoDB...");
foreach (var dynamoDbTable in dynamoDbTables)
{
    if (!string.IsNullOrEmpty(dynamoDbTable.TableName))
    {
        Console.WriteLine($"Creating DynamoDB table ({dynamoDbTable.TableName})");
        Helper.RunCmdCommand($"aws --endpoint-url={dynamoDbLocalUrl} dynamodb create-table --table-name {dynamoDbTable.TableName} --attribute-definitions {dynamoDbTable.AttributeDefinitions} --key-schema {dynamoDbTable.KeySchema} --billing-mode PAY_PER_REQUEST");
        Console.WriteLine($"DynamoDB table ({dynamoDbTable.TableName}) has been created.");
    }
}

//Cognito
Console.WriteLine("Configuring Cognito...");
Directory.CreateDirectory(cognitoLocalDbFolder);
File.Copy($@"{Directory.GetCurrentDirectory()}\CognitoLocalDb\clients.json", $@"{cognitoLocalDbFolder}\clients.json", true);
File.Copy($@"{Directory.GetCurrentDirectory()}\CognitoLocalDb\user-pool-test.json", $@"{cognitoLocalDbFolder}\user-pool-test.json", true);
Helper.RunCmdCommand($@"docker container restart {cognitoContainerId}");

Console.WriteLine("Environment configuration is done.");