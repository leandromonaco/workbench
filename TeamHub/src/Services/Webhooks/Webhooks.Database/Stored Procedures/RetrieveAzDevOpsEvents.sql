-- =============================================
-- Author:		Monaco, Leandro
-- Create date: 2021-01-11
-- Description:	Retrieve Webhooks Events
-- =============================================
CREATE PROCEDURE [dbo].[RetrieveAzDevOpsEvents]
	-- Add the parameters for the stored procedure here
	--<@Param1, sysname, @p1> <Datatype_For_Param1, , int> = <Default_Value_For_Param1, , 0>, 
	--<@Param2, sysname, @p2> <Datatype_For_Param2, , int> = <Default_Value_For_Param2, , 0>
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   SELECT  JSON_VALUE([EventData], '$.EventType') AS [Event],
		JSON_VALUE([EventData], '$.Resource.Repository.Name') AS Repository,
		CASE
			WHEN JSON_VALUE([EventData], '$.Resource.SourceRefName') IS NULL THEN JSON_VALUE([EventData], '$.Resource.RefUpdates[0].Name') 
			WHEN JSON_VALUE([EventData], '$.Resource.RefUpdates[0].Name') IS NULL THEN JSON_VALUE([EventData], '$.Resource.SourceRefName') 
			ELSE 'Unknown'
		END AS SourceBranch,
		JSON_VALUE([EventData], '$.Resource.PullRequestId') AS PullRequestID,
		JSON_VALUE([EventData], '$.Resource.PushId') AS PushID,
		JSON_VALUE([EventData], '$.Resource.Status') AS [PullRequestStatus],
				JSON_VALUE([EventData], '$.Resource.TargetRefName') AS TargetBranch,
		JSON_VALUE([EventData], '$.Resource.MergeStatus') AS MergeStatus,
		CASE
			WHEN JSON_VALUE([EventData], '$.Resource.CreatedBy.DisplayName') IS NULL THEN JSON_VALUE([EventData], '$.Resource.PushedBy.DisplayName') 
			WHEN JSON_VALUE([EventData], '$.Resource.PushedBy.DisplayName') IS NULL THEN JSON_VALUE([EventData], '$.Resource.CreatedBy.DisplayName') 
			ELSE 'Unknown'
		END AS CreatedBy,
		JSON_VALUE([EventData], '$.Message.Text') AS [Message],
		JSON_VALUE([EventData], '$.DetailedMessage.Text') AS DetailedMessage,
		CASE
			WHEN JSON_VALUE([EventData], '$.Resource.CreationDate') IS NULL THEN JSON_VALUE([EventData], '$.Resource.Date') 
			WHEN JSON_VALUE([EventData], '$.Resource.Date') IS NULL THEN JSON_VALUE([EventData], '$.Resource.CreationDate') 
			ELSE 'Unknown'
		END AS CreationDate,
		JSON_VALUE([EventData], '$.Resource.ClosedDate') AS ClosedDate
	FROM [Webhooks].[dbo].[Event]
	ORDER BY CreationDate

END
GO


