# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## 语言偏好

使用中文与用户交流。代码和注释可保留英文，但对话、解释和说明使用中文。

## 项目概述

Lime Framework 是基于 .NET 10 和 Abp vNext 的精简后端服务模板。

## 常用命令

```bash
# 构建项目
dotnet build

# 格式化代码
csharpier format .

# 运行项目（默认端口 5000/5001）
dotnet run --project src/Lime.Host

# 指定端口运行
dotnet run --project src/Lime.Host -- --port 8080
```

## Git 提交工作流

在提交和推送代码之前，按以下顺序执行：

1. `csharpier format .` — 格式化代码
2. `dotnet build` — 编译项目确保无错误
3. 执行 git 操作（add、commit、push）

## 项目架构

```
src
├── Lime.Core           # 核心层：实体、枚举、常量、领域模型、InternalApp 全局配置
├── Lime.Extensions     # 扩展层：Serilog 日志、Kestrel 端口、Swagger、MVC 约定路由
├── Lime.Middlewares    # 中间件层：请求管道、异常处理
├── Lime.SugarSql       # ORM 层：SqlSugar 配置（预留）
├── Lime.Repository     # 仓储层：数据访问接口与实现
├── Lime.Service        # 服务层：业务逻辑、ApplicationService
└── Lime.Host           # 主机层：Web API 入口、模块依赖配置
```

## 关键架构要点

- **模块化设计**：每个层都是独立的 Abp 模块，通过 `DependsOn` 声明依赖关系
- **中央包管理**：使用 `Directory.Packages.props` 统一管理 NuGet 包版本
- **全局属性**：`Directory.Build.props` 配置 TargetFramework、Nullable、ImplicitUsings 等
- **约定路由**：API 控制器自动注册到 `/api` 路径下，通过 `LimeConventionalRouteBuilder` 自定义
- **应用入口**：`Program.cs` 使用顶层语句，配置 Serilog → Autofac → Abp 模块 → 运行
- **Swagger 文档**：运行后访问 `/swagger` 查看 API 文档，标题使用 `InternalApp.ApplicationName`
- **分布式缓存**：通过 `appsettings.json` 的 `Cache` 节点配置 Redis，`IsEnabled: false` 时禁用
