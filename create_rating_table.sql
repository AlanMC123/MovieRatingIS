-- 创建Rate表，用于存储电影评分数据
CREATE TABLE Rate (
    RateID INT IDENTITY(1,1) PRIMARY KEY,
    Mno INT NOT NULL,
    Uno INT NULL,
    Rating DECIMAL(2,1) NOT NULL,
    Comment NVARCHAR(500) NULL,
    Time DATETIME NOT NULL,
    
    -- 添加外键约束
    CONSTRAINT FK_Rate_Movie FOREIGN KEY (Mno) REFERENCES Movie(MovieID),
    CONSTRAINT FK_Rate_User FOREIGN KEY (Uno) REFERENCES Users(UserID),
    
    -- 评分范围约束（1-5分）
    CONSTRAINT CK_Rating CHECK (Rating >= 1 AND Rating <= 5)
);

-- 为Movie表添加Rating字段（如果不存在）
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'Rating' AND Object_ID = Object_ID(N'Movie'))
BEGIN
    ALTER TABLE Movie ADD Rating DECIMAL(2,1) DEFAULT 0.0;
END

-- 更新现有电影的评分（初始化为0）
UPDATE Movie SET Rating = 0.0 WHERE Rating IS NULL;