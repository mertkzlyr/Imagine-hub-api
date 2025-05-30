CREATE TABLE CommentLikes (
      UserId INT NOT NULL,
      CommentId UNIQUEIDENTIFIER NOT NULL,
      CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
      PRIMARY KEY (UserId, CommentId),
      FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
      FOREIGN KEY (CommentId) REFERENCES PostComments(Id)
);