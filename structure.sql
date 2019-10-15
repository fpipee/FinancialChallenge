--Nombre Base de Datos [challenge]
GO

/****** Object:  Table [dbo].[registro]    Script Date: 14/10/2019 2:37:19 a. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[registro](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[name] [text] NULL,
	[email] [text] NULL,
	[datelog] [datetime] NOT NULL DEFAULT (getdate()),
	[idtrasa] [text] NULL,
	[ended] [bit] NULL,
	[userattender] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


CREATE TABLE [dbo].[userAttender](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[name] [text] NULL,
	[username] [text] NULL,
	[pass] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO