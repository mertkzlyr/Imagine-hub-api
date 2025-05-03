CREATE TABLE UserFollows (
     FollowerId INT NOT NULL,
     FolloweeId INT NOT NULL,
     FollowedAt DATETIME2 DEFAULT GETDATE(),

     CONSTRAINT PK_UserFollows PRIMARY KEY (FollowerId, FolloweeId),

     CONSTRAINT FK_UserFollows_Follower FOREIGN KEY (FollowerId)
         REFERENCES Users(Id) ON DELETE CASCADE,

     CONSTRAINT FK_UserFollows_Followee FOREIGN KEY (FolloweeId)
         REFERENCES Users(Id) ON DELETE CASCADE,

     CONSTRAINT CHK_NoSelfFollow CHECK (FollowerId <> FolloweeId)
);