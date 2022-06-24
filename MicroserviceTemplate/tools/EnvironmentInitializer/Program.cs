using System.Text.Json;
using CmdRunner;
using CmdRunner.Model;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
                            .SetBasePath(Environment.CurrentDirectory)
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true)
                            .Build();

var kmsLocalUrl = configuration["KmsLocalUrl"];
var dynamoDbLocalUrl = configuration["DynamoDbLocalUrl"];
var targetConfigurationFiles = configuration.GetSection("TargetConfigurationFiles").GetChildren().ToList();
var dynamoDbTables = configuration.GetSection("DynamoDbTables").Get<List<DynamoDbTable>>();

/*
 * TODO: Clean up environment to re-run Tye
 * Kill Tye process
 * wsl --shutdown
 * docker kill $(docker ps -q)
 * docker rm $(docker ps -a -q
 * docker network prune
 * tye run --port 10000 --dashboard
 */

//KMS
var sourceJson = Helper.RunCommand(@"docker container ls --filter ""name = kms*"" --format=""{{json .}}""");
var kmsDockerState = Helper.GetJsonPropertyValue("State", sourceJson);
if (kmsDockerState.Equals("running"))
{
    Console.WriteLine("KMS container is running");

    var existingKeys = Helper.RunCommand($"aws --endpoint-url={kmsLocalUrl} kms --region ap-southeast-2 list-keys");
    var existingKeysList = JsonSerializer.Deserialize<KmsKeys>(existingKeys);
    var KmsKeyId = existingKeysList.Keys.FirstOrDefault()?.KeyId;

    if (KmsKeyId == null)
    {
        //Create new KMS Key
        sourceJson = Helper.RunCommand($"aws --endpoint-url={kmsLocalUrl} kms --region ap-southeast-2 create-key --key-spec RSA_2048 --key-usage SIGN_VERIFY");
        var newKmsKeyId = Helper.GetJsonPropertyValue("KeyMetadata.KeyId", sourceJson);
        KmsKeyId = newKmsKeyId;
    }

    Console.WriteLine($"KeyId is {KmsKeyId}");

    //Get the new Public Key based on the newly created Key
    sourceJson = Helper.RunCommand($"aws --endpoint-url={kmsLocalUrl} kms --region ap-southeast-2 get-public-key --key-id {KmsKeyId}");

    var KmsPublicKey = Helper.GetJsonPropertyValue("PublicKey", sourceJson);

    Console.WriteLine($"PublicKey is {KmsPublicKey}");

    //Update the target configuration files
    foreach (var filename in targetConfigurationFiles.Select(t => t.Value))
    {
        Console.WriteLine($"Updating {filename}");
        var fileInfo = new FileInfo(filename);
        switch (fileInfo.Extension)
        {
            case ".json":
                Helper.UpdateTargetPropertyJson(filename, KmsKeyId, KmsPublicKey);
                break;

            case ".xml":
            case ".config":
                Helper.UpdateTargetPropertyXml(filename, KmsKeyId, KmsPublicKey);
                break;

            default:
                break;
        }
        Console.WriteLine($"{filename} has been updated");
    }
}
else
{
    Console.WriteLine("KMS container is not running");
}

//DynamoDB
sourceJson = Helper.RunCommand(@"docker container ls --filter ""name = dynamodb*"" --format=""{{json .}}""");
var dynamoDbDockerState = Helper.GetJsonPropertyValue("State", sourceJson);
if (dynamoDbDockerState.Equals("running"))
{
    Console.WriteLine("DynamoDB container is running");

    foreach (var dynamoDbTable in dynamoDbTables)
    {
        if (!string.IsNullOrEmpty(dynamoDbTable.TableName))
        {
            Console.WriteLine($"Creating DynamoDB table ({dynamoDbTable.TableName})");
            Helper.RunCommand($"aws --endpoint-url={dynamoDbLocalUrl} dynamodb create-table --table-name {dynamoDbTable.TableName} --attribute-definitions {dynamoDbTable.AttributeDefinitions} --key-schema {dynamoDbTable.KeySchema} --billing-mode PAY_PER_REQUEST");
            Console.WriteLine($"DynamoDB table ({dynamoDbTable.TableName}) has been created.");
        }
    }
}
else
{
    Console.WriteLine("DynamoDB container is not running");
}

Console.WriteLine("Press any key to finish the application");
Console.ReadLine();