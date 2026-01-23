# 一、系统概述
### 1.1 系统目标与愿景
开发一个电影评分系统，为用户提供集电影浏览、评价互动与个性化管理于一体的在线服务平台，包括以下功能特性：

1. 电影信息展示：展示电影的基本信息（名称、类型、年份、时长、发行商等），支持用户根据名称、类型或年份进行搜索。
2. 用户评价功能：用户可以为每部电影提交评分（1~5分）并留下文字评论；系统自动计算电影的平均评分并显示。
3. 用户中心功能：用户可以将电影加入收藏夹、查看已发送的电影评分与评论；可以修改密码、管理个人信息。
4. 权限管理：用户必须注册并登录后才能访问系统，使用评分和收藏等功能。
5. 展示UI：UI应该简洁优雅，展示用户需要看到的信息，切换动画流畅。

### 1.2 系统架构
系统采用 ASP.NET Web Forms + SQL Server 架构，前后端和数据库逻辑清晰分离。

#### 1.3.1 表示层
+ ASP.NET Web Forms 页面：使用Web 控件呈现用户界面。
+ 事件处理：当用户触发某些事件时，处理用户输入并执行相关操作。
+ 会话：存储用户的登录信息和状态，确保用户在整个会话期间的数据一致性。

#### 1.3.2 业务逻辑层
映射用户请求，执行系统操作，例如评分、评论、电影查询等。

+ Page Code-Behind：接收并处理来自表示层的用户请求。
+ 控制器逻辑：页面跳转逻辑的处理与执行。

#### 1.3.3 数据访问层
提供对数据库的直接访问，封装 SQL 查询和数据库操作，并为其他层提供统一的接口。

#### 1.3.4 数据库层
主要表：

+ `Users`：用户信息
+ `Movie`：电影信息
+ `Rate`：评分记录
+ `Favorite`：收藏记录

## 二、功能需求
### 2.1 用户认证
#### 2.1.1 用户注册
+ 用户可创建账户，需填写用户名、密码及基本信息。
+ 输入数据需通过校验。

#### 2.1.2用户登录
+ 系统验证用户名与密码。
+ 登录成功后将相关信息写入会话，并跳转至主页。
+ 登录失败时给出错误提示。

### 2.2 主页
+ 展示用户相关功能，如更改密码与退出登陆。
+ 分页展示电影列表与简略信息，包括平均分。可以点按按钮将电影添加至收藏，可以点击查看电影详细信息。
+ 搜索。

### 2.3 **电影详细信息**
+ 显示当前选中的电影信息，并按时间顺序展示评分与评论。
+ 每个电影的评分支持分页显示，每页最多10条。

### 2.4 评分
滑动选择星星数以对电影提交评分。

同时可以选择添加评论。

### 2.5 收藏
列表展示当前登陆用户收藏的所有电影。

点击按钮可将当前电影移除收藏。

## 三、数据层设计

### 3.1 Users表
| 数据项 | 含义 | 别名 | 类型 | 长度 | 取值范围 | 取值含义 | 与其他数据项关系 |
| --- | --- | --- | --- | --- | --- | --- | --- |
| Uno | 唯一标识用户 | 用户ID | char | 10 | | | primary key |
| Uname | | 用户名 | varchar | 30 | not null  | | |
| Usex | | 用户性别 | char | 2 | {'男', '女'} | | |
| Utelephone | | 用户电话号码 | varchar | 11 | | | |
| Upassword |  | 用户密码 | char | 10 | not null | | |
### 3.2 Movie表
| 数据项 | 含义 | 别名 | 类型 | 长度 | 取值范围 | 取值含义 | 与其他数据项关系 |
| --- | --- | --- | --- | --- | --- | --- | --- |
| Mno | 唯一标识电影 | 电影ID | char | 10 | | | primary key |
| Mname | | 电影名称 | varchar | 50 | not null | | |
| Mtype | | 电影类型 | varchar | 10 | | | |
| Myear | | 上映年份 | int | | >= 1900 | | |
| Mtime | | 电影时长 | int | |  > 0 | | |
| Distributer | | 发行商 | varchar | 20 | | | |
### 3.3 Rate表
| 数据项 | 含义 | 别名 | 类型 | 长度 | 取值范围 | 取值含义 | 与其他数据项关系 |
| --- | --- | --- | --- | --- | --- | --- | --- |
| Uno | 发布评分的用户ID | 用户ID | char | 10 | | | 外键，与Mno组成主键 |
| Mno | 被评分的电影ID | 电影ID | char | 10 | | | 外键，与Uno组成主键 |
| Time | | 评分时间 | datetime | | not null  | | |
| Rating | | 评分 | decimal | | Not null, 0<= Rating <= 10 | | |
| Comment | | 评论 | varchar | 3000 | | | |
### 3.4 Favorite表
| 数据项 | 含义 | 别名 | 类型 | 长度 | 取值范围 | 取值含义 | 与其他数据项关系 |
| --- | --- | --- | --- | --- | --- | --- | --- |
| Uno |  | 用户ID | char | 10 | | | 外键，与Mno组成主键 |
| Mno | | 电影ID | char | 10 | | | 外键，与Uno组成主键 |
| Type | 用户为收藏的电影自定义的分类 | 收藏分类 |  varchar | 100 | not null | | |

