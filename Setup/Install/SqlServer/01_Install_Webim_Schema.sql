﻿/****** Object:  Table [dbo].[spb_Webim_Histories]    Script Date: 09/06/2013 13:13:15 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Webim_Histories_todel]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Webim_Histories] DROP CONSTRAINT [DF_spb_Webim_Histories_todel]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Webim_Histories_fromdel]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Webim_Histories] DROP CONSTRAINT [DF_spb_Webim_Histories_fromdel]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_Webim_Histories]') AND type in (N'U'))
DROP TABLE [dbo].[spb_Webim_Histories]
GO
/****** Object:  Table [dbo].[spb_Webim_Settings]    Script Date: 09/06/2013 13:13:15 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_Webim_Settings]') AND type in (N'U'))
DROP TABLE [dbo].[spb_Webim_Settings]
GO
/****** Object:  Table [dbo].[spb_Webim_Settings]    Script Date: 09/06/2013 13:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_Webim_Settings]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[spb_Webim_Settings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Uid] [varchar](40)  NOT NULL,
	[Data] [text] NULL,
	[Created] [date] NULL,
	[Updated] [date] NULL,
 CONSTRAINT [PK_spb_Webim_Settings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[spb_Webim_Histories]    Script Date: 09/06/2013 13:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_Webim_Histories]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[spb_Webim_Histories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Send] [tinyint] NULL,
	[Type] [varchar](20) NULL,
	[ToUser] [varchar](50) NOT NULL,
	[FromUser] [varchar](50) NOT NULL,
	[Nick] [varchar](20) NULL,
	[Body] [text] NULL,
	[Style] [varchar](150) NULL,
	[Timestamp] [bigint] NULL,
	[ToDel] [tinyint] NOT NULL CONSTRAINT [DF_spb_Webim_Histories_todel]  DEFAULT ((0)),
	[FromDel] [tinyint] NOT NULL CONSTRAINT [DF_spb_Webim_Histories_fromdel]  DEFAULT ((0)),
	[Created] [date] NULL,
	[Updated] [date] NULL,
 CONSTRAINT [PK_spb_Webim_Histories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_Webim_Histories]') AND name = N'IX_spb_Webim_Histories_from')
CREATE NONCLUSTERED INDEX [IX_spb_Webim_Histories_from] ON [dbo].[spb_Webim_Histories] 
(
	[FromUser] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_Webim_Histories]') AND name = N'IX_spb_Webim_Histories_send')
CREATE NONCLUSTERED INDEX [IX_spb_Webim_Histories_send] ON [dbo].[spb_Webim_Histories] 
(
	[Send] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_Webim_Histories]') AND name = N'IX_spb_Webim_Histories_timestamp')
CREATE NONCLUSTERED INDEX [IX_spb_Webim_Histories_timestamp] ON [dbo].[spb_Webim_Histories] 
(
	[Timestamp] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_Webim_Histories]') AND name = N'IX_spb_Webim_Histories_to')
CREATE NONCLUSTERED INDEX [IX_spb_Webim_Histories_to] ON [dbo].[spb_Webim_Histories] 
(
	[ToUser] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Webim_Histories', N'COLUMN',N'Nick'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'from nick' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Webim_Histories', @level2type=N'COLUMN',@level2name=N'Nick'
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_Webim_Rooms]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[spb_Webim_Rooms](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Owner] [varchar](40) NOT NULL,
    [Name] [varchar](40) NOT NULL,
    [Nick] [varchar](60) NOT NULL,
    [Topic] [varchar](60) NULL,
    [Created] [date] NULL,
    [Updated] [date] NULL,
 CONSTRAINT [PK_spb_Webim_Rooms] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_Webim_Members]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[spb_Webim_Members](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Room] [varchar](40) NULL,
    [Nick] [varchar](40) NULL,
    [Uid] [varchar](40) NULL,
    [Joined] [date] NULL,
 CONSTRAINT [PK_spb_Webim_Members] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_Webim_Blocked]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[spb_Webim_Blocked](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Uid] [varchar](40) NOT NULL,
    [Room] [varchar](40) NOT NULL,
    [Blocked] [date] NULL,
 CONSTRAINT [PK_spb_Webim_Rooms] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_Webim_Visitors]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[spb_Webim_Visitors](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name] [varchar](60) NOT NULL,
    [Ipaddr] [varchar](60) NOT NULL,
    [Url] [varchar](60) NOT NULL,
    [Referer] [varchar](60) NOT NULL,
    [Location] [varchar](60) NOT NULL,
    [Created] [date] NULL,
 CONSTRAINT [PK_spb_Webim_Visitors] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
