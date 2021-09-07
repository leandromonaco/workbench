namespace IntegrationConnectors.VersionOne.Model
{
    /// <summary>
    /// //https://community.versionone.com/VersionOne_Connect/Developer_Library/Getting_Started/Platform_Concepts/Asset_State
    /// </summary>
    public enum VersionOneAssetState
    {
        Future = 0,
        Active = 64,
        Closed = 128,
        Template = 200,
        BrokenDown = 208,
        Deleted = 255
    }
}