## 四、逻辑层设计
### 4.1 数据库连接
在数据库连接中，项目采用ADO.NET机制，并设置了相应的web.config配置文件。Web.config中设置连接字符串如下：
```xml
<add name="MovieRatingConnection" 
  connectionString="Data Source=(local); Initial Catalog=MovieRating; User ID=xxx;Password=xxx;Integrated Security=False;MultipleActiveResultSets=True;" 
  providerName="System.Data.SqlClient" />
```

Main.aspx.cs中连接并调用：

```csharp
string connectionString = ConfigurationManager.ConnectionStrings["MovieRatingConnection"].ConnectionString;

using (SqlConnection connection = new SqlConnection(connectionString))
{
    using (SqlCommand command = new SqlCommand(query, connection))
    {
        if (!string.IsNullOrEmpty(searchTerm))
        {
            command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
        }

        SqlDataAdapter adapter = new SqlDataAdapter(command);
        DataTable dataTable = new DataTable();
        adapter.Fill(dataTable);

        gvMovies.DataSource = dataTable;
        gvMovies.DataBind();

        UpdatePaginationInfo();
    }
}
```

### 4.2 事件响应
#### 4.2.1 用户认证事件
- **登录事件**：connection.aspx.cs中处理用户登录，验证用户名密码，成功后存储用户信息到Session，并跳转至主页。
```csharp
// connection.aspx.cs - 登录验证
DataTable userTable = userDal.GetUserByUsername(username);
if (userTable.Rows.Count > 0) 
{
    string dbPwd = userTable.Rows[0]["Upassword"].ToString().Trim();
    if (dbPwd == password) 
    {
        Session["Username"] = username;
        Session["UserID"] = userTable.Rows[0]["Uno"].ToString();
        Response.Redirect("~/main_page/main.aspx");
    }
}
```

- **注册事件**：register.aspx.cs中处理用户注册，插入新用户记录到Users表，并检查用户名是否已存在。
```csharp
// register.aspx.cs - 用户注册
DataTable checkUser = userDal.GetUserByUsername(username);
if (checkUser.Rows.Count > 0)
{
    Response.Write("<script>alert('用户名已被注册！');</script>");
    return;
}

bool isSuccess = userDal.AddUser(user);
if (isSuccess)
{
    Response.Write("<script>alert('注册成功！请登录');</script>");
}
```

#### 4.2.2 主页事件
- **搜索电影**：main.aspx.cs中的btnSearch_Click事件根据输入的关键词查询电影列表。
```csharp
// main.aspx.cs - 搜索电影
protected void btnSearch_Click(object sender, EventArgs e)
{
    gvMovies.PageIndex = 0;
    BindGridView();
}
```

- **分页事件**：gvMovies_PageIndexChanging及分页按钮事件（首页、上页、下页、末页）实现电影列表分页浏览。
```csharp
// main.aspx.cs - GridView分页
protected void gvMovies_PageIndexChanging(object sender, GridViewPageEventArgs e)
{
    gvMovies.PageIndex = e.NewPageIndex;
    BindGridView();
}

protected void lnkFirst_Click(object sender, EventArgs e)
{
    gvMovies.PageIndex = 0;
    BindGridView();
}
```

