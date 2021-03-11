SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--------------- Project ---------------

DROP TABLE IF EXISTS [dbo].[Projects]
CREATE TABLE [dbo].[Projects] (
    [Id]                NVARCHAR (20)   NOT NULL,
    [Name]              NVARCHAR (50)   NOT NULL,
	[Description]		NVARCHAR (MAX)  NULL,

    [CreatorId]         NVARCHAR (50)   NOT NULL,
    [CreationTime]      DATETIME        NOT NULL,
    [EditorId]          NVARCHAR (50)   NULL,
    [LastEditTime]      DATETIME        NULL,
    [IsValid]           BIT             NOT NULL,

    PRIMARY KEY CLUSTERED (Id ASC)
)
GO

--------------- Seed Data ---------------