CREATE TABLE [dbo].[Candidates]
(
	[Id] INT IDENTITY (1, 1) NOT NULL, 
	[UserId] INT NOT NUll,
	[Position] NVARCHAR(MAX) NOT NULL,
	[LeadershipExperience] BIT NOT NULL,
	[CurrentLocation] NVARCHAR(MAX) NOT NULL,
	[ExperienceInYears] INT NOT NULL,
	[EnglishLevel] NVARCHAR(MAX) NOT NULL,
	[ConsiderRelocation] BIT NOT NULL,
	[IsRemote] BIT NOT NULL,
	[LinkedinUrl] NVARCHAR(MAX) NULL,
	[CvUrl] NVARCHAR(MAX) NULL,
	[IsApproved] BIT NULL,
	[RejectionReason] NVARCHAR(MAX) NULL,
	[ShowPersonalInfo] BIT NOT NULL,
	[DateCreated] DATETIME2 NULL,
	CONSTRAINT [PK_Candidates] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Candidates_Users] FOREIGN KEY (UserId) REFERENCES [dbo].[Users] ([Id])
)
