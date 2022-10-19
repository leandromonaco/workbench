using EnvironmentInitializer;
using Microsoft.Extensions.Configuration;

Console.ForegroundColor = ConsoleColor.White;

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
Helper.KillProcess("ServiceName.API");
Helper.KillProcess("Authentication.API");
Helper.KillProcess("FeatureManagement.API");
Helper.KillProcess("Analytics.API");
Helper.KillProcess("Mock.API");
Helper.RunCommand(ConsoleMode.Powershell, @"docker kill $(docker ps -q)");
Helper.RunCommand(ConsoleMode.Powershell, @"docker rm --force $(docker ps -a -q)");
Helper.RunCommand(ConsoleMode.Powershell, @"docker network prune --force");

if (exitEnabled)
{
    Console.WriteLine("Environment is no longer running.");
    return;
}

//TODO: Make this a parameter
var tyeYmlFolder = @"C:\Dev\GitHub\Workbench\MicroserviceTemplate";

if (debugEnabled)
{
    Console.WriteLine("Running Tye in debug mode... do not forget to attach the debugger!");
    Helper.RunCommand(ConsoleMode.Powershell, @"tye run --port 10000 --dashboard --debug *", tyeYmlFolder, false);
}
else
{
    Console.WriteLine("Spining up new docker containers...");
    Helper.RunCommand(ConsoleMode.Powershell, @"tye run --port 10000 --dashboard", tyeYmlFolder, false);
}


var sourceJson = string.Empty;

//Cognito
Console.WriteLine("Waiting for Cognito container to be in RUNNING state");
var containerState = string.Empty;
while (!containerState.Equals("running"))
{
    sourceJson = Helper.RunCommand(ConsoleMode.CommandPrompt, @"docker container ls --filter ""name = cognito*"" --format=""{{json .}}""");
    containerState = Helper.GetJsonPropertyValue("State", sourceJson!);
    cognitoContainerId = Helper.GetJsonPropertyValue("ID", sourceJson!);
}

//DynamoDB
Console.WriteLine("Waiting for DynamoDB container to be in RUNNING state");
containerState = string.Empty;
while (!containerState.Equals("running"))
{
    sourceJson = Helper.RunCommand(ConsoleMode.CommandPrompt, @"docker container ls --filter ""name = dynamodb*"" --format=""{{json .}}""");
    containerState = Helper.GetJsonPropertyValue("State", sourceJson!);
}

//Once the container is running, we wait 40 seconds for DynamoDB to warm up
Console.WriteLine("DynamoDB is warming up...");
Thread.Sleep(40000);

//DynamoDB
foreach (var dynamoDbTable in dynamoDbTables)
{
    if (!string.IsNullOrEmpty(dynamoDbTable.TableName))
    {
        Console.WriteLine($"Creating DynamoDB table ({dynamoDbTable.TableName})");
        Helper.RunCommand(ConsoleMode.CommandPrompt, $"aws --endpoint-url={dynamoDbLocalUrl} dynamodb create-table --table-name {dynamoDbTable.TableName} --attribute-definitions {dynamoDbTable.AttributeDefinitions} --key-schema {dynamoDbTable.KeySchema} --billing-mode PAY_PER_REQUEST --region ap-southeast-2");
        Console.WriteLine($"DynamoDB table ({dynamoDbTable.TableName}) has been created.");
    }
}

//Cognito
Console.WriteLine("Configuring Cognito...");
Directory.CreateDirectory(cognitoLocalDbFolder);
File.Copy($@"{Directory.GetCurrentDirectory()}\CognitoLocalDb\clients.json", $@"{cognitoLocalDbFolder}\clients.json", true);
File.Copy($@"{Directory.GetCurrentDirectory()}\CognitoLocalDb\user-pool-test.json", $@"{cognitoLocalDbFolder}\user-pool-test.json", true);
Helper.RunCommand(ConsoleMode.CommandPrompt, $@"docker container restart {cognitoContainerId} -t 0");

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Environment configuration is done.");
Console.ForegroundColor = ConsoleColor.White;