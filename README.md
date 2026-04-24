# Lime Framework

基于 .NET 10 与 Abp vNext 搭建的精简项目模板

[![NetCore](https://img.shields.io/badge/.NET-10-blue)](https://github.com/dotnet/runtime)
[![AbpVNext](https://img.shields.io/badge/AbpVNext-laster-red)](https://github.com/abpframework/abp)
[![License](https://img.shields.io/badge/License-MIT-yellowgreen)](https://github.com/username/lime-framework)

## 简短描述

Lime Framework 是一个基于 .NET 10 和 Abp vNext 的精简项目模板,旨在帮助开发者快速搭建后端服务项目。

## 功能特性

- 精简的项目结构和配置
- 支持 .NET 10 和 Abp vNext
- 后端服务项目模板

## 项目架构

```bash
src
├── Lime.Core           # 核心层：实体、枚举、常量、领域模型
├── Lime.Extensions     # 扩展层：Abp 模块扩展、第三方组件集成
├── Lime.Middlewares    # 中间件层：请求管道、异常处理、日志记录
├── Lime.SugarSql       # ORM 层：SqlSugar 配置、仓储基础实现
├── Lime.Repository     # 仓储层：数据访问接口与实现
├── Lime.Service        # 服务层：业务逻辑、领域服务
└── Lime.Host           # 主机层：Web API、依赖注入配置、启动入口
```

## 先决条件

- .NET 10 SDK
- Abp vNext
- Visual Studio Code 或其他 IDE

## 许可

Lime Framework 采用 MIT 许可协议。
