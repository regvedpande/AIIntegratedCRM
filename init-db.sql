-- ============================================================
-- AIIntegratedCRM - Database Initialization Script
-- Run AFTER EF Core migrations: dotnet ef database update
-- ============================================================

USE AIIntegratedCRM;
GO

-- =============================================
-- ADDITIONAL PERFORMANCE INDEXES
-- =============================================

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Leads_TenantId_Status_Score')
    CREATE NONCLUSTERED INDEX IX_Leads_TenantId_Status_Score
    ON [Leads] (TenantId, Status, AIScore DESC)
    WHERE IsDeleted = 0;

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Opportunities_TenantId_Stage')
    CREATE NONCLUSTERED INDEX IX_Opportunities_TenantId_Stage
    ON [Opportunities] (TenantId, Stage)
    INCLUDE (Amount, AIProbability, ExpectedCloseDate)
    WHERE IsDeleted = 0;

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Contacts_TenantId_Email')
    CREATE NONCLUSTERED INDEX IX_Contacts_TenantId_Email
    ON [Contacts] (TenantId, Email)
    WHERE IsDeleted = 0;

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Activities_TenantId_StartTime')
    CREATE NONCLUSTERED INDEX IX_Activities_TenantId_StartTime
    ON [Activities] (TenantId, StartTime DESC)
    WHERE IsDeleted = 0;

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_SupportTickets_TenantId_Status_Priority')
    CREATE NONCLUSTERED INDEX IX_SupportTickets_TenantId_Status_Priority
    ON [SupportTickets] (TenantId, Status, Priority)
    WHERE IsDeleted = 0;

GO

-- =============================================
-- DASHBOARD STATISTICS VIEW
-- =============================================

IF OBJECT_ID('vw_TenantDashboardStats', 'V') IS NOT NULL
    DROP VIEW vw_TenantDashboardStats;
GO

CREATE VIEW vw_TenantDashboardStats AS
SELECT
    t.Id                                                        AS TenantId,
    t.Name                                                      AS TenantName,
    (SELECT COUNT(*) FROM Leads       l  WHERE l.TenantId  = t.Id AND l.IsDeleted  = 0)                          AS TotalLeads,
    (SELECT COUNT(*) FROM Leads       l  WHERE l.TenantId  = t.Id AND l.IsDeleted  = 0 AND l.Status = 0)          AS NewLeads,
    (SELECT COUNT(*) FROM Contacts    c  WHERE c.TenantId  = t.Id AND c.IsDeleted  = 0)                          AS TotalContacts,
    (SELECT COUNT(*) FROM Accounts    a  WHERE a.TenantId  = t.Id AND a.IsDeleted  = 0)                          AS TotalAccounts,
    (SELECT COUNT(*) FROM Opportunities o WHERE o.TenantId = t.Id AND o.IsDeleted  = 0)                          AS TotalOpportunities,
    (SELECT ISNULL(SUM(o.Amount),0)   FROM Opportunities o WHERE o.TenantId = t.Id AND o.IsDeleted = 0 AND o.Stage NOT IN (5)) AS TotalPipelineValue,
    (SELECT COUNT(*) FROM SupportTickets st WHERE st.TenantId = t.Id AND st.IsDeleted = 0 AND st.Status = 0)     AS OpenTickets
FROM Tenants t
WHERE t.IsActive = 1;
GO

-- =============================================
-- PIPELINE FUNNEL STORED PROCEDURE
-- =============================================

IF OBJECT_ID('sp_GetPipelineFunnel', 'P') IS NOT NULL
    DROP PROCEDURE sp_GetPipelineFunnel;
GO

CREATE PROCEDURE sp_GetPipelineFunnel
    @TenantId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        Stage,
        COUNT(*)              AS DealCount,
        ISNULL(SUM(Amount),0) AS TotalValue,
        ISNULL(AVG(AIProbability),0) AS AvgWinProbability
    FROM Opportunities
    WHERE TenantId = @TenantId AND IsDeleted = 0
    GROUP BY Stage
    ORDER BY Stage;
END;
GO

-- =============================================
-- LEAD SOURCE CONVERSION STATS
-- =============================================

IF OBJECT_ID('sp_GetLeadConversionStats', 'P') IS NOT NULL
    DROP PROCEDURE sp_GetLeadConversionStats;
GO

CREATE PROCEDURE sp_GetLeadConversionStats
    @TenantId  UNIQUEIDENTIFIER,
    @StartDate DATETIME2 = NULL,
    @EndDate   DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET @StartDate = ISNULL(@StartDate, DATEADD(MONTH, -12, GETUTCDATE()));
    SET @EndDate   = ISNULL(@EndDate,   GETUTCDATE());

    SELECT
        Source,
        COUNT(*)                                                                                AS TotalLeads,
        SUM(CASE WHEN Status = 4 THEN 1 ELSE 0 END)                                           AS ConvertedLeads,
        CAST(SUM(CASE WHEN Status=4 THEN 1.0 ELSE 0 END) / NULLIF(COUNT(*),0) * 100 AS DECIMAL(5,2)) AS ConversionRate,
        AVG(AIScore)                                                                           AS AvgAIScore
    FROM Leads
    WHERE TenantId = @TenantId
      AND IsDeleted = 0
      AND CreatedAt BETWEEN @StartDate AND @EndDate
    GROUP BY Source
    ORDER BY TotalLeads DESC;
END;
GO

PRINT 'Database initialization completed successfully.';
