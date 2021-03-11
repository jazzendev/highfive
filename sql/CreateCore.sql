SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--DROP Existing table and recreate
DROP TABLE IF EXISTS [dbo].[_Logs]
GO

CREATE TABLE [dbo].[_Logs]
(
    [ID] [int] IDENTITY(1,1) NOT NULL,
    [Logged] [datetime] NOT NULL,
    [Level] [varchar](5) NOT NULL,
    [UserName] [nvarchar](200) NULL,
    [Message] [nvarchar](max) NOT NULL,
    [Properties] [nvarchar](max) NULL,
    [ServerName] [nvarchar](200) NULL,
    [Port] [nvarchar](100) NULL,
    [Url] [nvarchar](2000) NULL,
    [Https] [bit] NULL,
    [ServerAddress] [nvarchar](100) NULL,
    [RemoteAddress] [nvarchar](100) NULL,
    [Callsite] [nvarchar](300) NULL,
    [Exception] [nvarchar](max) NULL,
    [UserIdentity] [nvarchar](200) NULL,
    [Controller] [nvarchar](200) NULL,
    [Action] [nvarchar](200) NULL,
    [Operation] [nvarchar](200) NULL,
    [OperationCode] [nvarchar](100) NULL,
    [OperatorId] [nvarchar](50) NULL,
    CONSTRAINT [PK_dbo.Logs] PRIMARY KEY CLUSTERED ([ID] ASC) 
   WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];

-------------------- Portal Logins --------------------

