CREATE TABLE [dbo].[Transactions] ( 
    [TransactionId] INT IDENTITY (1, 1) NOT NULL,
    [DateTime] DATETIMEOFFSET(4) NOT NULL,
    [CorrespondentName] NVARCHAR (200) NOT NULL, 
    [TransactionAmount] INT NOT NULL,
    [ResultBalance] INT NOT NULL,
    [UserId] NVARCHAR (128) NOT NULL, 
    CONSTRAINT [PK_dbo.Transactions] PRIMARY KEY CLUSTERED ([TransactionId] ASC), 
    CONSTRAINT [FK_dbo.Transactions_dbo.AspNetUsers_Id] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE 
);