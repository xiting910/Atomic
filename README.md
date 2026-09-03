# ⚡ Xiting.Atomic

NuGet Gallery: [Xiting.Atomic](https://www.nuget.org/packages/Xiting.Atomic/)

一个基于 **Interlocked** 的无锁原子操作库，提供枚举与 primitive 类型的原子读写和基于自旋的比较-交换 (CAS) 操作，使用 **.NET 10.0** 构建。

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET 10.0](https://img.shields.io/badge/.NET-10.0-512BD4.svg)](https://dotnet.microsoft.com/)
[![NuGet](https://img.shields.io/nuget/v/Xiting.Atomic.svg)](https://www.nuget.org/packages/Xiting.Atomic)
[![Nuget Downloads](https://img.shields.io/nuget/dt/Xiting.Atomic.svg)](https://www.nuget.org/packages/Xiting.Atomic)
[![CI](https://github.com/xiting910/Atomic/actions/workflows/ci.yml/badge.svg)](https://github.com/xiting910/Atomic/actions/workflows/ci.yml)
[![CodeQL](https://github.com/xiting910/Atomic/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/xiting910/Atomic/actions/workflows/codeql-analysis.yml)
[![Dependency Review](https://github.com/xiting910/Atomic/actions/workflows/dependency-review.yml/badge.svg)](https://github.com/xiting910/Atomic/actions/workflows/dependency-review.yml)

---

## ✨ 特性

- ⚡ **无锁实现** — 基于 `Interlocked` 与 `Volatile`，不依赖 `lock`
- 🔢 **AtomicOperation 静态类** — `SpinPredicateAndSet` 自旋条件交换，支持 byte/sbyte/short/ushort/int/uint/long/ulong 及泛型（引用类型、primitive、枚举）重载，竞态时自动自旋重试
- 📌 **AtomicEnum\<TEnum\> 结构体** — 枚举的原子读写、`CompareAndSet`、`PredicateAndSet`、`SpinPredicateAndSet` 操作，与 `TEnum` 隐式互转并支持 `==`/`!=` 比较
- 📋 **AtomicOperationResult 枚举** — 原子操作结果（`AlreadySet`/`PredicateFailed`/`RaceCondition`/`Success`）
- 📦 **NuGet 发布** — 推 `v*` tag 自动构建、测试，并通过 Trusted Publishing (OIDC) 发布到 [nuget.org](https://www.nuget.org/packages/Xiting.Atomic)
- 🔒 **API 契约固定** — PublicAPI 分析器 + PackageValidation 基线对比，防止意外破坏公共 API
- 🧪 **完善的单元测试** — xUnit v3 + Moq + coverlet，覆盖常规与并发场景
- 🔁 **CI/CD 自动化** — GitHub Actions 自动构建、测试、CodeQL 安全分析、Release 发布
- 📦 **依赖自动更新** — Dependabot 自动更新 NuGet 与 Actions 依赖，保持依赖最新

---

## 🏗️ 项目结构

```
Atomic/
├── .github/                                        #   GitHub 配置
│   ├── ISSUE_TEMPLATE/                             #     Issue 模板
│   │   ├── bug_report.md                           #       Bug 报告模板
│   │   ├── config.yml                              #       Issue 模板选择配置 (文档链接)
│   │   └── feature_request.md                      #       功能建议模板
│   └── workflows/                                  #     GitHub Actions 工作流
│   │   ├── ci.yml                                  #       CI (构建/测试/上传测试结果)
│   │   ├── codeql-analysis.yml                     #       CodeQL 安全分析 (push/PR/每周定时)
│   │   ├── dependabot-auto-merge.yml               #       Dependabot PR 自动 approve + squash 合并
│   │   ├── dependency-review.yml                   #       依赖漏洞审查 (PR 评论区报告)
│   │   └── release-publish.yml                     #       发布 (推 v* tag 自动发布 NuGet 包与 GitHub Release)
│   ├── dependabot.yml                              #     Dependabot 依赖更新 (NuGet + Actions)
│   └── PULL_REQUEST_TEMPLATE.md                    #     PR 描述模板
├── Xiting.Atomic/                                  #   原子操作库项目
│   ├── AtomicEnum.cs                               #     枚举原子操作封装结构体
│   ├── AtomicOperation.cs                          #     原子操作静态类 (SpinPredicateAndSet)
│   ├── AtomicOperationResult.cs                    #     原子操作结果枚举
│   ├── PublicAPI.Shipped.txt                       #     已发布公共 API 契约 (PublicAPI 分析器)
│   ├── PublicAPI.Unshipped.txt                     #     待发布公共 API 变更
│   ├── README.md                                   #     包内 README (nuget.org 包页面展示)
│   └── Xiting.Atomic.csproj                        #     项目文件 (打包元数据/SourceLink/包验证)
├── Xiting.Atomic.Tests/                            #   原子操作库单元测试项目
│   ├── AtomicEnumTests.cs                          #     AtomicEnum 单元测试
│   ├── AtomicEnumConcurrencyTests.cs               #     AtomicEnum 并发测试
│   ├── AtomicOperationPrimitiveTests.cs            #     primitive 类型重载单元测试
│   ├── AtomicOperationGenericTests.cs              #     泛型重载单元测试
│   ├── AtomicOperationConcurrencyTests.cs          #     AtomicOperation 并发测试
│   ├── Test.cs                                     #     测试辅助类型 (Test/FlagsTest/UnsupportedStruct)
│   └── Xiting.Atomic.Tests.csproj                  #     项目文件
├── .editorconfig                                   #   代码风格统一配置
├── .gitattributes                                  #   Git 行尾归一化 (默认 LF), diff 策略与二进制标记
├── .gitignore                                      #   忽略规则
├── CHANGELOG.md                                    #   变更日志
├── Directory.Build.props                           #   全局构建属性 (TargetFramework / Nullable / CPM)
├── Directory.Packages.props                        #   集中包版本管理 (NuGet CPM)
├── global.json                                     #   测试平台运行器配置 (Microsoft.Testing.Platform)
├── LICENSE                                         #   MIT 许可证
├── README.md                                       #   本文档
├── TagPush.bat                                     #   打 tag 并推送发布脚本
└── Atomic.slnx                                     #   解决方案文件 (.NET XML 格式)
```

---

## 🚀 快速开始

### 环境要求

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### 安装

```bash
dotnet add package Xiting.Atomic
```

### 克隆

```bash
git clone https://github.com/xiting910/Atomic.git
```

### 构建

```bash
dotnet build
```

### 运行测试

```bash
dotnet test
```

---

## 📄 许可证

本项目采用 [MIT License](LICENSE)。
