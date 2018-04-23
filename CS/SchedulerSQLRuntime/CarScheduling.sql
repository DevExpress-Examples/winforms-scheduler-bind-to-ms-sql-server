USE [CarsDB]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CarScheduling](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CarId] [int] NULL,
	[UserId] [int] NULL,
	[Status] [int] NULL,
	[Subject] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[Label] [int] NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[Location] [nvarchar](50) NULL,
	[AllDay] [bit] NULL,
	[EventType] [int] NULL,
	[RecurrenceInfo] [nvarchar](max) NULL,
	[ReminderInfo] [nvarchar](max) NULL,
	[Price] [money] NULL,
	[ContactInfo] [nvarchar](max) NULL,
	[SSMA_TimeStamp] [timestamp] NOT NULL,
 CONSTRAINT [CarScheduling$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[CarScheduling] ADD  DEFAULT ((0)) FOR [CarId]
GO

ALTER TABLE [dbo].[CarScheduling] ADD  DEFAULT ((0)) FOR [UserId]
GO

ALTER TABLE [dbo].[CarScheduling] ADD  DEFAULT ((0)) FOR [Status]
GO

ALTER TABLE [dbo].[CarScheduling] ADD  DEFAULT ((0)) FOR [Label]
GO

ALTER TABLE [dbo].[CarScheduling] ADD  DEFAULT ((0)) FOR [AllDay]
GO

ALTER TABLE [dbo].[CarScheduling] ADD  DEFAULT ((0)) FOR [EventType]
GO

ALTER TABLE [dbo].[CarScheduling] ADD  DEFAULT ((0)) FOR [Price]
GO

