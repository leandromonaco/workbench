using System.Text;
using System.Text.Json;
using Amazon.AppConfigData;
using Amazon.AppConfigData.Model;

namespace FeatureFlag.API.Configuration
{
    public class AwsFeatureFlagConfigurationProvider : ConfigurationProvider
    {
        private readonly AwsSettings _cfg;
        private readonly StartConfigurationSessionRequest _startRequest;
        private readonly AmazonAppConfigDataClient _client;
        private readonly TimeSpan? _frequency;
        private CancellationToken _cancellationToken = new CancellationToken();
        private Task _task;

        private string _token = string.Empty;
        private readonly string _configSections = string.Empty;

        public AwsFeatureFlagConfigurationProvider(AmazonAppConfigDataClient client, AwsSettings cfg)
        {

            _cfg = cfg;
            _client = client;
            _frequency = _cfg.DataPollFrequency;
            if (!string.IsNullOrWhiteSpace(_cfg.ConfigSectionNaming))
            {
                _configSections = _cfg.ConfigSectionNaming.Trim().Trim(':') + ':';
            }
            _startRequest = new StartConfigurationSessionRequest
            {
                ApplicationIdentifier = _cfg.ApplicationId,
                EnvironmentIdentifier = _cfg.EnvironmentId,
                ConfigurationProfileIdentifier = _cfg.ConfigurationProfileId
            };
        }

        public override void Load()
        {
            RefreshData();

            if (_task == null && _frequency.HasValue)
            {
                _task = new Task(async () =>
                {
                    while (true && !_cancellationToken.IsCancellationRequested)
                    {
                        await Task.Delay(_frequency.Value);
                        RefreshData();
                    }
                });
                _task.Start();
            }
        }

        internal void RefreshData()
        {
            if (string.IsNullOrWhiteSpace(_token))
            {
                var initial = _client.StartConfigurationSessionAsync(_startRequest).ConfigureAwait(false).GetAwaiter().GetResult();
                _token = initial.InitialConfigurationToken;
            }

            var latestResponse = _client.GetLatestConfigurationAsync(
                new GetLatestConfigurationRequest { ConfigurationToken = _token }).ConfigureAwait(false).GetAwaiter().GetResult();
            _token = latestResponse.NextPollConfigurationToken;

            var configBytes = latestResponse.Configuration.ToArray();
            if (configBytes != null && configBytes.Length != 0) // when the length == 0 it means the config didn't change.
            {
                UpdateData(configBytes);
            }
        }

        private void UpdateData(byte[] configBytes)
        {
            var newConfig = Encoding.UTF8.GetString(configBytes);
            var doc = JsonDocument.Parse(newConfig);
            var newData = new Dictionary<string, string>();
            foreach (var item in doc.RootElement.EnumerateObject())
            {
                var featureName = item.Name;
                var isEnabled = item.Value.GetProperty("enabled").GetBoolean().ToString();
                newData[_configSections + featureName] = isEnabled;
            }
            Data = newData;
            OnReload();
        }
    }
}

