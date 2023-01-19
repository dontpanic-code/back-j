CREATE TABLE [dbo].[CandidateContacts]
(
	[RecruiterId]	INT NOT NULL,
    [CandidateId]   INT NOT NULL,
    [Message]		NVARCHAR(MAX) NULL,
    [RequestDate]   DATETIME2 NOT NULL,
    CONSTRAINT [PK_CandidateContacts] PRIMARY KEY CLUSTERED ([RecruiterId] ASC, [CandidateId] ASC),
    CONSTRAINT [FK_CandidateContacts_Users_RecruiterId] FOREIGN KEY ([RecruiterId]) REFERENCES [dbo].[Users] ([Id]),
    CONSTRAINT [FK_CandidateContacts_Users_CandidateId] FOREIGN KEY ([CandidateId]) REFERENCES [dbo].[Users] ([Id])
)
