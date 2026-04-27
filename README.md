# Lime Framework

基于 .NET 10 与 Abp vNext 搭建的精简项目模板

[![NetCore](https://img.shields.io/badge/.NET-10-blue)](https://github.com/dotnet/runtime)
[![AbpVNext](https://img.shields.io/badge/AbpVNext-laster-red)](https://github.com/abpframework/abp)
[![License](https://img.shields.io/badge/License-MIT-yellowgreen)](https://github.com/username/lime-framework)

## 简短描述

Lime Framework 是一个基于 .NET 10 和 Abp vNext 的精简项目模板，旨在帮助开发者快速搭建后端服务项目。

## 功能特性

- 精简的项目结构和配置
- 支持 .NET 10 和 Abp vNext
- 模块化设计，各层独立解耦
- 自动 Swagger API 文档
- 可选的 Redis 分布式缓存

## 项目架构

```bash
src
├── Lime.Core           # 核心层：实体、枚举、常量、领域模型、InternalApp 全局配置
├── Lime.Extensions     # 扩展层：Serilog 日志、Kestrel 端口、Swagger、MVC 约定路由
├── Lime.Middlewares    # 中间件层：请求管道、异常处理
├── Lime.SugarSql       # ORM 层：SqlSugar 配置（预留）
├── Lime.Repository     # 仓储层：数据访问接口与实现
├── Lime.Service        # 服务层：业务逻辑、ApplicationService
└── Lime.Host           # 主机层：Web API 入口、模块依赖配置
```

## 先决条件

- .NET 10 SDK
- Visual Studio Code 或其他 IDE

## 快速开始

```bash
# 构建项目
dotnet build

# 运行项目（默认端口 5000/5001）
dotnet run --project src/Lime.Host

# 指定端口运行
dotnet run --project src/Lime.Host -- --port 8080
```

运行后访问 `http://localhost:5000/swagger` 查看 API 文档。

## 配置说明

### 分布式缓存

在 `appsettings.json` 中配置 Redis：

```json
{
  "Cache": {
    "IsEnabled": true,
    "Configuration": "localhost:6379,password=xxx,defaultDatabase=0"
  }
}
```

设置 `IsEnabled: false` 可禁用缓存功能。

## 许可

Lime Framework 采用 MIT 许可协议。
