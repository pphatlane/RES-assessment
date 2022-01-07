USE [RES]
GO

/****** Object:  Table [dbo].[CustomerAccountDetails]    Script Date: 2022/01/05 15:38:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CustomerAccountDetails](
	[customerPin] [int] NOT NULL,
	[customerName] [nvarchar](max) NOT NULL,
	[customerAccountNo] [bigint] NOT NULL,
	[customerCurrentBalance] [decimal](18, 0) NOT NULL,
	[customerInitialBalance] [decimal](18, 0) NOT NULL,
	[customerOverdruftBalance] [numeric](18, 0) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


