# Lime Framework

基于 .NET 10 与 Abp vNext 搭建的精简项目模板

[![NetCore](https://img.shields.io/badge/.NET-10-blue)](https://github.com/dotnet/runtime)
[![AbpVNext](https://img.shields.io/badge/AbpVNext-latest-red)](https://github.com/abpframework/abp)
[![License](https://img.shields.io/badge/License-MIT-yellowgreen)](https://github.com/Jess-Qiu/lime)

## 简介

Lime Framework 是一个基于 .NET 10 和 Abp vNext 的精简项目模板，旨在帮助开发者快速搭建后端服务项目。

## 功能特性

- 精简的项目结构和配置
- 支持 .NET 10 和 Abp vNext
- 模块化设计，各层独立解耦
- 自动 Swagger API 文档
- Redis 分布式缓存支持
- HybridCache 混合缓存支持
- Mapster 对象映射支持

## 项目架构

```text
src
├── Lime.Core           # 核心层：实体基类、仓储接口、枚举、常量、领域模型
├── Lime.Extensions     # 扩展层：第三方库集成（Serilog、Cache、Mapster、ORM 配置）
├── Lime.Middlewares    # 中间件层：请求管道、异常处理
├── Lime.Repository     # 仓储层：仓储实现、数据库上下文
├── Lime.Service        # 服务层：业务逻辑、ApplicationService
└── Lime.Host           # 主机层：Web API 入口、模块依赖配置
```

### 层级职责

| 层 | 职责 |
|---|------|
| **Core** | 实体基类、仓储接口、领域模型，不依赖第三方库 |
| **Extensions** | 第三方库配置和服务注册（Serilog、Cache、Mapster、ORM） |
| **Repository** | 仓储接口实现、数据库上下文 |
| **Service** | 业务逻辑、ApplicationService |
| **Host** | Web API 入口、模块依赖配置 |

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

设置 `IsEnabled: false` 可禁用 Redis 缓存。

### HybridCache

项目默认启用 HybridCache，无需额外配置。HybridCache 提供内存和分布式缓存的双重缓存能力，当 Redis 启用时自动使用分布式缓存作为二级缓存。

### 对象映射 (Mapster)

项目集成了 Mapster 作为对象映射库，通过 ABP 的 `IObjectMapper` 接口使用：

```csharp
// 在 ApplicationService 中使用
public MapItemDto MapToDestinationObject(TestItemDto dto)
{
    return ObjectMapper.Map<TestItemDto, MapItemDto>(dto);
}
```

可在程序集中定义 `IRegister` 接口实现自定义映射配置。

## 许可

Lime Framework 采用 MIT 许可协议。
