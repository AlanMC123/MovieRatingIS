<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rating.aspx.cs" Inherits="rating_page_rating" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>评分处理</title>
    <style>
        /* 全局样式重置 */
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }
        
        /* 全局字体设置 */
        body {
            font-family: 'Microsoft YaHei', 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            min-height: 100vh;
            color: #333;
            line-height: 1.6;
            display: flex;
            justify-content: center;
            align-items: center;
        }
        
        /* 内容卡片 */
        .container {
            background: white;
            border-radius: 15px;
            box-shadow: 0 10px 40px rgba(0, 0, 0, 0.15);
            padding: 40px;
            max-width: 500px;
            text-align: center;
            animation: fadeIn 0.6s ease-out;
        }
        
        /* 标题样式 */
        h1 {
            color: #2c3e50;
            margin-bottom: 20px;
            font-size: 2rem;
            font-weight: 700;
        }
        
        /* 消息样式 */
        .message {
            font-size: 1.2rem;
            margin-bottom: 30px;
            padding: 20px;
            border-radius: 10px;
            background: #f8f9fa;
        }
        
        /* 成功消息 */
        .message.success {
            background: #d4edda;
            color: #155724;
            border: 1px solid #c3e6cb;
        }
        
        /* 错误消息 */
        .message.error {
            background: #f8d7da;
            color: #721c24;
            border: 1px solid #f5c6cb;
        }
        
        /* 按钮样式 */
        .btn {
            padding: 12px 30px;
            border: none;
            border-radius: 50px;
            cursor: pointer;
            font-size: 1rem;
            font-weight: 600;
            transition: all 0.3s ease;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            min-width: 150px;
            text-decoration: none;
            display: inline-block;
        }
        
        .btn-primary {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            box-shadow: 0 4px 15px rgba(102, 126, 234, 0.4);
        }
        
        .btn-primary:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 20px rgba(102, 126, 234, 0.6);
        }
        
        /* 动画效果 */
        @keyframes fadeIn {
            from {
                opacity: 0;
                transform: translateY(20px);
            }
            to {
                opacity: 1;
                transform: translateY(0);
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1>评分结果</h1>
            <div class="message" id="messageContainer">
                <asp:Label ID="lblMessage" runat="server" Text="" CssClass="message"></asp:Label>
            </div>
            <a href="../main_page/main.aspx" class="btn btn-primary">返回电影列表</a>
        </div>
    </form>
</body>
</html>