DROP TABLE IF EXISTS [dbo].[_Accounts]
CREATE TABLE [dbo].[_Accounts]
(
    [Id] NVARCHAR (50) NOT NULL,
    [DefaultTenantId] NVARCHAR (50) NOT NULL,

    [Username] NVARCHAR (50) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    [Mobile] NVARCHAR (50) NULL,
    [Email] NVARCHAR (50) NULL,
    [PwdHash] NVARCHAR (MAX) NULL,
    [IsPwdNeedChange] BIT NOT NULL,
    [AvatarUrl] NVARCHAR (MAX) NULL,

    [CreatorId] NVARCHAR (50) NOT NULL,
    [CreationTime] DATETIME NOT NULL,
    [EditorId] NVARCHAR (50) NULL,
    [LastEditTime] DATETIME NULL,
    [IsValid] BIT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

DROP TABLE IF EXISTS [dbo].[_Tenants]
CREATE TABLE [dbo].[_Tenants]
(
    [Id] NVARCHAR (50) NOT NULL,
    [RootId] NVARCHAR (50) NULL,
    [ParentId] NVARCHAR (50) NULL,
    [Name] NVARCHAR (50) NOT NULL,
    [Domain] NVARCHAR (50) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,

    [AccessStartTime] DATETIME NULL,
    [AccessEndTime] DATETIME NULL,
    [BlockStartTime] DATETIME NULL,
    [BlockEndTime] DATETIME NULL,

    [CreatorId] NVARCHAR (50) NOT NULL,
    [CreationTime] DATETIME NOT NULL,
    [EditorId] NVARCHAR (50) NULL,
    [LastEditTime] DATETIME NULL,
    [IsValid] BIT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

DROP TABLE IF EXISTS [dbo].[_TenantServices]
CREATE TABLE [dbo].[_TenantServices]
(
    [Id] NVARCHAR (50) NOT NULL,
    [TenantId] NVARCHAR (50) NOT NULL,
    [ServiceCode] NVARCHAR (50) NOT NULL,

    [AccessStartTime] DATETIME NULL,
    [AccessEndTime] DATETIME NULL,
    [BlockStartTime] DATETIME NULL,
    [BlockEndTime] DATETIME NULL,

    [CreatorId] NVARCHAR (50) NOT NULL,
    [CreationTime] DATETIME NOT NULL,
    [EditorId] NVARCHAR (50) NULL,
    [LastEditTime] DATETIME NULL,
    [IsValid] BIT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

DROP TABLE IF EXISTS [dbo].[_TenantAccounts]
CREATE TABLE [dbo].[_TenantAccounts]
(
    [Id] NVARCHAR (50) NOT NULL,
    [TenantId] NVARCHAR (50) NOT NULL,
    [AccountId] NVARCHAR (50) NOT NULL,

    [AccessStartTime] DATETIME NULL,
    [AccessEndTime] DATETIME NULL,
    [BlockStartTime] DATETIME NULL,
    [BlockEndTime] DATETIME NULL,

    [CreatorId] NVARCHAR (50) NOT NULL,
    [CreationTime] DATETIME NOT NULL,
    [EditorId] NVARCHAR (50) NULL,
    [LastEditTime] DATETIME NULL,
    [IsValid] BIT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

DROP TABLE IF EXISTS [dbo].[_Roles]
CREATE TABLE [dbo].[_Roles]
(
    [Id] NVARCHAR (50) NOT NULL,
    [TenantId] NVARCHAR (50) NULL,
    [Name] NVARCHAR (50) NOT NULL,
    [Description] NVARCHAR (MAX) NOT NULL,
    [AuthLevel] INT NOT NULL,
    [Type] INT NOT NULL,

    [CreatorId] NVARCHAR (50) NOT NULL,
    [CreationTime] DATETIME NOT NULL,
    [EditorId] NVARCHAR (50) NULL,
    [LastEditTime] DATETIME NULL,
    [IsValid] BIT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

DROP TABLE IF EXISTS [dbo].[_AccountRoles]
CREATE TABLE [dbo].[_AccountRoles]
(
    [Id] NVARCHAR (50) NOT NULL,
    [AccountId] NVARCHAR (50) NOT NULL,
    [RoleId] NVARCHAR (50) NOT NULL,

    [CreatorId] NVARCHAR (50) NOT NULL,
    [CreationTime] DATETIME NOT NULL,
    [EditorId] NVARCHAR (50) NULL,
    [LastEditTime] DATETIME NULL,
    [IsValid] BIT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

DROP TABLE IF EXISTS [dbo].[_Permissions]
CREATE TABLE [dbo].[_Permissions]
(
    [Id] NVARCHAR (50) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    [Description] NVARCHAR (MAX) NOT NULL,
    [AuthLevel] INT NOT NULL,
    [Type] INT NOT NULL,

    [CreatorId] NVARCHAR (50) NOT NULL,
    [CreationTime] DATETIME NOT NULL,
    [EditorId] NVARCHAR (50) NULL,
    [LastEditTime] DATETIME NULL,
    [IsValid] BIT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

DROP TABLE IF EXISTS [dbo].[_RolePermissions]
CREATE TABLE [dbo].[_RolePermissions]
(
    [Id] NVARCHAR (50) NOT NULL,
    [RoleId] NVARCHAR (50) NOT NULL,
    [PermissionId] NVARCHAR (50) NOT NULL,

    [CreatorId] NVARCHAR (50) NOT NULL,
    [CreationTime] DATETIME NOT NULL,
    [EditorId] NVARCHAR (50) NULL,
    [LastEditTime] DATETIME NULL,
    [IsValid] BIT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

-------------------- seed data --------------------

-- Default Tenant
INSERT INTO [_Tenants]
VALUES
    ('00000000000000000000', NULL, NULL, 'Jazzen Studio', 'jazzen.com', NULL, '2000-01-01', NULL, NULL, NULL, '00000000000000000000', GETDATE(), NULL, NULL, 1)
INSERT INTO [_Tenants]
VALUES
    ('00000000000000000001', NULL, NULL, 'New', 'new.com', NULL, '2000-01-01', NULL, NULL, NULL, '00000000000000000000', GETDATE(), NULL, NULL, 1)

-- Default Tenant Services
INSERT INTO [_TenantServices]
VALUES
('00000000000000000000','00000000000000000000', '00001', '2000-01-01', NULL, NULL, NULL, '00000000000000000000', GETDATE(), NULL, NULL, 1)
-- Default Tenant Services
INSERT INTO [_TenantServices]
VALUES
('00000000000000000001','00000000000000000001', '00001', '2000-01-01', NULL, NULL, NULL, '00000000000000000000', GETDATE(), NULL, NULL, 1)

-- Default Super Admin Account (password: Passw0rd01!)
INSERT INTO [_Accounts]
VALUES
    ('00000000000000000000', '00000000000000000000', 'admin@jazzen', N'超级管理员', NULL, NULL, 'AQAAAAEAACcRAAAAEGTwKPWsy3cvwFhruovTkTuW4HBISSJX93iaPuzW9t3AxR9hxYAdUt7ZrPJq/ZzbpA==', 0, NULL, '00000000000000000000', GETDATE(), NULL, NULL, 1)
INSERT INTO [_Accounts]
VALUES
    ('00000000000000000001', '00000000000000000001', 'sa@new', N'超级管理员', NULL, NULL, 'AQAAAAEAACcRAAAAEGTwKPWsy3cvwFhruovTkTuW4HBISSJX93iaPuzW9t3AxR9hxYAdUt7ZrPJq/ZzbpA==', 0, NULL, '00000000000000000000', GETDATE(), NULL, NULL, 1)


-- Default Tenant Accounts
INSERT INTO [_TenantAccounts]
VALUES
    ('00000000000000000000', '00000000000000000000', '00000000000000000000', '2000-01-01', NULL, NULL, NULL, '00000000000000000000', GETDATE(), NULL, NULL, 1)
INSERT INTO [_TenantAccounts]
VALUES
    ('00000000000000000001', '00000000000000000001', '00000000000000000001', '2000-01-01', NULL, NULL, NULL, '00000000000000000000', GETDATE(), NULL, NULL, 1)