- **评分电影**：btnRateMovie_Click事件获取选中的电影并跳转至评分页面。
```csharp
// main.aspx.cs - 跳转到评分页面
foreach (GridViewRow row in gvMovies.Rows)
{
    RadioButton rb = (RadioButton)row.FindControl("rbSelectMovie");
    if (rb != null && rb.Checked)
    {
        string movieIdStr = gvMovies.DataKeys[row.RowIndex].Value.ToString();
        string userId = Session["UserID"] != null ? Session["UserID"].ToString() : "";
        string movieTitle = row.Cells[2].Text;
        
        Response.Redirect("../rating_page/rating.html?movieId=" + movieIdStr + 
                         "&movieTitle=" + Server.UrlEncode(movieTitle) + 
                         "&userId=" + userId);
        return;
    }
}
```

- **收藏电影**：btnFavorite_Click事件将选中的电影添加到当前用户的收藏中。
```csharp
// main.aspx.cs - 添加收藏
private bool AddFavorite(string userId, string movieId)
{
    string checkQuery = "SELECT COUNT(*) FROM Favorite WHERE Uno = @Uno AND Mno = @Mno";
    using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
    {
        checkCommand.Parameters.AddWithValue("@Uno", userId);
        checkCommand.Parameters.AddWithValue("@Mno", movieId);
        int count = Convert.ToInt32(checkCommand.ExecuteScalar());
        if (count > 0)
        {
            return false; 
        }
    }
    
    string insertQuery = "INSERT INTO Favorite (Uno, Mno, Type) VALUES (@Uno, @Mno, @Type)";
    // 执行插入操作
}
```

- **查看收藏**：btnViewFavorites_Click跳转至收藏列表页面。
```csharp
// main.aspx.cs - 查看收藏列表
protected void btnViewFavorites_Click(object sender, EventArgs e)
{
    Response.Redirect("/favorite_page/favorite.aspx");
}
```

- **查看评分**：btnViewRatings_Click跳转至电影评分详情页面。
```csharp
// main.aspx.cs - 查看电影评分
foreach (GridViewRow row in gvMovies.Rows)
{
    RadioButton rb = (RadioButton)row.FindControl("rbSelectMovie");
    if (rb != null && rb.Checked)
    {
        string movieIdStr = gvMovies.DataKeys[row.RowIndex].Value.ToString();
        string movieTitle = row.Cells[2].Text;
        
        Response.Redirect("../view_rating_page/view_rating.aspx?movieId=" + 
                         movieIdStr + "&movieTitle=" + Server.UrlEncode(movieTitle));
        return;
    }
}
```

#### 4.2.3 收藏管理事件
- **移除收藏**：favorite.aspx.cs中的gvFavorites_RowCommand处理移除收藏操作，并更新Favorite表。
```csharp
// favorite.aspx.cs - 移除收藏
protected void gvFavorites_RowCommand(object sender, GridViewCommandEventArgs e)
{
    if (e.CommandName == "RemoveFavorite")
    {
        string movieId = e.CommandArgument.ToString();
        string userId = Session["UserID"].ToString();
        
        bool success = RemoveFavorite(userId, movieId);
        if (success)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", 
                "alert('Removed from favorites!');", true);
            BindFavoritesGridView();
        }
    }
}
```

- **返回主页**：btnBack_Click返回主页面。
```csharp
// favorite.aspx.cs - 返回主页
protected void btnBack_Click(object sender, EventArgs e)
{
    Response.Redirect("~/main_page/main.aspx");
}
```

#### 4.2.4 密码修改事件
- **提交修改**：change_password.aspx.cs中的btnSubmit_Click事件验证旧密码并更新为新密码。
```csharp
// change_password.aspx.cs - 修改密码
string checkPasswordQuery = "SELECT Uno FROM Users WHERE Uno = @UserID AND Upassword = @OldPassword";
using (SqlCommand checkCommand = new SqlCommand(checkPasswordQuery, connection))
{
    checkCommand.Parameters.AddWithValue("@UserID", userId);
    checkCommand.Parameters.AddWithValue("@OldPassword", oldPassword);
    
    object result = checkCommand.ExecuteScalar();
    if (result == null)
    {
        lblMessage.Text = "Old password is incorrect";
        return;
    }
}

string updatePasswordQuery = "UPDATE Users SET Upassword = @NewPassword WHERE Uno = @UserID";
// 执行更新操作
```

#### 4.2.5 评分查看事件
- **分页查看**：view_rating.aspx.cs中的gvRatings_PageIndexChanging实现评分评论列表的分页。
```csharp
// view_rating.aspx.cs - 评分列表分页
protected void gvRatings_PageIndexChanging(object sender, GridViewPageEventArgs e)
{
    gvRatings.PageIndex = e.NewPageIndex;
    string movieId = Request.QueryString["movieId"];
    BindRatingsGrid(movieId);
}
```

### 4.3 逻辑处理
#### 4.3.1 用户状态管理
- 使用Session存储UserID和Username，用于在页面间保持用户登录状态。若用户未登录，则重定向至登录页面。
```csharp
// main.aspx.cs - 检查用户登录状态
if (Session["Username"] != null)
{
    lblUsername.Text = Session["Username"].ToString();
}
else
{
    Response.Redirect("~/start_page/login.html");
}
```

#### 4.3.2 数据访问封装
- 数据访问层（DAL）如MovieDAL.cs和UserDAL.cs封装了常见的数据库操作（查询、插入、更新），供逻辑层调用。通过 DataTable 和 SqlDataAdapter 实现数据绑定与展示。
```csharp
// MovieDAL.cs - 查询电影列表
public DataTable GetMovies(string searchTerm)
{
    string query = "SELECT Mno as MovieID, Mname as Title, Mtype as Genre, Myear as ReleaseYear FROM Movie";
    if (!string.IsNullOrEmpty(searchTerm))
    {
        query += " WHERE Mname LIKE @SearchTerm OR Mtype LIKE @SearchTerm";
        cmd.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
    }
    // 执行查询并返回DataTable
}
```

#### 4.3.3 业务规则校验
- 注册时检查用户名是否重复
- 评分和收藏前检查用户是否已登录
- 收藏时检查是否已收藏过同一电影
- 修改密码时验证旧密码是否正确
```csharp
// main.aspx.cs - 收藏前检查用户登录状态
if (Session["UserID"] == null)
{
    Response.Write("<script>alert('Please login first!');window.location.href='../start_page/login.html';</script>");
    return;
}
```

#### 4.3.4 页面间参数传递
- 使用QueryString传递参数，如movieId和movieTitle，实现页面间数据共享。使用Server.UrlEncode和Server.UrlDecode处理中文参数。
```csharp
// main.aspx.cs - 传递电影参数到评分页面
Response.Redirect("../rating_page/rating.html?movieId=" + movieIdStr + 
                 "&movieTitle=" + Server.UrlEncode(movieTitle) + 
                 "&userId=" + userId);

// view_rating.aspx.cs - 接收电影参数
string movieId = Request.QueryString["movieId"];
string movieTitle = Server.UrlDecode(Request.QueryString["movieTitle"]);
```

## 五、展示层设计

### UI设计
- 交互反馈：悬停时会触发颜色变化与浮动效果。
```css
input:focus {
    border-color: #9face6;
    box-shadow: 0 0 8px rgba(159, 172, 230, 0.3); 
}
back-btn:hover {
    background-color: #c4c4c4;
    transform: translateY(-2px);
}
.gender-group label:hover {
    color: #4f66ca;
}
```
- 动画效果：所有交互都使用`transition`进行过度，页面变化流畅。
## 六、操作提示
### 6.1 数据库文件挂载
`\Database`中包含项目运行需要的示例数据库。

`\Database\MovieRating.mdf`为数据库文件。

`\Database\MovieRating_log.ldf`为日志文件。

### 6.2 数据库用户创建


### 6.3 测试

### 6.4 文件结构
- Web.config


- database
  - MovieRating.mdf
  - MovieRating_log.ldf
- create_rating_table.sql
  - MovieRatingIS.sln


- DAL
  - MovieDAL.cs
  - UserDAL.cs


- Model
  - Movie.cs
  - User.cs


- codes
  - connection.aspx
    - connection.aspx.cs
    - connection.aspx.designer.cs
  - register.aspx
    - register.aspx.cs
    - register.aspx.designer.cs


- main_page
  - main.aspx
    - main.aspx.cs
      - main.aspx.designer.cs
- start_page
  - login.html
  - register.html
  - start.html
  - style.css
- changepwd_page
- view_rating_page
- rating_page
- favorite_page


- test.aspx
  - test.aspx.cs


- obj
- Properties
- MovieRatingIS.csproj
- MovieRatingIS.csproj.user



